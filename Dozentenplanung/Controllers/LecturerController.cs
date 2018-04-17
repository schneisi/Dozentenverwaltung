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

        public IActionResult Create() {
            return View();
        }

        public List<Lecturer> Lecturers() {
            return this.DatabaseContext.Lecturers.ToList();
        }
    }
}
