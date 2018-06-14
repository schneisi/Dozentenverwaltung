using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dozentenplanung.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Course : BaseObject
    {
        [Key]
        public int Id { get; set; }

        public int Year { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        //[MaxLength(50)]
        //public int Students { get; set; } --> interessant falls Kurse zusammen gelegt werden sollen wegen der Grösse

        [Required]
        [MaxLength(128)]
        public string Designation { get; set; }

        public List<Module> Modules { get; set; }

        public Course() {
            this.Modules = new List<Module>();
        }

        public bool DeleteFromContext(ApplicationDbContext aContext) {
            //Delete the receiver
            foreach (Module eachModule in this.Modules) {
                eachModule.DeleteFromContext(aContext);
            }
            aContext.Courses.Remove(this);
            return true;
        }


        public Course CopyCourse(ApplicationDbContext aContext) {
            //Copy the receiver with all its modules and units
            CourseBuilder courseBuilder = new CourseBuilder(aContext);
            courseBuilder.Title = this.Title;
            courseBuilder.Year = this.Year;
            courseBuilder.Designation = this.Designation;
            courseBuilder.Save();
            Course course = courseBuilder.Course();

            foreach(Module eachModule in this.Modules) {
                eachModule.CopyToCourse(course, aContext);
            }

            return course;
        }
    }
}