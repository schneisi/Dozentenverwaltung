using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dozentenplanung.Models;
using Microsoft.AspNetCore.Identity;

namespace Dozentenplanung.Controllers
{
    public class DevController : BaseController
    {
        public DevController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager) : base(aContext, aUserManager, aSignInManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateTestData() {
            LecturerBuilder theLecturerBuilder = new LecturerBuilder(this.DatabaseContext);
            theLecturerBuilder.Firstname = "Max";
            theLecturerBuilder.Lastname = "Muster";
            theLecturerBuilder.Mail = "muster@dhbw-loerrach.de";
            theLecturerBuilder.Notes = "Sehr guter Informatiker";
            theLecturerBuilder.Save();

            return Redirect();
        }

        public IActionResult DropDatabase() {
            this.DatabaseContext.Delete();
            return this.Redirect();
        }

        public IActionResult CreateDatabase() {
            this.DatabaseContext.EnsureCreated();
            return this.Redirect();
        }

        private IActionResult Redirect() {
            return RedirectToAction("Index", "dev");
        }
    }
}