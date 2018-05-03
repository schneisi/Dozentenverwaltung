using System;
using System.ComponentModel.DataAnnotations;

namespace Dozentenplanung.Models
{
    public class Skill : BaseObject
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }


        public bool DeleteFromContext(ApplicationDbContext aContext)
        {
            aContext.Skills.Remove(this);
            return true;
        }
    }
}
