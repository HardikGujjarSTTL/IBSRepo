using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
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
        public async Task<IActionResult> LabListView(string FromDate,string ToDate,string Region)
        {
            LabInvoiceReportModel model = labInvoiceRepository.GetLabInvoice(FromDate, ToDate, Region);
            //GlobalDeclaration.AllGeneratedBillModel = dTResult.data.ToList();
            string FolderName = "Lab_Invoice";
            string htmlContent = "";
            if (model.lstlabInvoicelst.Count() > 0)
            {
                foreach (var item in model.lstlabInvoicelst)
                {
                    var path = env.WebRootPath + "/ReadWriteData/" + FolderName;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (Directory.Exists(path))
                    {
                        // check if the PDF file exists
                        string pdfFilePath = Path.Combine(path, item.InvoiceBillNo + ".pdf");
                        bool fileExists = System.IO.File.Exists(pdfFilePath);

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
                                Format = PaperFormat.A4,
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
                    }
                }
            }

            return PartialView(model);
        }

        //public async Task<IActionResult> GeneratePDF(DTResult<LabInvoiceReportModel>  dTResult)
        //{
        //    string htmlContent = "";
        //    string FolderName = GetFolderNameByRegion(dTResult.);
        //}

    }
}
