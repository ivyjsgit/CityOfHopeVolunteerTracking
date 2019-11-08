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
    public class DeleteModel : PageModel
    {
        private readonly CityOfHopeVolunteerTracking.Data.COHODatabaseContext _context;

        public DeleteModel(CityOfHopeVolunteerTracking.Data.COHODatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VolunteerActivity = await _context.VolunteerActivity.FindAsync(id);

            if (VolunteerActivity != null)
            {
                _context.VolunteerActivity.Remove(VolunteerActivity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
