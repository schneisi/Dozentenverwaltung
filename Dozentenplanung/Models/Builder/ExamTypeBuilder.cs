using System;
namespace Dozentenplanung.Models
{
    public class ExamTypeBuilder : BaseBuilder
    {
        public ExamTypeBuilder(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public ExamTypeBuilder(ApplicationDbContext aContext, BaseObject anObject) : base(aContext, anObject)
        {
        }

        public string Title { get; set; }

        protected override BaseObject saveChanges()
        {
            ExamType examType;
            if (this.isNew())
            {
                examType = new ExamType();
            }
            else
            {
                examType = this.ExamType();
            }

            examType.Title = this.Title;

            if (isNew())
            {
                this.DatabaseContext.ExamTypes.Add(examType);
            }
            return examType;
        }

        public ExamType ExamType() {
            return (ExamType)this.Object;
        }
    }
}