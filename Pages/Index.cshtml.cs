﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoHO.Data;
using CoHO.Models;
using Microsoft.EntityFrameworkCore;

/*
 *     Because the JS  was being funky, we have to use abbreviations to send toasts
      * CI = Clocked In
      * CO = Clocked Out
      * NCO = Not Clocked In
      * NCI = Not Clocked Out
      * UNF = User Not Found
 */


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
            ViewData["InitiativeId"] = new SelectList(ValidInitiatives.AsNoTracking(), "InitiativeID", "Description");

            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerID", "UserName");

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

            Console.WriteLine(ourVolunteer.Email);
            VolunteerActivity.InitiativeId = Initiative.InitiativeID;
            VolunteerActivity.StartTime = DateTime.Now;
            VolunteerActivity.EndTime = VolunteerActivity.StartTime.AddHours(12.0);
            VolunteerActivity.ClockedIn = true;
            VolunteerActivity.Volunteer = ourVolunteer;
            VolunteerActivity.Initiative = (from activity in _context.Initiative where activity.InitiativeID == Initiative.InitiativeID select activity).ToList()[0];

            _context.VolunteerActivity.Add(VolunteerActivity);
            await _context.SaveChangesAsync();
        }



        public IActionResult OnPostClockOut()
        {
            Console.WriteLine("Clocking in");
            //Move over the clock in code here
            Volunteer ourVolunteer = null;
            VolunteerActivity LastActivity = null;
            try
            {
                ourVolunteer = (from volunteer in _context.Volunteer where volunteer.Email.ToLower().Trim() == Volunteers.Email.ToLower().Trim() select volunteer).ToList()[0];
                LastActivity = GetLastActivity(ourVolunteer);


                if (LastActivity != null)
                {
                    if (LastActivity.ClockedIn)
                    {
                        DoClockout(ourVolunteer, false);
                        if (DateTime.Compare(LastActivity.StartTime, DateTime.Now) > 0)
                        {
                            DoClockout(ourVolunteer, false);
                        }
                        else
                        {
                            DoClockout(ourVolunteer, true);
                        }
                        TempData["message"] = "CO";
                    }
                    else
                    {
                        TempData["message"] = "NCI";
                    }
                }
                else
                {
                    TempData["message"] = "NCI";
                }
            }
            catch
            {
                TempData["message"] = "UNF";
            }


            System.Threading.Thread.Sleep(500);

            return RedirectToPage("./Index");


        }
        public IActionResult OnPostClockIn()
        {
     
            
            try
            {
                //Find our volunteer and their last activity
                Volunteer ourVolunteer = (from volunteer in _context.Volunteer where 
                                          volunteer.Email.ToLower().Trim() == Volunteers.Email.ToLower().Trim() select volunteer).ToList()[0];
                VolunteerActivity LastActivity = GetLastActivity(ourVolunteer);

                if (LastActivity != null && !LastActivity.ClockedIn)
                {
                    Clockin(ourVolunteer);
                    //Send a toast to the user saying Clocked in
                    TempData["message"] = "CI";
                    System.Threading.Thread.Sleep(500);
                }
                else if (LastActivity != null && LastActivity.ClockedIn)
                {
                    if (DateTime.Compare(LastActivity.EndTime, DateTime.Now) > 0)
                    {
                        //Send a toast to the user saying Not clocked out
                        TempData["message"] = "NCO";
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        DoClockout(ourVolunteer, true);
                        Clockin(ourVolunteer);
                        TempData["message"] = "CI";
                        System.Threading.Thread.Sleep(500);
                    }
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
        }
    }
}

