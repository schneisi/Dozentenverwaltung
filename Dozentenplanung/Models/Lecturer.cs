using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dozentenplanung.Models
{
    public class Lecturer : BaseObject
    {
        [Key]
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Mail { get; set; }

        public string Notes { get; set; }

        public bool IsDummyNone { get; set; }

        public bool IsDummyAll { get; set; }

        public List<Unit> Units;

        public List<LecturerSkill> LecturerSkills { get; set; }


        public Lecturer() {
            this.Firstname = "";
            this.Lastname = "";
            this.Mail = "";
            this.Notes = "";
            this.IsDummyNone = false;
            this.IsDummyAll = false;
            this.Units = new List<Unit>();
        }


        //API
        public string Fullname
        {
            //Answer the full name of the receiver
            get { return this.Firstname + " " + this.Lastname; }
        }

        public static void CreateDummiesInContext(ApplicationDbContext aContext)
        {
            //Create the dummy objects in the given context
            Lecturer dummyNoneLecturer = new Lecturer();
            dummyNoneLecturer.IsDummyNone = true;
            dummyNoneLecturer.Lastname = "Keiner";
            aContext.Lecturers.Add(dummyNoneLecturer);
            Lecturer dummyAllLecturer = new Lecturer();
            dummyAllLecturer.IsDummyAll = true;
            dummyAllLecturer.Lastname = "Alle";
            aContext.Lecturers.Add(dummyAllLecturer);
            aContext.SaveChanges();
        }
        public bool deleteFromContext(ApplicationDbContext aContext)
        {
            //Delete the receiver from the context
            UnitSearch search = new UnitSearch(aContext);
            search.LecturerId = this.Id;
            Lecturer dummyLecturer = aContext.DummyNoneLecturer();
            foreach (Unit eachUnit in search.Search()) {
                UnitBuilder unitBuilder = new UnitBuilder(aContext, eachUnit);
                unitBuilder.Lecturer = dummyLecturer;
                unitBuilder.Save(false);
            }
            aContext.Lecturers.Remove(this);
            return true;
        }

        public bool HasSkill(Skill aSkill) {
            //Answer true if the receiver has the given skill
            foreach (LecturerSkill eachLecturerSkill in this.LecturerSkills) {
                if (eachLecturerSkill.Skill == aSkill) return true;
            }
            return false;
        }

        public virtual HashSet<Skill> Skills()
        {
            //Answer all skills of the receiver
            HashSet<Skill> skillsList = new HashSet<Skill>();
            foreach (LecturerSkill eachLecturerSkill in this.LecturerSkills)
            {
                skillsList.Add(eachLecturerSkill.Skill);
            }
            return skillsList;
        }

        public String StringForUnit(Unit aUnit) {
            //Answer the representation string for the given unit with or without the recommendation
            string returnString = this.Fullname;
            if (!this.IsDummyNone && aUnit.Skills().IsSubsetOf(this.Skills())) {
                returnString += " (Empfohlen)";
            }
            return returnString;
        }

    }
}