using System;

namespace AMS.Models
{
    public class Account
    {
        public long RowId { get; set; }
        public DateTime AccountDate { get; set; } = DateTime.Today;
        public string AccountType { get; set; } = "Cash";
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountTitle { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public double OpeningBalance { get; set; }
        public double CurrentBalance { get; set; }
        public string DisplayName => AccountName;
    }
}
