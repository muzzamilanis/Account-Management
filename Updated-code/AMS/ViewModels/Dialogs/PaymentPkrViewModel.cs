using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class PaymentPkrViewModel : ViewModelBase
    {
        public bool IsYen { get; }
        public bool IsEdit { get; }
        public string Title => IsYen
            ? (IsEdit ? "Edit Yen Payment" : "Yen Payment")
            : (IsEdit ? "Edit Party Payment" : "Party Payment");
        private Payment _yenPayment = new Payment();
        private PaymentPkr _pkrPayment = new PaymentPkr();
        public Payment YenPayment { get => _yenPayment; set => SetField(ref _yenPayment, value); }
        public PaymentPkr PkrPayment { get => _pkrPayment; set => SetField(ref _pkrPayment, value); }
        public List<string> Accounts { get; } = new List<string>();
        public ObservableCollection<string> Customers { get; } = new ObservableCollection<string>();
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); if (IsYen) YenPayment.PaidFrom = value; else PkrPayment.PaidFrom = value; } }
        private string _selCustomer;
        public string SelectedCustomer { get => _selCustomer; set { SetField(ref _selCustomer, value); PkrPayment.PaidTo = value; } }
        public double ExchangeRate { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }
        public string AmountLabel => $"AMOUNT ({SettingsService.Instance.Settings.BaseCurrencySymbol})";

        private readonly Payment _originalYen;
        private readonly PaymentPkr _originalPkr;

        // Add mode
        public PaymentPkrViewModel(bool isYen) : this(isYen, null, null) { }
        // Edit mode — Yen payment
        public PaymentPkrViewModel(Payment existing) : this(true, existing, null) { }
        // Edit mode — Party (PKR) payment
        public PaymentPkrViewModel(PaymentPkr existing) : this(false, null, existing) { }

        private PaymentPkrViewModel(bool isYen, Payment existingYen, PaymentPkr existingPkr)
        {
            IsYen = isYen;
            IsEdit = existingYen != null || existingPkr != null;
            _originalYen = existingYen;
            _originalPkr = existingPkr;

            ExchangeRate = SettingsService.Instance.ExchangeRate;
            YenPayment.PaymentExcRate = ExchangeRate;
            if (existingYen != null)
            {
                YenPayment = new Payment
                {
                    RowId = existingYen.RowId, PaymentDate = existingYen.PaymentDate, PaymentAmountYen = existingYen.PaymentAmountYen,
                    PaymentExcRate = existingYen.PaymentExcRate, PaymentAmountPkr = existingYen.PaymentAmountPkr,
                    PaymentDetail = existingYen.PaymentDetail, PaidFrom = existingYen.PaidFrom
                };
            }
            if (existingPkr != null)
            {
                PkrPayment = new PaymentPkr
                {
                    RowId = existingPkr.RowId, PaymentDate = existingPkr.PaymentDate, PaymentAmount = existingPkr.PaymentAmount,
                    PaymentDetail = existingPkr.PaymentDetail, PaidFrom = existingPkr.PaidFrom, PaidTo = existingPkr.PaidTo
                };
            }

            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            foreach (var c in DatabaseService.Instance.GetCustomerNames()) Customers.Add(c);
            if (existingPkr != null && !string.IsNullOrEmpty(existingPkr.PaidTo) && !Customers.Contains(existingPkr.PaidTo))
                Customers.Insert(0, existingPkr.PaidTo);

            _selAccount = IsYen ? YenPayment.PaidFrom : PkrPayment.PaidFrom;
            _selCustomer = PkrPayment.PaidTo;
            if (string.IsNullOrEmpty(_selAccount) && Accounts.Count > 0) SelectedAccount = Accounts[0];
            if (string.IsNullOrEmpty(_selCustomer) && Customers.Count > 0) SelectedCustomer = Customers[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (IsYen)
            {
                YenPayment.PaymentAmountPkr = YenPayment.PaymentAmountYen * YenPayment.PaymentExcRate;
                if (YenPayment.PaymentAmountYen <= 0) { MessageBox.Show("Enter amount in Yen."); return; }

                if (IsEdit)
                {
                    DatabaseService.Instance.CreditAccountWithLedger(_originalYen.PaidFrom, _originalYen.PaymentAmountPkr, _originalYen.PaymentDate, "Reversal (edit): Yen Payment");
                    DatabaseService.Instance.UpdateYenPayment(YenPayment);
                }
                else
                {
                    DatabaseService.Instance.AddYenPayment(YenPayment);
                }
                DatabaseService.Instance.DebitAccountWithLedger(YenPayment.PaidFrom, YenPayment.PaymentAmountPkr, YenPayment.PaymentDate, $"Yen Payment: {YenPayment.PaymentDetail}");
            }
            else
            {
                if (string.IsNullOrEmpty(PkrPayment.PaidTo)) { MessageBox.Show("Select a customer."); return; }
                if (PkrPayment.PaymentAmount <= 0) { MessageBox.Show("Enter payment amount."); return; }

                if (IsEdit)
                {
                    DatabaseService.Instance.CreditAccountWithLedger(_originalPkr.PaidFrom, _originalPkr.PaymentAmount, _originalPkr.PaymentDate, $"Reversal (edit): Party Payment to {_originalPkr.PaidTo}");
                    DatabaseService.Instance.RecordCustomerPayment(_originalPkr.PaidTo, -_originalPkr.PaymentAmount);
                    DatabaseService.Instance.UpdatePkrPayment(PkrPayment);
                }
                else
                {
                    DatabaseService.Instance.AddPkrPayment(PkrPayment);
                }
                DatabaseService.Instance.DebitAccountWithLedger(PkrPayment.PaidFrom, PkrPayment.PaymentAmount, PkrPayment.PaymentDate, $"Party Payment to {PkrPayment.PaidTo}: {PkrPayment.PaymentDetail}");
                DatabaseService.Instance.RecordCustomerPayment(PkrPayment.PaidTo, PkrPayment.PaymentAmount);
            }
            CloseAction?.Invoke(true);
        }
    }
}
