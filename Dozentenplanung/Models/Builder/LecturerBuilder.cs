using System;

namespace Dozentenplanung.Models
{
    public class LecturerBuilder : BaseBuilder
    {

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mail { get; set; }
        public string Notes { get; set; }

        public LecturerBuilder(ApplicationDbContext aContext) : base(aContext)
        {}
        public LecturerBuilder(ApplicationDbContext aContext, Lecturer anObject) : base(aContext, anObject)
        {}

        public override void saveChanges()
        {
            Lecturer theLecturer;
            if (this.isNew())
            {
                theLecturer = new Lecturer();
            }
            else
            {
                theLecturer = this.Lecturer();
            }

            theLecturer.Firstname = this.Firstname;
            theLecturer.Lastname = this.Lastname;
            theLecturer.Mail = this.Mail;
            theLecturer.Notes = this.Notes;

            if (isNew())
            {
                this.DatabaseContext.Lecturers.Add(theLecturer);
            }
            this.Object = theLecturer;
        }

        public Lecturer Lecturer()
        {
            return (Lecturer)this.Object;
        }
    }
}
