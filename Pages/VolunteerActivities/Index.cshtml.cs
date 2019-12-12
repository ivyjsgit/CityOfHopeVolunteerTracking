using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using CoHO.Models;

namespace CoHO.Pages.VolunteerActivities
{
    public class IndexModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public IndexModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<VolunteerActivity> VolunteerActivity { get;set; }

        public async Task OnGetAsync()
        {
            IQueryable<VolunteerActivity> ActivitiesIQueryable = from v in _context.VolunteerActivity select v;
            ActivitiesIQueryable = ActivitiesIQueryable.OrderByDescending(v => v.StartTime);

            VolunteerActivity = await ActivitiesIQueryable
                .Include(v => v.Volunteer)
                .Include(v=> v.Initiative).ToListAsync();
        }

    }
}
