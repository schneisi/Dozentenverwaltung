using System;
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

       //public string Knowledge { get; set; } --> wegen der Gruppenzuteilen um automatisch Dozenten anzugeben

       //public string Absences{ get; set; } ? --> hier könnten solche Sachen wie Urlaub oder nur Mittwochs da usw stehen

        public string Fullname {
            get {
                return this.Firstname + " " + this.Lastname;
            }
        }
        //API
        public bool deleteFromContext(ApplicationDbContext aContext)
        {
            aContext.Lecturers.Remove(this);
            return true;
        }

    }
}