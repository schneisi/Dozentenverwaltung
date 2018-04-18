using System;
using System.Collections.Generic;
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

        public List<Unit> Units { get; set; }


        public void deleteFromContext(ApplicationDbContext aContext)
        {
            aContext.Modules.Remove(this);
        }
    }
}