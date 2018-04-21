﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Dozentenplanung.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Dozentenplanung.Controllers
{
    [Authorize]
    public class LecturerController : BaseController
    {
        public LecturerController(ApplicationDbContext aContext, UserManager<ApplicationUser> aUserManager, SignInManager<ApplicationUser> aSignInManager) : base(aContext, aUserManager, aSignInManager)
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

        public IActionResult SaveLecturer(int id, string firstname, string lastname, string mail, string notes) {
            LecturerBuilder theBuilder = new LecturerBuilder(this.DatabaseContext, this.LecturerForId(id));
            theBuilder.Firstname = firstname;
            theBuilder.Lastname = lastname;
            theBuilder.Mail = mail;
            theBuilder.Notes = notes;
            theBuilder.Save();
            return RedirectToAction("Index", "Lecturer");
        }

        private Lecturer LecturerForId(int id) {
            return this.DatabaseContext.Lecturers.Find(id);
        }

        public List<Lecturer> Lecturers() {
            return this.DatabaseContext.Lecturers.ToList();
        }
    }
}