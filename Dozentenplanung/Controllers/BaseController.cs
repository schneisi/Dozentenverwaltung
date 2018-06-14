using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dozentenplanung.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string UserId;
        protected ApplicationDbContext DatabaseContext;
        protected UserManager<ApplicationUser> UserManager;
        protected SignInManager<ApplicationUser> SignInManager;

        public BaseController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager, IHttpContextAccessor httpContextAccessor)
        {
            this.DatabaseContext = aContext;
            this.UserManager = aUserManager;
            this.SignInManager = aSignInManager;
            var user = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (user == null) {
                this.UserId = null;
            } else {
                this.UserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
        }

        protected async Task<ApplicationUser> GetCurrentUser() {
            //Answer the current user
            return await this.GetUserForId(this.UserId);
        }

        protected void SaveDatabaseContext() {
            //Save changes into the database
            this.DatabaseContext.SaveChanges();
        }

        protected Course CourseForId(int anId)
        {
            //Answer the course for the given id
            return this.DatabaseContext.CourseForId(anId);
        }

        protected async Task<ApplicationUser> GetUserForId(string anId) {
            //Answer the user for the given id
            return await this.UserManager.FindByIdAsync(anId);
        }


        protected void PutRolesInViewBag() {
            //Store the role booleans in the ViewBag
            ViewBag.CanWrite = this.CurrentUserCanWrite();
            ViewBag.IsAdministrator = this.CurrentUserIsAdministrator();
        }


        protected bool CurrentUserCanWrite()
        {
            //Answer whether the current user has write role
            ApplicationUser user = this.GetCurrentUser().Result;
            if (user != null) {
                return this.GetCurrentUser().Result.CanWrite;    
            } else {
                return false;
            }

        }
        protected bool CurrentUserIsAdministrator()
        {
            //Answer whether the current user is an admin
            ApplicationUser user = this.GetCurrentUser().Result;
            if (user != null)
            {
                return this.GetCurrentUser().Result.IsAdministrator;
            }
            else
            {
                return false;
            }
        }
    }
}
