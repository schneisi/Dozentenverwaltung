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
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? LecturerId { get; set; }
        public int Status { get; set; }
        public string CourseDesignation { get; set; }
        public List<Unit> Result { get; set; }

        public UnitSearch(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.Status = -1; //Means do not search
        }

        public void SetStatus(string status) {
            if (!String.IsNullOrEmpty(status)) {
                this.Status = Convert.ToInt32(status);
            }
        }

        public List<Unit> Search() {
            var query = this.DbContext.AllUnits();

            //Search Designation
            if (this.HasValue(this.Designation)) query = query.Where(eachUnit => eachUnit.Designation.Contains(this.Designation));
            //Search Title
            if (this.HasValue(this.Title)) query = query.Where(eachUnit => eachUnit.Title.Contains(this.Title));
            //Search Semester
            if (this.Semester.HasValue) query = query.Where(eachUnit => eachUnit.Semester == this.Semester);
            //Search Year
            if (this.BeginDate.HasValue) query = query.Where(eachUnit => eachUnit.EndDate >= this.BeginDate);
            //Search Quarter
            if (this.EndDate.HasValue) query = query.Where(eachUnit => eachUnit.BeginDate <= this.EndDate);
            //Search Lecturer
            if (this.LecturerId.HasValue && !(this.DbContext.LecturerForId(this.LecturerId.Value).IsDummyAll)) query = query.Where(eachUnit => eachUnit.LecturerId == this.LecturerId);
            //Search status
            if (this.Status >= 0) query = query.Where(eachUunit => eachUunit.Status == this.Status);
            //Search CourseDesignation
            if (this.HasValue(this.CourseDesignation))  query = query.Where(eachUnit => eachUnit.Module.Course.Designation.Contains(this.CourseDesignation));

            query = query.OrderBy(eachUnit => eachUnit.BeginDate);
            this.Result = query.ToList();
            return this.Result;
        }
    }
}
