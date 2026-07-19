using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class NoPlateExpViewModel : ViewModelBase
    {
        private NoPlateExp _exp;
        public NoPlateExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit No Plate Expense" : "No Plate Expense";
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        private string _selChassis;
        public string SelectedChassis { get => _selChassis; set { SetField(ref _selChassis, value); Exp.Chassis = value; } }
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.NoPlateExpPaidBy = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        private readonly NoPlateExp _original;

        public NoPlateExpViewModel(NoPlateExp existing = null)
        {
            IsEdit = existing != null;
            _original = existing;
            Exp = existing != null ? new NoPlateExp
            {
                RowId = existing.RowId, Chassis = existing.Chassis, NoPlateExpDate = existing.NoPlateExpDate,
                NoPlateExpAmount = existing.NoPlateExpAmount, NoPlateExpDetail = existing.NoPlateExpDetail, NoPlateExpPaidBy = existing.NoPlateExpPaidBy
            } : new NoPlateExp();

            Chassis.AddRange(DatabaseService.Instance.GetInStockChassisNumbers());
            if (existing != null && !string.IsNullOrEmpty(existing.Chassis) && !Chassis.Contains(existing.Chassis))
                Chassis.Insert(0, existing.Chassis);
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            _selChassis = Exp.Chassis;
            _selAccount = Exp.NoPlateExpPaidBy;
            if (string.IsNullOrEmpty(_selChassis) && Chassis.Count > 0) SelectedChassis = Chassis[0];
            if (string.IsNullOrEmpty(_selAccount) && Accounts.Count > 0) SelectedAccount = Accounts[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Exp.Chassis)) { MessageBox.Show("Select chassis."); return; }
            if (string.IsNullOrEmpty(Exp.NoPlateExpPaidBy)) { MessageBox.Show("Select an account."); return; }
            if (Exp.NoPlateExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }

            if (IsEdit)
            {
                DatabaseService.Instance.CreditAccountWithLedger(_original.NoPlateExpPaidBy, _original.NoPlateExpAmount, _original.NoPlateExpDate, $"Reversal (edit): No Plate Expense (Chassis {_original.Chassis})");
                DatabaseService.Instance.AdjustStockNoPlate(_original.Chassis, -_original.NoPlateExpAmount);
                DatabaseService.Instance.UpdateNoPlateExp(Exp);
            }
            else
            {
                DatabaseService.Instance.AddNoPlateExp(Exp);
            }
            DatabaseService.Instance.DebitAccountWithLedger(Exp.NoPlateExpPaidBy, Exp.NoPlateExpAmount, Exp.NoPlateExpDate, $"No Plate Expense (Chassis {Exp.Chassis}): {Exp.NoPlateExpDetail}");
            DatabaseService.Instance.AdjustStockNoPlate(Exp.Chassis, Exp.NoPlateExpAmount);
            CloseAction?.Invoke(true);
        }
    }
}
