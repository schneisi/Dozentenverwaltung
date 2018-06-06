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

        public bool IsDummy { get; set; }

        public List<Unit> Units;

        public List<LecturerSkill> LecturerSkills { get; set; }


        public string Fullname {
            get {
                return this.Firstname + " " + this.Lastname;
            }
        }

        public Lecturer() {
            Firstname = "";
            Lastname = "";
            Mail = "";
            Notes = "";
            IsDummy = false;
            Units = new List<Unit>();
        }


        //API
        public static void CreateDummyInContext(ApplicationDbContext aContext)
        {
            Lecturer dummyLecturer = new Lecturer();
            dummyLecturer.IsDummy = true;
            dummyLecturer.Lastname = "Keiner";
            aContext.Lecturers.Add(dummyLecturer);
            aContext.SaveChanges();
        }
        public bool deleteFromContext(ApplicationDbContext aContext)
        {
            aContext.Lecturers.Remove(this);
            return true;
        }

        public bool HasSkill(Skill aSkill) {
            foreach (LecturerSkill eachLecturerSkill in this.LecturerSkills) {
                if (eachLecturerSkill.Skill == aSkill) {
                    return true;
                }
            }
            return false;
        }

        public virtual HashSet<Skill> Skills()
        {
            HashSet<Skill> skillsList = new HashSet<Skill>();
            foreach (LecturerSkill eachLecturerSkill in this.LecturerSkills)
            {
                skillsList.Add(eachLecturerSkill.Skill);
            }
            return skillsList;
        }

        public String StringForUnit(Unit aUnit) {
            string returnString = this.Fullname;
            if (aUnit.Skills().IsSubsetOf(this.Skills())) {
                returnString += " (Empfohlen)";
            }
            return returnString;
        }

    }
}