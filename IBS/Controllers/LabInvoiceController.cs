using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace IBS.Controllers
{
    public class LabInvoiceController : BaseController
    {
        private readonly ILabInvoiceRepository labInvoiceRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration config;

        public LabInvoiceController(ILabInvoiceRepository _labInvoiceRepository, IWebHostEnvironment env, IConfiguration _config)
        {
            labInvoiceRepository = _labInvoiceRepository;
            this.env = env;
            config = _config;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<labInvoicelst> model = labInvoiceRepository.GetLabInvoice(dtParameters);
            GlobalDeclaration.LabInvoiceReport = model.data.ToList();
            string FolderName = "Lab_Invoice";
            string htmlContent = "";
            //if (model != null)
            //{
            //    foreach (var item in model.data.ToList())
            //    {
            //        var path = env.WebRootPath + "/ReadWriteData/" + FolderName;
            //        if (!Directory.Exists(path))
            //        {
            //            Directory.CreateDirectory(path);
            //        }

            //        if (Directory.Exists(path))
            //        {
            //            //check if the PDF file exists
            //            string pdfFilePath = Path.Combine(path, item.InvoiceBillNo + ".pdf");
            //            bool fileExists = System.IO.File.Exists(pdfFilePath);

            //            if (!fileExists)
            //            {
            //                htmlContent = await this.RenderViewToStringAsync("/Views/LabInvoice/LabInvoicePDF.cshtml", item);

            //                await new BrowserFetcher().DownloadAsync();
            //                await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            //                {
            //                    Headless = true,
            //                    DefaultViewport = null
            //                });
            //                await using var page = await browser.NewPageAsync();
            //                await page.EmulateMediaTypeAsync(MediaType.Screen);
            //                await page.SetContentAsync(htmlContent);

            //                var pdfContent = await page.PdfStreamAsync(new PdfOptions
            //                {
            //                    Landscape = true,
            //                    Format = PaperFormat.A4,
            //                    PrintBackground = true
            //                });

            //                await using (var pdfStream = new MemoryStream())
            //                {
            //                    await pdfContent.CopyToAsync(pdfStream);
            //                    byte[] pdfBytes = pdfStream.ToArray();
            //                    string base64String = Convert.ToBase64String(pdfBytes);
            //                    await System.IO.File.WriteAllBytesAsync(pdfFilePath, pdfBytes);
            //                }

            //            }
            //        }
            //    }
            //}

            return Json(model);
        }

        #region GeneratePDF
        public async Task<IActionResult> GeneratePDF(string InvoiceBillNo)
        {
            string pdfFileName = "";
            string htmlContent = string.Empty;
            List<labInvoicelst> selectedBill = GlobalDeclaration.LabInvoiceReport;

            labInvoicelst model = selectedBill.FirstOrDefault(bill => bill.InvoiceBillNo == InvoiceBillNo);

            string path = env.WebRootPath + "/images/";
            var imagePath = Path.Combine(path, "rites-logo.png");
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            model.base64Logo = "data:image/png;base64," + Convert.ToBase64String(imageBytes);


            htmlContent = await this.RenderViewToStringAsync("/Views/LabInvoice/LabInvoicePDF.cshtml", selectedBill);
            pdfFileName = "Lab_Invoice.pdf";

            await new BrowserFetcher().DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                DefaultViewport = null
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(htmlContent);

            string cssPath = env.WebRootPath + "/css/report.css";
            AddTagOptions bootstrapCSS = new AddTagOptions() { Path = cssPath };
            await page.AddStyleTagAsync(bootstrapCSS);

            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Landscape = false,
                Format = PaperFormat.A4,
                PrintBackground = false
            });

            await browser.CloseAsync();

            return File(pdfContent, "application/pdf", pdfFileName);
        }
        #endregion
    }
}
