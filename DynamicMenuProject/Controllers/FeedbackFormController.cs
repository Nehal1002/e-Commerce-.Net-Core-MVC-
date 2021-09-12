using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DynamicMenuProject.Data;
using DynamicMenuProject.Models;
using DynamicMenuProject.View_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

namespace DynamicMenuProject.Controllers
{
    public class FeedbackFormController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackFormController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

         [HttpGet]
        public IActionResult AddQuestions()
        {
            return View();
        }


        [HttpPost]

        public IActionResult AddQuestions(QuestionsViewModel model)
        {

            if (model != null)
            {
                var questionModel = new Questions();
                questionModel.Title = model.Title;
                questionModel.Type = model.Type;

                _context.Questions.Add(questionModel);
                _context.SaveChanges();

                return RedirectToAction("ViewQuestions");
            }
            return View();

        }




        public IActionResult ViewQuestions()
        {
            List<QuestionsViewModel> lstmodel = new List<QuestionsViewModel>();
            var models = _context.Questions;

            foreach (var item in models)
            {
                QuestionsViewModel qvmObj = new QuestionsViewModel();
                qvmObj.Id = item.Id;
                qvmObj.Title = item.Title;
                qvmObj.Type = item.Type;

                lstmodel.Add(qvmObj);
            }

            ViewBag.successMessage = TempData["successMessage"];
            return View(lstmodel);
        }

        [HttpGet]
        public IActionResult AddOptions(int Id)
        {
            AddOptionWithList addOption = new AddOptionWithList();
            var optionList = (from opt in _context.RelatedValues
                              join ques in _context.Questions
                              on opt.QuestionId equals Id
                              select new AddOption
                              {
                                  Id = opt.Id,
                                  Option = opt.Values,

                              }).GroupBy(n => new { n.Id, n.Option }).Select(g => g.FirstOrDefault()).ToList();

            addOption.lstAddOption = optionList;

            return View(addOption);

        }


        [HttpPost]
        public IActionResult AddOptions(AddOptionWithList model)
        {

            if (model != null)
            {
                RelatedValues obj = new RelatedValues();
                obj.QuestionId = model.Id;
                obj.Values = model.Option;

                _context.RelatedValues.Add(obj);
                _context.SaveChanges();

                return RedirectToAction("ViewQuestions");
            }

            return View();

        }



        public IActionResult Survey()
        {
            List<QuestionsRelatedValueViewModel> questionList = new List<QuestionsRelatedValueViewModel>();



            var lstquestion = (from t in _context.Questions
                               select new Questions
                               {
                                   Id = t.Id,
                                   Title = t.Title,
                                   Type = t.Type
                               }).ToList();

            foreach (var item in lstquestion)
            {
                QuestionsRelatedValueViewModel questions = new QuestionsRelatedValueViewModel();
                questions.Id = item.Id;
                questions.Title = item.Title;
                questions.Type = item.Type;

                if (item.Type == TitleType.RadioButton.ToString() || item.Type == TitleType.CheckBox.ToString() || item.Type == TitleType.DropDown.ToString())
                {

                    questions.SelectList = (from que in _context.RelatedValues.Where(r => r.QuestionId == item.Id)
                                            select new SelectListItem
                                            {
                                                Value = que.Id.ToString(),
                                                Text = que.Values
                                            }).ToList();
                }

                questionList.Add(questions);

            }

            return View(questionList);

        }

        [HttpGet]
        public IActionResult SurveyPost()
        {
            return View();

        }

        [HttpPost]
        public JsonResult SurveyPost([FromBody] FeedbackFormViewModel[] survey)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var item in survey)
                {
                FeedbackResult itemList = new FeedbackResult();
                    
                    itemList.UserId = userId;
                    itemList.SubmitDate = DateTime.Now;
                  
                    itemList.QuestionId = item.QuestionId;
                    if (item.Type == TitleType.TextBox.ToString()
                        || item.Type == TitleType.TextArea.ToString())
                    {
                        itemList.Answer = item.Answer;
                    }
                    else
                    {
                        itemList.OptionId = item.Answer;
                    }

                _context.FeedbackResult.Add(itemList);
                    _context.SaveChanges();
                }

            return Json("You have posted the feedback successfully");



        }


        //public IActionResult ManageReports()
        //{
        //    var userNames = (from users in _context.Users
        //                     join
        //                     f in _context.FeedbackForm
        //                     on users.Id equals Convert.ToString(f.UserId)
        //                     select new FeedbackFormViewModel
        //                     {
        //                         UserName = users.UserName,
        //                         UserId = users.Id,
        //                         SubmitDate = f.SubmitDate
        //                     }).GroupBy(n => new { n.UserId, n.UserName }).Select(g => g.FirstOrDefault()).ToList();

        //    return View(userNames);
        //}
        public IActionResult ManageReports()
        {
            var userNames = (from users in _context.Users
                             join
                             f in _context.FeedbackResult
                             on users.Id equals Convert.ToString(f.UserId)
                             select new FeedbackFormViewModel
                             {
                                 UserName = users.UserName,
                                 UserId = users.Id,
                                 SubmitDate = f.SubmitDate
                             }).GroupBy(n => new { n.UserId, n.UserName }).Select(g => g.FirstOrDefault()).ToList();

            return View(userNames);
        }

        //Static
        //public IActionResult ViewDetails(string Id)
        //{
        //    //var userNames = (from users in _context.Users where (users.Id == Id) select users.UserName).FirstOrDefault();
        //    //ViewBag.UserName = userNames;
        //    //var details = _context.FeedbackForm.Where(x => x.UserId == Id);
        //    var details = (from f in _context.FeedbackForm
        //                  join u in _context.Users on f.UserId equals u.Id
        //                  where (f.UserId == Id)
        //                  select new FeedbackFormViewModel
        //                  {
        //                      UserId = f.UserId,
        //                      UserName = u.UserName,
        //                      Question = f.Question,
        //                      Answer = f.Answer,
        //                      SubmitDate = f.SubmitDate
        //                  }).ToList();
        //    return View(details);
        //}
        public ActionResult ViewDetails(string Id)
        {
            
            var details = (from f in _context.FeedbackResult
                           join u in _context.Users on f.UserId equals u.Id
                           join q in _context.Questions on f.QuestionId equals q.Id
                          //join r in _context.RelatedValues on f.OptionId.Split(',').Select(Int32.Parse).Contains(r.Id) equals r.Id
                           where (f.UserId == Id)
                           select new FeedbackFormViewModel
                           {
                               UserId = Convert.ToString(f.UserId),
                               UserName = u.UserName,
                               QuestionId = f.QuestionId,
                               Answer = f.Answer,
                               SubmitDate = f.SubmitDate,
                               Question = q.Title,
                               //Options = optionsNew(f.OptionId != null ? f.OptionId : "").ToString()
                               OptionId=f.OptionId
                           }).ToList();

                            foreach (var items in details)
                            {
                                items.Options = optionsNew(items.OptionId);
                            }
      
            
            return View(details);
        }

        public ActionResult ViewDetailsNew()
        {
            var procedureData = _context.RelatedProcedure.FromSql("EXECUTE RelatedProcedure").ToList();
            
            return View(procedureData);
        }



        public async Task<string> options(string optionIds)
        {
            //p.Users.Split(',').Select(r=> r.Trim).Contains(username)
            //_roleService.GetAlRoles().Data.Where(x => x.Name.Split(',').Any(Roles.Contains))
            //.OrderBy(x => x.RoleLevel).FirstOrDefault();
            if (!string.IsNullOrEmpty(optionIds))
            {
                var items = optionIds.Split(',').Select(Int32.Parse).ToList();
                var result = await (from f in _context.RelatedValues
                              where items.Contains(f.Id)
                              select f.Values).ToListAsync();
                var resultNew = string.Join(",", result);
                return await Task.FromResult(resultNew);
                //return resultNew;
            }
            else
            {
                return null;
            }
            //var result = _context.RelatedValues.Where(items.Contains(id))
        }

        public string optionsNew(string optionIds)
        {
            
            if (!string.IsNullOrEmpty(optionIds))
            {
                var items = optionIds.Split(',').Select(Int32.Parse).ToList();
                var result = (from f in _context.RelatedValues
                                    where items.Contains(f.Id)
                                    select f.Values).ToList();
                var resultNew = string.Join(",", result);                
                return resultNew;
            }
            else
            {
                return "";
            }
        }

        public IActionResult ViewDetailsPdf(string Id)
        {
            //Static:
            //var userNames = (from users in _context.Users where (users.Id == Id) select users.UserName).FirstOrDefault();
            //ViewBag.UserName = userNames;
            //var details = _context.FeedbackForm.Where(x => x.UserId == Id);
            //var details = (from f in _context.FeedbackForm
            //               join u in _context.Users on f.UserId equals u.Id
            //               where (f.UserId == Id)
            //               select new FeedbackFormViewModel
            //               {
            //                   UserId = f.UserId,
            //                   UserName = u.UserName,
            //                   Question = f.Question,
            //                   Answer = f.Answer,
            //                   SubmitDate = f.SubmitDate
            //               }).ToList();


            var details = (from f in _context.FeedbackResult
                           join u in _context.Users on f.UserId equals u.Id
                           join q in _context.Questions on f.QuestionId equals q.Id 
                           where (f.UserId == Id)
                           select new FeedbackFormViewModel
                           {
                               UserId = Convert.ToString(f.UserId),
                               UserName = u.UserName,
                               QuestionId = f.QuestionId,
                               Answer = f.Answer,
                               SubmitDate = f.SubmitDate,
                               Question = q.Title,
                               OptionId = f.OptionId
                           }).ToList();

            foreach (var items in details)
            {
                items.Options = optionsNew(items.OptionId);
            }

            return new ViewAsPdf("ViewDetailsPdf", details);
        }
    }
}