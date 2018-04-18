using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    public abstract class BaseController : Controller
    {
        protected ApplicationDbContext DatabaseContext;
        public BaseController(ApplicationDbContext aContext)
        {
            DatabaseContext = aContext;

            DatabaseContext.Database.EnsureCreated();
        }


        protected void SaveDatabaseContext() {
            this.DatabaseContext.SaveChanges();
        }
    }
}
