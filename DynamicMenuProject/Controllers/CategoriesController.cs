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
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetCategories()
        {
            List<CategoryViewModel> lstmodel = new List<CategoryViewModel>();
            var models = _context.Categories;//s.Where(m=>m.MenuGrpId!=0).OrderBy(m=>m.MenuGrpId);

            foreach (var item in models)
            {
                CategoryViewModel mvm = new CategoryViewModel();
                mvm.Id = item.Id;
                mvm.CategoryName = item.CategoryName;
                mvm.Path = item.Path;
                mvm.ParentId = item.ParentId;
                mvm.CategoryLevel = item.CategoryLevel;
                mvm.CategoryGrpId = item.CategoryGrpId;

                lstmodel.Add(mvm);
            }

            ViewBag.successMessage = TempData["successMessage"];
            return View(lstmodel);
        }


        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryViewModel model)
        {
            if (model != null)
            {
                var categoryModel = new Categories();
                categoryModel.CategoryName = model.CategoryName;
                categoryModel.Path = model.Path;
                categoryModel.ParentId = model.ParentId;
                categoryModel.CategoryLevel = model.CategoryLevel;
                categoryModel.CategoryGrpId = model.CategoryGrpId;

                _context.Categories.Add(categoryModel);
                _context.SaveChanges();

                return RedirectToAction("GetCategories");
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditCategory(int Id)
        {
            CategoryViewModel mvm = new CategoryViewModel();
            var category = _context.Categories.FirstOrDefault(m => m.Id == Id);

            if (category != null)
            {
                mvm.Id = category.Id;
                mvm.CategoryName = category.CategoryName;
                mvm.Path = category.Path;
                mvm.ParentId = category.ParentId;
                mvm.CategoryLevel = category.CategoryLevel;
                mvm.CategoryGrpId = category.CategoryGrpId;
            }
            return View(mvm);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryViewModel model)
        {
            if (model != null)
            {
                var category = _context.Categories.FirstOrDefault(m => m.Id == model.Id);
                if (category != null)
                {
                    category.CategoryName = model.CategoryName;
                    category.Path = model.Path;
                    category.ParentId = model.ParentId;
                    category.CategoryLevel = model.CategoryLevel;
                    category.CategoryGrpId = model.CategoryGrpId;

                    _context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    return RedirectToAction("GetCategories");
                }
                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult DeleteCategory(int Id)
        {
            var category = _context.Categories.FirstOrDefault(m => m.Id == Id);
            _context.Categories.Remove(category);
            _context.SaveChanges();

            TempData["successMessage"] = "Category Deleted Successfully.";
            TempData.Keep();

            return RedirectToAction("GetCategories");

        }
    }
}
