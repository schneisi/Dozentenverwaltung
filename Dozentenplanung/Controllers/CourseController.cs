using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dozentenplanung.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    public class CourseController : BaseController
    {
        public CourseController(ApplicationDbContext aContext) : base(aContext) {

            if (!DatabaseContext.Courses.Any())
            {
                this.CreateCourse("WWI15B-SE", 2015, "Wirtschaftsinformatik - Software Engineering");
            }
        }

        #region Views
        public IActionResult Index()
        {
            return View(this.Courses());
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Course(int id)
        {
            Course theCourse = DatabaseContext.Courses.Include("Modules").SingleOrDefault(x => x.Id == id);
            return View(theCourse);
        }
        #endregion






        [HttpPost]
        public IActionResult CreateCourse(string title, string designation, int year) {
            this.CreateCourse(designation, year, title);
            return RedirectToAction("Index", this.Courses());
        }


        private void CreateCourse(string aDesignationString, int aYearInt, string aTitleString) {
           
            Course theCourse = new Course
            {
                Title = aTitleString,
                Year = aYearInt,
                Designation = aDesignationString,
            };
            Module theModule = new Module
            {
                Title = "Modul",
                Designation = "Modulbezeichnung"
            };
            theCourse.Modules.Add(theModule);
            DatabaseContext.Courses.Add(theCourse);
            DatabaseContext.Modules.Add(theModule);
            DatabaseContext.SaveChanges();
        }

        private List<Course> Courses() {
            return this.DatabaseContext.Courses.ToList();
        }
    }
}
