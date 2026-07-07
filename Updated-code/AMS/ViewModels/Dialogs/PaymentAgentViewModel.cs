using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class PaymentAgentViewModel : ViewModelBase
    {
        private PaymentAgent _payment = new PaymentAgent();
        public PaymentAgent Payment { get => _payment; set => SetField(ref _payment, value); }
        public List<string> Accounts { get; } = new List<string>();
        public List<string> Agents { get; } = new List<string>();
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Payment.PaidFrom = value; } }
        private string _selAgent;
        public string SelectedAgent { get => _selAgent; set { SetField(ref _selAgent, value); Payment.PaidTo = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public PaymentAgentViewModel()
        {
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            Agents.AddRange(DatabaseService.Instance.GetAgentNames());
            if (Accounts.Count > 0) SelectedAccount = Accounts[0];
            if (Agents.Count > 0) SelectedAgent = Agents[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (Payment.PaymentAmount <= 0) { MessageBox.Show("Enter payment amount."); return; }
            DatabaseService.Instance.AddAgentPayment(Payment);
            DatabaseService.Instance.DebitAccount(Payment.PaidFrom, Payment.PaymentAmount);
            CloseAction?.Invoke(true);
        }
    }
}
