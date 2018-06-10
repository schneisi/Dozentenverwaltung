using System;
using System.Collections.Generic;
using System.Linq;

namespace Dozentenplanung.Models
{
    public class UnitSearch : BaseSearch
    {
        public string Designation { get; set; }
        public string Title { get; set; }
        public int? Semester { get; set; }
        public int? Year { get; set; }
        public int? Quarter { get; set; }
        public int? LecturerId { get; set; }

        public List<Unit> Result { get; set; }

        public UnitSearch(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<Unit> Search() {
            var query = this.DbContext.AllUnits();

            //Search Designation
            if (this.HasValue(this.Designation)) {
                query = query.Where(eachUnit => eachUnit.Designation.Contains(this.Designation));
            }

            //Search Title
            if (this.HasValue(this.Title))
            {
                query = query.Where(eachUnit => eachUnit.Title.Contains(this.Title));
            }
            //Search Semester
            if (this.Semester.HasValue)
            {
                query = query.Where(eachUnit => eachUnit.Semester == this.Semester);
            }
            //Search Year
            if (this.Year.HasValue)
            {
                query = query.Where(eachUnit => eachUnit.Year == this.Year);
            }
            //Search Quarter
            if (this.Quarter.HasValue)
            {
                query = query.Where(eachUnit => eachUnit.Quarter == this.Quarter);
            }

            if (this.LecturerId.HasValue) {
                query = query.Where(eachUnit => eachUnit.LecturerId == this.LecturerId);
            }
            this.Result = query.ToList();
            return this.Result;
        }
    }
}
