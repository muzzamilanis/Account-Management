using System;

namespace AMS.Models
{
    public class MiscExp
    {
        public long RowId { get; set; }
        public string Chassis { get; set; }
        public DateTime MiscExpDate { get; set; } = DateTime.Today;
        public double MiscExpAmount { get; set; }
        public string MiscExpDetail { get; set; }
        public string MiscExpPaidBy { get; set; }
    }
}
