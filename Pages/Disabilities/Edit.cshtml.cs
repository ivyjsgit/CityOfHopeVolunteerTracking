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

namespace CoHO.Pages.Disabilities
{
    public class EditModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public EditModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Disability Disability { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Disability = await _context.Disability.FirstOrDefaultAsync(m => m.DisabilityID == id);

            if (Disability == null)
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

            _context.Attach(Disability).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisabilityExists(Disability.DisabilityID))
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

        private bool DisabilityExists(int id)
        {
            return _context.Disability.Any(e => e.DisabilityID == id);
        }
    }
}
