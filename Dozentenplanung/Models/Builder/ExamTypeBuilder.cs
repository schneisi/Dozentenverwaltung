using System;
namespace Dozentenplanung.Models
{
    public class ExamTypeBuilder : BaseBuilder
    {
        public string Title { get; set; }
        public bool IsDummy { get; set; }

        public ExamTypeBuilder(ApplicationDbContext aContext) : base(aContext)
        {
            this.IsDummy = false;
        }

        public ExamTypeBuilder(ApplicationDbContext aContext, BaseObject anObject) : base(aContext, anObject)
        {
            this.IsDummy = false;
        }



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
            examType.IsDummy = this.IsDummy;

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