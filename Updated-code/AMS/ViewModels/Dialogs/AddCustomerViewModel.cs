using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class AddCustomerViewModel : ViewModelBase
    {
        private Customer _customer;
        public Customer Customer { get => _customer; set => SetField(ref _customer, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Customer" : "Add New Customer";
        public static string[] Titles { get; } = { "Mr.", "Mrs.", "Ms.", "Dr.", "Engr." };
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public AddCustomerViewModel(Customer existing = null)
        {
            IsEdit = existing != null;
            Customer = existing != null ? new Customer { RowId = existing.RowId, Date = existing.Date, Title = existing.Title, Name = existing.Name, CNIC = existing.CNIC, Phone = existing.Phone, Address = existing.Address, PaymentReceived = existing.PaymentReceived, PaymentReceivable = existing.PaymentReceivable, PaymentPaid = existing.PaymentPaid } : new Customer();
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Customer.Name)) { MessageBox.Show("Customer name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (IsEdit) DatabaseService.Instance.UpdateCustomer(Customer);
            else DatabaseService.Instance.AddCustomer(Customer);
            CloseAction?.Invoke(true);
        }
    }
}
