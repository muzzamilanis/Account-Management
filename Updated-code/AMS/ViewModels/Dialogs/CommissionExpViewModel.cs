using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class CommissionExpViewModel : ViewModelBase
    {
        private CommissionExp _exp;
        public CommissionExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Commission Expense" : "Commission Expense";
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        private string _selChassis;
        public string SelectedChassis { get => _selChassis; set { SetField(ref _selChassis, value); Exp.Chassis = value; } }
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.CommissionExpPaidBy = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        private readonly CommissionExp _original;

        public CommissionExpViewModel(CommissionExp existing = null)
        {
            IsEdit = existing != null;
            _original = existing;
            Exp = existing != null ? new CommissionExp
            {
                RowId = existing.RowId, Chassis = existing.Chassis, CommissionExpDate = existing.CommissionExpDate,
                CommissionExpAmount = existing.CommissionExpAmount, CommissionExpDetail = existing.CommissionExpDetail, CommissionExpPaidBy = existing.CommissionExpPaidBy
            } : new CommissionExp();

            Chassis.AddRange(DatabaseService.Instance.GetInStockChassisNumbers());
            if (existing != null && !string.IsNullOrEmpty(existing.Chassis) && !Chassis.Contains(existing.Chassis))
                Chassis.Insert(0, existing.Chassis);
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            _selChassis = Exp.Chassis;
            _selAccount = Exp.CommissionExpPaidBy;
            if (string.IsNullOrEmpty(_selChassis) && Chassis.Count > 0) SelectedChassis = Chassis[0];
            if (string.IsNullOrEmpty(_selAccount) && Accounts.Count > 0) SelectedAccount = Accounts[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Exp.Chassis)) { MessageBox.Show("Select chassis."); return; }
            if (string.IsNullOrEmpty(Exp.CommissionExpPaidBy)) { MessageBox.Show("Select an account."); return; }
            if (Exp.CommissionExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }

            if (IsEdit)
            {
                DatabaseService.Instance.CreditAccountWithLedger(_original.CommissionExpPaidBy, _original.CommissionExpAmount, _original.CommissionExpDate, $"Reversal (edit): Commission Expense (Chassis {_original.Chassis})");
                DatabaseService.Instance.AdjustStockCommission(_original.Chassis, -_original.CommissionExpAmount);
                DatabaseService.Instance.UpdateCommissionExp(Exp);
            }
            else
            {
                DatabaseService.Instance.AddCommissionExp(Exp);
            }
            DatabaseService.Instance.DebitAccountWithLedger(Exp.CommissionExpPaidBy, Exp.CommissionExpAmount, Exp.CommissionExpDate, $"Commission Expense (Chassis {Exp.Chassis}): {Exp.CommissionExpDetail}");
            DatabaseService.Instance.AdjustStockCommission(Exp.Chassis, Exp.CommissionExpAmount);
            CloseAction?.Invoke(true);
        }
    }
}
