using System;

namespace AMS.Models
{
    public class Receipt
    {
        public long RowId { get; set; }
        public DateTime ReceiptDate { get; set; } = DateTime.Today;
        public double ReceiptAmount { get; set; }
        public string ReceiptDetail { get; set; }
        public string ReceivedIn { get; set; }
        public string ReceivedFrom { get; set; }
    }
}
