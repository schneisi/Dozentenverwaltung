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
            Lecturer theDummy = new Lecturer();
            theDummy.IsDummy = true;
            theDummy.Lastname = "Keiner";
            aContext.Lecturers.Add(theDummy);
            aContext.SaveChanges();
        }
        public bool deleteFromContext(ApplicationDbContext aContext)
        {
            aContext.Lecturers.Remove(this);
            return true;
        }

    }
}