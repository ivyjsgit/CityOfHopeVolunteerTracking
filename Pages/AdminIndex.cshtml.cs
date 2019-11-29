

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

        public VolunteerActivity VolunteerActivity { get; set; }
        public Volunteer Volunteer { get; set; }
        public Initiative Initiative { get; set; }

        public void OnGet()
        {

        }




        public FileStreamResult OnPostView()
        {
            //Initialize database variables.
            var volunteers = _context.Volunteer;
            var volunteerActivities = _context.VolunteerActivity;
            var valuesOfHours = _context.ValueOfHour;
            var initiatives = _context.Initiative;

            //Save information to dictionary
            int numInitiatives = initiatives.Count();
            double[,] initiativeMonth = new double[numInitiatives, 13];

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

            //Adding text to cells.
            for (int i = 1; i < numInitiatives + 1; i++)
            {
                worksheet.Range[i + 1, 1].Text = initiatives.Single(m => m.InitiativeID == i).Description;
                for (int j = 1; j < 13; j++)
                {
                    double hours = 0;
                    var activities = volunteerActivities.Where(m => m.StartTime.Month == j && m.InitiativeId == i);
                    foreach (var activity in activities)
                    {
                        hours += activity.ElapsedTime.Hours + activity.ElapsedTime.Minutes / 60;
                    }
                    initiativeMonth[i - 1, j - 1] = hours;
                    worksheet.Range[i + 1, j + 1].Number = hours;
                }
                
                
            }
            String[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            for (int i = 1; i < 13; i++)
            {
                worksheet.Range[1, i + 1].Text = months[i - 1];
            }

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
                double staffHours = 0;
                double volunteerHours = 0;
                foreach (var volunteerActivity in volunteerActivities)
                {
                    Volunteer thisVolunteer = volunteers.Single(m => m.VolunteerID == volunteerActivity.VolunteerId);
                    DateTime date = volunteerActivity.StartTime;
                    Console.Write(volunteerActivity.StartTime);
                    if (DateTime.Now.Year == date.Year && date.Month == i)
                    {
                        if (thisVolunteer.VolunteerTypeID == 1)
                        {
                            volunteerHours += volunteerActivity.ElapsedTime.Minutes / 60 + volunteerActivity.ElapsedTime.Hours;
                        }
                        else
                        {
                            staffHours += volunteerActivity.ElapsedTime.Minutes / 60 + volunteerActivity.ElapsedTime.Hours;
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
