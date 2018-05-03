using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dozentenplanung.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Dozentenplanung.Controllers
{
    [Authorize]
    public class SkillController : BaseController
    {
        public SkillController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager) : base(aContext, aUserManager, aSignInManager)
        {}

        public IActionResult Index()
        {
            return View("Skills", this.DatabaseContext.Skills);
        }

        public IActionResult Edit(int? id, int? moduleId)
        {
            Skill theSkill;
            if (id.HasValue)
            {
                theSkill = this.DatabaseContext.SkillForId(id.Value);
            }
            else
            {
                SkillBuilder theBuilder = new SkillBuilder(this.DatabaseContext);
                theBuilder.Title = "Skill";
                theBuilder.Description = "Bescheibung";
                theBuilder.Save();
                theSkill = theBuilder.Skill();
            }

            return View(theSkill);
        }

        public IActionResult Save(int id, string title, string description)
        {
            SkillBuilder theBuilder = new SkillBuilder(this.DatabaseContext, this.DatabaseContext.SkillForId(id));
            theBuilder.Title = title;
            theBuilder.Description = description;
            theBuilder.Save();
            return RedirectToAction("index", "skill");
        }

        public IActionResult Delete(int id)
        {
            Skill theSkill = this.DatabaseContext.SkillForId(id);
            theSkill.DeleteFromContext(this.DatabaseContext);
            this.SaveDatabaseContext();
            return RedirectToAction("index", "skill");
        }
    }
}
