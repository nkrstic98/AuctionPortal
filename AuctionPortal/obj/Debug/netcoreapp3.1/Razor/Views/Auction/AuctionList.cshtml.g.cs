#pragma checksum "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2920849ac59f0b5f8b69ba5bd63602bfe75174ef"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Auction_AuctionList), @"mvc.1.0.view", @"/Views/Auction/AuctionList.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2920849ac59f0b5f8b69ba5bd63602bfe75174ef", @"/Views/Auction/AuctionList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cc4ae3245cf5894f0bd17ab20552978964d5c15e", @"/Views/_ViewImports.cshtml")]
    public class Views_Auction_AuctionList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AuctionPortal.Models.View.SearchModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionList.cshtml"
  
    string auctionId = "";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 7 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionList.cshtml"
  
    await Html.RenderPartialAsync ("SignalRScriptsPartial");

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 11 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionList.cshtml"
 foreach (var item in Model.auctionList) {
    auctionId = "auction" + item.auction.id;


#line default
#line hidden
#nullable disable
            WriteLiteral("    <div");
            BeginWriteAttribute("id", " id=\"", 257, "\"", 272, 1);
#nullable restore
#line 14 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionList.cshtml"
WriteAttributeValue("", 262, auctionId, 262, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"col-sm-4 mb-4\">\r\n");
#nullable restore
#line 15 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionList.cshtml"
           await Html.RenderPartialAsync("Auction", item); 

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n");
#nullable restore
#line 17 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionList.cshtml"
}

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AuctionPortal.Models.View.SearchModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
