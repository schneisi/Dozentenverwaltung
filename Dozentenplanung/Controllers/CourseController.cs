using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dozentenplanung.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Dozentenplanung.Controllers
{
    [Authorize]
    public class CourseController : BaseController
    {
        public CourseController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager, IHttpContextAccessor httpContextAccessor) : base(aContext, aUserManager, aSignInManager, httpContextAccessor)
        {
        }

        [Authorize]
        public IActionResult Index()
        {
            this.PutRolesInViewBag();
            return View(this.Courses());
        }

        public IActionResult Edit(int? id)
        {
            if (this.CurrentUserCanWrite()) {
                Course course;
                if (id.HasValue)
                {
                    course = this.CourseForId(id.Value);
                }
                else
                {
                    CourseBuilder courseBuilder = new CourseBuilder(this.DatabaseContext);
                    courseBuilder.Title = "Neuer Kurs";
                    courseBuilder.Designation = "Bezeichnung";
                    courseBuilder.Year = DateTime.Now.Year;
                    courseBuilder.Save();
                    course = courseBuilder.Course();
                }
                return View(course);
            } else {
                return RedirectToAction("index");
            }

        }

        public IActionResult Course(int id)
        {
            this.PutRolesInViewBag();
            return View(this.CourseForId(id));
        }

        public IActionResult DeleteCourse(int id)
        {
            if (this.CurrentUserCanWrite()) {
                this.CourseForId(id).DeleteFromContext(this.DatabaseContext);
                this.SaveDatabaseContext();    
            }
            return RedirectToAction("index", "course");
        }

        [HttpPost]
        public IActionResult SaveCourse(string title, string designation, int year, int? id)
        {
            CourseBuilder courseBuilder = new CourseBuilder(this.DatabaseContext);
            if (id.HasValue) {
                courseBuilder.Object = this.CourseForId(id.Value);
            }
            courseBuilder.Title = title;
            courseBuilder.Designation = designation;
            courseBuilder.Year = year;
            courseBuilder.Save();
            return RedirectToAction("course", "course", new { id = courseBuilder.Course().Id});
        }

        private List<Course> Courses()
        {
            return this.DatabaseContext.Courses.ToList();
        }

        public IActionResult CopyCourse(int id) {
            Course course = this.CourseForId(id);
            Course newCourse = course.CopyCourse(this.DatabaseContext);
            return RedirectToAction("course", "course", new { id = newCourse.Id });
        }


    }
}