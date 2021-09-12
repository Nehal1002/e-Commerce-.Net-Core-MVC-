using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Models
{
    public class CartNew
    {
        [Key]
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
