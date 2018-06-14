using System;
namespace Dozentenplanung.Models
{
    public class ExamType : BaseObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDummy { get; set; }

        public bool DeleteFromContext(ApplicationDbContext aContext)
        {
            //Delete the receiver if is no dummy object
            if (this.IsDummy) {
                return false;
            } else {
                aContext.ExamTypes.Remove(this);
                return true;
            }
        }

        public static void CreateDummyInContext(ApplicationDbContext aContext) {
            ExamTypeBuilder builder = new ExamTypeBuilder(aContext);
            builder.Title = "Keine Prüfung";
            builder.IsDummy = true;
            builder.Save();
        }
    }
}