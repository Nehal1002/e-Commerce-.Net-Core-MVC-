using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public string CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
