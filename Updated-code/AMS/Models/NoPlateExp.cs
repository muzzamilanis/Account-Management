using System;

namespace AMS.Models
{
    public class NoPlateExp
    {
        public long RowId { get; set; }
        public string Chassis { get; set; }
        public DateTime NoPlateExpDate { get; set; } = DateTime.Today;
        public double NoPlateExpAmount { get; set; }
        public string NoPlateExpDetail { get; set; }
        public string NoPlateExpPaidBy { get; set; }
    }
}
