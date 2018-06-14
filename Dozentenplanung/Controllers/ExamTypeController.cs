using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dozentenplanung.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Dozentenplanung.Controllers
{
    public class ExamTypeController : BaseController
    {
        public ExamTypeController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager, IHttpContextAccessor httpContextAccessor) : base(aContext, aUserManager, aSignInManager, httpContextAccessor)
        {
        }


        public IActionResult Index()
        {
            this.PutRolesInViewBag();
            return View("examTypes", this.DatabaseContext.ExamTypes);
        }

        public IActionResult Edit(int? id)
        {
            if (this.CurrentUserIsAdministrator())
            {
                ExamType examType;
                if (id.HasValue)
                {
                    examType = this.DatabaseContext.ExamTypeForId(id.Value);
                }
                else
                {
                    ExamTypeBuilder examTypeBuilder = new ExamTypeBuilder(this.DatabaseContext);
                    examTypeBuilder.Title = "Neue Prüfungsart";
                    examTypeBuilder.Save();
                    examType = examTypeBuilder.ExamType();
                }
                return View(examType);
            } else {
                return RedirectToAction("index");
            }
        }

        public IActionResult Save(int id, string title, string description)
        {
            if (this.CurrentUserIsAdministrator()) {
                ExamTypeBuilder builder = new ExamTypeBuilder(this.DatabaseContext, this.DatabaseContext.ExamTypeForId(id));
                builder.Title = title;
                builder.Save();    
            }
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            if (this.CurrentUserIsAdministrator()) {
                ExamType examType = this.DatabaseContext.ExamTypeForId(id);
                examType.DeleteFromContext(this.DatabaseContext);
                this.SaveDatabaseContext();    
            }
            return RedirectToAction("index");
        }
    }
}
