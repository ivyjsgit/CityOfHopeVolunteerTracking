﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using CoHO.Data;
using CoHO.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace CoHO.Areas.Identity.Pages.Account
{

    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly CoHO.Data.ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender, CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public Volunteer Volunteer { get; set; }


        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[#$^+=!()@%&]).{8,}$", ErrorMessage = "The password must be at least 8 characters long and include upper case, lowercase, numerical, and special characters.")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [RegularExpression(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$", ErrorMessage = "Invalid Phone Number")]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Cell Phone")]
            public string Cell { get; set; }

            [RegularExpression(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$", ErrorMessage = "Invalid Phone Number")]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Home Phone")]
            public string Home { get; set; }
       


        }




        public async Task OnGetAsync(string returnUrl = null)
        {
            var ValidEducationLevels = from e in _context.EducationLevel
                                       where e.InActive == false
                                       orderby e.Description // Sort by name.
                                       select e;
            ViewData["EducationLevelID"] = new SelectList(ValidEducationLevels, "EducationLevelID", "Description");

            var ValidRaces = from r in _context.Race
                             where r.InActive == false
                             orderby r.Description // Sort by name.
                             select r;
            ViewData["RaceID"] = new SelectList(ValidRaces, "RaceID", "Description");

            var ValidInitiatives = from v in _context.VolunteerType
                                   where v.InActive == false
                                   orderby v.Description // Sort by name.
                                   select v;
            ViewData["VolunteerTypeID"] = new SelectList(ValidInitiatives, "VolunteerTypeID", "Description");
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = Url.Content("/Volunteers/index");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            Volunteer.UserName = Volunteer.Email.ToLower();
            Volunteer.Cell = Input.Cell;
            Volunteer.Home = Input.Home;

            var Cell = new string(Volunteer.Cell.Where(c => char.IsDigit(c)).ToArray());
            var Home = new string(Volunteer.Home.Where(c => char.IsDigit(c)).ToArray());

            Console.WriteLine($"Volunteers cell is {Volunteer.Cell}");

            //Add the volunteer to the Volunteers table
            _context.Volunteer.Add(Volunteer);
            await _context.SaveChangesAsync();
            //Set up the Identity user object
            var user = new IdentityUser
            {
                UserName = Volunteer.Email.ToLower(),
                Email = Volunteer.Email.ToLower(),
                EmailConfirmed = true
            };

            //Put it in the Identity database
            var result = await _userManager.CreateAsync(user, Input.Password);
            //Make admin

            if (Volunteer.Admin)
            {
                await _userManager.AddClaimAsync(user, new Claim("super", "true"));
            }

            if (result.Succeeded)
            {
                Console.WriteLine("It worked! " + user);


                Console.WriteLine("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(Volunteer.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Volunteer.Email });
                }
                else
                {
                    return LocalRedirect(returnUrl);
                }


            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
