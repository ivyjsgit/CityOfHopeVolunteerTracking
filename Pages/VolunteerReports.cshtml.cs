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
        public VolunteerActivity VolunteerActivity { get; set; }
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


        public FileStreamResult OnPostView()
        {
            int row = 0;
            Console.WriteLine('H');
            var volunteers = _context.Volunteer;
            var volunteerActivities = _context.VolunteerActivity;
            var initiatives = _context.Initiative;
            CoHO.Models.Volunteer user = volunteers.Single(k => k.First == "Jane");
            WordDocument report = new WordDocument();
            IWSection section = report.AddSection();
            IWParagraph name = section.AddParagraph();
            name.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
            name.ParagraphFormat.AfterSpacing = 18f;
            IWTable info = section.AddTable();
            info.ResetCells(1, 5);
            info.Rows[0].Cells[0].AddParagraph().AppendText("Date");
            info.Rows[0].Cells[1].AddParagraph().AppendText("Initiative");
            info.Rows[0].Cells[2].AddParagraph().AppendText("Start Time");
            info.Rows[0].Cells[3].AddParagraph().AppendText("End Time");
            info.Rows[0].Cells[4].AddParagraph().AppendText("Elapsed Time");
            IWTextRange nameText = name.AppendText(user.FullName);
            foreach(CoHO.Models.VolunteerActivity activity in volunteerActivities.ToArray())
            {
                if (activity.VolunteerId == user.VolunteerID)
                {
                    info.AddRow();
                    row += 1;
                    string Start = activity.StartTime.ToString();
                    string End = activity.EndTime.ToString();
                    int id = activity.InitiativeId;
                    info.Rows[row].Cells[0].AddParagraph().AppendText(Start.Split(' ')[0]);
                    info.Rows[row].Cells[1].AddParagraph().AppendText(initiatives.Single(m => m.InitiativeID == id).Description);
                    info.Rows[row].Cells[2].AddParagraph().AppendText(Start.Split(' ')[1]);
                    info.Rows[row].Cells[3].AddParagraph().AppendText(End.Split(' ')[1]);
                    info.Rows[row].Cells[4].AddParagraph().AppendText(activity.ElapsedTime.ToString().Split('.')[0]);
                }
            }
            nameText.CharacterFormat.FontName = "Times New Roman";
            nameText.CharacterFormat.FontSize = 14;
            MemoryStream stream = new MemoryStream();
            report.Save(stream, FormatType.Docx);
            report.Close();
            stream.Position = 0;
            return File(stream, "application/msword", "UserReport.docx");

        }
    }
}