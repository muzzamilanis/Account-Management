using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using AMS.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AMS.ViewModels
{
    public class AccountsViewModel : ViewModelBase
    {
        public ObservableCollection<Account> Accounts { get; } = new ObservableCollection<Account>();
        public ObservableCollection<Models.MiscExp> MiscExps { get; } = new ObservableCollection<Models.MiscExp>();
        public ObservableCollection<Models.DutyExp> DutyExps { get; } = new ObservableCollection<Models.DutyExp>();
        public ObservableCollection<Models.OfficeExp> OfficeExps { get; } = new ObservableCollection<Models.OfficeExp>();
        public ObservableCollection<Models.Receipt> Receipts { get; } = new ObservableCollection<Models.Receipt>();
        public ObservableCollection<Models.Payment> YenPayments { get; } = new ObservableCollection<Models.Payment>();
        public ObservableCollection<Models.PaymentPkr> PkrPayments { get; } = new ObservableCollection<Models.PaymentPkr>();
        public ObservableCollection<Models.PaymentAgent> AgentPayments { get; } = new ObservableCollection<Models.PaymentAgent>();
        public ObservableCollection<Models.OfficeAccount> Transfers { get; } = new ObservableCollection<Models.OfficeAccount>();

        private Account _selectedAccount;
        public Account SelectedAccount { get => _selectedAccount; set => SetField(ref _selectedAccount, value); }

        private string _filterText;
        public string FilterText { get => _filterText; set { SetField(ref _filterText, value); LoadAccounts(); } }

        public double TotalProfit { get; private set; }
        public double TotalYenPayable { get; private set; }

        public ICommand AddAccountCommand { get; }
        public ICommand EditAccountCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand AddMiscExpCommand { get; }
        public ICommand AddDutyExpCommand { get; }
        public ICommand AddOfficeExpCommand { get; }
        public ICommand AddReceiptCommand { get; }
        public ICommand AddYenPaymentCommand { get; }
        public ICommand AddPkrPaymentCommand { get; }
        public ICommand AddAgentPaymentCommand { get; }
        public ICommand AddTransferCommand { get; }
        public ICommand WithdrawProfitCommand { get; }

        public AccountsViewModel()
        {
            AddAccountCommand = new RelayCommand(() => OpenAddAccount(null));
            EditAccountCommand = new RelayCommand(() => OpenAddAccount(SelectedAccount), () => SelectedAccount != null);
            RefreshCommand = new RelayCommand(LoadAll);
            AddMiscExpCommand = new RelayCommand(OpenMiscExp);
            AddDutyExpCommand = new RelayCommand(OpenDutyExp);
            AddOfficeExpCommand = new RelayCommand(OpenOfficeExp);
            AddReceiptCommand = new RelayCommand(OpenReceipt);
            AddYenPaymentCommand = new RelayCommand(OpenYenPayment);
            AddPkrPaymentCommand = new RelayCommand(OpenPkrPayment);
            AddAgentPaymentCommand = new RelayCommand(OpenAgentPayment);
            AddTransferCommand = new RelayCommand(OpenTransfer);
            WithdrawProfitCommand = new RelayCommand(OpenProfitWithdrawal);
        }

        public void LoadAll()
        {
            LoadAccounts();
            LoadMiscExps();
            LoadDutyExps();
            LoadOfficeExps();
            LoadReceipts();
            LoadYenPayments();
            LoadPkrPayments();
            LoadAgentPayments();
            LoadTransfers();
            TotalProfit = DatabaseService.Instance.GetTotalProfit();
            TotalYenPayable = DatabaseService.Instance.GetTotalYenPayable();
            OnPropertyChanged(nameof(TotalProfit));
            OnPropertyChanged(nameof(TotalYenPayable));
        }

        public void LoadAccounts()
        {
            Accounts.Clear();
            foreach (var a in DatabaseService.Instance.GetAccounts(FilterText ?? ""))
                Accounts.Add(a);
        }
        private void LoadMiscExps() { MiscExps.Clear(); foreach (var e in DatabaseService.Instance.GetMiscExps()) MiscExps.Add(e); }
        private void LoadDutyExps() { DutyExps.Clear(); foreach (var e in DatabaseService.Instance.GetDutyExps()) DutyExps.Add(e); }
        private void LoadOfficeExps() { OfficeExps.Clear(); foreach (var e in DatabaseService.Instance.GetOfficeExps()) OfficeExps.Add(e); }
        private void LoadReceipts() { Receipts.Clear(); foreach (var e in DatabaseService.Instance.GetReceipts()) Receipts.Add(e); }
        private void LoadYenPayments() { YenPayments.Clear(); foreach (var e in DatabaseService.Instance.GetYenPayments()) YenPayments.Add(e); }
        private void LoadPkrPayments() { PkrPayments.Clear(); foreach (var e in DatabaseService.Instance.GetPkrPayments()) PkrPayments.Add(e); }
        private void LoadAgentPayments() { AgentPayments.Clear(); foreach (var e in DatabaseService.Instance.GetAgentPayments()) AgentPayments.Add(e); }
        private void LoadTransfers() { Transfers.Clear(); foreach (var e in DatabaseService.Instance.GetOfficeAccounts()) Transfers.Add(e); }

        private void OpenAddAccount(Account existing)
        {
            var dlg = new AddAccountDialog(existing);
            if (dlg.ShowDialog() == true) LoadAccounts();
        }
        private void OpenMiscExp() { var d = new MiscExpDialog(); if (d.ShowDialog() == true) LoadMiscExps(); }
        private void OpenDutyExp() { var d = new DutyExpDialog(); if (d.ShowDialog() == true) LoadDutyExps(); }
        private void OpenOfficeExp() { var d = new OfficeExpDialog(); if (d.ShowDialog() == true) LoadOfficeExps(); }
        private void OpenReceipt() { var d = new ReceiptDialog(); if (d.ShowDialog() == true) LoadReceipts(); }
        private void OpenYenPayment() { var d = new PaymentPkrDialog(isYen: true); if (d.ShowDialog() == true) LoadYenPayments(); }
        private void OpenPkrPayment() { var d = new PaymentPkrDialog(isYen: false); if (d.ShowDialog() == true) LoadPkrPayments(); }
        private void OpenAgentPayment() { var d = new PaymentAgentDialog(); if (d.ShowDialog() == true) LoadAgentPayments(); }
        private void OpenTransfer() { var d = new AccountTransferDialog(); if (d.ShowDialog() == true) LoadTransfers(); }
        private void OpenProfitWithdrawal() { System.Windows.MessageBox.Show("Profit withdrawal — use the Profit Withdrawal tab."); }
    }
}
