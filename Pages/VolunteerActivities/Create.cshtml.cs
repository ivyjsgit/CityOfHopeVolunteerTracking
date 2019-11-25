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

        public VolunteerActivity GetLastActivity(Volunteer volunteer)
        {

            List<VolunteerActivity> VolunteerActivities = (from volunteeractivity in _context.VolunteerActivity where volunteeractivity.VolunteerId == volunteer.VolunteerID orderby volunteeractivity.EndTime select volunteeractivity).ToList();
            VolunteerActivities.Reverse();

            return VolunteerActivities[0];
        }


        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            //Vi add the clock out stuff near here.

            VolunteerActivity.StartTime = DateTime.Now;
            VolunteerActivity.EndTime = VolunteerActivity.StartTime.AddHours(2.0);
            VolunteerActivity.ClockedIn = true;

            Volunteer ourVolunteer = (from volunteer in _context.Volunteer where volunteer.VolunteerID == VolunteerActivity.VolunteerId select volunteer).ToList()[0];

            VolunteerActivity LastActivity = GetLastActivity(ourVolunteer);


            Console.WriteLine(LastActivity.StartTime);


            _context.VolunteerActivity.Add(VolunteerActivity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}