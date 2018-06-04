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
            return View("examTypes", this.DatabaseContext.ExamTypes);
        }

        public IActionResult Edit(int? id)
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
        }

        public IActionResult Save(int id, string title, string description)
        {
            ExamTypeBuilder builder = new ExamTypeBuilder(this.DatabaseContext, this.DatabaseContext.ExamTypeForId(id));
            builder.Title = title;
            builder.Save();
            return RedirectToAction("index", "ExamType");
        }

        public IActionResult Delete(int id)
        {
            ExamType examType = this.DatabaseContext.ExamTypeForId(id);
            examType.DeleteFromContext(this.DatabaseContext);
            this.SaveDatabaseContext();
            return RedirectToAction("index", "ExamType");
        }
    }
}
