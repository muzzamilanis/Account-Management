using System;

namespace AMS.Models
{
    public class Customer
    {
        public long RowId { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public string Title { get; set; } = "Mr.";
        public string Name { get; set; }
        public string CNIC { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double PaymentReceived { get; set; }
        public double PaymentReceivable { get; set; }
        public double PaymentPaid { get; set; }
        public string FullName => $"{Title} {Name}";
    }
}
