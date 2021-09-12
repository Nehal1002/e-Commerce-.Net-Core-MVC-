using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Models
{
    public class State
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual List<City> City { get; set; }
    }
}
