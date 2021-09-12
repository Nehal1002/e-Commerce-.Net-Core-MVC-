using DynamicMenuProject.Data;
using DynamicMenuProject.Models;
using DynamicMenuProject.View_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.ViewComponents
{
    public class NewsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public NewsViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var result = (from News in _context.News
                              //join Permissions in _context.MenuPermissions.Where(r => r.RoleId == RoleId)
                              //on Menus.Id equals Permissions.MenuId
                          select new NewsViewModel
                          {
                              Id = News.Id,
                              Title = News.Title,
                              Description = News.Description,
                          }).ToList();
            return View(result);
        }
    }
}
