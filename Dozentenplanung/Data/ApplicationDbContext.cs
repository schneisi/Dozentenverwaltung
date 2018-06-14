using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Dozentenplanung.Models;
using Microsoft.Data.Sqlite;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace Dozentenplanung
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; } 
        public DbSet<Unit> Units { get; set; }
        public DbSet<ExamType> ExamTypes { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<LecturerSkill> LecturerSkills { get; set; }
        public DbSet<UnitSkill> UnitSkills { get; set; }
               
        //Database representation model
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        //API
        //Course
        public Course CourseForId(int anId)
        {
            //Answer the course with the given id
            return this.Courses
                       .Include("Modules")
                       .Include("Modules.Units")
                       .Include("Modules.Units.UnitSkills")
                       .Include("Modules.Units.UnitSkills.Skill")
                       .SingleOrDefault(course => course.Id == anId);
        }

        //Module
        public Module ModuleForId(int anId)
        {
            //Answer the module with the given id
            return this.AllModules().SingleOrDefault(module => module.Id == anId);
        }
        public IQueryable<Module> AllModules() {
            //Answer all modules
            return this.Modules
                       .Include("Units")
                       .Include("Course")
                       .Include("Units.Lecturer");
        }

        //Unit
        public Unit UnitForId(int anId) {
            //answer the unit with the given id
            return this.AllUnits()
                       .Include("UnitSkills")
                       .Include("UnitSkills.Skill")
                       .SingleOrDefault(unit => unit.Id == anId);
        }
        public IQueryable<Unit> AllUnits() {
            //Answer all units
            return this.Units
                       .Include("Module")
                       .Include("Module.Course")
                       .Include("ExamType")
                       .Include("Lecturer");
        }

        //Lecturer
        public Lecturer LecturerForId(int anId) {
            //Answer the lecturer with the given id
            return this.LecturersWithSkills().SingleOrDefault(lecturer => lecturer.Id == anId);
        }
        public Lecturer DummyNoneLecturer() 
        {
            //Answer the lecturer representing the dummy none lecturer
            var theDummies = this.Lecturers.Where(lecturer => lecturer.IsDummyNone);
            if (theDummies.Any()) {
                return theDummies.First();
            } else {
                return null;
            }
        }
        public IQueryable<Lecturer> LecturersWithSkills() {
            //Answer the lecturers with the skills
            return this.Lecturers
                       .Include("LecturerSkills")
                       .Include("LecturerSkills.Skill");
        }

        //Skill
        public Skill SkillForId(int anId) {
            //Answer the skill for the given id
            return this.Skills.Find(anId);
        }

        //ExamType
        public ExamType ExamTypeForId(int anId) {
            //Answer the exam type with the given id
            return this.ExamTypes.Find(anId);
        }

        public void EnsureCreated() {
            //Make sure the database is created. Create dummy objects if needed
            this.Database.EnsureCreated();
            if (this.DummyNoneLecturer() == null)
            {
                Lecturer.CreateDummiesInContext(this);
            }
            if (this.ExamTypes.Count() == 0)
            {
                ExamType.CreateDummyInContext(this);
            }

        }

        public void Delete() {
            //Delete the database
            this.Database.EnsureDeleted();
        }
    }
}
