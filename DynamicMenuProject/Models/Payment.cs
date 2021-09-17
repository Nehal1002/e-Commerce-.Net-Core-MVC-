using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string TransactionId { get; set; }
        public string Intent { get; set; }
        public string CartId { get; set; }
        public string State { get; set; }
        public string PaymentMethod { get; set; }
        public string Amount { get; set; }
        public string TransactionFee { get; set; }
        public string SaleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderId { get; set; }
    }
}
