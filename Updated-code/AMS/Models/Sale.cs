using System;

namespace AMS.Models
{
    public class Sale
    {
        public long RowId { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Today;
        public string SaleChassis { get; set; }
        public string SaleCustomer { get; set; }
        public double SalePrice { get; set; }
        public double SaleAmountReceived { get; set; }
        public string PaymentReceivedIn { get; set; }
        public double SaleBalance => SalePrice - SaleAmountReceived;

        // Credit terms: 0 = cash sale, no due-date tracking.
        public int CreditDays { get; set; }
        public int ReminderDaysBefore { get; set; }

        public DateTime? DueDate => CreditDays > 0 ? SaleDate.Date.AddDays(CreditDays) : (DateTime?)null;
        public int? DaysUntilDue => DueDate.HasValue ? (int?)(DueDate.Value - DateTime.Today).Days : null;

        public bool HasActiveReminder =>
            CreditDays > 0 && SaleBalance > 0 && DaysUntilDue.HasValue && DaysUntilDue.Value <= ReminderDaysBefore;

        public string ReminderText
        {
            get
            {
                if (!DaysUntilDue.HasValue) return "";
                string when = DaysUntilDue.Value < 0
                    ? $"OVERDUE by {-DaysUntilDue.Value} day{(-DaysUntilDue.Value == 1 ? "" : "s")}"
                    : DaysUntilDue.Value == 0
                        ? "due today"
                        : $"due in {DaysUntilDue.Value} day{(DaysUntilDue.Value == 1 ? "" : "s")}";
                return $"{SaleCustomer} — Chassis {SaleChassis} — {when} ({DueDate:dd MMM yyyy})";
            }
        }
    }
}
