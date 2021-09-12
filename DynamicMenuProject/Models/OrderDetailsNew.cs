using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Models
{
    public class OrderDetailsNew
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int ProductId { get; set; }
        public Decimal Price { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
    }
}
