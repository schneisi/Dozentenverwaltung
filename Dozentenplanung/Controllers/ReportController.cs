using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Dozentenplanung.Helper;
using Dozentenplanung.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dozentenplanung.Controllers
{
    public class ReportController : BaseController
    {
        public ReportController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager, IHttpContextAccessor httpContextAccessor) : base(aContext, aUserManager, aSignInManager, httpContextAccessor)
        {
        }

        public ActionResult UnitReport(string designation, string title, int? semester, DateTime beginDate, DateTime endDate, int? lecturerId, string status)
        {
            UnitSearch unitSearch = new UnitSearch(this.DatabaseContext);
            unitSearch.Designation = designation;
            unitSearch.Title = title;
            unitSearch.Semester = semester;
            unitSearch.BeginDate = beginDate;
            unitSearch.EndDate = endDate;
            unitSearch.LecturerId = lecturerId;
            unitSearch.SetStatus(status);

            UnitReport report = new UnitReport();
            report.Search = unitSearch;
            return Content(report.CreateReport(), "text/html");
        }


    }
}
