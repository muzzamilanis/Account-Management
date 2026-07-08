namespace AMS.Models
{
    public class CompanySettings
    {
        public string CompanyName { get; set; } = "My Auto Company";
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyTagline { get; set; } = "Auto Dealership Management System";
        public double DefaultExchangeRate { get; set; } = 1.0;
        public string DatabasePassword { get; set; } = "karachi123";
        public string LoginPassword { get; set; } = "karachi123";
        public string LastDatabasePath { get; set; }
        public bool EnableCreditSales { get; set; } = true;
    }
}
