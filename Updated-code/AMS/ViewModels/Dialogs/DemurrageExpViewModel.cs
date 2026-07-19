using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class DemurrageExpViewModel : ViewModelBase
    {
        private DemurrageExp _exp;
        public DemurrageExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Demurrage Expense" : "Demurrage Expense";
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        private string _selChassis;
        public string SelectedChassis { get => _selChassis; set { SetField(ref _selChassis, value); Exp.Chassis = value; } }
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.DemurrageExpPaidBy = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        private readonly DemurrageExp _original;

        public DemurrageExpViewModel(DemurrageExp existing = null)
        {
            IsEdit = existing != null;
            _original = existing;
            Exp = existing != null ? new DemurrageExp
            {
                RowId = existing.RowId, Chassis = existing.Chassis, DemurrageExpDate = existing.DemurrageExpDate,
                DemurrageExpAmount = existing.DemurrageExpAmount, DemurrageExpDetail = existing.DemurrageExpDetail, DemurrageExpPaidBy = existing.DemurrageExpPaidBy
            } : new DemurrageExp();

            Chassis.AddRange(DatabaseService.Instance.GetInStockChassisNumbers());
            if (existing != null && !string.IsNullOrEmpty(existing.Chassis) && !Chassis.Contains(existing.Chassis))
                Chassis.Insert(0, existing.Chassis);
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            _selChassis = Exp.Chassis;
            _selAccount = Exp.DemurrageExpPaidBy;
            if (string.IsNullOrEmpty(_selChassis) && Chassis.Count > 0) SelectedChassis = Chassis[0];
            if (string.IsNullOrEmpty(_selAccount) && Accounts.Count > 0) SelectedAccount = Accounts[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Exp.Chassis)) { MessageBox.Show("Select chassis."); return; }
            if (string.IsNullOrEmpty(Exp.DemurrageExpPaidBy)) { MessageBox.Show("Select an account."); return; }
            if (Exp.DemurrageExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }

            if (IsEdit)
            {
                DatabaseService.Instance.CreditAccountWithLedger(_original.DemurrageExpPaidBy, _original.DemurrageExpAmount, _original.DemurrageExpDate, $"Reversal (edit): Demurrage Expense (Chassis {_original.Chassis})");
                DatabaseService.Instance.AdjustStockDemurrage(_original.Chassis, -_original.DemurrageExpAmount);
                DatabaseService.Instance.UpdateDemurrageExp(Exp);
            }
            else
            {
                DatabaseService.Instance.AddDemurrageExp(Exp);
            }
            DatabaseService.Instance.DebitAccountWithLedger(Exp.DemurrageExpPaidBy, Exp.DemurrageExpAmount, Exp.DemurrageExpDate, $"Demurrage Expense (Chassis {Exp.Chassis}): {Exp.DemurrageExpDetail}");
            DatabaseService.Instance.AdjustStockDemurrage(Exp.Chassis, Exp.DemurrageExpAmount);
            CloseAction?.Invoke(true);
        }
    }
}
