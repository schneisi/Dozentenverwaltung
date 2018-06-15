using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dozentenplanung.Models
{
    public class Module : BaseObject
    {
        [Key]
        public int Id { get; set; }

        public string Designation { get; set; }

        public string Title { get; set; }

        public Course Course { get; set; }
        public virtual int CourseId { get; set; }

        public List<Unit> Units { get; set; }

        public void DeleteFromContext(ApplicationDbContext aContext)
        {
            //Delete the receiver from the given context
            aContext.Modules.Remove(this);
        }


        public void CopyToCourse(Course aCourse, ApplicationDbContext aContext) {
            //Copy the receiver with its units to the given course in the given context
            ModuleBuilder moduleBuilder = new ModuleBuilder(aContext);
            moduleBuilder.Title = this.Title;
            moduleBuilder.Course = aCourse;
            moduleBuilder.Save();
            Module module = moduleBuilder.Module();
            foreach (Unit eachUnit in this.Units) {
                eachUnit.CopyToModule(module, aContext);
            }
        }
    }
}