using System;

namespace AMS.Models
{
    public class OfficeAccount
    {
        public long RowId { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public double Amount { get; set; }
        public string Detail { get; set; }
        public string CreditFrom { get; set; }
        public string DebitTo { get; set; }
        public long LedgerRowId { get; set; }
    }
}
