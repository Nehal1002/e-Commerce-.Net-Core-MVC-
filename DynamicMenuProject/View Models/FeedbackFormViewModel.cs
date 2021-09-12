using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.View_Models
{
    public class FeedbackFormViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime SubmitDate { get; set; }
        public int QuestionId { get; set; }
        public string OptionId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Type { get; set; }

        public string Options { get; set; }
    }
}
