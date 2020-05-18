#pragma checksum "C:\Users\mplad\source\repos\MLDShopping-Admin\MLDShopping-Admin\Views\Inventory\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "901d47012ab36aa4a0d95d4e39454b2df7c05ff7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Inventory_Index), @"mvc.1.0.view", @"/Views/Inventory/Index.cshtml")]
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
#line 1 "C:\Users\mplad\source\repos\MLDShopping-Admin\MLDShopping-Admin\Views\_ViewImports.cshtml"
using MLDShopping_Admin;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\mplad\source\repos\MLDShopping-Admin\MLDShopping-Admin\Views\_ViewImports.cshtml"
using MLDShopping_Admin.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"901d47012ab36aa4a0d95d4e39454b2df7c05ff7", @"/Views/Inventory/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e003c84cb468250d08076c5ee79fe52094a0abf4", @"/Views/_ViewImports.cshtml")]
    public class Views_Inventory_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<MLDShopping_Admin.Models.ProductVM>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\mplad\source\repos\MLDShopping-Admin\MLDShopping-Admin\Views\Inventory\Index.cshtml"
  
    var title = ViewData["Title"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral(@"
<!-- Main content -->
<section class=""content container-fluid"">
    <div class=""row mt-3 pl-3"">


        <div class=""card w-100"">
            <div class=""card-body p-4"">
                <div class=""row"">
                    <div class=""input-group mb-3"">
                        <input type=""text"" class=""form-control form-control-sm"" placeholder=""Type to search..."" aria-label=""Type to search..."" aria-describedby=""Type to search..."">
                        <div class=""input-group-append"">
                            <button class=""btn btn-sm btn-outline-secondary"" type=""button"">Button</button>
                        </div>
                    </div>
                </div>
                <div class=""row mt-3"">
                    <button class=""btn btn-primary btn-sm"">Add New Product</button>
                </div>
                <div class=""row mt-3"">
                    <table class=""table table-light table-bordered table-hover table-sm"" id=""tblInventory"">
                        <th");
            WriteLiteral(@"ead class=""thead-dark"">
                            <tr>
                                <th>Product Id</th>
                                <th>Image</th>
                                <th>Name</th>
                                <th>Category</th>
                                <th>Price</th>
                                <th>Quantity</th>
                                <th>Action</th>

                            </tr>
                        </thead>
                        <tbody>
");
            WriteLiteral("                        </tbody>\r\n                    </table>\r\n                </div>\r\n            </div>\r\n        </div>\r\n\r\n\r\n    </div>\r\n</section>\r\n\r\n");
            DefineSection("scripts", async() => {
                WriteLiteral(@"
    <!-- DataTables -->
    <script type=""text/javascript"" charset=""utf8"" src=""//cdn.datatables.net/1.10.5/js/jquery.dataTables.js""></script>
    <script language=""javascript"">
        $(document).ready(function () {
            $(""#tblInventory"").DataTable({
                serverSide: true,
                ajax: {
                    url: '/inventory/read/',
                    dataSrc: """"
                },
                paging: true,
                dataSrc: """",
                sAjaxDataProp: """",
                columns: [
                    { data: ""ProductId"" },
                    { data: ""Image"" },
                    { data: ""Name"" },
                    { data: ""Category"" },
                    { data: ""Price"" },
                    { data: ""Quantity"" },
                    {
                        data: ""ProductId"",
                        render: function (data) {
                            console.log(data)
                            return '<a href=""/inventory/edi");
                WriteLiteral("t/\' + data + \'\">Edit</a>\';\r\n                        }\r\n                    }\r\n                ]\r\n            });\r\n        })\r\n    </script>\r\n\r\n\r\n");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<MLDShopping_Admin.Models.ProductVM>> Html { get; private set; }
    }
}
#pragma warning restore 1591
