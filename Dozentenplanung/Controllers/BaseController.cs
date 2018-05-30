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
            return await this.GetUserForId(this.UserId);
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

        protected void SendMail(bool useHtml, string receiver, string subject, string content) {
            MailHelper theMailHelper = new MailHelper
            {
                Subject = subject,
                Content = content,
                RecipientAddress = receiver,
                isHtmlMail = useHtml
            };
            theMailHelper.Send();
        }
    }
}
