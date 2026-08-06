using System;

namespace AMS.Models
{
    public class Stock
    {
        public long RowId { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public string Chassis { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public double PriceYen { get; set; }
        public double Rate { get; set; }
        public double PricePkr { get; set; }
        // Displayed as "Clearance" in the UI — kept as Duty internally to avoid a schema rename.
        public double Duty { get; set; }
        public double MiscExpense { get; set; }
        public double Demurrage { get; set; }
        public double NoPlate { get; set; }
        public double Commission { get; set; }
        public double Tax { get; set; }
        public double Cost { get; set; }
        public string Status { get; set; } = "InStock";
        public double PaidYen { get; set; }
        public double PaidAmount { get; set; }
        public string Comments { get; set; }
        public bool IsSold => Status == "Sold";
        public double BalanceYen => PriceYen - PaidYen;
    }
}
