using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicMenuProject.Data;
using DynamicMenuProject.Helpers;
using DynamicMenuProject.Models;
using DynamicMenuProject.View_Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicMenuProject.Controllers
{
    [AuthorizeActionFilter]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context; 
        }
        public IActionResult Index()
        {
            List<NewsViewModel> lstmodel = new List<NewsViewModel>();
            var models = _context.News;

            foreach (var item in models)
            {
                NewsViewModel mvm = new NewsViewModel();
                mvm.Id = item.Id;
                mvm.Title = item.Title;
                mvm.Description = item.Description;
                mvm.CreatedAt = item.CreatedAt;
                mvm.ModifiedAt = item.ModifiedAt;
                lstmodel.Add(mvm);
            }

            return View(lstmodel);
        }

        [HttpGet]
        public IActionResult CreateNews()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateNews(NewsViewModel model)
        {
            if (model != null)
            {
                var newsModel = new News();
                newsModel.Id = model.Id;
                newsModel.Title = model.Title;
                newsModel.Description = model.Description;
                newsModel.CreatedAt = DateTime.Now;
                newsModel.ModifiedAt = DateTime.Now;
                _context.News.Add(newsModel);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditNews(int Id)
        {
            NewsViewModel mvm = new NewsViewModel();
            var news = _context.News.FirstOrDefault(m => m.Id == Id);

            if (news != null)
            {
                mvm.Title = news.Title;
                mvm.Description = news.Description;
            }
            return View(mvm);
        }

        [HttpPost]
        public IActionResult EditNews(NewsViewModel model)
        {
            if (model != null)
            {
                var newsItem = _context.News.FirstOrDefault(m => m.Id == model.Id);
                if (newsItem != null)
                {
                    newsItem.Title = model.Title;
                    newsItem.Description = model.Description;
                    newsItem.ModifiedAt = DateTime.Now;

                    
                    _context.Entry(newsItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View();
            }
            return View();
        }

        
        public IActionResult DeleteNews(int Id)
        {
            var news = _context.News.FirstOrDefault(m => m.Id == Id);
            _context.News.Remove(news);
            _context.SaveChanges();

            TempData["successMessage"] = "News Deleted Successfully.";
            TempData.Keep();

            return RedirectToAction("Index");

        }
    }
}