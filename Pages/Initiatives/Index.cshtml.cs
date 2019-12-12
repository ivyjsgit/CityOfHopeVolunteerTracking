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
    public class IndexModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public IndexModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Initiative> Initiative { get;set; }

        public async Task OnGetAsync()
        {
            IQueryable<Initiative> InitiativeIQueryable = from i in _context.Initiative select i;
            InitiativeIQueryable = InitiativeIQueryable.OrderBy(i => i.Description);
            Initiative = await InitiativeIQueryable.ToListAsync();
        }
    }
}
