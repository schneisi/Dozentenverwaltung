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

        public IActionResult Index(string designation, string title, int? semester, int? year, int? quarter, int? lecturerId, string status, string courseDesignation)
        {
            UnitSearch unitSearch = new UnitSearch(this.DatabaseContext);
            unitSearch.Designation = designation;
            unitSearch.Title = title;
            unitSearch.Semester = semester;
            unitSearch.Year = year;
            unitSearch.Quarter = quarter;
            unitSearch.LecturerId = lecturerId;
            unitSearch.SetStatus(status);
            unitSearch.CourseDesignation = courseDesignation;
            LecturerSearch lecturerSearch = new LecturerSearch(this.DatabaseContext);
            lecturerSearch.ShowDummyAll = true;
            lecturerSearch.ShowDummyNone = true;
            ViewBag.Lecturers = lecturerSearch.Search().Select(eachLecturer => new SelectListItem
            {
                Text = eachLecturer.Fullname,
                Value = eachLecturer.Id.ToString(),
                Selected = (lecturerId.HasValue && eachLecturer.Id == lecturerId.Value) || (!lecturerId.HasValue && eachLecturer.IsDummyAll)
            });

            ViewBag.UnitTitle = title;
            ViewBag.Designation = designation;
            ViewBag.Semester = semester;
            ViewBag.Year = year;
            ViewBag.Quarter = quarter;
            ViewBag.Status = status;
            ViewBag.CourseDesignation = courseDesignation;
            this.PutRolesInViewBag();
            return View(unitSearch.Search());
        }

        public IActionResult Edit(int? id, int? moduleId) {
            if (this.CurrentUserCanWrite()) {
                Unit unit;
                if (id.HasValue)
                {
                    unit = this.DatabaseContext.UnitForId(id.Value);
                }
                else
                {
                    UnitBuilder unitBuilder = new UnitBuilder(this.DatabaseContext);
                    unitBuilder.Title = "";
                    unitBuilder.Designation = "Unitbezeichnung";
                    unitBuilder.Module = this.DatabaseContext.ModuleForId(moduleId.Value);
                    unitBuilder.Save();
                    unit = unitBuilder.Unit();
                }
                List<SelectListItem> theSkills = new List<SelectListItem>();
                foreach (Skill eachSkill in this.DatabaseContext.Skills)
                {
                    SelectListItem listItem = new SelectListItem();
                    listItem.Text = eachSkill.Title;
                    listItem.Value = eachSkill.Id.ToString();
                    listItem.Selected = unit.HasSkill(eachSkill);
                    theSkills.Add(listItem);
                }
                ViewBag.Skills = theSkills;
                ViewBag.SuitableLecturers = this.DatabaseContext.LecturersWithSkills().Select(eachLecturer => new SelectListItem
                {
                    Text = eachLecturer.StringForUnit(unit),
                    Value = eachLecturer.Id.ToString(),
                    Selected = eachLecturer.Id == unit.LecturerId
                });
                ViewBag.ExamTypes = this.DatabaseContext.ExamTypes.Select(eachExamType => new SelectListItem
                {
                    Text = eachExamType.Title,
                    Value = eachExamType.Id.ToString(),
                    Selected = eachExamType.Id == unit.ExamTypeId
                });
                return View(unit);  
            } else {
                return RedirectToAction("index");
            }

        }

        public IActionResult Unit(int id) {
            this.PutRolesInViewBag();
            return View(this.DatabaseContext.UnitForId(id));
        }

        [HttpPost]
        public IActionResult Save(int id, string title, string designation, int lecturer, List<int> SkillIds, int Semester, int Year, int Quarter, int DurationOfExam, int ExamType, int status) {
            if (this.CurrentUserCanWrite()) {
                this.CreateBuilderAndSave(id, title, designation, lecturer, SkillIds, Semester, Year, Quarter, DurationOfExam, ExamType, status);
                return RedirectToAction("unit", "unit", new { id = id });    
            } else {
                return RedirectToAction("index");
            }
        }

        [HttpPost]
        public IActionResult SetSkills(int id, string title, string designation, int lecturer, List<int> SkillIds, int Semester, int Year, int Quarter, int DurationOfExam, int ExamType, int status) {
            this.CreateBuilderAndSave(id, title, designation, lecturer, SkillIds, Semester, Year, Quarter, DurationOfExam, ExamType, status);
            return RedirectToAction("edit", "unit", new { id = id });
        }

        public IActionResult Delete(int id) {
            if (this.CurrentUserCanWrite()) {
                Unit unit = this.DatabaseContext.UnitForId(id);
                unit.DeleteFromContext(this.DatabaseContext);
                this.SaveDatabaseContext();
                return RedirectToAction("module", "module", new { id = unit.ModuleId });   
            } else {
                return RedirectToAction("index");
            }
        }

        private void CreateBuilderAndSave(int id, string title, string designation, int lecturer, List<int> SkillIds, int Semester, int Year, int Quarter, int DurationOfExam, int ExamType, int status) {
            UnitBuilder unitBuilder = new UnitBuilder(this.DatabaseContext, this.DatabaseContext.UnitForId(id));
            unitBuilder.Title = title;
            unitBuilder.Designation = designation;
            unitBuilder.Lecturer = this.DatabaseContext.LecturerForId(lecturer);
            unitBuilder.Semester = Semester;
            unitBuilder.Year = Year;
            unitBuilder.Quarter = Quarter;
            unitBuilder.SetExamType(ExamType);
            unitBuilder.DurationOfExam = DurationOfExam;
            unitBuilder.Status = status;
            List<Skill> skillList = new List<Skill>();
            foreach (int eachId in SkillIds)
            {
                skillList.Add(this.DatabaseContext.SkillForId(eachId));
            }
            unitBuilder.Skills = skillList;
            unitBuilder.Save();
        }
    }
}
