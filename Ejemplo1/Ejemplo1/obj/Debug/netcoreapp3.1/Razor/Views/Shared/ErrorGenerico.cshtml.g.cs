#pragma checksum "C:\Users\Dani\OneDrive\Documents\CURSOS PROGRAMACION\Curso .NET Core\Ejemplo1\Ejemplo1\Views\Shared\ErrorGenerico.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "eca7813737ee845fab806ff60ec2e90b05e12ca7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_ErrorGenerico), @"mvc.1.0.view", @"/Views/Shared/ErrorGenerico.cshtml")]
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
#line 1 "C:\Users\Dani\OneDrive\Documents\CURSOS PROGRAMACION\Curso .NET Core\Ejemplo1\Ejemplo1\Views\_ViewImports.cshtml"
using Ejemplo1.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Dani\OneDrive\Documents\CURSOS PROGRAMACION\Curso .NET Core\Ejemplo1\Ejemplo1\Views\_ViewImports.cshtml"
using Ejemplo1.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Dani\OneDrive\Documents\CURSOS PROGRAMACION\Curso .NET Core\Ejemplo1\Ejemplo1\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"eca7813737ee845fab806ff60ec2e90b05e12ca7", @"/Views/Shared/ErrorGenerico.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"46b4a4b300bbe46a9a692c134b2c62bbd9323e19", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_ErrorGenerico : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\Dani\OneDrive\Documents\CURSOS PROGRAMACION\Curso .NET Core\Ejemplo1\Ejemplo1\Views\Shared\ErrorGenerico.cshtml"
 if (ViewBag.ErrorTitle == null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h3>\r\n        Ocurrio un error procesando su solicitud\r\n    </h3>\r\n    <h5>Por favor contacta con dany12rp13@gmail.com</h5>\r\n    <br />\r\n");
#nullable restore
#line 8 "C:\Users\Dani\OneDrive\Documents\CURSOS PROGRAMACION\Curso .NET Core\Ejemplo1\Ejemplo1\Views\Shared\ErrorGenerico.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1 class=\"text-danger\">");
#nullable restore
#line 11 "C:\Users\Dani\OneDrive\Documents\CURSOS PROGRAMACION\Curso .NET Core\Ejemplo1\Ejemplo1\Views\Shared\ErrorGenerico.cshtml"
                       Write(ViewBag.ErrorTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n    <h6 class=\"text-danger\">");
#nullable restore
#line 12 "C:\Users\Dani\OneDrive\Documents\CURSOS PROGRAMACION\Curso .NET Core\Ejemplo1\Ejemplo1\Views\Shared\ErrorGenerico.cshtml"
                       Write(ViewBag.ErrorMessage);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h6>\r\n");
#nullable restore
#line 13 "C:\Users\Dani\OneDrive\Documents\CURSOS PROGRAMACION\Curso .NET Core\Ejemplo1\Ejemplo1\Views\Shared\ErrorGenerico.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<!--ANTES-->\r\n");
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