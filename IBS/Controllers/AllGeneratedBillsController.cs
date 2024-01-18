using IBS.Helper;
using IBS.Interfaces.WebsitePages;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using IBS.Helper;
using System.Collections.Specialized;
using System.Globalization;
using static Org.BouncyCastle.Math.EC.ECCurve;
using IBS.Repositories;
using DocumentFormat.OpenXml.Office2010.Excel;
using IBS.Interfaces;
using IBS.Filters;
using MessagePack;
using static IBS.Helper.Enums;
namespace IBS.Controllers
{
    //[Authorization]
    public class AllGeneratedBillsController : BaseController
    {
        private readonly IAllGeneratedBillsRepository allGeneratedBillsRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration config;

        public AllGeneratedBillsController(IAllGeneratedBillsRepository _allGeneratedBillsRepository, IWebHostEnvironment env, IConfiguration _config)
        {
            allGeneratedBillsRepository = _allGeneratedBillsRepository;
            this.env = env;
            config = _config;
        }

        [Authorization("AllGeneratedBills", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<AllGeneratedBills> dTResult = allGeneratedBillsRepository.GetBillDetails(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult NorthBillGeneratePDF(AllGeneratedBills fromdata) //CreateBill
        {
            AllGeneratedBills model = allGeneratedBillsRepository.GenerateBill(fromdata);

            string regionCode = "";
            if (model.REGION_CODE == "N")
            { regionCode = "North_Bills"; }
            else if (model.REGION_CODE == "S")
            { regionCode = "South_Bills"; }
            else if (model.REGION_CODE == "E")
            { regionCode = "East_Bills"; }
            else if (model.REGION_CODE == "W")
            { regionCode = "West_Bills"; }
            else if (model.REGION_CODE == "C")
            { regionCode = "Central_Bills"; }
            else if (model.REGION_CODE == "Q")
            { regionCode = "Q_Bills"; }

            if (model.lstBillDetailsForPDF.Count() > 0)
            {
                foreach (var item in model.items)
                {
                    //decimal totalBillAmount = (item.sgst ?? 0) + (item.cgst ?? 0) + (item.igst ?? 0) + (item.insp_fee ?? 0);
                    //item.BILL_AMOUNT = totalBillAmount;

                    //DateTime billDate = Convert.ToDateTime(item.BILL_DT);
                    //string formattedDate = billDate.ToString("yyyy-MM-dd");

                    //var expire = "2020-10-01";

                    //if (Convert.ToDateTime(formattedDate) >= Convert.ToDateTime(expire) && (item.qr_code == "" || item.qr_code == null))
                    //{
                    //    continue;
                    //}

                    //var path = env.WebRootPath + "/ReadWriteData/" + regionCode;
                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}
                    //else
                    //{
                    //    string pdfFilePath = Path.Combine(path, model.BILL_NO + ".PDF");

                    //    // check bill No Pdf exists.
                    //    if (System.IO.File.Exists(pdfFilePath))
                    //    {
                    //        // PDF file exists
                    //    }
                    //    else
                    //    {
                    //        // PDF file does not exist
                    //    }
                    //}
                }
            }

            return View(model);
        }
        
        [HttpPost]
        public IActionResult ReturnBill([FromBody] AllGeneratedBills fromdata)
        {
            return View();
        }

        public IActionResult NorthBill(AllGeneratedBills obj)
        {
            AllGeneratedBills model = new AllGeneratedBills();//allGeneratedBillsRepository.GenerateBill(fromdata);
            return View(model);
        }

        #region GeneratePDF
        [HttpPost]
        public async Task<IActionResult> GeneratePDF(AllGeneratedBills model)
        {
            string htmlContent = string.Empty;
            //OnlinePaymentGateway model = GlobalDeclaration.OnlinePaymentResponse;
            htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/NorthBillGeneratePDF.cshtml", model);

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
                Landscape = true,
                Format = PaperFormat.Letter,
                PrintBackground = true
            });

            await browser.CloseAsync();

            return File(pdfContent, "application/pdf", Guid.NewGuid().ToString() + ".pdf");
        }
        #endregion
    }
}
