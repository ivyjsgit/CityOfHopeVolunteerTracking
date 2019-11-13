using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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


    }
}
