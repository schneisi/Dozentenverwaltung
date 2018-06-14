using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dozentenplanung.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        public UserController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager, IHttpContextAccessor httpContextAccessor) : base(aContext, aUserManager, aSignInManager, httpContextAccessor)
        {}

        public IActionResult Index()
        {
            this.PutRolesInViewBag();
            return View(this.UserManager.Users.ToList());
        }

        public async Task<IActionResult> applicationUser(string id) {
            this.PutRolesInViewBag();
            ApplicationUser user = await this.UserManager.FindByIdAsync(id);
            return View(user);
        }

        public IActionResult CreateUserView() {
            if (this.CurrentUserIsAdministrator()) {
                return View("Create");
            }
            return RedirectToUsers();
        }

        public async Task<IActionResult> Create(string mail, string password) {
            var result = await this.UserManager.CreateAsync(new ApplicationUser
            {
                UserName = mail,
                Email = mail
            }, password);
            if (result.Succeeded)
            {
                return this.RedirectToUsers();
            }
            else
            {
                return Content("Creation failed:" + result.ToString(), "text/html");
            }
        }

        public async Task<IActionResult> Edit(string id) {
            ApplicationUser user = await this.GetUserForId(id);
            return View(user);
        }

        public async Task<IActionResult> Save(string id, string password, bool IsAdministrator, bool CanWrite) {
            ApplicationUser user = await this.GetUserForId(id);
            if (!string.IsNullOrEmpty(password)) {
                //Do not change password if there is no new password
                string token = await this.UserManager.GeneratePasswordResetTokenAsync(user);
                var result = await this.UserManager.ResetPasswordAsync(user, token, password);
                if (!result.Succeeded) {
                    return Content("Action failed: " + result.Errors.ElementAt(0), "text/html");
                }
            }

            user.CanWrite = CanWrite;
            user.SetAdministratorBooleanInContext(IsAdministrator, this.DatabaseContext);
            this.DatabaseContext.SaveChanges();
            return this.RedirectToUsers();
        }

        public async Task<IActionResult> Delete(string id) {
            ApplicationUser user = await this.GetUserForId(id);
            await user.DeleteInMangerAndContext(this.UserManager, this.DatabaseContext);
            return RedirectToUsers();
        }

        public IActionResult RedirectToUsers() {
            //Redirect the user 
            return RedirectToAction("Index");
        }
    }
}