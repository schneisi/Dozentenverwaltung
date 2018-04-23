using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dozentenplanung.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager) : base(aContext, aUserManager, aSignInManager)
        { }


        public async Task<IActionResult> CreateDevAccount()
        {
            var result = await this.UserManager.CreateAsync(new ApplicationUser
            {
                UserName = "dev",
                Email = "simsaschneider@icloud.com"
            }, "password");
            if (result.Succeeded){
                return RedirectToAction("Login", "account");
            } else {
                return Content("Creation failed", "text/html");
            }
        }


        [Route("account/login")]
        public IActionResult Login(string ReturnUrl) {
            ViewBag.redirectUrl = ReturnUrl;
            return View();
        }


        [Route("account/logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("login", "account");
        }


        [Route("account/loginUser")]
        public async Task<IActionResult> LoginUser(string ReturnUrl, string username, string password) {
            ApplicationUser theUser = await this.UserManager.FindByNameAsync(username);
            var result = await this.SignInManager.PasswordSignInAsync(theUser, password, true, false);
            if (result.Succeeded) {
                if (string.IsNullOrEmpty(ReturnUrl)) {
                    return RedirectToAction("Index", "Course");
                } else {
                    return Redirect(ReturnUrl);
                }
            } else {
                return RedirectToAction("login", "account");
            }
        }

        public IActionResult ResetPassword() {
            return View();
        }


        [Route("account/CreatePasswordToken")]
        public async Task<IActionResult> CreatePasswordToken(string email) {
            var theUser = await this.UserManager.FindByEmailAsync(email);
            if (theUser != null) {
                string theToken = await this.UserManager.GeneratePasswordResetTokenAsync(theUser);
                return Content("ResetToken: userId="  + theUser.Id + "&passwordToken="+ System.Web.HttpUtility.UrlEncode(theToken));
            } else {
                return Content("Fehler aufgetreten");
            }
        }

        public IActionResult SetNewPassword(string userId, string passwordToken) {
            ViewBag.passwordToken = passwordToken;
            ViewBag.userId = userId;
            return View();
        }

        public async Task<IActionResult> setNewPasswordWithToken (string userId, string password, string passwordToken) {
            ApplicationUser theUser = await this.GetUserForId(userId);
            var result = await this.UserManager.ResetPasswordAsync(theUser, passwordToken, password);
            if (result.Succeeded) {
                return RedirectToAction("index", "course");
            } else {
                return Content(result.Errors.ToString());
            }
        }
    }
}
