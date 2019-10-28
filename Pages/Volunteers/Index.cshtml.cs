using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CityOfHopeVolunteerTracking.Data;
using CityOfHopeVolunteerTracking.Models;

namespace CityOfHopeVolunteerTracking.Pages.Volunteers
{
    public class IndexModel : PageModel
    {
        private readonly CityOfHopeVolunteerTracking.Data.CityOfHopeVolunteerTrackingContext _context;

        public IndexModel(CityOfHopeVolunteerTracking.Data.CityOfHopeVolunteerTrackingContext context)
        {
            _context = context;
        }

        public IList<Volunteer> Volunteer { get;set; }

        public async Task OnGetAsync()
        {
            Volunteer = await _context.Volunteer.ToListAsync();
        }
    }
}
