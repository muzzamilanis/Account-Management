using System;

namespace AMS.Models
{
    public class OfficeExp
    {
        public long RowId { get; set; }
        public DateTime OfficeExpDate { get; set; } = DateTime.Today;
        public double OfficeExpAmount { get; set; }
        public string OfficeExpDetail { get; set; }
        public string OfficeExpPaidBy { get; set; }
    }
}
