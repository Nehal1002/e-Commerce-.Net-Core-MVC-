using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DynamicMenuProject.Models;
using Microsoft.AspNetCore.Authorization;
using DynamicMenuProject.Data;
using Newtonsoft.Json;
using DynamicMenuProject.View_Models;
using Microsoft.AspNetCore.Identity;

namespace DynamicMenuProject.Controllers
{
    //[AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult News()
        {
            ViewBag.News = _context.News;
            return View();
        }

        
        [AllowAnonymous]
        public IActionResult Products(int Id)
        {
            var PId = _context.Categories.Where(y => y.ParentId == 0);
            if (Id == 0)
            {
                ViewBag.product = _context.ProductNew.Where(p=>p.isActive==true).ToList();
            }
            else
            {
                foreach (var item in PId)
                {
                    if (item.Id == Id)
                    {
                        ViewBag.product = (from c in _context.Categories.Where(c => c.ParentId == Id)
                                           join p in _context.ProductNew.Where(x=>x.isActive==true) on c.Id equals p.SubCategoryId
                                           select p).ToList();
                    }
                    else
                    {
                        ViewBag.product = _context.ProductNew.Where(x => x.SubCategoryId == Id && x.isActive==true).ToList();
                    }
                }
            }
            
            return View();
        }



        //public IActionResult ProductsNew(int Id)
        //{
        //    List<AddProductViewModel> lstmodel = new List<AddProductViewModel>();
        //    var PId = _context.Categories.Where(y => y.ParentId == 0);
        //    var p= _context.ProductNew.Where(x => x.SubCategoryId == Id).ToList();

        //    if (Id == 0)
        //    {
        //        ViewBag.product = _context.ProductNew.ToList();
        //    }
        //    else if (PId.FirstOrDefault().Id == Id)
        //    {
        //        ViewBag.product = (from c in _context.Categories.Where(c => c.ParentId == Id)
        //                           join p in _context.ProductNew on c.Id equals p.SubCategoryId
        //                           select p).ToList();
        //    }
        //    else
        //    {
        //        ViewBag.product = _context.ProductNew.Where(x => x.SubCategoryId == Id).ToList();
        //    }
        //    return View(p);
        //}

        


        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult Pages(string Id)
        {
            CMSItems mvm = new CMSItems();
            var page = (_context.CMSItems.Where(p => p.PageUrl == Id).ToList());
            foreach (var item in page)
            {
                mvm.PageName = item.PageName;
                mvm.Description = item.Description;
                mvm.BannerImage = item.BannerImage;
            }
            return View(mvm);
        }

        [AllowAnonymous]
        public JsonResult Country()
        {
            var coun = _context.Countries.
                Select(x => new { value = x.CountryId, text = x.CountryName }).ToList();
            return Json(coun);
        }


        [AllowAnonymous]
        public JsonResult GetStatesByCountryId(int CountryId)
        {
            var sta = _context.States.Where(x => x.CountryId == CountryId).
                Select(x => new { value = x.StateId, text = x.StateName }).ToList();
            return Json(sta);
        }
        
        [AllowAnonymous]
        public JsonResult GetCitiesByStateId(int StateId)
        {
            var city = _context.Cities.Where(x => x.StateId == StateId).
                Select(x => new { value = x.CityId, text = x.CityName }).ToList();
            return Json(city);
        }


        [AllowAnonymous]
        public JsonResult Category(int productId)
        {
            var sub = (from p in _context.Categories
                       where p.ParentId==0
                       select new { value = p.Id, text = p.CategoryName }).ToList();
   
            return Json(sub);
        }


        [AllowAnonymous]
        public JsonResult GetSubCategoryByCategory(int CategoryId)
        {
            var sta = _context.Categories.Where(x => x.ParentId == CategoryId).
                Select(x => new { value = x.Id, text = x.CategoryName }).ToList();
            return Json(sta);
        }












        public IActionResult TreeView()
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            //Loop and add the parent nodes.
            foreach(State type in _context.State)
            {
                nodes.Add(new TreeViewNode { id = type.Id.ToString(), parent = "#", text = type.Title });
            }
            //Loop and add the child nodes.
            foreach (City subType in _context.City)
            {
                nodes.Add(new TreeViewNode
                {
                    id = subType.StateId.ToString()+"-"+subType.Id.ToString(),parent=subType.StateId.ToString(),text=subType.Name
                });
            }
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }
        [HttpPost]
        public ActionResult TreeView(string selectedItems)
        {
            List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);
            return RedirectToAction("TreeView");
        }

        
    }
}
