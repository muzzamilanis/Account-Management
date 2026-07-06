using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class ReceiptViewModel : ViewModelBase
    {
        private Receipt _receipt = new Receipt();
        public Receipt Receipt { get => _receipt; set => SetField(ref _receipt, value); }
        public List<string> Accounts { get; } = new List<string>();
        public List<string> Customers { get; } = new List<string>();
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Receipt.ReceivedIn = value; } }
        private string _selCustomer;
        public string SelectedCustomer { get => _selCustomer; set { SetField(ref _selCustomer, value); Receipt.ReceivedFrom = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public ReceiptViewModel()
        {
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            Customers.AddRange(DatabaseService.Instance.GetCustomerNames());
            if (Accounts.Count > 0) SelectedAccount = Accounts[0];
            if (Customers.Count > 0) SelectedCustomer = Customers[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (Receipt.ReceiptAmount <= 0) { MessageBox.Show("Enter receipt amount."); return; }
            DatabaseService.Instance.AddReceipt(Receipt);
            CloseAction?.Invoke(true);
        }
    }
}
