using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CityOfHopeVolunteerTracking.Models;

namespace CityOfHopeVolunteerTracking.Data
{
    public class CityOfHopeVolunteerTrackingContext : DbContext
    {
        public CityOfHopeVolunteerTrackingContext (DbContextOptions<CityOfHopeVolunteerTrackingContext> options)
            : base(options)
        {
        }

        public DbSet<CityOfHopeVolunteerTracking.Models.Volunteer> Volunteer { get; set; }
    }
}
