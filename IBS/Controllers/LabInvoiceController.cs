using IBS.Filters;
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

        [Authorization("LabInvoice", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LabInvoiceList(string FromDate, string ToDate, string Region)
        {
            labInvoicelst model = labInvoiceRepository.GetLabInvoice(FromDate, ToDate, Region);
            model.RegionChar = Region;
            GlobalDeclaration.LabInvoiceReport = model;
            string FolderName = "Lab_Invoice_SIGN";
            string htmlContent = "";
            if (model.lstlabInvoicelst.Count > 0)
            {
                foreach (var item in model.lstlabInvoicelst)
                {
                    item.items = labInvoiceRepository.GetBillItems(item.InvoiceNo);
                    decimal? totalTestingCharges = item.items.Sum(billItem => billItem.TESTING_CHARGES);
                    decimal? SubTotal = item.items.Sum(billItem => billItem.IGST + totalTestingCharges);
                    item.TotalTESTING_CHARGES = totalTestingCharges;
                    item.GrandTotal = SubTotal;

                    string imgPath = env.WebRootPath + "/images/";
                    var imagePath = Path.Combine(imgPath, "rites-logo.png");
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                    model.base64Logo = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

                    if (!string.IsNullOrEmpty(item.qr_code))
                    {
                        item.qr_code = Common.QRCodeGenerate(item.qr_code);
                    }

                    var path = env.WebRootPath + "/ReadWriteData/" + FolderName;
                    var RelativePath = "/ReadWriteData/Lab_Invoice_SIGN/";
                    model.pdfFolder = RelativePath;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (Directory.Exists(path))
                    {
                        //check if the PDF file exists
                        string pdfFilePath = Path.Combine(path, item.InvoiceBillNo + ".pdf");
                        bool fileExists = System.IO.File.Exists(pdfFilePath);
                        var PDFNamee = item.InvoiceBillNo + ".pdf";
                        if (!fileExists)
                        {
                            htmlContent = await this.RenderViewToStringAsync("/Views/LabInvoice/LabInvoicePDF.cshtml", item);

                            await new BrowserFetcher().DownloadAsync();
                            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                            {
                                Headless = true,
                                DefaultViewport = null
                            });
                            await using var page = await browser.NewPageAsync();
                            await page.EmulateMediaTypeAsync(MediaType.Screen);
                            await page.SetContentAsync(htmlContent);

                            var pdfContent = await page.PdfStreamAsync(new PdfOptions
                            {
                                Landscape = true,
                                Format = PaperFormat.Letter,
                                PrintBackground = true
                            });

                            await using (var pdfStream = new MemoryStream())
                            {
                                await pdfContent.CopyToAsync(pdfStream);
                                byte[] pdfBytes = pdfStream.ToArray();
                                string base64String = Convert.ToBase64String(pdfBytes);
                                await System.IO.File.WriteAllBytesAsync(pdfFilePath, pdfBytes);
                            }
                        }
                        string msg = labInvoiceRepository.UpdatePDFDetails(item, PDFNamee, RelativePath);
                    }
                }
            }

            return PartialView(model);
        }
    }
}
