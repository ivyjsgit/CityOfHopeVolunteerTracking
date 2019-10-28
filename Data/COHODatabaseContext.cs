﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityOfHopeVolunteerTracking.Models;

namespace CityOfHopeVolunteerTracking.Data
{
    public class COHODatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=COHODatabase.db");
        }
        public DbSet<CityOfHopeVolunteerTracking.Models.Volunteer> Volunteer { get; set; }
    }
}
