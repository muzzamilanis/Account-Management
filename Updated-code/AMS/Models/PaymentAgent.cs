using System;

namespace AMS.Models
{
    public class PaymentAgent
    {
        public long RowId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Today;
        public double PaymentAmount { get; set; }
        public string PaymentDetail { get; set; }
        public string PaidFrom { get; set; }
        public string PaidTo { get; set; }
    }
}
