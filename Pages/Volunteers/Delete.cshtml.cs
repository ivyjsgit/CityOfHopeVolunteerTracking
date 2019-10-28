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
    public class DeleteModel : PageModel
    {
        private readonly CityOfHopeVolunteerTracking.Data.COHODatabaseContext _context;

        public DeleteModel(CityOfHopeVolunteerTracking.Data.COHODatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Volunteer Volunteer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Volunteer = await _context.Volunteer.FirstOrDefaultAsync(m => m.ID == id);

            if (Volunteer == null)
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

            Volunteer = await _context.Volunteer.FindAsync(id);

            if (Volunteer != null)
            {
                _context.Volunteer.Remove(Volunteer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
