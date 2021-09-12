using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.Models
{
    public class RelatedProcedure
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Answer { get; set; }
        public int QuestionId { get; set; }
       
        public string OptionId { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string Values { get; set; }
    }
}
