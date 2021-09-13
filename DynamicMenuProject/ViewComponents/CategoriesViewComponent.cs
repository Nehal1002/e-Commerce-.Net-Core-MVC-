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
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoriesViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            //var userId = userManager.GetUserId(HttpContext.User);
            //var Role = _context.UserRoles.FirstOrDefault(u => u.UserId == userId);
            //if (Role != null)
            //{
            //  var RoleId = Guid.Parse(Role.RoleId);

            var result = (from Categories in _context.Categories
                              //join Permissions in _context.MenuPermissions.Where(r => r.RoleId == RoleId)
                              //on Menus.Id equals Permissions.MenuId
                          select new CategoryViewModel
                          {
                              Id = Categories.Id,
                              CategoryName = Categories.CategoryName,
                              Path = Categories.Path,
                              ParentId = Categories.ParentId,
                          }).ToList();
                return View(result);
                // TODO: Fill menu with data here
            //}

            //List<PermissionViewModel> listpmv = new List<PermissionViewModel>();
            //return View();
        }
    }
}
