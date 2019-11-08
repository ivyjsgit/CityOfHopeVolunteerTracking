using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CityOfHopeVolunteerTracking.Data;
using CityOfHopeVolunteerTracking.Models;

namespace CityOfHopeVolunteerTracking.Pages.VolunteerActivities
{
    public class DetailsModel : PageModel
    {
        private readonly CityOfHopeVolunteerTracking.Data.COHODatabaseContext _context;

        public DetailsModel(CityOfHopeVolunteerTracking.Data.COHODatabaseContext context)
        {
            _context = context;
        }

        public VolunteerActivity VolunteerActivity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VolunteerActivity = await _context.VolunteerActivity.FirstOrDefaultAsync(m => m.ID == id);

            if (VolunteerActivity == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
