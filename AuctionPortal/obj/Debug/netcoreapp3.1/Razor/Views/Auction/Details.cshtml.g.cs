#pragma checksum "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "07dea4ae54ecbb84b620e95b89ad4c6d06cf8907"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Auction_Details), @"mvc.1.0.view", @"/Views/Auction/Details.cshtml")]
namespace AspNetCore
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
#line 1 "C:\Users\Nikola\Desktop\AuctionPortal\Views\_ViewImports.cshtml"
using AuctionPortal;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Nikola\Desktop\AuctionPortal\Views\_ViewImports.cshtml"
using AuctionPortal.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"07dea4ae54ecbb84b620e95b89ad4c6d06cf8907", @"/Views/Auction/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cc4ae3245cf5894f0bd17ab20552978964d5c15e", @"/Views/_ViewImports.cshtml")]
    public class Views_Auction_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AuctionPortal.Models.View.AuctionView>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\Details.cshtml"
  
    ViewData["Title"] = "Details";

    string id = "auctionDetails" + Model.auction.id.ToString();

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Auction Details</h1>\r\n\r\n<hr>\r\n<br>\r\n\r\n<div");
            BeginWriteAttribute("id", " id=\"", 207, "\"", 215, 1);
#nullable restore
#line 14 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\Details.cshtml"
WriteAttributeValue("", 212, id, 212, 3, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"row\">\r\n");
#nullable restore
#line 15 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\Details.cshtml"
       await Html.RenderPartialAsync("AuctionDetails", Model); 

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AuctionPortal.Models.View.AuctionView> Html { get; private set; }
    }
}
#pragma warning restore 1591
