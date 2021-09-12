using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Path { get; set; }
        public int ParentId { get; set; }
        public int CategoryLevel { get; set; }
        public int CategoryGrpId { get; set; }
    }
}
