using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using CoHO.Models;

namespace CoHO.Pages.Skills
{
    public class DetailsModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public DetailsModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Skill Skill { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Skill = await _context.Skill.FirstOrDefaultAsync(m => m.SkillID == id);

            if (Skill == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
