

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
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Initialize Application.
                IApplication application = excelEngine.Excel;

                //Set default version for application.
                application.DefaultVersion = ExcelVersion.Excel2013;

                //Create a new workbook.
                IWorkbook workbook = application.Workbooks.Create(1);

                //Accessing first worksheet in the workbook.
                IWorksheet worksheet = workbook.Worksheets[0];

                //Adding text to a cell
                worksheet.Range["A1"].Text = "Hello World";

                //Saving the Excel to the MemoryStream 
                MemoryStream stream = new MemoryStream();

                workbook.SaveAs(stream);

                //Set the position as '0'.
                stream.Position = 0;

                //Download the Excel file in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");

                fileStreamResult.FileDownloadName = "Output.xlsx";

                return fileStreamResult;
            }


        }
    }
}