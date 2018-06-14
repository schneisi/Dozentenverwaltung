using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dozentenplanung.Models
{
    public class Unit : BaseObject
    {
        [Key]
        public int Id { get; set; }

        //public string UnitCode { get; set; }

        public string Title { get; set; }

        public string Designation { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Hours { get; set; }

        public int Semester { get; set; }

        public int Status { get; set; } //0: none; 1: requested, 2: confirmed

        public string Remark { get; set; }

        public int DurationOfExam { get; set; }

        public List<UnitSkill> UnitSkills { get; set; }

        public virtual Module Module { get; set; }
        public int ModuleId { get; set; }

        public virtual Lecturer Lecturer { get; set; }
        public int? LecturerId { get; set; }

        public virtual ExamType ExamType { get; set; }
        public int? ExamTypeId { get; set; }


        //public virtual List<Lecturer> SuitableLecturers { get; set; }
        public Unit()
        {
            Title = "Titel";
            Designation = "Code";
            BeginDate = DateTime.Now;
            EndDate = DateTime.Now;
            Hours = 10;
            Semester = 1;
            DurationOfExam = 60;
            UnitSkills = new List<UnitSkill>();
        }

        public string BeginDateString {
            //Answer the string for the begin date
            get {
                return this.BeginDate.ToString("dd.MM.yyyy");
            }
        }

        public string EndDateString
        {
            //Answer the string for the end date
            get
            {
                return this.EndDate.ToString("dd.MM.yyyy");
            }
        }

        public string BeginDateHtmlString() {
            //Answer the begin date string for the browser field
            return this.BeginDate.ToString("yyyy-MM-dd");
        }
        public string EndDateHtmlString()
        {
            //Answer the end date string for the browser field
            return this.EndDate.ToString("yyyy-MM-dd");
        }

        public bool HasLecturer()
        {
            //Answer whether the receiver has an assigned lecturer
            return !this.Lecturer.IsDummyNone;
        }

        public string LecturerName
        {
            //Answer the name of the lecturer
            get { return this.Lecturer.Fullname; }
        }
        public string CourseDesignation {
            //Answer the designation (code) of the course
            get { return this.Module.Course.Designation; }
        }

        public bool IsStatusOpen
        {
            //Answer whether the status is open
            get { return this.Status == 0; }
        }
        public bool IsStatusRequested
        {
            //Answer whether the status is requested
            get { return this.Status == 1; }
        }
        public bool IsStatusConfirmed
        {
            //Answer whether the status is confirmed
            get { return this.Status == 2; }
        }

        public void DeleteFromContext(ApplicationDbContext aContext)
        {
            //Delete the receiver from the given context
            aContext.Units.Remove(this);
        }

        public bool HasSkill(Skill aSkill)
        {
            //Answer whether the receiver has the given skill
            foreach (UnitSkill eachUnitSkill in this.UnitSkills)
            {
                if (eachUnitSkill.Skill == aSkill) return true;
            }
            return false;
        }

        public virtual HashSet<Skill> Skills()
        {
            //Answer the Skills of the receiver
            HashSet<Skill> skillsSet = new HashSet<Skill>();
            foreach (UnitSkill eachUnitSkill in this.UnitSkills)
            {
                skillsSet.Add(eachUnitSkill.Skill);
            }
            return skillsSet;
        }

        public string StatusIconString()
        {
            //Answer the path of the icon, depentend on the status
            string iconName = "";
            if (this.HasLecturer()) {
                if (this.IsStatusConfirmed) iconName = "green_dot";
                if (this.IsStatusRequested) iconName = "yellow_dot";
                if (this.IsStatusOpen) iconName = "grey_dot";
            } else {
                iconName = "red_dot";
            }
            return "/img/" + iconName + ".png";
        }

        public void CopyToModule(Module aModule, ApplicationDbContext aContext) {
            //Copy the receiver to the given module and context
            UnitBuilder unitBuilder = new UnitBuilder(aContext);
            unitBuilder.Title = this.Title;
            unitBuilder.Module = aModule;
            unitBuilder.Semester = this.Semester;
            unitBuilder.BeginDate = this.BeginDate;
            unitBuilder.EndDate = this.BeginDate;
            unitBuilder.ExamType = this.ExamType;
            unitBuilder.Skills = this.Skills().ToList();
            unitBuilder.DurationOfExam = this.DurationOfExam;
            unitBuilder.Save();
        }
    }
}