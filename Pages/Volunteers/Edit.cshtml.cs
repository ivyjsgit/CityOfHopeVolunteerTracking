﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using CoHO.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CoHO.Pages.Volunteers
{
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly CoHO.Data.ApplicationDbContext _context;

        public EditModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            CoHO.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public Volunteer Volunteer { get; set; }
        public string Username { get; set; }
        public Boolean OldAdminValue { get; set; }
        public IdentityUser VolunteerIdentity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Volunteer = await _context.Volunteer
                .Include(v => v.EducationLevel)
                .Include(v => v.Race)
                .Include(v => v.VolunteerType).FirstOrDefaultAsync(m => m.VolunteerID == id);

            if (Volunteer == null)
            {
                return NotFound();
            }
            OldAdminValue = Volunteer.Admin;
         //   var user = await _userManager.FindByNameAsync(Volunteer.UserName);
        //    if (user == null)
        //    {
        //        return NotFound($"Unable to load user with ID '{_userManager.FindByNameAsync(Volunteer.UserName)}'.");
        //    }
        //    VolunteerIdentity = user;
            ViewData["EducationLevelID"] = new SelectList(_context.EducationLevel, "EducationLevelID", "Description");
            ViewData["RaceID"] = new SelectList(_context.Race, "RaceID", "Description");
            ViewData["VolunteerTypeID"] = new SelectList(_context.VolunteerType, "VolunteerTypeID", "Description");
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Volunteer).State = EntityState.Modified;

            VolunteerIdentity = await _userManager.FindByNameAsync(Volunteer.UserName);

            if (OldAdminValue != Volunteer.Admin)
            { 
                if (Volunteer.Admin)
                {
                    await _userManager.AddClaimAsync(VolunteerIdentity, new Claim("super", "true"));
                } else
                {
                    await _userManager.RemoveClaimAsync(VolunteerIdentity, new Claim("super", "true"));
                }

            } 


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolunteerExists(Volunteer.VolunteerID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool VolunteerExists(int id)
        {
            return _context.Volunteer.Any(e => e.VolunteerID == id);
        }
    }
}
