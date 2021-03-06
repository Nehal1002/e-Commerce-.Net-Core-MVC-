using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Models
{
    public class ProductNew
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        [Required(ErrorMessage = "Sub Category is required.")]
        public int SubCategoryId { get; set; }

        public string UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
