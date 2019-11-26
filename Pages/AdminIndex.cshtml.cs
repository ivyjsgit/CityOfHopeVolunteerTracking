

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Office.Interop.Excel;
using Syncfusion.XlsIO;
using System.IO;


namespace CoHO.Pages
{
    public class AdminIndexModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public AdminIndexModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Volunteer Volunteer { get; set; }

        public void OnGet()
        {

        }




        public FileStreamResult OnPostView()
        {
            //Initialize database variables.
            var volunteers = _context.Volunteer;
            var volunteerActivities = _context.VolunteerActivity;
            var valuesOfHours = _context.ValueOfHour;

            using ExcelEngine excelEngine = new ExcelEngine();
            //Initialize Application.
            IApplication application = excelEngine.Excel;

            //Set default version for application.
            application.DefaultVersion = ExcelVersion.Excel2013;

            //Create a new workbook.
            IWorkbook workbook = application.Workbooks.Create(1);

            //Accessing first worksheet in the workbook.
            IWorksheet worksheet = workbook.Worksheets[0];

            
            worksheet.SetColumnWidth(1, 25);
            for (int i = 2; i < 15; i++)
            {
                worksheet.SetColumnWidth(i, 15);
            }

            //Adding text to cells.
            worksheet.Range["A1"].Text = _context.Volunteer.FirstOrDefault(m => m.VolunteerID == 1).VolunteerTypeID.ToString();
            worksheet.Range["A16"].Text = "January";
            worksheet.Range["A17"].Text = "Feburary";
            worksheet.Range["A18"].Text = "March";
            worksheet.Range["A19"].Text = "April";
            worksheet.Range["A20"].Text = "May";
            worksheet.Range["A21"].Text = "June";
            worksheet.Range["A22"].Text = "July";
            worksheet.Range["A23"].Text = "August";
            worksheet.Range["A24"].Text = "September";
            worksheet.Range["A25"].Text = "October";
            worksheet.Range["A26"].Text = "November";
            worksheet.Range["A27"].Text = "December";
            worksheet.Range["B15"].Text = "Staff Hours";
            worksheet.Range["C15"].Text = "Staff Value";
            worksheet.Range["D15"].Text = "Volunteer Hours";
            worksheet.Range["E15"].Text = "Volunteer Value";
            worksheet.Range["F15"].Text = "Total Hours";
            worksheet.Range["G15"].Text = "Total Value";

            for (int i = 1; i < 13; i++)
            {
                int staffHours = 0;
                int volunteerHours = 0;
                foreach (var volunteerActivity in volunteerActivities)
                {
                    Volunteer thisVolunteer = volunteers.Single(m => m.VolunteerID == volunteerActivity.VolunteerId);
                    DateTime date = volunteerActivity.StartTime;
                    if (DateTime.Now.Year == volunteerActivity.StartTime.Year && volunteerActivity.StartTime.Month == i)
                    {
                        if (thisVolunteer.VolunteerTypeID == 1)
                        {
                            volunteerHours += volunteerActivity.ElapsedTime.Hours;
                        }
                        else
                        {
                            staffHours += volunteerActivity.ElapsedTime.Hours;
                        }
                    }
                }
                DateTime dateTime = new DateTime(DateTime.Today.Year, i, 1);
                ValueOfHour valueOfHour = valuesOfHours.OrderBy(m => m.EffectiveDate).Last(m => m.EffectiveDate <= dateTime);
                worksheet.Range["B" + (i + 15)].Number = staffHours;
                worksheet.Range["C" + (i + 15)].Number = Math.Round((staffHours * valueOfHour.Value), 2);
                worksheet.Range["D" + (i + 15)].Number = volunteerHours;
                worksheet.Range["E" + (i + 15)].Number = Math.Round((volunteerHours * valueOfHour.Value), 2);
                worksheet.Range["F" + (i + 15)].Number = staffHours + volunteerHours;
                worksheet.Range["G" + (i + 15)].Number = Math.Round(((staffHours + volunteerHours) * valueOfHour.Value), 2);


            }




            //Saving the Excel to the MemoryStream 
            MemoryStream stream = new MemoryStream();

            workbook.SaveAs(stream);

            //Set the position as '0'.
            stream.Position = 0;

            //Download the Excel file in the browser
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel")
            {
                FileDownloadName = "Hours/Values.xlsx"
            };

            return fileStreamResult;



        }
    }
}