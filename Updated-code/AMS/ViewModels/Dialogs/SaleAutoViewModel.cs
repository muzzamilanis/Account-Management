using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class SaleAutoViewModel : ViewModelBase
    {
        private Sale _sale;
        public Sale Sale { get => _sale; set => SetField(ref _sale, value); }
        public List<string> Chassis { get; } = new List<string>();
        public List<string> Customers { get; } = new List<string>();
        public List<string> Accounts { get; } = new List<string>();
        private string _selectedChassis;
        public string SelectedChassis { get => _selectedChassis; set { SetField(ref _selectedChassis, value); Sale.SaleChassis = value; } }
        private string _selectedCustomer;
        public string SelectedCustomer { get => _selectedCustomer; set { SetField(ref _selectedCustomer, value); Sale.SaleCustomer = value; } }
        private string _selectedAccount;
        public string SelectedAccount { get => _selectedAccount; set { SetField(ref _selectedAccount, value); Sale.PaymentReceivedIn = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public SaleAutoViewModel()
        {
            Sale = new Sale { SaleDate = DateTime.Today };
            Chassis.AddRange(DatabaseService.Instance.GetInStockChassisNumbers());
            Customers.AddRange(DatabaseService.Instance.GetCustomerNames());
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            if (Chassis.Count > 0) SelectedChassis = Chassis[0];
            if (Customers.Count > 0) SelectedCustomer = Customers[0];
            if (Accounts.Count > 0) SelectedAccount = Accounts[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Sale.SaleChassis)) { MessageBox.Show("Select a chassis.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (string.IsNullOrEmpty(Sale.SaleCustomer)) { MessageBox.Show("Select a customer.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (Sale.SalePrice <= 0) { MessageBox.Show("Enter sale price.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (Sale.SaleAmountReceived > 0 && string.IsNullOrEmpty(Sale.PaymentReceivedIn)) { MessageBox.Show("Select an account to receive payment in.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            DatabaseService.Instance.AddSale(Sale);
            DatabaseService.Instance.MarkStockSold(Sale.SaleChassis);
            DatabaseService.Instance.RecordCustomerSale(Sale.SaleCustomer, Sale.SaleAmountReceived, Sale.SaleBalance);
            if (Sale.SaleAmountReceived > 0 && !string.IsNullOrEmpty(Sale.PaymentReceivedIn))
                DatabaseService.Instance.CreditAccountWithLedger(Sale.PaymentReceivedIn, Sale.SaleAmountReceived, Sale.SaleDate, $"Sale: {Sale.SaleChassis} to {Sale.SaleCustomer}");
            CloseAction?.Invoke(true);
        }
    }
}
