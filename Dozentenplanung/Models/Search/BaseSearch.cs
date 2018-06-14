using System;
namespace Dozentenplanung.Models
{
    public class BaseSearch
    {
        public ApplicationDbContext DbContext { get; set; }
        public BaseSearch(ApplicationDbContext applicationDbContext)
        {
            this.DbContext = applicationDbContext;
        }

        protected bool HasValue(string aString)
        {
            //Answer whether the given string if not empty or nil
            return !String.IsNullOrEmpty(aString);
        }
    }
}
