using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dozentenplanung.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Dozentenplanung.Controllers
{
    [Authorize]
    public class ModuleController : BaseController
    {
        public ModuleController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager, IHttpContextAccessor httpContextAccessor) : base(aContext, aUserManager, aSignInManager, httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            ModuleSearch moduleSearch = new ModuleSearch(this.DatabaseContext);
            this.PutRolesInViewBag();
            return View(moduleSearch.Search());
        }
        public IActionResult Module(int id)
        {
            this.PutRolesInViewBag();
            return View(this.ModuleForId(id));
        }
        public IActionResult Edit (int? id, int? courseId)
        {
            if (this.CurrentUserCanWrite()) {
                Module theModule;
                if (id.HasValue)
                {
                    theModule = DatabaseContext.Modules.Find(id);
                }
                else
                {
                    ModuleBuilder builder = new ModuleBuilder(this.DatabaseContext);
                    builder.Course = this.CourseForId(courseId.Value);
                    builder.Designation = "Modulbezeichnung";
                    builder.Title = "Modultitel";
                    builder.Save();
                    theModule = builder.Module();
                }

                return View(theModule);    
            } else {
                return RedirectToAction("index");
            }
        }

        public IActionResult Delete(int id) {
            Module module = this.ModuleForId(id);
            if (this.CurrentUserCanWrite()) {
                module.DeleteFromContext(this.DatabaseContext);
                this.SaveDatabaseContext();   
            }
            return RedirectToAction("course", "course", new { id = module.CourseId });
        }

        [HttpPost]
        public IActionResult SaveModule(int id, int courseId, string title, string designation)
        {
            if (this.CurrentUserCanWrite()) {
                Module module = this.ModuleForId(id);
                ModuleBuilder moduleBuilder = new ModuleBuilder(this.DatabaseContext, module);
                moduleBuilder.Title = title;
                moduleBuilder.Designation = designation;
                moduleBuilder.Save();
                return RedirectToAction("course", "course", new { id = module.CourseId });
            } else {
                return RedirectToAction("index");
            }

        }


        private Module ModuleForId(int anId) {
            return this.DatabaseContext.ModuleForId(anId);
        }
    }
}