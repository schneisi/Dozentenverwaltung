using System;
using System.ComponentModel.DataAnnotations;

namespace Dozentenplanung.Models
{
    public class Unit : BaseObject
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Designation { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public int hours;

        public int semester;

        public int durationOfExams { get; set; }

        public Module Module { get; set; }
        public virtual int ModuleId { get; set; }
    }
}