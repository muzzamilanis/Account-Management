using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class AccountTransferViewModel : ViewModelBase
    {
        private OfficeAccount _transfer = new OfficeAccount();
        public OfficeAccount Transfer { get => _transfer; set => SetField(ref _transfer, value); }
        public List<string> Accounts { get; } = new List<string>();
        private string _selCredit;
        public string CreditFrom { get => _selCredit; set { SetField(ref _selCredit, value); Transfer.CreditFrom = value; } }
        private string _selDebit;
        public string DebitTo { get => _selDebit; set { SetField(ref _selDebit, value); Transfer.DebitTo = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public AccountTransferViewModel()
        {
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            if (Accounts.Count > 0) { CreditFrom = Accounts[0]; DebitTo = Accounts.Count > 1 ? Accounts[1] : Accounts[0]; }
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (Transfer.Amount <= 0) { MessageBox.Show("Enter transfer amount."); return; }
            if (CreditFrom == DebitTo) { MessageBox.Show("Cannot transfer to same account."); return; }
            DatabaseService.Instance.AddOfficeAccountTransfer(Transfer);
            DatabaseService.Instance.DebitAccount(CreditFrom, Transfer.Amount);
            DatabaseService.Instance.CreditAccount(DebitTo, Transfer.Amount);
            CloseAction?.Invoke(true);
        }
    }
}
