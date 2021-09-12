using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DynamicMenuProject.Models
{
    public partial class Countries
    {
        [Key]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
