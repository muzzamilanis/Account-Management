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
    }
}
