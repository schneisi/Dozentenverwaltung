using System;
namespace Dozentenplanung.Models
{
    public class CourseBuilder : BaseBuilder
    {
        public string Title { get; set; }
        public string Designation { get; set; }
        public int Year;

        public CourseBuilder(ApplicationDbContext aContext) : base(aContext)
        {
        }

        protected override BaseObject saveChanges()
        {
            Course theCourse;
            if (this.isNew()) {
                theCourse = new Course();
            }
            else {
                theCourse = this.Course();
            }

            theCourse.Title = this.Title;
            theCourse.Designation = this.Designation;
            theCourse.Year = this.Year;

            if (this.isNew()) {
                this.DatabaseContext.Courses.Add(theCourse);
            }
            return theCourse;
        }

        public Course Course()
        {
            return (Course)this.Object;
        }
    }
}
