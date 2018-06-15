using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dozentenplanung.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager, IHttpContextAccessor httpContextAccessor) : base(aContext, aUserManager, aSignInManager, httpContextAccessor)
        { }


        public async Task<IActionResult> CreateAdminAccount(string mail, string password)
        {
            //Checks again in case User creates request manually -> security reason
            if (this.CanCreateAdminAccount()) {
                var result = await this.UserManager.CreateAsync(new ApplicationUser
                {
                    UserName = mail,
                    Email = mail,
                    IsAdministrator = true,
                    CanWrite = true
                }, password);
                if (result.Succeeded)
                {
                    await this.SignInManager.PasswordSignInAsync(mail, password, true, false);
                    return RedirectToAction("index", "course");
                }
                else
                {
                    return Content("Creation failed", "text/html");
                }
            } else {
                return RedirectToAction("Login", "account");
            }

        }


        [Route("account/login")]
        public IActionResult Login(string ReturnUrl) {
            if (this.CanCreateAdminAccount())
            {
                return RedirectToAction("register");
            }
            else
            {
                ViewBag.redirectUrl = ReturnUrl;
                return View();
            }

        }


        [Route("account/logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("login", "account");
        }


        [Route("account/loginUser")]
        public async Task<IActionResult> LoginUser(string ReturnUrl, string username, string password) {
            bool theSuccessBoolean = false;
            if (!string.IsNullOrEmpty(username)) {
                ApplicationUser user = await this.UserManager.FindByNameAsync(username);
                if (user != null)
                {
                    var result = await this.SignInManager.PasswordSignInAsync(user, password, true, false);
                    theSuccessBoolean = result.Succeeded;
                }
                if (theSuccessBoolean)
                {
                    if (string.IsNullOrEmpty(ReturnUrl))
                    {
                        return RedirectToAction("Index", "Course");
                    }
                    else
                    {
                        return Redirect(ReturnUrl);
                    }
                }
            }
            return RedirectToAction("loginFailed", "account");
        }

        [Route("account/register")]
        public IActionResult Register()
        {
            if (this.CanCreateAdminAccount()) {
                return View();
            } else {
                return RedirectToAction("login");
            }
        }

        [Route("account/loginFailed")]
        public IActionResult LoginFailed()
        {
            return View();
        }

        private bool CanCreateAdminAccount() {
            //Answer true if there is no user
            return this.UserManager.Users.Count() == 0;
        }
    }
}
