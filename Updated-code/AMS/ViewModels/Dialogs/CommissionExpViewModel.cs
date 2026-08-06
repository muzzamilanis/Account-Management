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
        private CommissionExp _exp = new CommissionExp();
        public CommissionExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        private string _selChassis;
        public string SelectedChassis { get => _selChassis; set { SetField(ref _selChassis, value); Exp.Chassis = value; } }
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.CommissionExpPaidBy = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public CommissionExpViewModel()
        {
            Chassis.AddRange(DatabaseService.Instance.GetInStockChassisNumbers());
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            if (Chassis.Count > 0) SelectedChassis = Chassis[0];
            if (Accounts.Count > 0) SelectedAccount = Accounts[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Exp.Chassis)) { MessageBox.Show("Select chassis."); return; }
            if (string.IsNullOrEmpty(Exp.CommissionExpPaidBy)) { MessageBox.Show("Select an account."); return; }
            if (Exp.CommissionExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }
            DatabaseService.Instance.AddCommissionExp(Exp);
            DatabaseService.Instance.DebitAccountWithLedger(Exp.CommissionExpPaidBy, Exp.CommissionExpAmount, Exp.CommissionExpDate, $"Commission Expense (Chassis {Exp.Chassis}): {Exp.CommissionExpDetail}");
            DatabaseService.Instance.AdjustStockCommission(Exp.Chassis, Exp.CommissionExpAmount);
            CloseAction?.Invoke(true);
        }
    }
}
