using System;

namespace AMS.Models
{
    public class DemurrageExp
    {
        public long RowId { get; set; }
        public string Chassis { get; set; }
        public DateTime DemurrageExpDate { get; set; } = DateTime.Today;
        public double DemurrageExpAmount { get; set; }
        public string DemurrageExpDetail { get; set; }
        public string DemurrageExpPaidBy { get; set; }
    }
}
