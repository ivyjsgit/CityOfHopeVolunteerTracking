using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CoHO.Models;

namespace CoHO.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Models.Volunteer> Volunteer { get; set; }
        public DbSet<Models.ValueOfHour> ValueOfHour { get; set; }
        public DbSet<Models.Initiative> Initiative { get; set; }
        public DbSet<Models.VolunteerActivity> VolunteerActivity { get; set; }
        public DbSet<CoHO.Models.Disability> Disability { get; set; }
        public DbSet<CoHO.Models.Race> Race { get; set; }
        public DbSet<CoHO.Models.VolunteerType> VolunteerType { get; set; }
        public DbSet<CoHO.Models.Skill> Skill { get; set; }
        public DbSet<CoHO.Models.EducationLevel> EducationLevel { get; set; }
    }
}
