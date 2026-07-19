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
        private Receipt _receipt;
        public Receipt Receipt { get => _receipt; set => SetField(ref _receipt, value); }
        public bool IsEdit { get; }
        public string Title => IsEdit ? "Edit Receipt" : "Receipt Entry";
        public List<string> Accounts { get; } = new List<string>();
        public List<string> Customers { get; } = new List<string>();
        private string _selAccount;
        public string SelectedAccount { get => _selAccount; set { SetField(ref _selAccount, value); Receipt.ReceivedIn = value; } }
        private string _selCustomer;
        public string SelectedCustomer { get => _selCustomer; set { SetField(ref _selCustomer, value); Receipt.ReceivedFrom = value; } }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        private readonly Receipt _original;

        public ReceiptViewModel(Receipt existing = null)
        {
            IsEdit = existing != null;
            _original = existing;
            Receipt = existing != null ? new Receipt
            {
                RowId = existing.RowId, ReceiptDate = existing.ReceiptDate, ReceiptAmount = existing.ReceiptAmount,
                ReceiptDetail = existing.ReceiptDetail, ReceivedIn = existing.ReceivedIn, ReceivedFrom = existing.ReceivedFrom
            } : new Receipt();

            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            Customers.AddRange(DatabaseService.Instance.GetCustomerNames());
            if (existing != null && !string.IsNullOrEmpty(existing.ReceivedFrom) && !Customers.Contains(existing.ReceivedFrom))
                Customers.Insert(0, existing.ReceivedFrom);
            _selAccount = Receipt.ReceivedIn;
            _selCustomer = Receipt.ReceivedFrom;
            if (string.IsNullOrEmpty(_selAccount) && Accounts.Count > 0) SelectedAccount = Accounts[0];
            if (string.IsNullOrEmpty(_selCustomer) && Customers.Count > 0) SelectedCustomer = Customers[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(Receipt.ReceivedIn)) { MessageBox.Show("Select an account."); return; }
            if (string.IsNullOrEmpty(Receipt.ReceivedFrom)) { MessageBox.Show("Select a customer."); return; }
            if (Receipt.ReceiptAmount <= 0) { MessageBox.Show("Enter receipt amount."); return; }

            if (IsEdit)
            {
                DatabaseService.Instance.DebitAccountWithLedger(_original.ReceivedIn, _original.ReceiptAmount, _original.ReceiptDate, $"Reversal (edit): Receipt from {_original.ReceivedFrom}");
                DatabaseService.Instance.RecordCustomerReceipt(_original.ReceivedFrom, -_original.ReceiptAmount);
                DatabaseService.Instance.UpdateReceipt(Receipt);
            }
            else
            {
                DatabaseService.Instance.AddReceipt(Receipt);
            }
            DatabaseService.Instance.CreditAccountWithLedger(Receipt.ReceivedIn, Receipt.ReceiptAmount, Receipt.ReceiptDate, $"Receipt from {Receipt.ReceivedFrom}: {Receipt.ReceiptDetail}");
            DatabaseService.Instance.RecordCustomerReceipt(Receipt.ReceivedFrom, Receipt.ReceiptAmount);
            CloseAction?.Invoke(true);
        }
    }
}
