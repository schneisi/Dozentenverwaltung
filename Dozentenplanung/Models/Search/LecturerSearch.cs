using System;
using System.Collections.Generic;
using System.Linq;

namespace Dozentenplanung.Models
{
    public class LecturerSearch : BaseSearch
    {
        public String Firstname { get; set; }
        public String Lastname { get; set; }
        public bool ShowDummyNone { get; set; }
        public bool ShowDummyAll { get; set; }

        public List<Lecturer> Result { get; set; }

        public LecturerSearch(ApplicationDbContext dbContext) : base(dbContext) {
            this.ShowDummyNone = false;
            this.ShowDummyAll = false;
        }

        public List<Lecturer> Search() {
            IQueryable<Lecturer> query = this.DbContext.Lecturers;
            if (this.HasValue(this.Firstname)) {
                query = query.Where(eachLecturer => eachLecturer.Firstname.Contains(this.Firstname));
            }
            if (this.HasValue(this.Lastname)) {
                query = query.Where(eachLecturer => eachLecturer.Lastname.Contains(this.Lastname));
            }
            if (!this.ShowDummyNone) {
                query = query.Where(eachLecturer => !eachLecturer.IsDummyNone);
            }
            if (!this.ShowDummyNone)
            {
                query = query.Where(eachLecturer => !eachLecturer.IsDummyAll);
            }
            this.Result = query.ToList();
            return Result;
        }



    }
}
