using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Dozentenplanung.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        public ProfileController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager, IHttpContextAccessor httpContextAccessor) : base(aContext, aUserManager, aSignInManager, httpContextAccessor)
        {}

        public IActionResult Index()
        {
            ApplicationUser theUser = this.GetCurrentUser().Result;
            return View(theUser);
        }

        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword) {
            ApplicationUser user = this.GetCurrentUser().Result;
            var result = await this.UserManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return RedirectToAction("index");
        }
    }
}
