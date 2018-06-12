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
        public ExamType ExamType { get; set; }
        public List<Skill> Skills { get; set; }
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
            Unit unit;
            if (this.isNew()) {
                unit = new Unit();
            } else {
                unit = this.Unit();
            }

            if (!String.IsNullOrEmpty(this.Designation)) unit.Designation = this.Designation;
            if (!String.IsNullOrEmpty(this.Title)) unit.Title = this.Title;
            if (this.Semester.HasValue) unit.Semester = this.Semester.Value;
            if (this.Year.HasValue) unit.Year = this.Year.Value;
            if (this.Quarter.HasValue) unit.Quarter = this.Quarter.Value;
            if (this.DurationOfExam.HasValue )unit.DurationOfExam = this.DurationOfExam.Value;
            if (this.Status.HasValue) unit.Status = this.Status.Value;
            if (this.ExamType != null) unit.ExamType = this.ExamType;
            unit.Remark = this.Remark;
            if (this.Module != null) {
                unit.Module = this.Module;
            }
            if(this.Lecturer != null) {
                unit.Lecturer = this.Lecturer;
            } else if(unit.Lecturer == null) {
                unit.Lecturer = this.DatabaseContext.DummyNoneLecturer();
            }
            List<UnitSkill> theUnitSkills = new List<UnitSkill>();
            foreach (Skill eachSkill in this.Skills){
                UnitSkill unitSkill = new UnitSkill();
                unitSkill.Unit = unit;
                unitSkill.Skill = eachSkill;
                this.DatabaseContext.UnitSkills.Add(unitSkill);
                theUnitSkills.Add(unitSkill);
            }
            unit.UnitSkills = theUnitSkills;

            if (this.isNew()) {
                this.DatabaseContext.Units.Add(unit);
            }
            return unit;
		}
        public void AddSkill(Skill aSkill)
        {
            this.Skills.Add(aSkill);
        }

        public void SetExamType(int anId) {
            this.ExamType = this.DatabaseContext.ExamTypeForId(anId);
        }

        public Unit Unit() {
            return (Unit)this.Object;
        }
	}
}
