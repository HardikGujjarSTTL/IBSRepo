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
using IBS.DataAccess;

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
            ViewBag.Region = Region;
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<AllGeneratedBills> dTResult = allGeneratedBillsRepository.GetBillDetails(dtParameters);
            GlobalDeclaration.AllGeneratedBillModel = dTResult.data.ToList();
            return Json(dTResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBill([FromBody] AllGeneratedBills fromdata) //CreateBill
        {
            AllGeneratedBills model = new();
            string htmlContent = "";
            try
            {
                model = allGeneratedBillsRepository.CreateBills(fromdata);

                string FolderName = GetFolderNameByRegion(model.REGION_CODE);

                if (model.lstBillDetailsForPDF.Count() > 0)
                {
                    foreach (var item in model.lstBillDetailsForPDF)
                    {
                        item.items = allGeneratedBillsRepository.GetBillItems(item.BILL_NO);
                        decimal totalBillAmount = (item.sgst) + (item.cgst) + (item.igst) + (item.insp_fee);
                        item.BILL_AMOUNT = totalBillAmount;

                        DateTime billDate = Convert.ToDateTime(item.BILL_DT);
                        string formattedDate = billDate.ToString("yyyy-MM-dd");

                        var expire = "2020-10-01";

                        if (Convert.ToDateTime(formattedDate) >= Convert.ToDateTime(expire) && (item.qr_code == "" || item.qr_code == null))
                        {
                            continue;
                        }

                        var path = env.WebRootPath + "/ReadWriteData/" + FolderName;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        if (Directory.Exists(path))
                        {
                            // check if the PDF file exists
                            string pdfFilePath = Path.Combine(path, item.BILL_NO + ".pdf");
                            bool fileExists = System.IO.File.Exists(pdfFilePath);

                            if (!fileExists)
                            {
                                if (model.REGION_CODE == "N")
                                {
                                    htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/NorthBill.cshtml", item);
                                }
                                else if (model.REGION_CODE == "S")
                                {
                                    htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/SouthBill.cshtml", item);
                                }
                                else if (model.REGION_CODE == "E")
                                {
                                    htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/EastBill.cshtml", item);
                                }
                                else if (model.REGION_CODE == "W")
                                {
                                    htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/WestBill.cshtml", item);
                                }
                                else if (model.REGION_CODE == "Q")
                                {
                                    htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/COQABill.cshtml", item);
                                }
                                else if (model.REGION_CODE == "C")
                                {
                                    htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/CentralBill.cshtml", item);
                                }

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
                        }
                    }
                }
                
                AlertAddSuccess("Bill Generated !!");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AllGeneratedBills", "NorthBillGeneratePDF", 1, GetIPAddress());
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ReturnBill([FromBody] AllGeneratedBills fromdata)
        {
            AllGeneratedBills model = new();
            string htmlContent = "";
            try
            {
                model = allGeneratedBillsRepository.ReturnBills(fromdata);

                string FolderName = GetFolderNameByRegion(model.REGION_CODE);

                if (model.lstBillDetailsForPDF.Count() > 0)
                {
                    foreach (var item in model.lstBillDetailsForPDF)
                    {
                        decimal totalBillAmount = (item.sgst) + (item.cgst) + (item.igst) + (item.insp_fee);
                        item.BILL_AMOUNT = totalBillAmount;

                        DateTime billDate = Convert.ToDateTime(item.BILL_DT);
                        string formattedDate = billDate.ToString("yyyy-MM-dd");

                        var expire = "2020-10-01";

                        if (Convert.ToDateTime(formattedDate) >= Convert.ToDateTime(expire) && (item.qr_code == "" || item.qr_code == null))
                        {
                            continue;
                        }

                        var path = env.WebRootPath + "/ReadWriteData/" + FolderName;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            // check if the PDF file exists
                            string pdfFilePath = Path.Combine(path, item.BILL_NO + ".pdf");
                            bool fileExists = System.IO.File.Exists(pdfFilePath);

                            if (fileExists)
                            {
                                // If the file exists, delete it
                                System.IO.File.Delete(pdfFilePath);
                            }

                            if (model.REGION_CODE == "N")
                            {
                                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/NorthBill.cshtml", item);
                            }
                            else if (model.REGION_CODE == "S")
                            {
                                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/SouthBill.cshtml", item);
                            }
                            else if (model.REGION_CODE == "E")
                            {
                                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/EastBill.cshtml", item);
                            }
                            else if (model.REGION_CODE == "W")
                            {
                                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/WestBill.cshtml", item);
                            }
                            else if (model.REGION_CODE == "Q")
                            {
                                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/COQABill.cshtml", item);
                            }
                            else if (model.REGION_CODE == "C")
                            {
                                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/CentralBill.cshtml", item);
                            }

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

                                // Save the new PDF file
                                await System.IO.File.WriteAllBytesAsync(pdfFilePath, pdfBytes);
                            }

                        }
                    }
                }

                AlertAddSuccess("Bill Returned !!");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AllGeneratedBills", "ReturnBillPDF", 1, GetIPAddress());
            }

            return View(model);
        }

        public IActionResult NorthBill(AllGeneratedBills obj)
        {
            obj.REGION_CODE = "N";
            obj.CLIENT_TYPE = "R";
            obj.FromDate = "01/01/2021";
            obj.ToDate = "31/01/2021";
            obj.BPO_NAME = null;

            AllGeneratedBills model = allGeneratedBillsRepository.CreateBills(obj);
            if (model.lstBillDetailsForPDF.Count() > 0)
            {
                foreach (var item in model.lstBillDetailsForPDF)
                {
                    item.items = allGeneratedBillsRepository.GetBillItems(item.BILL_NO);
                    decimal totalBillAmount = (item.sgst) + (item.cgst) + (item.igst) + (item.insp_fee);
                    item.BILL_AMOUNT = totalBillAmount;
                }
            }
            return View(model.lstBillDetailsForPDF[0]);
        }

        public IActionResult SouthBill(AllGeneratedBills obj)
        {
            obj.REGION_CODE = "N";
            obj.CLIENT_TYPE = "R";
            obj.FromDate = "01/01/2021";
            obj.ToDate = "31/01/2021";
            obj.BPO_NAME = null;

            AllGeneratedBills model = allGeneratedBillsRepository.CreateBills(obj);
            if (model.lstBillDetailsForPDF.Count() > 0)
            {
                foreach (var item in model.lstBillDetailsForPDF)
                {
                    item.items = allGeneratedBillsRepository.GetBillItems(item.BILL_NO);
                    decimal totalBillAmount = (item.sgst) + (item.cgst) + (item.igst) + (item.insp_fee);
                    item.BILL_AMOUNT = totalBillAmount;
                }
            }
            return View(model.lstBillDetailsForPDF[0]);
        }

        public IActionResult COQABill(AllGeneratedBills obj)
        {
            obj.REGION_CODE = "N";
            obj.CLIENT_TYPE = "R";
            obj.FromDate = "01/01/2021";
            obj.ToDate = "31/01/2021";
            obj.BPO_NAME = null;

            AllGeneratedBills model = allGeneratedBillsRepository.CreateBills(obj);
            if (model.lstBillDetailsForPDF.Count() > 0)
            {
                foreach (var item in model.lstBillDetailsForPDF)
                {
                    item.items = allGeneratedBillsRepository.GetBillItems(item.BILL_NO);
                    decimal totalBillAmount = (item.sgst) + (item.cgst) + (item.igst) + (item.insp_fee);
                    item.BILL_AMOUNT = totalBillAmount;
                }
            }
            return View(model.lstBillDetailsForPDF[0]);
        }

        private string GetFolderNameByRegion(string regionCode)
        {
            if (regionCode == "N")
            { regionCode = "North_Bills"; }
            else if (regionCode == "S")
            { regionCode = "South_Bills"; }
            else if (regionCode == "E")
            { regionCode = "East_Bills"; }
            else if (regionCode == "W")
            { regionCode = "West_Bills"; }
            else if (regionCode == "C")
            { regionCode = "Central_Bills"; }
            else if (regionCode == "Q")
            { regionCode = "Q_Bills"; }

            return regionCode;
        }

        #region GeneratePDF
        public async Task<IActionResult> GeneratePDF(string BillNo)
        {
            List<T22Bill> BillData = allGeneratedBillsRepository.GetBillByBillNo(BillNo);

            if (BillData.Count > 0 && BillData[0].BillResentStatus == "R")
            {
                bool isBillResentCountNullOrEmpty = !BillData[0].BillResentCount.HasValue;

                int count = isBillResentCountNullOrEmpty ? 1 : (BillData[0].BillResentCount.Value ? 2 : 0);

                string Bill_No = allGeneratedBillsRepository.UpdateBillCount(BillNo,count);
            }

            string Bill_Date = allGeneratedBillsRepository.UpdateGEN_Bill_Date(BillNo);

            string pdfFileName = "";
            string htmlContent = string.Empty;
            List<AllGeneratedBills> model = GlobalDeclaration.AllGeneratedBillModel;

            AllGeneratedBills selectedBill = model.FirstOrDefault(bill => bill.BILL_NO == BillNo);

            if (selectedBill.REGION_CODE == "North")
            {
                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/North_Bill.cshtml", selectedBill);
                pdfFileName = "NorthBill.pdf";
            }
            else if (selectedBill.REGION_CODE == "South")
            {
                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/SouthBill.cshtml", selectedBill);
                pdfFileName = "SouthBill.pdf";
            }
            else if (selectedBill.REGION_CODE == "East")
            {
                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/EastBill.cshtml", selectedBill);
                pdfFileName = "EastBill.pdf";
            }
            else if (selectedBill.REGION_CODE == "West")
            {
                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/WestBill.cshtml", selectedBill);
                pdfFileName = "WestBill.pdf";
            }
            else if (selectedBill.REGION_CODE == "Central")
            {
                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/CentralBill.cshtml", selectedBill);
                pdfFileName = "CentralBill.pdf";
            }
            else if (selectedBill.REGION_CODE == "Q")
            {
                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/COQABill.cshtml", selectedBill);
                pdfFileName = "Q.pdf";
            }

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

            return File(pdfContent, "application/pdf", pdfFileName);
        }
        #endregion
    }
}
