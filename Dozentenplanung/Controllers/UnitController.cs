using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dozentenplanung.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    public class UnitController : BaseController
    {
        public UnitController(ApplicationDbContext aContext) : base(aContext) { }

        public IActionResult Index()
        {
            return View(DatabaseContext.Units.ToList());
        }

        public IActionResult Edit(int? id, int? moduleId) {
            Unit theUnit;
            if (id.HasValue) {
                theUnit = this.DatabaseContext.UnitForId(id.Value);
            } else {
                UnitBuilder theBuilder = new UnitBuilder(this.DatabaseContext);
                theBuilder.Title = "";
                theBuilder.Designation = "Unitbezeichnung";
                theBuilder.Module = this.DatabaseContext.ModuleForId(moduleId.Value);
                theBuilder.Save();
                theUnit = theBuilder.Unit();
            }

            return View(theUnit);
        }

        public IActionResult Unit(int id) {
            return View(this.DatabaseContext.UnitForId(id));
        }

        [HttpPost]
        public IActionResult Save(int id, string title, string designation) {
            UnitBuilder theBuilder = new UnitBuilder(this.DatabaseContext, this.DatabaseContext.UnitForId(id));
            theBuilder.Title = title;
            theBuilder.Designation = designation;
            theBuilder.Save();
            return RedirectToAction("unit", "unit", new {id = id});
        }

        public IActionResult Delete(int id) {
            Unit theUnit = this.DatabaseContext.UnitForId(id);
            theUnit.DeleteFromContext(this.DatabaseContext);
            return RedirectToAction("module", "module", new { id = theUnit.ModuleId });
        }
    }
}
