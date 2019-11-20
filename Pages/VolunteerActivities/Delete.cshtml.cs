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
    public class DeleteModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public DeleteModel(CoHO.Data.ApplicationDbContext context)
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

            VolunteerActivity = await _context.VolunteerActivity
                .Include(v => v.Initiative)
                .Include(v => v.Volunteer).FirstOrDefaultAsync(m => m.ID == id);

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
