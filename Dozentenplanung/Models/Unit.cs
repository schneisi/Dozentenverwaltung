using System;
using System.ComponentModel.DataAnnotations;

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

        public int hours;

        public int semester;

        public int durationOfExams { get; set; }

        //public string TypeOfExams { get; set; }
        //public int TypeOfJoining { get; set; } --> wie wird verrechnet Note /Prozent ggf Dropdown?
        //public int LiteratureBib { get; set; }
        //public int LiteratureNotInBib { get; set; }
        //public string NeededKnowledgle { get; set; }

        public Module Module { get; set; }
        public virtual int ModuleId { get; set; }


        public void DeleteFromContext(ApplicationDbContext aContext)
        {
            aContext.Units.Remove(this);
        }
    }
}