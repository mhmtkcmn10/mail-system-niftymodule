#pragma checksum "C:\Users\mehmet.kocaman\source\repos\mailMebis_REPO\mailNiftymebis_netcore\Views\Shared\_CookiePopup.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "39f1c074a64fa27d00db41e8804a4f53cefd69b9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__CookiePopup), @"mvc.1.0.view", @"/Views/Shared/_CookiePopup.cshtml")]
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
#line 1 "C:\Users\mehmet.kocaman\source\repos\mailMebis_REPO\mailNiftymebis_netcore\Views\_ViewImports.cshtml"
using mailNiftymebis_netcore;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\mehmet.kocaman\source\repos\mailMebis_REPO\mailNiftymebis_netcore\Views\_ViewImports.cshtml"
using mailNiftymebis_netcore.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"39f1c074a64fa27d00db41e8804a4f53cefd69b9", @"/Views/Shared/_CookiePopup.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d61746cd77d38f309c2f6c7fde1baebf2d8c502d", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__CookiePopup : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<div class=""modal fade"" id=""myModalCookie"" tabindex=""-1"" role=""dialog"" aria-labelledby=""exampleModalLabel"" aria-hidden=""true"" data-toggle=""modal"" data-backdrop=""static"" data-keyboard=""false"">
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h4 class=""modal-title"">Oturum Süresi</h4>

            </div>

            <div class=""modal-body"">
                <span id=""sayac""></span> sn.    Oturum süreniz doldu. Devam etmek ister misiniz ?
            </div>

            <div class=""modal-footer"">
                <button class=""btn btn-default"" data-dismiss=""modal"" type=""button"" id=""btnAuthenticationClose"">Close</button>
                <button class=""btn btn-primary"" type=""button"" id=""btnAuthenticationConfirm"">Confirm</button>
            </div>
        </div>

    </div>
</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
