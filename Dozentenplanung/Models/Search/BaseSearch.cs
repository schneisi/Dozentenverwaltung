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
            return !string.IsNullOrEmpty(aString);
        }
    }
}
