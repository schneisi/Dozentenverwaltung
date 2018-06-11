using System;
using System.Collections.Generic;

namespace Dozentenplanung.Models
{
    public class UnitBuilder : BaseBuilder
    {
        public string Designation { get; set; } 
        public string Title { get; set; }
        public int? Semester { get; set; }
        public int? Year { get; set; } 
        public int? Quarter { get; set; }
        public Module Module { get; set; }
        public Lecturer Lecturer { get; set; }
        public List<Skill> Skills { get; set; }
        public string ExamType { get; set; }
        public int? DurationOfExam { get; set; }
        public int? Status { get; set; }
        public string Remark { get; set; }

        public UnitBuilder(ApplicationDbContext aContext) : base(aContext)
        {
            this.Skills = new List<Skill>();
            this.Semester = 1;
            this.Year = DateTime.Now.Year;
            this.Quarter = 1;
            this.DurationOfExam = 60;

        }
        public UnitBuilder(ApplicationDbContext aContext, BaseObject anObject) : base(aContext, anObject)
        {
            this.Skills = new List<Skill>();
        }

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
            if (this.Semester.HasValue) theUnit.Semester = this.Semester.Value;
            if (this.Year.HasValue) theUnit.Year = this.Year.Value;
            if (this.Quarter.HasValue) theUnit.Quarter = this.Quarter.Value;
            if (this.DurationOfExam.HasValue )theUnit.DurationOfExam = this.DurationOfExam.Value;
            if (this.Status.HasValue) theUnit.Status = this.Status.Value;
            theUnit.ExamType = this.ExamType;
            theUnit.Remark = this.Remark;
            if (this.Module != null) {
                theUnit.Module = this.Module;
            }
            if(this.Lecturer != null) {
                theUnit.Lecturer = this.Lecturer;
            } else if(theUnit.Lecturer == null) {
                theUnit.Lecturer = this.DatabaseContext.DummyLecturer();
            }
            List<UnitSkill> theUnitSkills = new List<UnitSkill>();
            foreach (Skill eachSkill in this.Skills){
                UnitSkill theUnitSkill = new UnitSkill();
                theUnitSkill.Unit = theUnit;
                theUnitSkill.Skill = eachSkill;
                this.DatabaseContext.UnitSkills.Add(theUnitSkill);
                theUnitSkills.Add(theUnitSkill);
            }
            theUnit.UnitSkills = theUnitSkills;

            if (this.isNew()) {
                this.DatabaseContext.Units.Add(theUnit);
            }
            return theUnit;
		}
        public void AddSkill(Skill aSkill)
        {
            this.Skills.Add(aSkill);
        }

        public Unit Unit() {
            return (Unit)this.Object;
        }
	}
}
