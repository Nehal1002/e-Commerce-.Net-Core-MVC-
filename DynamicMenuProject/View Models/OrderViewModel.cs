using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.View_Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public Decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public Guid UniqueId { get; set; }
        public int OrderDetailsId { get; set; }
        public List<OrderDetailsViewModel> orderDetailsViewModel { get; set; }
    }
    public class OrderDetailsViewModel
    {
    public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
