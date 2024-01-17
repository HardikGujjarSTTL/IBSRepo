﻿using IBS.Helper;
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

namespace IBS.Controllers
{
    [Authorization]
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
        public IActionResult CreateBill([FromBody] AllGeneratedBills fromdata)
        {
            AllGeneratedBills model = allGeneratedBillsRepository.GenerateBill(fromdata);
            return View("NorthBillGeneratePDF", model);
        }
        
        [HttpPost]
        public IActionResult ReturnBill([FromBody] AllGeneratedBills fromdata)
        {
            return View();
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
