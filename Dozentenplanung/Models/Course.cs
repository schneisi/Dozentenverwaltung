using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dozentenplanung.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Course : BaseObject
    {
        [Key]
        public int Id { get; set; }

        public int Year { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        [Required]
        [MaxLength(128)]
        public string Designation { get; set; }

        public List<Module> Modules { get; set; }

        public Course() {
            this.Modules = new List<Module>();
        }
    }
}