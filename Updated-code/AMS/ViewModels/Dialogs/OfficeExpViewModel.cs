using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class OfficeExpViewModel : ViewModelBase
    {
        private OfficeExp _exp;
        public OfficeExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Office Expense" : "Office Expense";
        public List<string> Accounts { get; } = new List<string>();
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.OfficeExpPaidBy = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        private readonly OfficeExp _original;

        public OfficeExpViewModel(OfficeExp existing = null)
        {
            IsEdit = existing != null;
            _original = existing;
            Exp = existing != null ? new OfficeExp
            {
                RowId = existing.RowId, OfficeExpDate = existing.OfficeExpDate,
                OfficeExpAmount = existing.OfficeExpAmount, OfficeExpDetail = existing.OfficeExpDetail, OfficeExpPaidBy = existing.OfficeExpPaidBy
            } : new OfficeExp();

            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            _selAccount = Exp.OfficeExpPaidBy;
            if (string.IsNullOrEmpty(_selAccount) && Accounts.Count > 0) SelectedAccount = Accounts[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Exp.OfficeExpPaidBy)) { MessageBox.Show("Select an account."); return; }
            if (Exp.OfficeExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }

            if (IsEdit)
            {
                DatabaseService.Instance.CreditAccountWithLedger(_original.OfficeExpPaidBy, _original.OfficeExpAmount, _original.OfficeExpDate, "Reversal (edit): Office Expense");
                DatabaseService.Instance.UpdateOfficeExp(Exp);
            }
            else
            {
                DatabaseService.Instance.AddOfficeExp(Exp);
            }
            DatabaseService.Instance.DebitAccountWithLedger(Exp.OfficeExpPaidBy, Exp.OfficeExpAmount, Exp.OfficeExpDate, $"Office Expense: {Exp.OfficeExpDetail}");
            CloseAction?.Invoke(true);
        }
    }
}
