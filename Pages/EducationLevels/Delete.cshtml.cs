using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using CoHO.Models;

namespace CoHO.Pages.EducationLevels
{
    public class DeleteModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public DeleteModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EducationLevel EducationLevel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EducationLevel = await _context.EducationLevel.FirstOrDefaultAsync(m => m.EducationLevelID == id);

            if (EducationLevel == null)
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

            EducationLevel = await _context.EducationLevel.FindAsync(id);

            if (EducationLevel != null)
            {
                _context.EducationLevel.Remove(EducationLevel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
