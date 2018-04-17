using System;
using Microsoft.EntityFrameworkCore;

using Dozentenplanung.Models;
using Microsoft.Data.Sqlite;
using System.Reflection.Emit;

namespace Dozentenplanung
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; } 
        public DbSet<Lecturer> Lecturers { get; set; }
       
        //Database representation model
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder anOptionsBuilder)
        {
            var theConnectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "Database.db" };
            var theConnectionString = theConnectionStringBuilder.ToString();
            var theSqlLiteConnection = new SqliteConnection(theConnectionString);

            anOptionsBuilder.UseSqlite(theSqlLiteConnection);
        }*/
    }
}
