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
    public class PaymentAgentViewModel : ViewModelBase
    {
        private PaymentAgent _payment;
        public PaymentAgent Payment { get => _payment; set => SetField(ref _payment, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Agent Payment" : "Agent Payment";
        public List<string> Accounts { get; } = new List<string>();
        public ObservableCollection<string> Agents { get; } = new ObservableCollection<string>();
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Payment.PaidFrom = value; } }
        private string _selAgent;
        public string SelectedAgent { get => _selAgent; set { SetField(ref _selAgent, value); Payment.PaidTo = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }
        public string AmountLabel => $"AMOUNT ({SettingsService.Instance.Settings.BaseCurrencySymbol})";

        private readonly PaymentAgent _original;

        public PaymentAgentViewModel(PaymentAgent existing = null)
        {
            IsEdit = existing != null;
            _original = existing;
            Payment = existing != null ? new PaymentAgent
            {
                RowId = existing.RowId, PaymentDate = existing.PaymentDate, PaymentAmount = existing.PaymentAmount,
                PaymentDetail = existing.PaymentDetail, PaidFrom = existing.PaidFrom, PaidTo = existing.PaidTo
            } : new PaymentAgent();

            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            foreach (var a in DatabaseService.Instance.GetAgentNames()) Agents.Add(a);
            if (existing != null && !string.IsNullOrEmpty(existing.PaidTo) && !Agents.Contains(existing.PaidTo))
                Agents.Insert(0, existing.PaidTo);

            _selAccount = Payment.PaidFrom;
            _selAgent = Payment.PaidTo;
            if (string.IsNullOrEmpty(_selAccount) && Accounts.Count > 0) SelectedAccount = Accounts[0];
            if (string.IsNullOrEmpty(_selAgent) && Agents.Count > 0) SelectedAgent = Agents[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Payment.PaidTo)) { MessageBox.Show("Select an agent."); return; }
            if (Payment.PaymentAmount <= 0) { MessageBox.Show("Enter payment amount."); return; }

            if (IsEdit)
            {
                DatabaseService.Instance.CreditAccountWithLedger(_original.PaidFrom, _original.PaymentAmount, _original.PaymentDate, $"Reversal (edit): Agent Payment to {_original.PaidTo}");
                DatabaseService.Instance.RecordAgentPayment(_original.PaidTo, -_original.PaymentAmount);
                DatabaseService.Instance.UpdateAgentPayment(Payment);
            }
            else
            {
                DatabaseService.Instance.AddAgentPayment(Payment);
            }
            DatabaseService.Instance.DebitAccountWithLedger(Payment.PaidFrom, Payment.PaymentAmount, Payment.PaymentDate, $"Agent Payment to {Payment.PaidTo}: {Payment.PaymentDetail}");
            DatabaseService.Instance.RecordAgentPayment(Payment.PaidTo, Payment.PaymentAmount);
            CloseAction?.Invoke(true);
        }
    }
}
