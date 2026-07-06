using System;

namespace AMS.Models
{
    public class Agent
    {
        public long RowId { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public string Name { get; set; }
        public string CNIC { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double PaymentReceivable { get; set; }
        public double PaymentPaid { get; set; }
    }
}
