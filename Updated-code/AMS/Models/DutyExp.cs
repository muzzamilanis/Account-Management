using System;

namespace AMS.Models
{
    public class DutyExp
    {
        public long RowId { get; set; }
        public string Chassis { get; set; }
        public DateTime DutyExpDate { get; set; } = DateTime.Today;
        public double DutyExpAmount { get; set; }
        public string DutyExpDetail { get; set; }
        public string DutyExpPaidBy { get; set; }
        public string DutyExpAgent { get; set; }
    }
}
