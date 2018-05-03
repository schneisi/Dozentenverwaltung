using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dozentenplanung.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Dozentenplanung.Controllers
{
    [Authorize]
    public class CourseController : BaseController
    {
        public CourseController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager) : base(aContext, aUserManager, aSignInManager)
        {
        }
        #region Views
        [Authorize]
        public IActionResult Index()
        {
            return View(this.Courses());
        }

        public IActionResult Edit(int? id)
        {
            Course theCourse;
            if (id.HasValue)
            {
                theCourse = this.CourseForId(id.Value);
            }
            else
            {
                CourseBuilder theBuilder = new CourseBuilder(this.DatabaseContext);
                theBuilder.Title = "Neuer Kurs";
                theBuilder.Designation = "Bezeichnung";
                theBuilder.Year = DateTime.Now.Year;
                theBuilder.Save();
                theCourse = theBuilder.Course();
            }
            return View(theCourse);
        }

        public IActionResult Course(int id)
        {
            Course theCourse = this.CourseForId(id);
            return View(theCourse);
        }
        #endregion

        public IActionResult DeleteCourse(int id)
        {
            this.CourseForId(id).DeleteFromContext(this.DatabaseContext);
            this.SaveDatabaseContext();
            return RedirectToAction("index", "course");
        }

        [HttpPost]
        public IActionResult CreateCourse(string title, string designation, int year, int? id)
        {
            CourseBuilder theCourseBuilder = new CourseBuilder(this.DatabaseContext);
            if (id.HasValue) {
                theCourseBuilder.Object = this.CourseForId(id.Value);
            }
            theCourseBuilder.Title = title;
            theCourseBuilder.Designation = designation;
            theCourseBuilder.Year = year;
            theCourseBuilder.Save();
            Course theCourse = theCourseBuilder.Course();
            return RedirectToAction("course", "course", new { id = theCourse.Id});
        }

        private List<Course> Courses()
        {
            return this.DatabaseContext.Courses.ToList();
        }


    }
}