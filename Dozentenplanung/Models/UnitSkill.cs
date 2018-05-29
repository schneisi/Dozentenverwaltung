using System;
using System.ComponentModel.DataAnnotations;

namespace Dozentenplanung.Models
{
    public class UnitSkill : BaseObject
    {
        [Key]
        public int Id { get; set; }

        public virtual int UnitId { get; set; }
        public Unit Unit { get; set; }

        public virtual int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}
