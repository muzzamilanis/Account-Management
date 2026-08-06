using System;

namespace AMS.Models
{
    public class TaxExp
    {
        public long RowId { get; set; }
        public string Chassis { get; set; }
        public DateTime TaxExpDate { get; set; } = DateTime.Today;
        public double TaxExpAmount { get; set; }
        public string TaxExpDetail { get; set; }
        public string TaxExpPaidBy { get; set; }
    }
}
