using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dozentenplanung.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        public UserController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager) : base(aContext, aUserManager, aSignInManager)
        {}

        public IActionResult Index()
        {
            return View(this.UserManager.Users.ToList());
        }

        public async Task<IActionResult> applicationUser(string id) {
            ApplicationUser theUser = await this.UserManager.FindByIdAsync(id);
            return View(theUser);
        }
    }
}