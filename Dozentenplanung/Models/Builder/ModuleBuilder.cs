using System;
namespace Dozentenplanung.Models
{
    public class ModuleBuilder : BaseBuilder 
    {
        public string Title { get; set; }
        public string Designation { get; set; }
        public Course Course { get; set; }

        public ModuleBuilder(ApplicationDbContext aContext, BaseObject anObject) : base(aContext, anObject)
        {
            
        }

        public override void saveChanges() {
            Module theModule;
            if (this.isNew()) {
                theModule = new Module();
            } else {
                theModule = this.Module();
            }

            theModule.Title = this.Title;
            theModule.Designation = this.Designation;
            if (this.Course != null) {
                theModule.Course = this.Course;
            }

            if (isNew())
            {
                this.DatabaseContext.Modules.Add(theModule);
            }
        }

        private Module Module() {
            return (Module)this.Object;
        }
    }
}
