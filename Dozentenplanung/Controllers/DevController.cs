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
            SkillBuilder theSkillBuilder = new SkillBuilder(this.DatabaseContext);
            theSkillBuilder.Title = "Java Programmierung";
            theSkillBuilder.Description = "Programmierung in Java";
            theSkillBuilder.Save(false);

            LecturerBuilder theLecturerBuilder = new LecturerBuilder(this.DatabaseContext);
            theLecturerBuilder.Firstname = "Max";
            theLecturerBuilder.Lastname = "Muster";
            theLecturerBuilder.Mail = "muster@dhbw-loerrach.de";
            theLecturerBuilder.Notes = "Sehr guter Informatiker";
            theLecturerBuilder.AddSkill(theSkillBuilder.Skill());
            theLecturerBuilder.Save(false);

            CourseBuilder theCourseBuilder = new CourseBuilder(this.DatabaseContext);
            theCourseBuilder.Title = "Wirtschaftsinformatik";
            theCourseBuilder.Designation = "WWWI15B-SE";
            theCourseBuilder.Year = 2015;
            theCourseBuilder.Save(false);
            Course theCourse = theCourseBuilder.Course();

            ModuleBuilder theModuleBuilder = new ModuleBuilder(this.DatabaseContext);
            theModuleBuilder.Course = theCourse;
            theModuleBuilder.Designation = "WWISE_106";
            theModuleBuilder.Title = "Neue Konzepte";
            theModuleBuilder.Save(false);
            Module theModule = theModuleBuilder.Module();

            UnitBuilder theUnitBuilder = new UnitBuilder(this.DatabaseContext);
            theUnitBuilder.Designation = "WWISE_106.1";
            theUnitBuilder.Title = "Robotik";
            theUnitBuilder.Module = theModule;
            theUnitBuilder.Save(false);



            this.DatabaseContext.SaveChanges();
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