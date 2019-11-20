using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoHO.Pages
{
    public class ButtonTestModel : PageModel
    {
        public string Message { get; set; }

        private readonly CoHO.Data.ApplicationDbContext _context;

        public ButtonTestModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Volunteer Volunteer { get; set; }

        public void OnGet()
        {
            Message = "OnGet used";
        }

        public void OnPost()
        {
            Message = "OnPost used";
        }

        public void OnPostView()
        {
            Message = "You called the OnPostView Method";
        }

        public void OnPostRecord(int? id)
        {
            if (id == null)
            {
                Message = "Invalid Record ID";
            }

            Volunteer = _context.Volunteer.FirstOrDefault(m => m.VolunteerID == id);

            if (Volunteer == null)
            {
                Message = "Record not Found";
            }
            else
            {
                Message = Volunteer.First + " " + Volunteer.Last;
            }
        }

        public void OnPostFreeForm()
        {
            Message = Request.Form["stuff"];
        }

    }
}