#pragma checksum "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionDetails.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "87a5b09436f87c1cca98d21f9cfe34a80be1fc73"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Auction_AuctionDetails), @"mvc.1.0.view", @"/Views/Auction/AuctionDetails.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"87a5b09436f87c1cca98d21f9cfe34a80be1fc73", @"/Views/Auction/AuctionDetails.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cc4ae3245cf5894f0bd17ab20552978964d5c15e", @"/Views/_ViewImports.cshtml")]
    public class Views_Auction_AuctionDetails : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AuctionPortal.Models.View.AuctionView>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionDetails.cshtml"
  
    string style_class = "";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div");
            BeginWriteAttribute("id", " id=\"", 92, "\"", 114, 1);
#nullable restore
#line 7 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionDetails.cshtml"
WriteAttributeValue("", 97, Model.auction.id, 97, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"col-sm-4\">\r\n");
#nullable restore
#line 8 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionDetails.cshtml"
       await Html.RenderPartialAsync("Auction", Model); 

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>
<div class=""col-sm-2""></div>
<div class=""col-sm-4"">
    <table class=""table table-hover text-center"">
        <thead class=""thead-light"">
            <tr>
                <th>
                    Bidder
                </th>
                <th>
                    Bidding time
                </th>
            </tr>
        </thead>
        <tbody>
");
#nullable restore
#line 24 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionDetails.cshtml"
             if(Model.auction.biddingList.Any()) {
                for (int i = 0; i < (Model.auction.biddingList.Count > 10 ? 10 : Model.auction.biddingList.Count); i++)
                {
                    if(i == 0) { style_class = "table-success"; }
                    else { style_class = ""; }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <tr");
            BeginWriteAttribute("class", " class=\"", 898, "\"", 918, 1);
#nullable restore
#line 29 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionDetails.cshtml"
WriteAttributeValue("", 906, style_class, 906, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                        <td>\r\n                            ");
#nullable restore
#line 31 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionDetails.cshtml"
                       Write(Model.auction.biddingList.ElementAt(i).user.UserName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
#nullable restore
#line 34 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionDetails.cshtml"
                       Write(Model.auction.biddingList.ElementAt(i).bidTime);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </td>\r\n                    </tr>\r\n");
#nullable restore
#line 37 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionDetails.cshtml"
                }
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n</div>");
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