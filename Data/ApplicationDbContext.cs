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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<VolunteerType>().HasData(
                new VolunteerType
                {
                    VolunteerTypeID = 1,
                    Description = "Volunteer"
                },
                new VolunteerType
                {
                    VolunteerTypeID = 2,
                    Description = "Staff"
                },
                new VolunteerType
                {
                    VolunteerTypeID = 3,
                    Description = "Board"
                });

            builder.Entity<Race>().HasData(
                new Race
                {
                    RaceID = 1,
                    Description = "American Indian",
                },
                new Race
                {
                    RaceID = 2,
                    Description = "Asian",
                },
                new Race
                {
                    RaceID = 3,
                    Description = "White",
                },
                new Race
                {
                    RaceID = 4,
                    Description = "Back or African American",
                });

            builder.Entity<Initiative>().HasData(
                new Initiative
                {
                    InitiativeID = 1,
                    Description = "Academy",
                },
                new Initiative
                {
                    InitiativeID = 2,
                    Description = "Community Garden",
                },
                new Initiative
                {
                    InitiativeID = 3,
                    Description = "Office",
                },
                new Initiative
                {
                    InitiativeID = 4,
                    Description = "Housing",
                });

            builder.Entity<EducationLevel>().HasData(
                new EducationLevel
                {
                    EducationLevelID = 1,
                    Description = "GED",
                },
                new EducationLevel
                {
                    EducationLevelID = 2,
                    Description = "High School Degree",
                });
            builder.Entity<Volunteer>().HasData(
                new Volunteer
                {
                    VolunteerID = 1,
                    UserName = "admin",
                    Email = "admin",
                    InActive = true,
                    Admin = true,
                    First = "Zadmin",
                    Last = "Zadmin",
                    VolunteerTypeID = 2
                  
                });
        }







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
