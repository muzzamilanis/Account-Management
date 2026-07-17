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

        // Installment plan: 0 = cash sale, no schedule. When > 0, the remaining SaleBalance
        // at the time of sale is split evenly across this many monthly installments
        // (see DatabaseService.AddInstallmentPlan). The actual due dates/amounts/paid status
        // live in the Installment table, not here — this just records the plan's shape.
        public int InstallmentMonths { get; set; }
        public int ReminderDaysBefore { get; set; }
    }
}
