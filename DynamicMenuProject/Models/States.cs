using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DynamicMenuProject.Models
{
    public partial class States
    {
        [Key]
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }
    }
}
