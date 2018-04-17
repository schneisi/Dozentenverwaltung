using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dozentenplanung.Models;


namespace Dozentenplanung.Controllers
{
    public class ModuleController : BaseController
    {
        public ModuleController(ApplicationDbContext aContext) : base(aContext) {}

        public IActionResult Index()
        {
            return View(DatabaseContext.Modules.ToList());
        }
        public IActionResult Module(int id)
        {
            return View(this.moduleForId(id));
        }
        public IActionResult Edit (int id)
        {
            return View(DatabaseContext.Modules.Find(id));
        }

        [HttpPost]
        public IActionResult SaveModule(int id, int courseId, string title, string designation)
        {
            Module theModule = this.moduleForId(id);
            ModuleBuilder theBuilder = new ModuleBuilder(this.DatabaseContext, theModule);
            theBuilder.Title = title;
            theBuilder.Designation = designation;
            theBuilder.save();
            return RedirectToAction("course", "course", new { id = theModule.CourseId});
        }




        private Module moduleForId(int anId) {
            return this.DatabaseContext.Modules.Find(anId);
        }
    }
}