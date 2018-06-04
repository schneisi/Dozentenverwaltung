using System;
namespace Dozentenplanung.Models
{
    public class ExamType : BaseObject
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public bool DeleteFromContext(ApplicationDbContext aContext)
        {
            aContext.ExamTypes.Remove(this);
            return true;
        }
    }
}