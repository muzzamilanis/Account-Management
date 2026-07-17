using System;

namespace AMS.Models
{
    public class Installment
    {
        public long RowId { get; set; }
        public long SaleRowId { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime DueDate { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
        public double PaidAmount { get; set; }
        public string PaidInAccount { get; set; }

        // Populated alongside the row when joined for display/reporting — not persisted here.
        public string SaleCustomer { get; set; }
        public string SaleChassis { get; set; }
        public int TotalInstallments { get; set; }
        public int ReminderDaysBefore { get; set; }

        public int DaysUntilDue => (DueDate.Date - DateTime.Today).Days;

        public bool HasActiveReminder =>
            !IsPaid && DaysUntilDue <= ReminderDaysBefore;

        public string ReminderText
        {
            get
            {
                string when = DaysUntilDue < 0
                    ? $"OVERDUE by {-DaysUntilDue} day{(-DaysUntilDue == 1 ? "" : "s")}"
                    : DaysUntilDue == 0 ? "due today" : $"due in {DaysUntilDue} day{(DaysUntilDue == 1 ? "" : "s")}";
                return $"{SaleCustomer} — Chassis {SaleChassis} — Installment {InstallmentNumber}/{TotalInstallments} — {when} ({DueDate:dd MMM yyyy})";
            }
        }
    }
}
