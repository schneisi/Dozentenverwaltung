using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dozentenplanung.Models;


namespace Dozentenplanung.Controllers
{
    public class DevController : BaseController
    {
        public DevController(ApplicationDbContext aContext) : base (aContext) {}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateTestData() {
            LecturerBuilder theLecturerBuilder = new LecturerBuilder(this.DatabaseContext);
            theLecturerBuilder.Firstname = "Max";
            theLecturerBuilder.Lastname = "Muster";
            theLecturerBuilder.Mail = "muster@dhbw-loerrach.de";
            theLecturerBuilder.Notes = "Sehr guter Informatiker";
            theLecturerBuilder.Save();

            return RedirectToAction("Index", "dev");
        }

        public IActionResult DropDatabase() {
            this.DatabaseContext.Delete();
            return RedirectToAction("Index", "dev");
        }
    }
}