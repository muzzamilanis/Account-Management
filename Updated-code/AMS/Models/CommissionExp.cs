using System;

namespace AMS.Models
{
    public class CommissionExp
    {
        public long RowId { get; set; }
        public string Chassis { get; set; }
        public DateTime CommissionExpDate { get; set; } = DateTime.Today;
        public double CommissionExpAmount { get; set; }
        public string CommissionExpDetail { get; set; }
        public string CommissionExpPaidBy { get; set; }
    }
}
