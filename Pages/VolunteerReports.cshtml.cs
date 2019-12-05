using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;

namespace CoHO.Pages
{
    public class VolunteerReportsModel : PageModel
    {
        public void OnGet()
        {

        }

        public FileStreamResult OnPostView() {
            Console.WriteLine('H');
            WordDocument report = new WordDocument();
            report.EnsureMinimal();
            IWSection section = report.AddSection();
            IWParagraph name = section.AddParagraph();
            name.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
            IWTextRange nameText = name.AppendText("Jack Frey");
            nameText.CharacterFormat.FontName = "Times New Roman";
            nameText.CharacterFormat.FontSize = 14;
            MemoryStream stream = new MemoryStream();
            stream.Position = 0;
            report.Close();
            return File(stream, "application/msword", "JackFreyReport.docx");
        }
    }
}