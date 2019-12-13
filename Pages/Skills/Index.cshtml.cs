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
    public class IndexModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public IndexModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Skill> Skill { get; set; }

        public async Task OnGetAsync()
        {
            Skill = await _context.Skill.ToListAsync();
        }
    }
}
