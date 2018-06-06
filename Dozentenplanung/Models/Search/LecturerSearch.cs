using System;
using System.Collections.Generic;
using System.Linq;

namespace Dozentenplanung.Models
{
    public class LecturerSearch : BaseSearch
    {
        public String Firstname { get; set; }
        public String Lastname { get; set; }
        public bool ShowDummy { get; set; }

        public List<Lecturer> Result { get; set; }

        public LecturerSearch(ApplicationDbContext dbContext) : base(dbContext) {
            this.ShowDummy = false;
        }

        public List<Lecturer> Search() {
            IQueryable<Lecturer> query = this.DbContext.Lecturers;
            if (this.HasValue(this.Firstname)) {
                query = query.Where(eachLecturer => eachLecturer.Firstname.Contains(this.Firstname));
            }
            if (this.HasValue(this.Lastname)) {
                query = query.Where(eachLecturer => eachLecturer.Lastname.Contains(this.Lastname));
            }
            if (!this.ShowDummy) {
                query = query.Where(eachLecturer => !eachLecturer.IsDummy);
            }
            this.Result = query.ToList();
            return Result;
        }



    }
}
