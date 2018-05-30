﻿using System;
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

        public int Hours;

        public int Semester;

        public int DurationOfExam { get; set; }

        public List<UnitSkill> UnitSkills { get; set; }

        public virtual Module Module { get; set; }
        public int ModuleId { get; set; }

        public virtual Lecturer Lecturer { get; set; }
        public int? LecturerId { get; set; }


        //public virtual List<Lecturer> SuitableLecturers { get; set; }
        public Unit() {
            Title = "Titel";
            Designation = "Code";
            BeginDate = DateTime.Now;
            EndDate = DateTime.Now;
            Hours = 10;
            Semester = 1;
            DurationOfExam = 60;
            UnitSkills = new List<UnitSkill>();
        }


        public bool HasLecturer() {
            return this.Lecturer != null;
        }
        public List<Lecturer> GetSuitableLecturersForContext(ApplicationDbContext DatabaseContext) {
            return DatabaseContext.Lecturers.Where(Lecturer => true).ToList();
        }

        public string LecturerName {
            get { return this.Lecturer.Fullname; }
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

        public virtual List<Skill> Skills()
        {
            List<Skill> theSkills = new List<Skill>();
            foreach (UnitSkill eachUnitSkill in this.UnitSkills)
            {
                theSkills.Add(eachUnitSkill.Skill);
            }
            return theSkills;
        }
    }
}