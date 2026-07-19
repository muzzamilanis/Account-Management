using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class DutyExpViewModel : ViewModelBase
    {
        private DutyExp _exp;
        public DutyExp Exp { get => _exp; set => SetField(ref _exp, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Clearance Expense" : "Clearance Expense";
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        public ObservableCollection<string> Agents { get; } = new ObservableCollection<string>();
        private string _selChassis;
        public string SelectedChassis { get => _selChassis; set { SetField(ref _selChassis, value); Exp.Chassis = value; } }
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Exp.DutyExpPaidBy = value; } }
        private string _selAgent;
        public string SelectedAgent { get => _selAgent; set { SetField(ref _selAgent, value); Exp.DutyExpAgent = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        private readonly DutyExp _original;

        public DutyExpViewModel(DutyExp existing = null)
        {
            IsEdit = existing != null;
            _original = existing;
            Exp = existing != null ? new DutyExp
            {
                RowId = existing.RowId, Chassis = existing.Chassis, DutyExpDate = existing.DutyExpDate,
                DutyExpAmount = existing.DutyExpAmount, DutyExpDetail = existing.DutyExpDetail,
                DutyExpPaidBy = existing.DutyExpPaidBy, DutyExpAgent = existing.DutyExpAgent
            } : new DutyExp();

            Chassis.AddRange(DatabaseService.Instance.GetInStockChassisNumbers());
            if (existing != null && !string.IsNullOrEmpty(existing.Chassis) && !Chassis.Contains(existing.Chassis))
                Chassis.Insert(0, existing.Chassis);
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            foreach (var a in DatabaseService.Instance.GetAgentNames()) Agents.Add(a);
            if (existing != null && !string.IsNullOrEmpty(existing.DutyExpAgent) && !Agents.Contains(existing.DutyExpAgent))
                Agents.Insert(0, existing.DutyExpAgent);

            _selChassis = Exp.Chassis;
            _selAccount = Exp.DutyExpPaidBy;
            _selAgent = Exp.DutyExpAgent;
            if (string.IsNullOrEmpty(_selChassis) && Chassis.Count > 0) SelectedChassis = Chassis[0];
            if (string.IsNullOrEmpty(_selAccount) && Accounts.Count > 0) SelectedAccount = Accounts[0];
            if (string.IsNullOrEmpty(_selAgent) && Agents.Count > 0) SelectedAgent = Agents[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Exp.Chassis)) { MessageBox.Show("Select chassis."); return; }
            if (string.IsNullOrEmpty(Exp.DutyExpAgent)) { MessageBox.Show("Select an agent."); return; }
            if (Exp.DutyExpAmount <= 0) { MessageBox.Show("Enter expense amount."); return; }

            if (IsEdit)
            {
                DatabaseService.Instance.AdjustAgentPayable(_original.DutyExpAgent, _original.DutyExpAmount);
                DatabaseService.Instance.AdjustStockDuty(_original.Chassis, -_original.DutyExpAmount);
                DatabaseService.Instance.UpdateDutyExp(Exp);
            }
            else
            {
                DatabaseService.Instance.AddDutyExp(Exp);
            }
            DatabaseService.Instance.AdjustAgentPayable(Exp.DutyExpAgent, -Exp.DutyExpAmount);
            DatabaseService.Instance.AdjustStockDuty(Exp.Chassis, Exp.DutyExpAmount);
            CloseAction?.Invoke(true);
        }
    }
}
