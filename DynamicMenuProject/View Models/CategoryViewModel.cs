using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.View_Models
{
    public class CategoryViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [Remote("IsCategoryNameExist", "Categories", AdditionalFields = "Id",
                ErrorMessage = "Category name already exists.")]
        public string CategoryName { get; set; }

        public string Path { get; set; }

        [Required(ErrorMessage = "Parent Id is required.")]
        public int ParentId { get; set; }
    }
}
