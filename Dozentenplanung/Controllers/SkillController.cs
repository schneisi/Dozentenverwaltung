using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dozentenplanung.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Dozentenplanung.Controllers
{
    [Authorize]
    public class SkillController : BaseController
    {
        public SkillController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager, IHttpContextAccessor httpContextAccessor) : base(aContext, aUserManager, aSignInManager, httpContextAccessor)
        {}

        public IActionResult Index()
        {
            this.PutRolesInViewBag();
            return View("Skills", this.DatabaseContext.Skills);
        }

        public IActionResult Edit(int? id)
        {
            if (this.CurrentUserIsAdministrator()) {
                Skill skill;
                if (id.HasValue)
                {
                    skill = this.DatabaseContext.SkillForId(id.Value);
                }
                else
                {
                    SkillBuilder skillBuilder = new SkillBuilder(this.DatabaseContext);
                    skillBuilder.Title = "Skill";
                    skillBuilder.Description = "Bescheibung";
                    skillBuilder.Save();
                    skill = skillBuilder.Skill();
                }
                return View(skill);    
            } else {
                return RedirectToAction("index");
            }

        }

        public IActionResult Save(int id, string title, string description)
        {
            if (this.CurrentUserIsAdministrator()) {
                SkillBuilder skillBuilder = new SkillBuilder(this.DatabaseContext, this.DatabaseContext.SkillForId(id));
                skillBuilder.Title = title;
                skillBuilder.Description = description;
                skillBuilder.Save();    
            }
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            if (this.CurrentUserIsAdministrator()) {
                Skill skill = this.DatabaseContext.SkillForId(id);
                skill.DeleteFromContext(this.DatabaseContext);
                this.SaveDatabaseContext();    
            }
            return RedirectToAction("index", "skill");
        }
    }
}
