using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Dozentenplanung.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    [Authorize]
    public class UnitController : BaseController
    {
        public UnitController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager, IHttpContextAccessor httpContextAccessor) : base(aContext, aUserManager, aSignInManager, httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            return View(DatabaseContext.Units.Include("Lecturer").ToList());
        }

        public IActionResult Edit(int? id, int? moduleId) {
            Unit theUnit;
            if (id.HasValue) {
                theUnit = this.DatabaseContext.UnitForId(id.Value);
            } else {
                UnitBuilder theBuilder = new UnitBuilder(this.DatabaseContext);
                theBuilder.Title = "";
                theBuilder.Designation = "Unitbezeichnung";
                theBuilder.DurationOfExam = 0 ;
                theBuilder.ExamType = "";
                theBuilder.Module = this.DatabaseContext.ModuleForId(moduleId.Value);
                theBuilder.Save();
                theUnit = theBuilder.Unit();
            }
            List<SelectListItem> theSkills = new List<SelectListItem>();
            foreach (Skill eachSkill in this.DatabaseContext.Skills)
            {
                SelectListItem theItem = new SelectListItem();
                theItem.Text = eachSkill.Title;
                theItem.Value = eachSkill.Id.ToString();
                theItem.Selected = theUnit.HasSkill(eachSkill);
                theSkills.Add(theItem);
            }
            ViewBag.Skills = theSkills;
            ViewBag.SuitableLecturers = theUnit.GetSuitableLecturersForContext(this.DatabaseContext).Select(lecturer => new SelectListItem
            {
                Text = lecturer.Fullname,
                Value = lecturer.Id.ToString(),
                Selected = lecturer.Id == theUnit.LecturerId
            });

            return View(theUnit);
        }

        public IActionResult Unit(int id) {
            return View(this.DatabaseContext.UnitForId(id));
        }

        [HttpPost]
        public IActionResult Save(int id, string title, string designation, int lecturer, List<int> SkillIds, int DurationOfExam, string ExamType) {
            UnitBuilder theBuilder = new UnitBuilder(this.DatabaseContext, this.DatabaseContext.UnitForId(id));
            theBuilder.Title = title;
            theBuilder.Designation = designation;
            theBuilder.Lecturer = this.DatabaseContext.LecturerForId(lecturer);
            theBuilder.ExamType = ExamType;
            theBuilder.DurationOfExam = DurationOfExam;
            List<Skill> theSkillList = new List<Skill>();
            foreach (int eachId in SkillIds)
            {
                theSkillList.Add(this.DatabaseContext.SkillForId(eachId));
            }
            theBuilder.Skills = theSkillList;
            theBuilder.Save();
            return RedirectToAction("unit", "unit", new {id = id});
        }

        public IActionResult Delete(int id) {
            Unit theUnit = this.DatabaseContext.UnitForId(id);
            theUnit.DeleteFromContext(this.DatabaseContext);
            this.SaveDatabaseContext();
            return RedirectToAction("module", "module", new { id = theUnit.ModuleId });
        }
    }
}
