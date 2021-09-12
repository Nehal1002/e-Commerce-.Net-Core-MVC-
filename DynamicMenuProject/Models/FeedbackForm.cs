using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Models
{
    public class FeedbackForm
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime SubmitDate { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
