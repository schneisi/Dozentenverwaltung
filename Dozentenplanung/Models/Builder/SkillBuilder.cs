using System;
namespace Dozentenplanung.Models
{
    public class SkillBuilder : BaseBuilder
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public SkillBuilder(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public SkillBuilder(ApplicationDbContext aContext, BaseObject anObject) : base(aContext, anObject)
        {
        }

        protected override BaseObject saveChanges()
        {
            Skill theSkill;
            if (this.isNew())
            {
                theSkill = new Skill();
            }
            else
            {
                theSkill = this.Skill();
            }

            theSkill.Title = this.Title;
            theSkill.Description = this.Description;

            if (isNew())
            {
                this.DatabaseContext.Skills.Add(theSkill);
            }
            return theSkill;
        }

        public Skill Skill() {
            return (Skill)this.Object;
        }
    }
}
