#pragma checksum "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a619d6feae96c1f891aa6a9650bdf7ae2ff517a8"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a619d6feae96c1f891aa6a9650bdf7ae2ff517a8", @"/Views/Auction/AuctionList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cc4ae3245cf5894f0bd17ab20552978964d5c15e", @"/Views/_ViewImports.cshtml")]
    public class Views_Auction_AuctionList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<AuctionPortal.Models.View.AuctionView>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 4 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionList.cshtml"
 foreach (var item in Model) {

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div id=\"auctions\" class=\"col-sm-4 mb-4\">\r\n");
#nullable restore
#line 6 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionList.cshtml"
          
            await Html.RenderPartialAsync("Auction", item);
        

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n");
#nullable restore
#line 10 "C:\Users\Nikola\Desktop\AuctionPortal\Views\Auction\AuctionList.cshtml"
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<AuctionPortal.Models.View.AuctionView>> Html { get; private set; }
    }
}
#pragma warning restore 1591
