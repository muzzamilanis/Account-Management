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
        private DemurrageExp _exp = new DemurrageExp();
        public DemurrageExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        private string _selChassis;
        public string SelectedChassis { get => _selChassis; set { SetField(ref _selChassis, value); Exp.Chassis = value; } }
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.DemurrageExpPaidBy = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public DemurrageExpViewModel()
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
            if (string.IsNullOrEmpty(Exp.DemurrageExpPaidBy)) { MessageBox.Show("Select an account."); return; }
            if (Exp.DemurrageExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }
            DatabaseService.Instance.AddDemurrageExp(Exp);
            DatabaseService.Instance.DebitAccountWithLedger(Exp.DemurrageExpPaidBy, Exp.DemurrageExpAmount, Exp.DemurrageExpDate, $"Demurrage Expense (Chassis {Exp.Chassis}): {Exp.DemurrageExpDetail}");
            DatabaseService.Instance.AdjustStockDemurrage(Exp.Chassis, Exp.DemurrageExpAmount);
            CloseAction?.Invoke(true);
        }
    }
}
