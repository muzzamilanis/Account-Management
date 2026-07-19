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

        // Off by default so nothing changes until explicitly opted into (client relocated
        // Pakistan -> Uganda and needs USD as the reporting currency going forward, while old
        // PKR-denominated data stays untouched). See CurrencyLabel for how this is consumed.
        public bool EnableMultiCurrency { get; set; } = false;
        public string BaseCurrencyCode { get; set; } = "PKR";
        public string BaseCurrencySymbol { get; set; } = "PKR";

        // How many UGX equal 1 unit of the base currency — entered the way a client actually looks
        // it up (e.g. Google "USD to UGX" -> ~3,690), not the tiny inverse fraction. A UGX sale
        // amount is converted to base currency by dividing by this, not multiplying.
        public double UgxExchangeRate { get; set; } = 1.0;
    }
}
