using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DynamicMenuProject.Areas.Identity.Pages.Account;
using DynamicMenuProject.Helpers;
using DynamicMenuProject.Models;
using DynamicMenuProject.View_Models;
//using FinalProjectSecond.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicMenuProject.Controllers
{
    [AuthorizeActionFilter]
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
                                    IHostingEnvironment hostEnvironment)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Email = user.Email;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(RegisterModel.InputModel model)
        {
            //if (ModelState.IsValid)
            //{
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ProfilePicture = model.ProfilePicture,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    CountryId = model.CountryId,
                    StateId = model.StateId,
                    CityId = model.CityId
                };
                if (model.ProfilePictureFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(model.ProfilePictureFile.FileName);
                    string extension = Path.GetExtension(model.ProfilePictureFile.FileName);
                    user.ProfilePicture = DateTime.Now.ToString("yymmssfff") + extension;


                    string path = Path.Combine(wwwRootPath, "Upload", user.ProfilePicture);
                    //var mem = new MemoryStream();
                    //user.ProfilePicture
                    var fileStream = new FileStream(path, FileMode.Create);
                    model.ProfilePictureFile.CopyTo(fileStream);
                }
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    return RedirectToAction("Index", "UserRoles");
                }
                return View(model);
            //}
            //else
            //    return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            //var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new EditUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                CountryId = user.CountryId,
                StateId = user.StateId,
                CityId = user.CityId,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                Address1 = user.Address1,
                Address2 = user.Address2,
                UserName = user.Email,
                Roles = userRoles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.CountryId = model.CountryId;
                user.StateId = model.StateId;
                user.CityId = model.CityId;
                user.Address1 = model.Address1;
                user.Address2 = model.Address2;
                //user.ProfilePicture = model.ProfilePicture;
                //user.UserName = model.UserName;
                var profilePicture = user.ProfilePicture;
                if (model.ProfilePictureFile != null)
                {
                    if (model.ProfilePicture != profilePicture)
                    {
                        var paths = Path.Combine(_hostEnvironment.WebRootPath, "Upload", profilePicture);

                        if (System.IO.File.Exists(paths))
                        {
                            // If file found, delete it    
                            System.IO.File.Delete(Path.Combine(paths));
                        }
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(model.ProfilePictureFile.FileName);
                        string extension = Path.GetExtension(model.ProfilePictureFile.FileName);
                        user.ProfilePicture = DateTime.Now.ToString("yymmssfff") + extension;


                        string path = Path.Combine(wwwRootPath, "Upload", user.ProfilePicture);
                        var fileStream = new FileStream(path, FileMode.Create);
                        model.ProfilePictureFile.CopyTo(fileStream);
                        await _userManager.UpdateAsync(user);

                    }
                    else if (profilePicture == null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(model.ProfilePictureFile.FileName);
                        string extension = Path.GetExtension(model.ProfilePictureFile.FileName);
                        user.ProfilePicture = DateTime.Now.ToString("yymmssfff") + extension;


                        string path = Path.Combine(wwwRootPath, "Upload", user.ProfilePicture);
                        var fileStream = new FileStream(path, FileMode.Create);
                        model.ProfilePictureFile.CopyTo(fileStream);
                        await _userManager.UpdateAsync(user);
                    }
                }
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var manageuserRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    manageuserRolesViewModel.IsSelected = true;
                }
                else
                {
                    manageuserRolesViewModel.IsSelected = false;
                }
                model.Add(manageuserRolesViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles.");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index", new { Id = userId });
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
}