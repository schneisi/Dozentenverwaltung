using System;
namespace Dozentenplanung.Models
{
    public class UnitBuilder : BaseBuilder
    {
        public string Designation { get; set; } 
        public string Name { get; set; }
        public DateTime BeginDate { get; set; } 
        public DateTime EndDate { get; set; }
        public Module Module { get; set; }

        public UnitBuilder(ApplicationDbContext aContext) : base(aContext){}
        public UnitBuilder(ApplicationDbContext aContext, BaseObject anObject) : base(aContext, anObject){}

		public override BaseObject saveChanges()
		{
            Unit theUnit;
            if (this.isNew()) {
                theUnit = new Unit();
            } else {
                theUnit = this.Unit();
            }

            theUnit.Designation = this.Designation;
            theUnit.Name = this.Name;
            theUnit.BeginDate = this.BeginDate;
            theUnit.EndDate = this.EndDate;
            theUnit.Module = this.Module;

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
