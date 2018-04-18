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


        //API
        public bool deleteFromContext(ApplicationDbContext aContext)
        {
            aContext.Lecturers.Remove(this);
            return true;
        }

    }
}