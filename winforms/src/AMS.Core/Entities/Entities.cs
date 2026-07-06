using System;

namespace AMS.Core.Entities
{
    public class Account
    {
        public long AccountId { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public double BalanceAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
    }

    public class Customer { }
    public class Agent { }
    public class Stock { }
    public class Sale { }
    public class OfficeExp { }
    public class Receipt { }
    public class Payment { }
    public class PaymentPkr { }
    public class PaymentAgent { }
    public class OfficeAccount { }
    public class MiscExp { }
    public class DutyExp { }
}
