using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using CoHO.Models;

namespace CoHO.Pages.Races
{
    public class DeleteModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public DeleteModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Race Race { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Race = await _context.Race.FirstOrDefaultAsync(m => m.RaceID == id);

            if (Race == null)
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

            Race = await _context.Race.FindAsync(id);

            if (Race != null)
            {
                _context.Race.Remove(Race);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
