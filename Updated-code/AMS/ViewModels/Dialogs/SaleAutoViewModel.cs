using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class SaleAutoViewModel : ViewModelBase
    {
        private Sale _sale;
        public Sale Sale { get => _sale; set => SetField(ref _sale, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Sale Entry" : "Auto Sale Entry";
        // Chassis (and the installment plan) can't change once a sale exists — editing is only
        // offered for non-installment sales in the first place (see MainWindow's edit guard), and
        // re-pointing a sale at a different car has its own can of worms (stock status, etc.) that
        // isn't worth opening here. Matches the legacy app, which also locks chassis during edit.
        public bool IsChassisLocked => IsEdit;
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Customers { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        private string _selectedChassis;
        public string SelectedChassis { get => _selectedChassis; set { SetField(ref _selectedChassis, value); Sale.SaleChassis = value; } }
        private string _selectedCustomer;
        public string SelectedCustomer { get => _selectedCustomer; set { SetField(ref _selectedCustomer, value); Sale.SaleCustomer = value; } }
        private string _selectedAccount;
        public string SelectedAccount { get => _selectedAccount; set { SetField(ref _selectedAccount, value); Sale.PaymentReceivedIn = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }
        public bool IsCreditSalesEnabled => SettingsService.Instance.Settings.EnableCreditSales && !IsEdit;

        private bool _isInstallmentSale;
        public bool IsInstallmentSale { get => _isInstallmentSale; set => SetField(ref _isInstallmentSale, value); }

        public bool IsMultiCurrencyEnabled => SettingsService.Instance.Settings.EnableMultiCurrency;
        public string BaseCurrencyLabel => SettingsService.Instance.Settings.BaseCurrencySymbol;
        public string SalePriceLabel => $"SALE PRICE ({BaseCurrencyLabel})";

        private bool _isForeignCurrencySale;
        public bool IsForeignCurrencySale
        {
            get => _isForeignCurrencySale;
            set { SetField(ref _isForeignCurrencySale, value); OnPropertyChanged(nameof(ConvertedAmountText)); }
        }

        // AmountUnitEntry (the amount+unit control on the UGX field) handles the "avoid
        // zero-counting mistakes" job on its own via its magnitude dropdown — e.g. "2.5" + Billion —
        // so this stays a plain settable amount, same as every other money field in the app.
        private double _foreignAmount;
        public double ForeignAmount
        {
            get => _foreignAmount;
            set { SetField(ref _foreignAmount, value); OnPropertyChanged(nameof(ConvertedAmountText)); }
        }

        // Advance/amount received on a UGX sale must be entered in UGX too, not base currency —
        // otherwise a UGX sale price and a base-currency advance silently produce a nonsense
        // balance (a real bug hit during testing: SalePrice ~19,000 USD, advance typed as
        // "50 Million" but read as 50,000,000 USD, giving a ~-50M balance).
        private double _foreignAmountReceived;
        public double ForeignAmountReceived
        {
            get => _foreignAmountReceived;
            set { SetField(ref _foreignAmountReceived, value); OnPropertyChanged(nameof(ConvertedReceivedText)); }
        }

        // UgxExchangeRate is stored the way people actually check it (Google, XE, etc.): how many
        // UGX equal 1 unit of the base currency, e.g. "1 USD = 3,690.40 UGX" — so converting a UGX
        // amount to base currency means dividing by the rate, not multiplying by it.
        public string ConvertedAmountText => ConvertUgxText(ForeignAmount);
        public string ConvertedReceivedText => ConvertUgxText(ForeignAmountReceived);

        private string ConvertUgxText(double ugxAmount)
        {
            double rate = SettingsService.Instance.Settings.UgxExchangeRate;
            double converted = rate > 0 ? ugxAmount / rate : 0;
            return $"= {BaseCurrencyLabel} {converted:N2} at rate 1 {SettingsService.Instance.Settings.BaseCurrencyCode} = {rate.ToString("0.########")} UGX";
        }

        // Reversal target on Save() when editing — the original figures this record's customer
        // balance and account credit were already applied against.
        private readonly Sale _original;

        public SaleAutoViewModel(Sale existing = null)
        {
            IsEdit = existing != null;
            _original = existing;
            Sale = existing != null ? new Sale
            {
                RowId = existing.RowId, SaleDate = existing.SaleDate, SaleChassis = existing.SaleChassis, SaleCustomer = existing.SaleCustomer,
                SalePrice = existing.SalePrice, SaleAmountReceived = existing.SaleAmountReceived, PaymentReceivedIn = existing.PaymentReceivedIn,
                InstallmentMonths = existing.InstallmentMonths, ReminderDaysBefore = existing.ReminderDaysBefore,
                SaleCurrency = existing.SaleCurrency, SaleForeignAmount = existing.SaleForeignAmount, SaleRate = existing.SaleRate
            } : new Sale { SaleDate = DateTime.Today };

            Chassis.AddRange(DatabaseService.Instance.GetInStockChassisNumbers());
            if (existing != null && !string.IsNullOrEmpty(existing.SaleChassis) && !Chassis.Contains(existing.SaleChassis))
                Chassis.Insert(0, existing.SaleChassis);
            Customers.AddRange(DatabaseService.Instance.GetCustomerNames());
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());

            _selectedChassis = Sale.SaleChassis;
            _selectedCustomer = Sale.SaleCustomer;
            _selectedAccount = Sale.PaymentReceivedIn;
            if (string.IsNullOrEmpty(_selectedChassis) && Chassis.Count > 0) SelectedChassis = Chassis[0];
            if (string.IsNullOrEmpty(_selectedCustomer) && Customers.Count > 0) SelectedCustomer = Customers[0];
            if (string.IsNullOrEmpty(_selectedAccount) && Accounts.Count > 0) SelectedAccount = Accounts[0];

            if (IsEdit && Sale.SaleCurrency == "UGX")
            {
                _isForeignCurrencySale = true;
                _foreignAmount = Sale.SaleForeignAmount;
                _foreignAmountReceived = Sale.SaleRate > 0 ? Sale.SaleAmountReceived * Sale.SaleRate : 0;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Sale.SaleChassis)) { MessageBox.Show("Select a chassis.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (string.IsNullOrEmpty(Sale.SaleCustomer)) { MessageBox.Show("Select a customer.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (IsMultiCurrencyEnabled && IsForeignCurrencySale)
            {
                double rate = SettingsService.Instance.Settings.UgxExchangeRate;
                if (rate <= 0) { MessageBox.Show("UGX exchange rate is not set. Set it from Company Settings.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
                if (ForeignAmount <= 0) { MessageBox.Show("Enter sale price in UGX.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
                Sale.SaleCurrency = "UGX";
                Sale.SaleForeignAmount = ForeignAmount;
                Sale.SaleRate = rate;
                Sale.SalePrice = ForeignAmount / rate;
                Sale.SaleAmountReceived = ForeignAmountReceived / rate;
            }
            else
            {
                Sale.SaleCurrency = null;
                Sale.SaleForeignAmount = 0;
                Sale.SaleRate = 0;
            }

            if (Sale.SalePrice <= 0) { MessageBox.Show("Enter sale price.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (Sale.SaleAmountReceived > 0 && string.IsNullOrEmpty(Sale.PaymentReceivedIn)) { MessageBox.Show("Select an account to receive payment in.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (!IsEdit)
            {
                if (!IsInstallmentSale) Sale.InstallmentMonths = 0;
                if (IsInstallmentSale && Sale.InstallmentMonths <= 0) { MessageBox.Show("Enter the number of months for the installment plan.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            }

            if (IsEdit)
            {
                // Reverse the old customer-balance and account-credit effects before applying the
                // new ones — mirrors what the legacy app does on Sale edit, just via the existing
                // reusable methods instead of hand-rolled SQL deltas.
                DatabaseService.Instance.RecordCustomerSale(_original.SaleCustomer, -_original.SaleAmountReceived, -_original.SaleBalance);
                if (_original.SaleAmountReceived > 0 && !string.IsNullOrEmpty(_original.PaymentReceivedIn))
                    DatabaseService.Instance.DebitAccountWithLedger(_original.PaymentReceivedIn, _original.SaleAmountReceived, _original.SaleDate, $"Reversal (edit): Sale {_original.SaleChassis}");
                DatabaseService.Instance.UpdateSale(Sale);
            }
            else
            {
                long saleRowId = DatabaseService.Instance.AddSale(Sale);
                DatabaseService.Instance.MarkStockSold(Sale.SaleChassis);
                if (IsInstallmentSale && Sale.InstallmentMonths > 0 && Sale.SaleBalance > 0)
                    DatabaseService.Instance.AddInstallmentPlan(saleRowId, Sale.SaleDate, Sale.SaleBalance, Sale.InstallmentMonths);
            }
            DatabaseService.Instance.RecordCustomerSale(Sale.SaleCustomer, Sale.SaleAmountReceived, Sale.SaleBalance);
            if (Sale.SaleAmountReceived > 0 && !string.IsNullOrEmpty(Sale.PaymentReceivedIn))
                DatabaseService.Instance.CreditAccountWithLedger(Sale.PaymentReceivedIn, Sale.SaleAmountReceived, Sale.SaleDate, $"Sale: {Sale.SaleChassis} to {Sale.SaleCustomer}");
            CloseAction?.Invoke(true);
        }
    }
}
