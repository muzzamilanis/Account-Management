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
        private NoPlateExp _exp = new NoPlateExp();
        public NoPlateExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        private string _selChassis;
        public string SelectedChassis { get => _selChassis; set { SetField(ref _selChassis, value); Exp.Chassis = value; } }
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.NoPlateExpPaidBy = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public NoPlateExpViewModel()
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
            if (string.IsNullOrEmpty(Exp.NoPlateExpPaidBy)) { MessageBox.Show("Select an account."); return; }
            if (Exp.NoPlateExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }
            DatabaseService.Instance.AddNoPlateExp(Exp);
            DatabaseService.Instance.DebitAccountWithLedger(Exp.NoPlateExpPaidBy, Exp.NoPlateExpAmount, Exp.NoPlateExpDate, $"No Plate Expense (Chassis {Exp.Chassis}): {Exp.NoPlateExpDetail}");
            DatabaseService.Instance.AdjustStockNoPlate(Exp.Chassis, Exp.NoPlateExpAmount);
            CloseAction?.Invoke(true);
        }
    }
}
