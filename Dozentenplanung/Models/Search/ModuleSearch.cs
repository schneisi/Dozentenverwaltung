using System;
using System.Collections.Generic;
using System.Linq;

namespace Dozentenplanung.Models
{
    public class ModuleSearch : BaseSearch
    {
        public String Title { get; set; }
        public String Designation { get; set; }
        public int? CourseId { get; set; }

        public List<Module> Result { get; set; }

        public ModuleSearch(ApplicationDbContext dbContext) : base(dbContext) {
        }

        public List<Module> Search() {
            IQueryable<Module> query = this.DbContext.AllModules();
            if (this.HasValue(this.Title))
            {
                query = query.Where(eachModule => eachModule.Title.Contains(this.Title));
            }
            if (this.HasValue(this.Designation))
            {
                query = query.Where(eachModule => eachModule.Designation.Contains(this.Designation));
            }
            if (this.CourseId.HasValue)
            {
                query = query.Where(eachModule => eachModule.CourseId == this.CourseId.Value);
            }
            this.Result = query.ToList();
            return Result;
        }
    }
}
