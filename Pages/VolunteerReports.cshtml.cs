using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.XlsIO;
using System.IO;
using CoHO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CoHO.Pages
{
    public class VolunteerReportsModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        
        public VolunteerReportsModel(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            CoHO.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public Volunteer Volunteer { get; set; }
        public IList<VolunteerActivity> Activities { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            Volunteer = await _context.Volunteer
                .Include(v => v.EducationLevel)
                .Include(v => v.Race)
                .Include(v => v.VolunteerType)
                .FirstOrDefaultAsync(m => m.UserName.ToLower() == user.UserName.ToLower());

            if (Volunteer == null)
            {
                return NotFound();
            }
            Activities = await _context.VolunteerActivity
                .Where(a => a.VolunteerId == Volunteer.VolunteerID)
                .OrderByDescending(v => v.StartTime)
                .Include(a => a.Initiative).ToListAsync();

            return Page();
        }
  

        public FileStreamResult OnPostView() {
                Console.WriteLine('H');
                WordDocument report = new WordDocument();
                IWSection section = report.AddSection();
                IWParagraph name = section.AddParagraph();
                name.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                IWTable info = section.AddTable();
                info.ResetCells(2, 2);
                IWTextRange nameText = name.AppendText("Jack Frey");
                nameText.CharacterFormat.FontName = "Times New Roman";
                nameText.CharacterFormat.FontSize = 14;
                MemoryStream stream = new MemoryStream();
                report.Save(stream, FormatType.Docx);
                report.Close();
                stream.Position = 0;
                return File(stream, "application/msword", "JackFreyReport.docx");
            }
        }
}