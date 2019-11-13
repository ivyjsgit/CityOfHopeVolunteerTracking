using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using CoHO.Models;

namespace CoHO.Pages.Initiatives
{
    public class DeleteModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public DeleteModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Initiative Initiative { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Initiative = await _context.Initiative.FirstOrDefaultAsync(m => m.InitiativeID == id);

            if (Initiative == null)
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

            Initiative = await _context.Initiative.FindAsync(id);

            if (Initiative != null)
            {
                _context.Initiative.Remove(Initiative);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
