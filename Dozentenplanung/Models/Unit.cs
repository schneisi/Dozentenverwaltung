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

        public int hours;

        public int semester;

        public int durationOfExam { get; set; }

        public virtual Module Module { get; set; }
        public int ModuleId { get; set; }

        public virtual Lecturer Lecturer { get; set; }
        public int? LecturerId { get; set; }

        //public virtual List<Lecturer> SuitableLecturers { get; set; }


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
    }
}