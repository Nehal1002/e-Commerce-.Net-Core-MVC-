using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.View_Models
{
    public class QuestionsRelatedValueViewModel
    {
        public DateTime SubmitDate { get; set; }
        //public QuestionsRelatedValueViewModel()
        //{
        //    SubmitDate = DateTime.Now;
        //}
        public int Id { get; set; }

        public string Title { get; set; }

        [Required]
        public string Type { get; set; }

        public int QuestionId { get; set; }
        public string Values { get; set; }

        public List<SelectListItem> SelectList { get; set; }
    }
}
