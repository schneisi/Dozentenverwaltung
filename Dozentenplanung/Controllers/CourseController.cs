using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dozentenplanung.Models;
using Microsoft.EntityFrameworkCore;

namespace Dozentenplanung.Controllers
{
    public class CourseController : BaseController
    {
        public CourseController(ApplicationDbContext aContext) : base(aContext)
        {
        }

        #region Views
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
                theBuilder.save();
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
            this.CourseForId(id).deleteFromContext(this.DatabaseContext);
            this.DatabaseContext.SaveChanges();
            return View("index", this.Courses());
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
            theCourseBuilder.save();
            return RedirectToAction("Index", this.Courses());
        }

        private List<Course> Courses()
        {
            return this.DatabaseContext.Courses.ToList();
        }

        private Course CourseForId(int id)
        {
            return DatabaseContext.Courses.Include("Modules").SingleOrDefault(course => course.Id == id);
        }
    }
}