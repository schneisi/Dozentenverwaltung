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

        public int Year { get; set; }
        public int Quarter { get; set; }

        public int Hours { get; set; }

        public int Semester { get; set; }

        public int Status { get; set; } //0: none; 1: requested, 2: confirmed

        public string Remark { get; set; }

        public int DurationOfExam { get; set; }
        public string ExamType { get; set; }

        public List<UnitSkill> UnitSkills { get; set; }

        public virtual Module Module { get; set; }
        public int ModuleId { get; set; }

        public virtual Lecturer Lecturer { get; set; }
        public int? LecturerId { get; set; }


        //public virtual List<Lecturer> SuitableLecturers { get; set; }
        public Unit()
        {
            Title = "Titel";
            Designation = "Code";
            Year = 2018;
            Quarter = 2;
            Hours = 10;
            Semester = 1;
            DurationOfExam = 60;
            ExamType = "Klausur";
            UnitSkills = new List<UnitSkill>();
        }


        public bool HasLecturer()
        {
            return !this.Lecturer.IsDummy;
        }

        public string LecturerName
        {
            get { return this.Lecturer.Fullname; }
        }
        public bool IsStatusOpen
        {
            get { return this.Status == 0; }
        }
        public bool IsStatusRequested
        {
            get { return this.Status == 1; }
        }
        public bool IsStatusConfirmed
        {
            get { return this.Status == 2; }
        }

        public void DeleteFromContext(ApplicationDbContext aContext)
        {
            aContext.Units.Remove(this);
        }

        public bool HasSkill(Skill aSkill)
        {
            foreach (UnitSkill eachUnitSkill in this.UnitSkills)
            {
                if (eachUnitSkill.Skill == aSkill)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual HashSet<Skill> Skills()
        {
            HashSet<Skill> skillsSet = new HashSet<Skill>();
            foreach (UnitSkill eachUnitSkill in this.UnitSkills)
            {
                skillsSet.Add(eachUnitSkill.Skill);
            }
            return skillsSet;
        }

        public string StatusIconString()
        {
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

        public void CopyToModule(Module aModule, ApplicationDbContext dbContext) {
            UnitBuilder unitBuilder = new UnitBuilder(dbContext);
            unitBuilder.Title = this.Title;
            unitBuilder.Module = aModule;
            unitBuilder.Semester = this.Semester;
            unitBuilder.Year = this.Year;
            unitBuilder.Quarter = this.Quarter;
            unitBuilder.ExamType = this.ExamType;
            unitBuilder.Skills = this.Skills().ToList();
            unitBuilder.DurationOfExam = this.DurationOfExam;
            unitBuilder.Save();
        }
    }
}