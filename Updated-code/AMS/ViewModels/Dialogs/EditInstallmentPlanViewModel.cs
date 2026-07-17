using AMS.Helpers;
using AMS.Models;
using AMS.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AMS.ViewModels.Dialogs
{
    public class EditInstallmentPlanViewModel : ViewModelBase
    {
        private readonly Sale _sale;
        public string SummaryText { get; }

        private int _months;
        public int Months { get => _months; set => SetField(ref _months, value); }

        private int _reminderDaysBefore;
        public int ReminderDaysBefore { get => _reminderDaysBefore; set => SetField(ref _reminderDaysBefore, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action<bool?> CloseAction { get; set; }

        public EditInstallmentPlanViewModel(Sale sale)
        {
            _sale = sale;
            var installments = DatabaseService.Instance.GetInstallmentsForSale(sale.RowId);
            double paidSoFar = installments.Where(i => i.IsPaid).Sum(i => i.PaidAmount);
            double remaining = sale.SaleBalance - paidSoFar;
            int unpaidCount = installments.Count(i => !i.IsPaid);
            SummaryText = $"{sale.SaleCustomer} — Chassis {sale.SaleChassis}\nRemaining balance: {remaining:N2} PKR across {unpaidCount} unpaid installment(s)";
            Months = unpaidCount > 0 ? unpaidCount : sale.InstallmentMonths;
            ReminderDaysBefore = sale.ReminderDaysBefore;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke(false));
        }

        private void Save()
        {
            if (Months <= 0) { MessageBox.Show("Enter the new number of months.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            DatabaseService.Instance.ReplanInstallments(_sale.RowId, Months, ReminderDaysBefore);
            CloseAction?.Invoke(true);
        }
    }
}
