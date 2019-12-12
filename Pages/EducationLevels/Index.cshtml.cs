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
    public class IndexModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public IndexModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EducationLevel> EducationLevel { get;set; }

        public async Task OnGetAsync()
        {
            IQueryable<EducationLevel> EducationIQueryable = from e in _context.EducationLevel select e;
            EducationIQueryable = EducationIQueryable.OrderBy(e => e.Description);
            EducationLevel = await EducationIQueryable.ToListAsync();
        }
    }
}
