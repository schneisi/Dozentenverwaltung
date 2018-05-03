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
        //public string ModuleCode { get; set; } --> für mich eindeutiger + es gibt einen unterschied Modul und Unitocde

        public string Title { get; set; }

        public Course Course { get; set; }
        public virtual int CourseId { get; set; }

        public List<Unit> Units { get; set; }


        public void DeleteFromContext(ApplicationDbContext aContext)
        {
            aContext.Modules.Remove(this);
        }
    }
}