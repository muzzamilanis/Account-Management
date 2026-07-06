using System;

namespace AMS.Models
{
    public class Payment
    {
        public long RowId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Today;
        public double PaymentAmountYen { get; set; }
        public double PaymentExcRate { get; set; }
        public double PaymentAmountPkr { get; set; }
        public string PaymentDetail { get; set; }
        public string PaidFrom { get; set; }
    }
}
