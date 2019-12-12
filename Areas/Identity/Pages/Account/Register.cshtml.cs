using System;
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
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly CoHO.Data.ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
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
            //[Required]
            //[EmailAddress]
            //[Display(Name = "Email")]
            //public string Email { get; set; }

            [Required]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[#$^+=!()@%&]).{8,}$", ErrorMessage = "The password must be at least 8 characters long and include upper case, lowercase, numerical, and special characters.")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
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
            //Console.WriteLine(Volunteer.First);
            returnUrl = Url.Content("/Volunteers/index");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            Volunteer.UserName = Volunteer.Email.ToLower();

            //Volunteer.Password = "Please";
            //Volunteer.Email= Input.


            _context.Volunteer.Add(Volunteer);
            await _context.SaveChangesAsync();
            //if (ModelState.IsValid)
            //{
                
                var user = new IdentityUser { UserName = Volunteer.Email.ToLower(), Email = Volunteer.Email.ToLower(),
                    EmailConfirmed = true
                };
                Console.WriteLine(user);
                       

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (Volunteer.Admin)
                {
                    await _userManager.AddClaimAsync(user, new Claim("super", "true"));
                }

            if (result.Succeeded)
                {
                    Console.WriteLine("It worked! " + user);


                    _logger.LogInformation("User created a new account with password.");

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
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    return LocalRedirect(returnUrl);

                //}

                string messages = string.Join(";", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                Console.WriteLine(messages);

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }


                return RedirectToPage("/Volunteers/index");




            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
