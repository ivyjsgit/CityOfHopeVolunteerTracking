using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using CoHO.Models;

namespace CoHO.Pages.VolunteerTypes
{
    public class EditModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public EditModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VolunteerType VolunteerType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VolunteerType = await _context.VolunteerType.FirstOrDefaultAsync(m => m.VolunteerTypeID == id);

            if (VolunteerType == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(VolunteerType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolunteerTypeExists(VolunteerType.VolunteerTypeID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool VolunteerTypeExists(int id)
        {
            return _context.VolunteerType.Any(e => e.VolunteerTypeID == id);
        }
    }
}
