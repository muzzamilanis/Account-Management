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
        private OfficeAccount _transfer;
        public OfficeAccount Transfer { get => _transfer; set => SetField(ref _transfer, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Account Transfer" : "Account to Account Transfer";
        public List<string> Accounts { get; } = new List<string>();
        private string _selCredit;
        public string CreditFrom { get => _selCredit; set { SetField(ref _selCredit, value); Transfer.CreditFrom = value; } }
        private string _selDebit;
        public string DebitTo { get => _selDebit; set { SetField(ref _selDebit, value); Transfer.DebitTo = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        private readonly OfficeAccount _original;

        public AccountTransferViewModel(OfficeAccount existing = null)
        {
            IsEdit = existing != null;
            _original = existing;
            Transfer = existing != null ? new OfficeAccount
            {
                RowId = existing.RowId, Date = existing.Date, Amount = existing.Amount,
                Detail = existing.Detail, CreditFrom = existing.CreditFrom, DebitTo = existing.DebitTo
            } : new OfficeAccount();

            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            _selCredit = Transfer.CreditFrom;
            _selDebit = Transfer.DebitTo;
            if (string.IsNullOrEmpty(_selCredit) && Accounts.Count > 0) CreditFrom = Accounts[0];
            if (string.IsNullOrEmpty(_selDebit) && Accounts.Count > 0) DebitTo = Accounts.Count > 1 ? Accounts[1] : Accounts[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (Transfer.Amount <= 0) { MessageBox.Show("Enter transfer amount."); return; }
            if (CreditFrom == DebitTo) { MessageBox.Show("Cannot transfer to same account."); return; }

            if (IsEdit)
            {
                DatabaseService.Instance.CreditAccountWithLedger(_original.CreditFrom, _original.Amount, _original.Date, $"Reversal (edit): Transfer to {_original.DebitTo}");
                DatabaseService.Instance.DebitAccountWithLedger(_original.DebitTo, _original.Amount, _original.Date, $"Reversal (edit): Transfer from {_original.CreditFrom}");
                DatabaseService.Instance.UpdateOfficeAccountTransfer(Transfer);
            }
            else
            {
                DatabaseService.Instance.AddOfficeAccountTransfer(Transfer);
            }
            DatabaseService.Instance.DebitAccountWithLedger(CreditFrom, Transfer.Amount, Transfer.Date, $"Transfer to {DebitTo}: {Transfer.Detail}");
            DatabaseService.Instance.CreditAccountWithLedger(DebitTo, Transfer.Amount, Transfer.Date, $"Transfer from {CreditFrom}: {Transfer.Detail}");
            CloseAction?.Invoke(true);
        }
    }
}
