using System;
using System.ComponentModel.DataAnnotations;

namespace Dozentenplanung.Models
{
    public class LecturerSkill : BaseObject
    {
        [Key]
        public int Id { get; set; }

        public virtual int LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }

        public virtual int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}