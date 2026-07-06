using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class DutyExpViewModel : ViewModelBase
    {
        private DutyExp _exp = new DutyExp();
        public DutyExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        public List<string> Agents { get; } = new List<string>();
        private string _selChassis;
        public string SelectedChassis { get => _selChassis; set { SetField(ref _selChassis, value); Exp.Chassis = value; } }
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.DutyExpPaidBy = value; } }
        private string _selAgent;
        public string SelectedAgent { get => _selAgent; set { SetField(ref _selAgent, value); Exp.DutyExpAgent = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public DutyExpViewModel()
        {
            Chassis.AddRange(DatabaseService.Instance.GetInStockChassisNumbers());
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            Agents.AddRange(DatabaseService.Instance.GetAgentNames());
            if (Chassis.Count > 0) SelectedChassis = Chassis[0];
            if (Accounts.Count > 0) SelectedAccount = Accounts[0];
            if (Agents.Count > 0) SelectedAgent = Agents[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Exp.Chassis)) { MessageBox.Show("Select chassis."); return; }
            if (Exp.DutyExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }
            DatabaseService.Instance.AddDutyExp(Exp);
            CloseAction?.Invoke(true);
        }
    }
}
