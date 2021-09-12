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
        public string CategoryName { get; set; }

        public string Path { get; set; }

        [Required(ErrorMessage = "Parent Id is required.")]
        public int ParentId { get; set; }

        [Required(ErrorMessage = "Category Level is required.")]
        public int CategoryLevel { get; set; }

        [Required(ErrorMessage = "Category Grp Id is required.")]
        public int CategoryGrpId { get; set; }
    }
}
