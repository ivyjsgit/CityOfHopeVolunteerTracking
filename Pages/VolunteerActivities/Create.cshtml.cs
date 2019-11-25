using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoHO.Data;
using CoHO.Models;
using Microsoft.EntityFrameworkCore;

namespace CoHO.Pages.VolunteerActivities
{
    public class CreateModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public CreateModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerID", "First");
            return Page();
        }

        [BindProperty]
        public VolunteerActivity VolunteerActivity { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for



        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            //Vi add the clock out stuff near here.

            VolunteerActivity.StartTime = DateTime.Now;
            VolunteerActivity.EndTime = VolunteerActivity.StartTime.AddHours(2.0);
            VolunteerActivity.ClockedIn = true;



            _context.VolunteerActivity.Add(VolunteerActivity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}