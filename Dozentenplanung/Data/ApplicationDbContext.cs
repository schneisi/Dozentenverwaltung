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

        public DbSet<Setting> Settings { get; set; }
       
        //Database representation model
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        public Course CourseForId(int id)
        {
            return this.Courses
                       .Include("Modules")
                       .SingleOrDefault(course => course.Id == id);
        }
        public Module ModuleForId(int id)
        {
            return this.Modules
                       .Include("Units")
                       .Include("Course")
                       .Include("Units.Lecturer")
                       .SingleOrDefault(module => module.Id == id);
        }
        public Unit UnitForId(int id) {
            return this.Units
                       .Include("Module")
                       .Include("Module.Course")
                       .Include("Lecturer")
                       .SingleOrDefault(unit => unit.Id == id);
        }
        public Lecturer LecturerForId(int id) {
            return this.Lecturers.Find(id);
        }
        public Lecturer DummyLecturer() 
        {
            var theDummies = this.Lecturers.Where(lecturer => lecturer.IsDummy);
            if (theDummies.Any()) {
                return theDummies.First();
            } else {
                return null;
            }
        }


        public void EnsureCreated() {
            this.Database.EnsureCreated();
            if (this.DummyLecturer() == null)
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
