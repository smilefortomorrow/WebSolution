using Azure;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using Radzen;

using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;


namespace WorkFlowSystem.InvoiceFunction
{
    public static class Extensions
    {
        public static float ToDpi(this float centimeter)
        {
            var inch = centimeter / 2.54;
            return (float)(inch * 72);
        }
    }

    public class ClaPDF
    {
        [Inject] public PackageService PackageService { get; set; }

        public static BasicConverter converter = new BasicConverter(new PdfTools());
        
        public async Task Generate(IJSRuntime js, InvoiceView invoice, List<PackageView> packageList)
        {
            byte[] data = null;
            try {
                data = ReportPDF(invoice, packageList);
            }catch(Exception ex) {
                return;
            }

            await js.InvokeVoidAsync("jsDownloadFile",
                                invoice.InvoiceNo+".pdf",
                                data
                                );
        }



        private byte[] ReportPDF(InvoiceView invoice, List<PackageView> packageList)
        {
            string html = ClaMail.GenerateBody(invoice, packageList);

            var globalSettings = new GlobalSettings {
                ColorMode = ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Landscape,
                PaperSize = PaperKind.A4,
                // Other necessary properties here...
            };

            var objectSettings = new ObjectSettings {
                PagesCount = true,
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8" },
                // Other necessary properties here...
            };

            var pdfDocument = new HtmlToPdfDocument() {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            // Convert HTML string to PDF and get the buffer
            var pdfBuffer = converter.Convert(pdfDocument);

            // Return the PDF buffer
            return pdfBuffer;

        }
    }
}
