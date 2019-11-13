using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using CoHO.Models;

namespace CoHO.Pages.VolunteerActivities
{
    public class DetailsModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public DetailsModel(CoHO.Data.ApplicationDbContext context)
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

            VolunteerActivity = await _context.VolunteerActivity
                .Include(v => v.Volunteer).FirstOrDefaultAsync(m => m.ID == id);

            if (VolunteerActivity == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
