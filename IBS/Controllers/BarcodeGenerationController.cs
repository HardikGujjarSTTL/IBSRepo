
using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using IronBarCode;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Text.Json;

namespace IBS.Controllers
{
    public class BarcodeGenerationController : BaseController
    {
        #region Variables
        private readonly IBarcodeGeneration BarcodeGen;
        private readonly IWebHostEnvironment env;
        #endregion
        public BarcodeGenerationController(IBarcodeGeneration _BarcodeGen, IWebHostEnvironment _env)
        {
            BarcodeGen = _BarcodeGen;
            this.env = _env;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<BarcodeGenerate> dTResult = BarcodeGen.GetBarcodeData(dtParameters);
            return Json(dTResult);
        }
        public IActionResult AddBarcode(BarcodeGenerate barcodeGenerate)
        {
            barcodeGenerate.CURRENT_DATE = DateTime.Now.ToString("dd/MM/yyyy");
            barcodeGenerate.Region = GetRegionCode;
            return View(barcodeGenerate);
        }
        [HttpPost]
        public IActionResult CaseNoSearch([FromBody] DTParameters dtParameters)
        {
            DTResult<BarcodeGenerate> dTResult = BarcodeGen.CaseNoSearch(dtParameters);
            return Json(dTResult);
        }
        [HttpPost]

        public IActionResult Save(BarcodeGenerate BarcodeGenerate)
        {
            string IPADDRESS = this.HttpContext.Connection.RemoteIpAddress.ToString();
            BarcodeGenerate.USERID = Convert.ToString(UserId);
            BarcodeGenerate.CREATEDBY = UserName.Trim();
            string Region = GetRegionCode;
            if (Region == "N")
                BarcodeGenerate.Region = "NR";
            else if (Region == "S")
                BarcodeGenerate.Region = "SR";
            else if (Region == "E")
                BarcodeGenerate.Region = "ER";
            else if (Region == "W")
                BarcodeGenerate.Region = "WR";
            else if (Region == "C")
                BarcodeGenerate.Region = "CR";
            try
            {
                string msg = "Barcode Generated Successfully.";

                bool dTResult = BarcodeGen.SaveBarCode(BarcodeGenerate, IPADDRESS);
                if (dTResult == true)
                {
                    return Json(new { status = true, responseText = msg, Id = dTResult });
                }
                else
                {
                    return Json(new { status = false, responseText = msg, Id = dTResult });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AddBarcode", "BarcodeGeneration", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        public IActionResult LabCalculation()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult LoadCalculation([FromBody] DTParameters dtParameters)
        {
            DTResult<BarcodeGenerate> dTResult = BarcodeGen.LoadCalculation(dtParameters);
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult InsertDataForLabTran(BarcodeGenerate BarcodeGenerate)
        {
            BarcodeGenerate.IPADDRESS = this.HttpContext.Connection.RemoteIpAddress.ToString();
            BarcodeGenerate.USERID = Convert.ToString(UserId);
            BarcodeGenerate.CREATEDBY = UserName.Trim();
            string Region = GetRegionCode;
            if (Region == "N")
                BarcodeGenerate.Region = "NR";
            else if (Region == "S")
                BarcodeGenerate.Region = "SR";
            else if (Region == "E")
                BarcodeGenerate.Region = "ER";
            else if (Region == "W")
                BarcodeGenerate.Region = "WR";
            else if (Region == "C")
                BarcodeGenerate.Region = "CR";
            BarcodeGenerate.CURRENT_DATE = DateTime.Now.ToString("dd/MM/yyyy");
            //BarcodeGenerate.TypeGST = Request.Form["TypeGST"];
            //BarcodeGenerate.SGST = Request.Form["SGST"];
            try
            {
                string msg = "Barcode Generated Successfully.";
                bool dTResult = BarcodeGen.InsertDataForLabTran(BarcodeGenerate);
                if (dTResult == true)
                {
                    return Json(new { status = true, responseText = msg, Id = dTResult, BarcodeGenerate = BarcodeGenerate });
                }
                else
                {
                    return Json(new { status = false, responseText = msg, Id = dTResult });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AddBarcode", "BarcodeGeneration", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        
        public IActionResult GenerateBarcode(string Barcode, int quantity)
        {
            try
            {
                
                List<string> imageUrls = new List<string>();

                for (int i = 0; i < quantity; i++)
                {
                    string uniqueBarcode = $"{Barcode}\n{DateTime.Now.ToString("dd-MM-yyyy")}";
                    GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode(uniqueBarcode, BarcodeWriterEncoding.Code128);
                    barcode.ResizeTo(400, 120);
                    barcode.AddBarcodeValueTextBelowBarcode();
                    
                    barcode.ChangeBarCodeColor(System.Drawing.Color.Black);
                    barcode.SetMargins(10);

                    string path = Path.Combine(env.WebRootPath, "GeneratedBarcode");
                    //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "GeneratedBarcode");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string fileName = $"barcode_{i + 1}.png";
                    string filePath = Path.Combine(env.WebRootPath, "GeneratedBarcode", fileName);
                    //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "GeneratedBarcode", fileName);
                    barcode.SaveAsPng(filePath);
                    //string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}{filePath}";
                    //string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/wwwroot/ReadWriteData/GeneratedBarcode/{fileName}";
                    string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedBarcode/" + fileName;
                    //string imageUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "GeneratedBarcode" , fileName);
                    imageUrls.Add(imageUrl);
                }

                ViewBag.QrCodeUris = imageUrls;
            }
            catch (Exception)
            {
                throw;
            }
            return PartialView();
            
        }
        [HttpPost]
        public async Task<IActionResult> GeneratePDF(string htmlContent)
        {
           
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

    }
}
