#pragma checksum "/Users/ivy/Desktop/web/CityOfHopeVolunteerTracking/Pages/VolunteerReports.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c55538c198a4ac333baafe9d7711200ba1633051"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(CoHO.Pages.Pages_VolunteerReports), @"mvc.1.0.razor-page", @"/Pages/VolunteerReports.cshtml")]
namespace CoHO.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/ivy/Desktop/web/CityOfHopeVolunteerTracking/Pages/_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/ivy/Desktop/web/CityOfHopeVolunteerTracking/Pages/_ViewImports.cshtml"
using CoHO;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "/Users/ivy/Desktop/web/CityOfHopeVolunteerTracking/Pages/_ViewImports.cshtml"
using CoHO.Data;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c55538c198a4ac333baafe9d7711200ba1633051", @"/Pages/VolunteerReports.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b465d8a6655fc06d310850aeff27ffb7f1e02066", @"/Pages/_ViewImports.cshtml")]
    public class Pages_VolunteerReports : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "/Users/ivy/Desktop/web/CityOfHopeVolunteerTracking/Pages/VolunteerReports.cshtml"
  
    ViewData["Title"] = "VolunteerReports";
    Layout = "~/Pages/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"


<div class=""createReportPage"">
    <div class=""container"">
        <div class=""row"">
            <div class=""col-sm-4"">
                <label style=""padding-top: 75px"">
                    <p style=""font-size: 40px"">
                        Volunteer Reports
                    </p>
                </label>
            </div>
            <div class=""col-sm-4"">
                <label style=""padding-bottom: 20px; padding-top: 20px"">
                    Start Date: <input type=""date"" />
                </label>
                <label style=""padding-bottom: 20px"">
                    End Date: <input type=""date"" />
                </label>
                <label style=""padding-left: 70px"">
                    <button type=""button"" id=""volReport"">Generate Report</button>
                </label>
            </div>
            <div class=""col-sm-4"">
                <label style=""padding-left: 120px; padding-top:20px"">
                    <a href =""/Identity/Account/Manage/ChangePassword"">Change Password</a>
   ");
            WriteLiteral(@"             </label>
                <label style=""padding-left: 120px; padding-top:20px"">
                    <button type=""button"" id=""changeHours"">Time Change Request</button>
                </label>
            </div>
        </div>
    </div>
</div>

");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<CoHO.Pages.VolunteerReportsModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<CoHO.Pages.VolunteerReportsModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<CoHO.Pages.VolunteerReportsModel>)PageContext?.ViewData;
        public CoHO.Pages.VolunteerReportsModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591