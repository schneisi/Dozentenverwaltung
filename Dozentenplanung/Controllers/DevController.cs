using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Dozentenplanung.Controllers
{
    public class DevController : BaseController
    {
        public DevController(ApplicationDbContext aContext) : base (aContext) {}

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult DropDatabase() {
            return RedirectToAction("Index", "dev");
        }
    }
}