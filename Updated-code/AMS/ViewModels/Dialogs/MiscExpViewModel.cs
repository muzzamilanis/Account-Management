using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class MiscExpViewModel : ViewModelBase
    {
        private MiscExp _exp;
        public MiscExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Misc Expense" : "Misc Auto Expense";
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        private string _selChassis;
        public string SelectedChassis { get => _selChassis; set { SetField(ref _selChassis, value); Exp.Chassis = value; } }
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.MiscExpPaidBy = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        // Kept for reversal on Save() when editing — the original amount/chassis/account this
        // record's side effects (account debit, stock cost) were applied against.
        private readonly MiscExp _original;

        public MiscExpViewModel(MiscExp existing = null)
        {
            IsEdit = existing != null;
            _original = existing;
            Exp = existing != null ? new MiscExp
            {
                RowId = existing.RowId, Chassis = existing.Chassis, MiscExpDate = existing.MiscExpDate,
                MiscExpAmount = existing.MiscExpAmount, MiscExpDetail = existing.MiscExpDetail, MiscExpPaidBy = existing.MiscExpPaidBy
            } : new MiscExp();

            Chassis.AddRange(DatabaseService.Instance.GetInStockChassisNumbers());
            if (existing != null && !string.IsNullOrEmpty(existing.Chassis) && !Chassis.Contains(existing.Chassis))
                Chassis.Insert(0, existing.Chassis); // may have been sold since this expense was entered
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            _selChassis = Exp.Chassis;
            _selAccount = Exp.MiscExpPaidBy;
            if (string.IsNullOrEmpty(_selChassis) && Chassis.Count > 0) SelectedChassis = Chassis[0];
            if (string.IsNullOrEmpty(_selAccount) && Accounts.Count > 0) SelectedAccount = Accounts[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Exp.Chassis)) { MessageBox.Show("Select chassis."); return; }
            if (string.IsNullOrEmpty(Exp.MiscExpPaidBy)) { MessageBox.Show("Select an account."); return; }
            if (Exp.MiscExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }

            if (IsEdit)
            {
                // Reverse the old entry's effects, then apply the new ones — never a blind row
                // overwrite, since the account/stock figures depend on this record's amount.
                DatabaseService.Instance.CreditAccountWithLedger(_original.MiscExpPaidBy, _original.MiscExpAmount, _original.MiscExpDate, $"Reversal (edit): Misc Expense (Chassis {_original.Chassis})");
                DatabaseService.Instance.AdjustStockMiscExpense(_original.Chassis, -_original.MiscExpAmount);
                DatabaseService.Instance.UpdateMiscExp(Exp);
            }
            else
            {
                DatabaseService.Instance.AddMiscExp(Exp);
            }
            DatabaseService.Instance.DebitAccountWithLedger(Exp.MiscExpPaidBy, Exp.MiscExpAmount, Exp.MiscExpDate, $"Misc Expense (Chassis {Exp.Chassis}): {Exp.MiscExpDetail}");
            DatabaseService.Instance.AdjustStockMiscExpense(Exp.Chassis, Exp.MiscExpAmount);
            CloseAction?.Invoke(true);
        }
    }
}
