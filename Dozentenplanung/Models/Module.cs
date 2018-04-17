using System;
using System.ComponentModel.DataAnnotations;

namespace Dozentenplanung.Models
{
    public class Module : BaseObject
    {
        [Key]
        public int Id { get; set; }

        public string Designation { get; set; }

        public string Title { get; set; }

        public Course Course { get; set; }

        public virtual int CourseId { get; set; }
    }
}