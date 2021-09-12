using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Models
{
    public class AddOption
    {
        public int Id { get; set; }


        public string Option { get; set; }
    }
    public class AddOptionWithList
    {
        public int Id { get; set; }

        [Required]
        public string Option { get; set; }

        public List<AddOption> lstAddOption { get; set; }
    }
}
