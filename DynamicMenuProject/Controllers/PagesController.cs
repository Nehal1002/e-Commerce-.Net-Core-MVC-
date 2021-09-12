using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicMenuProject.Data;
using DynamicMenuProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicMenuProject.Controllers
{
    [AllowAnonymous]
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagesController(ApplicationDbContext context)
        {
            _context = context;
        }
        //public IActionResult Index(string Id)
        //{
        //    List<CMSItems> mvm = new List<CMSItems>();
        //    if(Id!=null)
        //    { 
        //    var page = (_context.CMSItems.Where(p => p.PageUrl == Id).ToList());
        //    foreach (var item in page)
        //    {
        //        mvm.PageName = item.PageName;
        //        mvm.Description = item.Description;
        //        mvm.BannerImage = item.BannerImage;
        //    }
        //    return View(mvm);
        //}
            public IActionResult Index(string Id)
            {
                List<CMSItems> mvm = new List<CMSItems>();
                if (Id != null)
                {

                    var page = _context.CMSItems.Where(p => p.PageUrl == Id).ToList();

                    return View(page);
                }
                else
                {
                    mvm = (_context.CMSItems).ToList();
                }
                return View(mvm);
            }
        }
}