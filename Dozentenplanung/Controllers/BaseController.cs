using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dozentenplanung.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    public abstract class BaseController : Controller
    {
        protected ApplicationDbContext DatabaseContext;
        protected UserManager<ApplicationUser> UserManager;
        protected SignInManager<ApplicationUser> SignInManager;

        public BaseController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager)
        {
            this.DatabaseContext = aContext;
            this.UserManager = aUserManager;
            this.SignInManager = aSignInManager;
        }


        protected void SaveDatabaseContext() {
            this.DatabaseContext.SaveChanges();
        }

        protected Course CourseForId(int anIdInt)
        {
            return this.DatabaseContext.CourseForId(anIdInt);
        }

        protected async Task<ApplicationUser> GetUserForId(string id) {
            return await this.UserManager.FindByIdAsync(id);
        }
    }
}
