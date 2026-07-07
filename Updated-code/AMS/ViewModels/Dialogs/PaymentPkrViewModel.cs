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
    public class PaymentPkrViewModel : ViewModelBase
    {
        public bool IsYen { get; }
        public string Title => IsYen ? "Yen Payment" : "Party Payment";
        private Payment _yenPayment = new Payment();
        private PaymentPkr _pkrPayment = new PaymentPkr();
        public Payment YenPayment { get => _yenPayment; set => SetField(ref _yenPayment, value); }
        public PaymentPkr PkrPayment { get => _pkrPayment; set => SetField(ref _pkrPayment, value); }
        public List<string> Accounts { get; } = new List<string>();
        public ObservableCollection<string> Customers { get; } = new ObservableCollection<string>();
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); if (IsYen) YenPayment.PaidFrom = value; else PkrPayment.PaidFrom = value; } }
        private string _selCustomer;
        public string SelectedCustomer { get => _selCustomer; set { SetField(ref _selCustomer, value); PkrPayment.PaidTo = value; } }
        public double ExchangeRate { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public PaymentPkrViewModel(bool isYen)
        {
            IsYen = isYen;
            ExchangeRate = SettingsService.Instance.ExchangeRate;
            YenPayment.PaymentExcRate = ExchangeRate;
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            foreach (var c in DatabaseService.Instance.GetCustomerNames()) Customers.Add(c);
            if (Accounts.Count > 0) SelectedAccount = Accounts[0];
            if (Customers.Count > 0) SelectedCustomer = Customers[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (IsYen)
            {
                YenPayment.PaymentAmountPkr = YenPayment.PaymentAmountYen * YenPayment.PaymentExcRate;
                if (YenPayment.PaymentAmountYen <= 0) { MessageBox.Show("Enter amount in Yen."); return; }
                DatabaseService.Instance.AddYenPayment(YenPayment);
                DatabaseService.Instance.DebitAccount(YenPayment.PaidFrom, YenPayment.PaymentAmountPkr);
            }
            else
            {
                if (string.IsNullOrEmpty(PkrPayment.PaidTo)) { MessageBox.Show("Select a customer."); return; }
                if (PkrPayment.PaymentAmount <= 0) { MessageBox.Show("Enter payment amount."); return; }
                DatabaseService.Instance.AddPkrPayment(PkrPayment);
                DatabaseService.Instance.DebitAccount(PkrPayment.PaidFrom, PkrPayment.PaymentAmount);
                DatabaseService.Instance.RecordCustomerPayment(PkrPayment.PaidTo, PkrPayment.PaymentAmount);
            }
            CloseAction?.Invoke(true);
        }
    }
}
