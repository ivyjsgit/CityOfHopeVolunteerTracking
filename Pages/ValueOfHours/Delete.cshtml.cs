using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using CoHO.Models;

namespace CoHO.Pages.ValueOfHours
{
    public class DeleteModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public DeleteModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ValueOfHour ValueOfHour { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ValueOfHour = await _context.ValueOfHour.FirstOrDefaultAsync(m => m.ID == id);

            if (ValueOfHour == null)
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

            ValueOfHour = await _context.ValueOfHour.FindAsync(id);

            if (ValueOfHour != null)
            {
                _context.ValueOfHour.Remove(ValueOfHour);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
