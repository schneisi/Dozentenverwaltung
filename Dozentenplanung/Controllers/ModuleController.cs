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
            return View(this.ModuleForId(id));
        }
        public IActionResult Edit (int? id, int? courseId)
        {
            Module theModule;
            if (id.HasValue) {
                theModule = DatabaseContext.Modules.Find(id);
            } else {
                ModuleBuilder theBuilder = new ModuleBuilder(this.DatabaseContext);
                theBuilder.Course = this.CourseForId(courseId.Value);
                theBuilder.Designation = "Modulbezeichnung";
                theBuilder.Title = "Modultitel";
                theBuilder.Save();
                theModule = theBuilder.Module();
            }

            return View(theModule);

        }

        public IActionResult Delete(int id) {
            Module theModule = this.ModuleForId(id);
            theModule.deleteFromContext(this.DatabaseContext);
            this.SaveDatabaseContext();
            return RedirectToAction("course", "course", new { id = theModule.CourseId });
        }

        [HttpPost]
        public IActionResult SaveModule(int id, int courseId, string title, string designation)
        {
            Module theModule = this.ModuleForId(id);
            ModuleBuilder theBuilder = new ModuleBuilder(this.DatabaseContext, theModule);
            theBuilder.Title = title;
            theBuilder.Designation = designation;
            theBuilder.Save();
            return RedirectToAction("course", "course", new { id = theModule.CourseId});
        }




        private Module ModuleForId(int anId) {
            return this.DatabaseContext.ModuleForId(anId);
        }
    }
}