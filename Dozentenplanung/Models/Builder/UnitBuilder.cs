using System;
namespace Dozentenplanung.Models
{
    public class UnitBuilder : BaseBuilder
    {
        public string Designation { get; set; } 
        public string Title { get; set; }
        public DateTime BeginDate { get; set; } 
        public DateTime EndDate { get; set; }
        public Module Module { get; set; }
        public Lecturer Lecturer { get; set; }

        public UnitBuilder(ApplicationDbContext aContext) : base(aContext){}
        public UnitBuilder(ApplicationDbContext aContext, BaseObject anObject) : base(aContext, anObject){}

        protected override BaseObject saveChanges()
		{
            Unit theUnit;
            if (this.isNew()) {
                theUnit = new Unit();
            } else {
                theUnit = this.Unit();
            }

            theUnit.Designation = this.Designation;
            theUnit.Title = this.Title;
            theUnit.BeginDate = this.BeginDate;
            theUnit.EndDate = this.EndDate;
            if (this.Module != null) {
                theUnit.Module = this.Module;
            }
            if(this.Lecturer != null) {
                theUnit.Lecturer = this.Lecturer;
            } else if(theUnit.Lecturer == null) {
                theUnit.Lecturer = this.DatabaseContext.DummyLecturer();
            }

            if (this.isNew()) {
                this.DatabaseContext.Units.Add(theUnit);
            }
            return theUnit;
		}


        public Unit Unit() {
            return (Unit)this.Object;
        }
	}
}
