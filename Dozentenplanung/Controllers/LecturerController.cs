using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Dozentenplanung.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Dozentenplanung.Controllers
{
    [Authorize]
    public class LecturerController : BaseController
    {

        public LecturerController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager, IHttpContextAccessor httpContextAccessor) : base(aContext, aUserManager, aSignInManager, httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            return View(this.Lecturers());
        }
        public IActionResult Edit(int? id)
        {
            Lecturer theLecturer;
            if (id.HasValue) {
                theLecturer = this.LecturerForId(id.Value);
            } else {
                LecturerBuilder theBuilder = new LecturerBuilder(this.DatabaseContext);
                theBuilder.Lastname = "Nachname";
                theBuilder.Firstname = "Vorname";
                theBuilder.Mail = "mail@dhbw-loerrach.de";
                theBuilder.Notes = "Notizen";
                theBuilder.Save();
                theLecturer = theBuilder.Lecturer();
            }
            List<SelectListItem> theSkills = new List<SelectListItem>();
            foreach (Skill eachSkill in this.DatabaseContext.Skills) {
                SelectListItem theItem = new SelectListItem();
                theItem.Text = eachSkill.Title;
                theItem.Value = eachSkill.Id.ToString();
                theItem.Selected = theLecturer.HasSkill(eachSkill);
                theSkills.Add(theItem);
            }
            ViewBag.Skills = theSkills;

            return View(theLecturer);
        }

        public IActionResult Lecturer(int id) {
            Lecturer theLecturer = this.LecturerForId(id);
            return View(theLecturer);
        }
        public IActionResult Create() {
            return View();
        }

        public IActionResult DeleteLecteurer(int id) {
            Lecturer theLecturer = this.LecturerForId(id);
            if (theLecturer.deleteFromContext(this.DatabaseContext)) {
                this.SaveDatabaseContext();
                return RedirectToAction("Index", "Lecturer");
            }
            return RedirectToAction("Edit", id);
        }

        public IActionResult SaveLecturer(int id, string firstname, string lastname, string mail, string notes, List<int> SkillIds) {
            LecturerBuilder theBuilder = new LecturerBuilder(this.DatabaseContext, this.LecturerForId(id));
            theBuilder.Firstname = firstname;
            theBuilder.Lastname = lastname;
            theBuilder.Mail = mail;
            theBuilder.Notes = notes;
            List<Skill> theSkillList = new List<Skill>();
            foreach (int eachId in SkillIds) {
                theSkillList.Add(this.DatabaseContext.SkillForId(eachId));
            }
            theBuilder.Skills = theSkillList;
            theBuilder.Save();
            return RedirectToAction("Index", "Lecturer");
        }

        private Lecturer LecturerForId(int id) {
            return this.DatabaseContext.LecturerForId(id);
        }

        public List<Lecturer> Lecturers() {
            return this.DatabaseContext.Lecturers.ToList();
        }
    }
}