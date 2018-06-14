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

        public IActionResult Index(string firstname, string lastname)
        {
            LecturerSearch search = new LecturerSearch(this.DatabaseContext);
            search.Firstname = firstname;
            search.Lastname = lastname;
            ViewBag.Firstname = firstname;
            ViewBag.Lastname = lastname;
            this.PutRolesInViewBag();
            return View(search.Search());
        }
        public IActionResult Edit(int? id)
        {
            if (this.CurrentUserCanWrite()) {
                Lecturer theLecturer;
                if (id.HasValue)
                {
                    theLecturer = this.LecturerForId(id.Value);
                }
                else
                {
                    LecturerBuilder lecturerBuilder = new LecturerBuilder(this.DatabaseContext);
                    lecturerBuilder.Lastname = "Nachname";
                    lecturerBuilder.Firstname = "Vorname";
                    lecturerBuilder.Mail = "mail@dhbw-loerrach.de";
                    lecturerBuilder.Notes = "Notizen";
                    lecturerBuilder.Save();
                    theLecturer = lecturerBuilder.Lecturer();
                }
                List<SelectListItem> theSkills = new List<SelectListItem>();
                foreach (Skill eachSkill in this.DatabaseContext.Skills)
                {
                    SelectListItem listItem = new SelectListItem();
                    listItem.Text = eachSkill.Title;
                    listItem.Value = eachSkill.Id.ToString();
                    listItem.Selected = theLecturer.HasSkill(eachSkill);
                    theSkills.Add(listItem);
                }
                ViewBag.Skills = theSkills;
                return View(theLecturer);
            } else {
                return RedirectToAction("index");
            }

        }

        public IActionResult Lecturer(int id) {
            this.PutRolesInViewBag();
            return View(this.LecturerForId(id));
        }

        public IActionResult DeleteLecteurer(int id) {
            Lecturer lecturer = this.LecturerForId(id);
            if (this.CurrentUserCanWrite() && lecturer.deleteFromContext(this.DatabaseContext)) {
                //And connection in if statemant to ensure there is no illegal deletion
                this.SaveDatabaseContext();
                return RedirectToAction("Index", "Lecturer");
            }
            return RedirectToAction("Edit", id);
        }

        public IActionResult SaveLecturer(int id, string firstname, string lastname, string mail, string notes, List<int> SkillIds) {
            LecturerBuilder builder = new LecturerBuilder(this.DatabaseContext, this.LecturerForId(id));
            builder.Firstname = firstname;
            builder.Lastname = lastname;
            builder.Mail = mail;
            builder.Notes = notes;
            List<Skill> skillList = new List<Skill>();
            foreach (int eachId in SkillIds) {
                skillList.Add(this.DatabaseContext.SkillForId(eachId));
            }
            builder.Skills = skillList;
            builder.Save();
            return RedirectToAction("Index", "Lecturer");
        }

        private Lecturer LecturerForId(int id) {
            return this.DatabaseContext.LecturerForId(id);
        }
    }
}