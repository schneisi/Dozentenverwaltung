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

        public DbSet<Setting> Settings { get; set; }
       
        //Database representation model
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<LecturerSkill>().HasKey(lecturerSkill => new {
                lecturerSkill.LecturerId, lecturerSkill.SkillId
            });*/
            base.OnModelCreating(modelBuilder);
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder anOptionsBuilder)
        {
            var theConnectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "Database.db" };
            var theConnectionString = theConnectionStringBuilder.ToString();
            var theSqlLiteConnection = new SqliteConnection(theConnectionString);

            anOptionsBuilder.UseSqlite(theSqlLiteConnection);
        }*/


        //API
        //Course
        public Course CourseForId(int id)
        {
            return this.Courses
                       .Include("Modules")
                       .Include("Modules.Units")
                       .Include("Modules.Units.UnitSkills")
                       .Include("Modules.Units.UnitSkills.Skill")
                       .SingleOrDefault(course => course.Id == id);
        }

        //Module
        public Module ModuleForId(int id)
        {
            return this.Modules
                       .Include("Units")
                       .Include("Course")
                       .Include("Units.Lecturer")
                       .SingleOrDefault(module => module.Id == id);
        }

        //Unit
        public Unit UnitForId(int id) {
            return this.AllUnits()
                       .Include("UnitSkills")
                       .Include("UnitSkills.Skill")
                       .SingleOrDefault(unit => unit.Id == id);
        }
        public IQueryable<Unit> AllUnits() {
            return this.Units
                       .Include("Module")
                       .Include("Module.Course")
                       .Include("Lecturer");
        }

        //Lecturer
        public Lecturer LecturerForId(int id) {
            return this.LecturersWithSkills()
                       .SingleOrDefault(lecturer => lecturer.Id == id);
        }
        public Lecturer DummyNoneLecturer() 
        {
            var theDummies = this.Lecturers.Where(lecturer => lecturer.IsDummyNone);
            if (theDummies.Any()) {
                return theDummies.First();
            } else {
                return null;
            }
        }
        public IQueryable<Lecturer> LecturersWithSkills() {
            return this.Lecturers
                       .Include("LecturerSkills")
                       .Include("LecturerSkills.Skill");
        }

        //Skill
        public Skill SkillForId(int id) {
            return this.Skills.Find(id);
        }

        //ExamType
        public ExamType ExamTypeForId(int id) {
            return this.ExamTypes.Find(id);
        }






        public void EnsureCreated() {
            this.Database.EnsureCreated();
            if (this.DummyNoneLecturer() == null)
            {
                Lecturer.CreateDummyInContext(this);
            }
        }

        public void Delete() {
            this.Database.EnsureDeleted();
        }

        public Setting settingForName(string aName) {
            return this.Settings.Single(setting => setting.Name == aName);
        }
    }
}
