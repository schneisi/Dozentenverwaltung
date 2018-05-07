using System;
using System.Collections.Generic;

namespace Dozentenplanung.Models
{
    public class LecturerBuilder : BaseBuilder
    {

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mail { get; set; }
        public string Notes { get; set; }
        public List<Skill> Skills { get; set; } 

        public LecturerBuilder(ApplicationDbContext aContext) : base(aContext)
        {
            this.Skills = new List<Skill>();
        }
        public LecturerBuilder(ApplicationDbContext aContext, Lecturer anObject) : base(aContext, anObject)
        {
            this.Skills = new List<Skill>();
        }

        protected override BaseObject saveChanges()
        {
            Lecturer theLecturer;
            if (this.isNew())
            {
                theLecturer = new Lecturer();
            }
            else
            {
                theLecturer = this.Lecturer();
            }

            theLecturer.Firstname = this.Firstname;
            theLecturer.Lastname = this.Lastname;
            theLecturer.Mail = this.Mail;
            theLecturer.Notes = this.Notes;
            List<LecturerSkill> theLecturerSkills = new List<LecturerSkill>();
            foreach(Skill eachSkill in this.Skills) {
                LecturerSkill theLecturerSkill = new LecturerSkill();
                theLecturerSkill.Lecturer = theLecturer;
                theLecturerSkill.Skill = eachSkill;
                this.DatabaseContext.LecturerSkills.Add(theLecturerSkill);
                theLecturerSkills.Add(theLecturerSkill);
            }
            theLecturer.LecturerSkills = theLecturerSkills;

            if (isNew())
            {
                this.DatabaseContext.Lecturers.Add(theLecturer);
            }
            return theLecturer;
        }

        public void AddSkill(Skill aSkill) {
            this.Skills.Add(aSkill);
        }

        public Lecturer Lecturer()
        {
            return (Lecturer)this.Object;
        }
    }
}
