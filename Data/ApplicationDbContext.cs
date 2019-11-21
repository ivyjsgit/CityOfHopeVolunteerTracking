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
  
/*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Disability>().ToTable("Disability");
            modelBuilder.Entity<EducationLevel>().ToTable("EducationLevel");
            modelBuilder.Entity<Initiative>().ToTable("Initiative");
            modelBuilder.Entity<InitiativeSelection>().ToTable("InitiativeSelection");
            modelBuilder.Entity<Race>().ToTable("Race");
            modelBuilder.Entity<Skill>().ToTable("Skill");
            modelBuilder.Entity<ValueOfHour>().ToTable("ValueOfHour");
            modelBuilder.Entity<Volunteer>().ToTable("Volunteer");
            modelBuilder.Entity<VolunteerActivity>().ToTable("VolunteerActivity");
            modelBuilder.Entity<VolunteerType>().ToTable("VolunteerType");

            modelBuilder.Entity<InitiativeSelection>()
                .HasKey(c => new { c.InitiativeID, c.VolunteerActivityID });
        }*/
    }
}
