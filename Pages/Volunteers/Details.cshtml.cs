using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using CoHO.Models;

namespace CoHO.Pages.Volunteers
{
    public class DetailsModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public DetailsModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Volunteer Volunteer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Volunteer = await _context.Volunteer
                .Include(v => v.Race)
                .Include(v => v.VolunterrType).FirstOrDefaultAsync(m => m.VolunteerID == id);

            if (Volunteer == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
