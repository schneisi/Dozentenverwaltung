using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Dozentenplanung.Models;

namespace Dozentenplanung.Controllers
{
    public class LecturerController : BaseController
    {
        public LecturerController(ApplicationDbContext aContext) : base(aContext){}
        
        public IActionResult Index()
        {
            return View(this.Lecturers());
        }
        public IActionResult Edit()
        {
            LecturerBuilder theBuilder = new LecturerBuilder(this.DatabaseContext);
            theBuilder.Lastname = "Nachname";
            theBuilder.Firstname = "Vorname";
            theBuilder.Mail = "E - Mail";
            theBuilder.Notes = "Notizen";
            theBuilder.save();

            return View(theBuilder.Lecturer());
        }
        public IActionResult Create() {
            return View();
        }

        public List<Lecturer> Lecturers() {
            return this.DatabaseContext.Lecturers.ToList();
        }
    }
}
