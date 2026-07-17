using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class PayInstallmentViewModel : ViewModelBase
    {
        public Installment Installment { get; }
        public bool IsFinalInstallment => Installment.InstallmentNumber == Installment.TotalInstallments;
        public string SummaryText
        {
            get
            {
                string text = $"{Installment.SaleCustomer} — Chassis {Installment.SaleChassis}\nInstallment {Installment.InstallmentNumber} of {Installment.TotalInstallments}, due {Installment.DueDate:dd MMM yyyy}\nAmount due: {Installment.Amount:N0}";
                if (IsFinalInstallment) text += "\nThis is the final installment — the full amount above must be received.";
                return text;
            }
        }

        public List<string> Accounts { get; } = new List<string>();
        private string _selectedAccount;
        public string SelectedAccount { get => _selectedAccount; set => SetField(ref _selectedAccount, value); }

        private DateTime _paidDate = DateTime.Today;
        public DateTime PaidDate { get => _paidDate; set => SetField(ref _paidDate, value); }

        private double _amountPaid;
        public double AmountPaid { get => _amountPaid; set => SetField(ref _amountPaid, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public PayInstallmentViewModel(Installment installment)
        {
            Installment = installment;
            AmountPaid = installment.Amount;
            Accounts.AddRange(DatabaseService.Instance.GetAccountNames());
            if (Accounts.Count > 0) SelectedAccount = Accounts[0];
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (AmountPaid <= 0) { MessageBox.Show("Enter the amount paid.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            if (string.IsNullOrEmpty(SelectedAccount)) { MessageBox.Show("Select an account.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            try
            {
                DatabaseService.Instance.PayInstallment(Installment, PaidDate, AmountPaid, SelectedAccount);
                CloseAction?.Invoke(true);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
