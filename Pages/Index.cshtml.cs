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

namespace CoHO.Pages
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
            var ValidInitiatives = from i in _context.Initiative
                                   where i.InActive == false
                                   orderby i.Description // Sort by name.
                                   select i;
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerID", "UserName");
            ViewData["InitiativeId"] = new SelectList(ValidInitiatives.AsNoTracking(), "InitiativeID", "Description");

            return Page();
        }

        [BindProperty]
        public VolunteerActivity VolunteerActivity { get; set; }
        [BindProperty]
        public Volunteer Volunteers { get; set; }
        [BindProperty]
        public Initiative Initiative { get; set; }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for

        public VolunteerActivity GetLastActivity(Volunteer volunteer)
        {

            List<VolunteerActivity> VolunteerActivities = (from volunteeractivity in _context.VolunteerActivity where volunteeractivity.VolunteerId == volunteer.VolunteerID orderby volunteeractivity.EndTime select volunteeractivity).ToList();
            VolunteerActivities.Reverse();
            //Console.WriteLine("Our good one");

            //Console.WriteLine(VolunteerActivities[0]);
            try
            {
                return VolunteerActivities[0];

            }
            catch
            {
                return null;
            }
        }


        public async void DoClockout(Volunteer ourVolunteer, Boolean after)
        {
            VolunteerActivity LastActivity = GetLastActivity(ourVolunteer);
            if (!after)
            {
                LastActivity.EndTime = DateTime.Now;

            }
            LastActivity.ClockedIn = false;

            _context.Attach(LastActivity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

     

        public async void Clockin(Volunteer ourVolunteer)
        {

            Console.WriteLine("Our volunteer still is ");
            Console.WriteLine(ourVolunteer.Email);
            VolunteerActivity.InitiativeId = Initiative.InitiativeID;
            VolunteerActivity.StartTime = DateTime.Now;
            VolunteerActivity.EndTime = VolunteerActivity.StartTime.AddHours(2.0);
            VolunteerActivity.ClockedIn = true;
            VolunteerActivity.Volunteer = ourVolunteer;
            VolunteerActivity.Initiative = (from activity in _context.Initiative where activity.InitiativeID == Initiative.InitiativeID select activity).ToList()[0];



            _context.VolunteerActivity.Add(VolunteerActivity);
            await _context.SaveChangesAsync();
        }



        public async Task<IActionResult> OnPostClockOut()
        {
            Console.WriteLine("Clocking in");
            //Move over the clock in code here
            Volunteer ourVolunteer = null;
            VolunteerActivity LastActivity = null;
            try
            {
                ourVolunteer = (from volunteer in _context.Volunteer where volunteer.Email.ToLower() == Volunteers.Email.ToLower() select volunteer).ToList()[0];
                LastActivity = GetLastActivity(ourVolunteer);
         

                if (LastActivity != null)
                {
                    if (LastActivity.ClockedIn)
                    {
                        if (DateTime.Compare(LastActivity.EndTime, DateTime.Now) > 0)
                        {
                            DoClockout(ourVolunteer, false);
                        }
                        else
                        {
                            DoClockout(ourVolunteer, true);
                        }
                        TempData["message"] = "CO";
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        TempData["message"] = "NCI";
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
            catch
            {
                TempData["message"] = "UNF";
            }


            System.Threading.Thread.Sleep(500);

            return RedirectToPage("./Index");


        }
        public async Task<IActionResult> OnPostClockIn()
        {
            try
            {
                Volunteer ourVolunteer = (from volunteer in _context.Volunteer where volunteer.Email.ToLower() == Volunteers.Email.ToLower() select volunteer).ToList()[0];
                VolunteerActivity LastActivity = GetLastActivity(ourVolunteer);
                if (LastActivity != null && !LastActivity.ClockedIn)
                {
                    Clockin(ourVolunteer);
                    TempData["message"] = "CI";
                    System.Threading.Thread.Sleep(500);
                }
                else if (LastActivity != null && LastActivity.ClockedIn)
                {
                    TempData["message"] = "NCO";
                    System.Threading.Thread.Sleep(500);

                }
                else
                {
                    Clockin(ourVolunteer);
                    TempData["message"] = "CI";
                    System.Threading.Thread.Sleep(500);

                }
            }
            catch
            {
                TempData["message"] = "UNF";
            }        
         
            return RedirectToPage("./Index");
            //Put the clockout code here.

        }


    }
}

