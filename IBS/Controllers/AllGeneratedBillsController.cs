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
using System.Xml;
using Org.BouncyCastle.Asn1.Ocsp;


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
            List<DigitalSignModel> lstXmlData = new List<DigitalSignModel>();
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

                        string imgPath = env.WebRootPath + "/images/";
                        var imagePath = Path.Combine(imgPath, "rites-logo.png");
                        byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                        item.base64Logo = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

                        if (!string.IsNullOrEmpty(item.qr_code))
                        {
                            // Generate Base64String QR Code and Display in PDF.
                            item.qr_code = Common.QRCodeGenerate(item.qr_code);
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
                            Landscape = false,
                            Format = PaperFormat.Letter,
                            PrintBackground = false,
                        });

                        await using (var pdfStream = new MemoryStream())
                        {
                            await pdfContent.CopyToAsync(pdfStream);
                            byte[] pdfBytes = pdfStream.ToArray();
                            string base64String = Convert.ToBase64String(pdfBytes);
                            //base64String = base64String.Replace("\"", "");

                            pdfStream.Position = 0;
                            int pageCount = CountPdfPages(pdfStream);

                            string xmlData = GenerateDigitalSignatureXML(base64String, pageCount);

                            DigitalSignModel obj = new DigitalSignModel();
                            obj.Bill_No = item.BILL_NO;
                            obj.Base64String = xmlData;
                            lstXmlData.Add(obj);
                        }
                    }
                }
                //AlertAddSuccess("Bill Generated !!");
                return Json(new { status = 1, list = lstXmlData });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AllGeneratedBills", "CreateBill", 1, GetIPAddress());
            }

            return Json(new { status = 0, list = lstXmlData });
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
                        if (Directory.Exists(path))
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
                                Landscape = false,
                                Format = PaperFormat.Letter,
                                PrintBackground = false,
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
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AllGeneratedBills", "ReturnBill", 1, GetIPAddress());
            }
            return View(model);
        }

        public IActionResult NorthBill(AllGeneratedBills obj)
        {
            obj.FromDate = "01/01/2021";
            obj.ToDate = "31/01/2021";
            obj.REGION_CODE = "N";
            //obj.LOA = "A";
            obj.RailwayChk = "true";
            obj.CLIENT_NAME = null;
            obj.CLIENT_TYPE = "R";
            obj.BPO_NAME = null;

            AllGeneratedBills model = allGeneratedBillsRepository.CreateBills(obj);
            if (model.lstBillDetailsForPDF.Count() > 0)
            {
                foreach (var item in model.lstBillDetailsForPDF)
                {
                    item.items = allGeneratedBillsRepository.GetBillItems(item.BILL_NO);
                    decimal totalBillAmount = (item.sgst) + (item.cgst) + (item.igst) + (item.insp_fee);
                    item.BILL_AMOUNT = totalBillAmount;

                    if (!string.IsNullOrEmpty(item.qr_code))
                    {
                        item.qr_code = Common.QRCodeGenerate(item.qr_code);
                    }

                    string path = env.WebRootPath + "/images/";
                    var imagePath = Path.Combine(path, "rites-logo.png");
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                    item.base64Logo = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                }
            }

            if (model.lstBillDetailsForPDF.Count > 0)
            {

                return View(model.lstBillDetailsForPDF[0]);
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult SouthBill(AllGeneratedBills obj)
        {
            obj.FromDate = "01/01/2021";
            obj.ToDate = "31/01/2021";
            obj.REGION_CODE = "S";
            obj.LOA = "A";
            obj.RailwayChk = "true";
            obj.CLIENT_NAME = null;
            obj.CLIENT_TYPE = "R";
            obj.BPO_NAME = null;

            AllGeneratedBills model = allGeneratedBillsRepository.CreateBills(obj);
            if (model.lstBillDetailsForPDF.Count() > 0)
            {
                foreach (var item in model.lstBillDetailsForPDF)
                {
                    item.items = allGeneratedBillsRepository.GetBillItems(item.BILL_NO);
                    decimal totalBillAmount = (item.sgst) + (item.cgst) + (item.igst) + (item.insp_fee);
                    item.BILL_AMOUNT = totalBillAmount;

                    if (!string.IsNullOrEmpty(item.qr_code))
                    {
                        item.qr_code = Common.QRCodeGenerate(item.qr_code);
                    }

                    string path = env.WebRootPath + "/images/";
                    var imagePath = Path.Combine(path, "rites-logo.png");
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                    item.base64Logo = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                }
            }

            if (model.lstBillDetailsForPDF.Count > 0)
            {

                return View(model.lstBillDetailsForPDF[0]);
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult CentralBill(AllGeneratedBills obj)
        {
            obj.FromDate = "01/01/2021";
            obj.ToDate = "31/01/2021";
            obj.REGION_CODE = "N";
            obj.LOA = "A";
            obj.RailwayChk = "true";
            obj.CLIENT_NAME = null;
            obj.CLIENT_TYPE = "R";
            obj.BPO_NAME = null;

            AllGeneratedBills model = allGeneratedBillsRepository.CreateBills(obj);
            if (model.lstBillDetailsForPDF.Count() > 0)
            {
                foreach (var item in model.lstBillDetailsForPDF)
                {
                    item.items = allGeneratedBillsRepository.GetBillItems(item.BILL_NO);
                    decimal totalBillAmount = (item.sgst) + (item.cgst) + (item.igst) + (item.insp_fee);
                    item.BILL_AMOUNT = totalBillAmount;

                    if (!string.IsNullOrEmpty(item.qr_code))
                    {
                        item.qr_code = Common.QRCodeGenerate(item.qr_code);
                    }

                    string path = env.WebRootPath + "/images/";
                    var imagePath = Path.Combine(path, "rites-logo.png");
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                    item.base64Logo = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                }
            }

            if (model.lstBillDetailsForPDF.Count > 0)
            {

                return View(model.lstBillDetailsForPDF[0]);
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult EastBill(AllGeneratedBills obj)
        {
            obj.FromDate = "01/01/2021";
            obj.ToDate = "31/01/2021";
            obj.REGION_CODE = "E";
            obj.LOA = "A";
            obj.RailwayChk = "true";
            obj.CLIENT_NAME = null;
            obj.CLIENT_TYPE = "R";
            obj.BPO_NAME = null;

            AllGeneratedBills model = allGeneratedBillsRepository.CreateBills(obj);
            if (model.lstBillDetailsForPDF.Count() > 0)
            {
                foreach (var item in model.lstBillDetailsForPDF)
                {
                    item.items = allGeneratedBillsRepository.GetBillItems(item.BILL_NO);
                    decimal totalBillAmount = (item.sgst) + (item.cgst) + (item.igst) + (item.insp_fee);
                    item.BILL_AMOUNT = totalBillAmount;

                    if (!string.IsNullOrEmpty(item.qr_code))
                    {
                        item.qr_code = Common.QRCodeGenerate(item.qr_code);
                    }

                    string path = env.WebRootPath + "/images/";
                    var imagePath = Path.Combine(path, "rites-logo.png");
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                    item.base64Logo = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                }
            }

            if (model.lstBillDetailsForPDF.Count > 0)
            {

                return View(model.lstBillDetailsForPDF[0]);
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult WestBill(AllGeneratedBills obj)
        {
            obj.FromDate = "01/01/2021";
            obj.ToDate = "31/01/2021";
            obj.REGION_CODE = "W";
            obj.LOA = "A";
            obj.RailwayChk = "true";
            obj.CLIENT_NAME = null;
            obj.CLIENT_TYPE = "R";
            obj.BPO_NAME = null;

            AllGeneratedBills model = allGeneratedBillsRepository.CreateBills(obj);
            if (model.lstBillDetailsForPDF.Count() > 0)
            {
                foreach (var item in model.lstBillDetailsForPDF)
                {
                    item.items = allGeneratedBillsRepository.GetBillItems(item.BILL_NO);
                    decimal totalBillAmount = (item.sgst) + (item.cgst) + (item.igst) + (item.insp_fee);
                    item.BILL_AMOUNT = totalBillAmount;

                    if (!string.IsNullOrEmpty(item.qr_code))
                    {
                        item.qr_code = Common.QRCodeGenerate(item.qr_code);
                    }

                    string path = env.WebRootPath + "/images/";
                    var imagePath = Path.Combine(path, "rites-logo.png");
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                    item.base64Logo = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                }
            }

            if (model.lstBillDetailsForPDF.Count > 0)
            {

                return View(model.lstBillDetailsForPDF[0]);
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult COQABill(AllGeneratedBills obj)
        {
            obj.FromDate = "01/01/2021";
            obj.ToDate = "31/01/2021";
            obj.REGION_CODE = "Q";
            obj.LOA = "A";
            obj.RailwayChk = "true";
            obj.CLIENT_NAME = null;
            obj.CLIENT_TYPE = "R";
            obj.BPO_NAME = null;

            AllGeneratedBills model = allGeneratedBillsRepository.CreateBills(obj);
            if (model.lstBillDetailsForPDF.Count() > 0)
            {
                foreach (var item in model.lstBillDetailsForPDF)
                {
                    item.items = allGeneratedBillsRepository.GetBillItems(item.BILL_NO);
                    decimal totalBillAmount = (item.sgst) + (item.cgst) + (item.igst) + (item.insp_fee);
                    item.BILL_AMOUNT = totalBillAmount;

                    if (!string.IsNullOrEmpty(item.qr_code))
                    {
                        item.qr_code = Common.QRCodeGenerate(item.qr_code);
                    }

                    string path = env.WebRootPath + "/images/";
                    var imagePath = Path.Combine(path, "rites-logo.png");
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                    item.base64Logo = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                }
            }

            if (model.lstBillDetailsForPDF.Count > 0)
            {

                return View(model.lstBillDetailsForPDF[0]);
            }
            else
            {
                return View(model);
            }
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

        public async Task<IActionResult> GeneratePDF(string BillNo)
        {
            List<T22Bill> BillData = allGeneratedBillsRepository.GetBillByBillNo(BillNo);

            if (BillData.Count > 0 && BillData[0].BillResentStatus == "R")
            {
                bool isBillResentCountNullOrEmpty = !BillData[0].BillResentCount.HasValue;

                int count = isBillResentCountNullOrEmpty ? 1 : (BillData[0].BillResentCount.Value ? 2 : 0);

                string Bill_No = allGeneratedBillsRepository.UpdateBillCount(BillNo, count);
            }

            string Bill_Date = allGeneratedBillsRepository.UpdateGEN_Bill_Date(BillNo);

            string pdfFileName = "";
            string htmlContent = string.Empty;
            List<AllGeneratedBills> model = GlobalDeclaration.AllGeneratedBillModel;

            AllGeneratedBills selectedBill = model.FirstOrDefault(bill => bill.BILL_NO == BillNo);
            selectedBill.items = allGeneratedBillsRepository.GetBillItems(BillNo);

            string path = env.WebRootPath + "/images/";
            var imagePath = Path.Combine(path, "rites-logo.png");
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            selectedBill.base64Logo = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

            if (selectedBill.REGION_CODE == "North")
            {
                htmlContent = await this.RenderViewToStringAsync("/Views/AllGeneratedBills/NorthBill.cshtml", selectedBill);
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
                Landscape = false,
                Format = PaperFormat.Letter,
                PrintBackground = false,
            });

            await browser.CloseAsync();

            return File(pdfContent, "application/pdf", pdfFileName);
        }

        public string GenerateDigitalSignatureXML(string base64String, int pageNo)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode requestNode = doc.CreateElement("request");
            doc.AppendChild(requestNode);

            XmlNode commandNode = doc.CreateElement("command");
            commandNode.AppendChild(doc.CreateTextNode("pkiNetworkSign"));
            requestNode.AppendChild(commandNode);

            XmlNode tsNode = doc.CreateElement("ts");
            string tym = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
            tsNode.AppendChild(doc.CreateTextNode(tym));
            requestNode.AppendChild(tsNode);
            Random random = new Random();
            string otp = Convert.ToString(random.Next(1000, 9999));

            XmlNode txnNode = doc.CreateElement("txn");
            txnNode.AppendChild(doc.CreateTextNode(otp));
            requestNode.AppendChild(txnNode);

            XmlNode certNode = doc.CreateElement("certificate");
            requestNode.AppendChild(certNode);

            XmlNode nameNode1 = doc.CreateElement("attribute");
            XmlAttribute nameNode1Attr = doc.CreateAttribute("name");
            nameNode1Attr.Value = "CN";
            nameNode1.Attributes.Append(nameNode1Attr);
            certNode.AppendChild(nameNode1);

            XmlNode nameNode2 = doc.CreateElement("attribute");
            XmlAttribute nameNode2Attr = doc.CreateAttribute("name");
            nameNode2Attr.Value = "O";
            nameNode2.Attributes.Append(nameNode2Attr);
            certNode.AppendChild(nameNode2);

            XmlNode nameNode3 = doc.CreateElement("attribute");
            XmlAttribute nameNode3Attr = doc.CreateAttribute("name");
            nameNode3Attr.Value = "OU";
            nameNode3.Attributes.Append(nameNode3Attr);
            certNode.AppendChild(nameNode3);

            XmlNode nameNode4 = doc.CreateElement("attribute");
            XmlAttribute nameNode4Attr = doc.CreateAttribute("name");
            nameNode4Attr.Value = "T";
            nameNode4.Attributes.Append(nameNode4Attr);
            certNode.AppendChild(nameNode4);

            XmlNode nameNode5 = doc.CreateElement("attribute");
            XmlAttribute nameNode5Attr = doc.CreateAttribute("name");
            nameNode5Attr.Value = "E";
            nameNode5.Attributes.Append(nameNode5Attr);
            certNode.AppendChild(nameNode5);

            XmlNode nameNode6 = doc.CreateElement("attribute");
            XmlAttribute nameNode6Attr = doc.CreateAttribute("name");
            nameNode6Attr.Value = "SN";
            nameNode6.Attributes.Append(nameNode6Attr);
            certNode.AppendChild(nameNode6);

            XmlNode nameNode7 = doc.CreateElement("attribute");
            XmlAttribute nameNode7Attr = doc.CreateAttribute("name");
            nameNode7Attr.Value = "CA";
            nameNode7.Attributes.Append(nameNode7Attr);
            certNode.AppendChild(nameNode7);

            XmlNode nameNode8 = doc.CreateElement("attribute");
            XmlAttribute nameNode8Attr = doc.CreateAttribute("name");
            nameNode8Attr.Value = "TC";
            nameNode8.Attributes.Append(nameNode8Attr);
            nameNode8.AppendChild(doc.CreateTextNode("SG"));
            certNode.AppendChild(nameNode8);

            XmlNode nameNode9 = doc.CreateElement("attribute");
            XmlAttribute nameNode9Attr = doc.CreateAttribute("name");
            nameNode9Attr.Value = "AP";
            nameNode9.Attributes.Append(nameNode9Attr);
            nameNode9.AppendChild(doc.CreateTextNode("1"));
            certNode.AppendChild(nameNode9);

            XmlNode nameNode10 = doc.CreateElement("attribute");
            XmlAttribute nameNode10Attr = doc.CreateAttribute("name");
            nameNode10Attr.Value = "VD";
            nameNode10.Attributes.Append(nameNode10Attr);
            certNode.AppendChild(nameNode10);

            XmlNode fileNode = doc.CreateElement("file");
            requestNode.AppendChild(fileNode);

            XmlNode nameNode11 = doc.CreateElement("attribute");
            XmlAttribute nameNode11Attr = doc.CreateAttribute("name");
            nameNode11Attr.Value = "type";
            nameNode11.Attributes.Append(nameNode11Attr);
            nameNode11.AppendChild(doc.CreateTextNode("pdf"));
            fileNode.AppendChild(nameNode11);

            XmlNode pdfNode = doc.CreateElement("pdf");
            requestNode.AppendChild(pdfNode);

            XmlNode pageNode = doc.CreateElement("page");
            pageNode.AppendChild(doc.CreateTextNode(pageNo.ToString()));
            pdfNode.AppendChild(pageNode);

            XmlNode coodNode = doc.CreateElement("cood");
            coodNode.AppendChild(doc.CreateTextNode("400,45"));
            pdfNode.AppendChild(coodNode);

            XmlNode sizeNode = doc.CreateElement("size");
            sizeNode.AppendChild(doc.CreateTextNode("165,60"));

            pdfNode.AppendChild(sizeNode);

            XmlNode dataNode = doc.CreateElement("data");
            dataNode.AppendChild(doc.CreateTextNode(base64String));
            requestNode.AppendChild(dataNode);

            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            doc.WriteTo(tx);

            return sw.ToString();
        }

        public static int CountPdfPages(Stream pdfStream)
        {
            using (iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(pdfStream))
            {
                iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfReader);
                return pdfDocument.GetNumberOfPages();
            }
        }

        [HttpPost]
        public IActionResult UploadSignedPdf1(IEnumerable<DigitalSignModel> model)
        {
            if (model != null)
            {
                foreach (var item in model)
                {
                    byte[] pdfBytes = Convert.FromBase64String(item.Base64String);
                    string path = env.WebRootPath + "/ReadWriteData/Signed_Invoices/";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = path + item.Bill_No + ".pdf";
                    System.IO.File.WriteAllBytes(path, pdfBytes);

                    var imagePath = "/ReadWriteData/Signed_Invoices/" + item.Bill_No + ".pdf";
                    var result = allGeneratedBillsRepository.SaveUploadFile(imagePath, item.Bill_No);
                }

                return Json(new { status = 1 });
            }
            else
            {
                return Json(new { status = 0 });
            }
        }

        [HttpPost]
        public IActionResult UploadSignedPdf(string base64SignedPdf, string Bill_No)
        {
            byte[] pdfBytes = Convert.FromBase64String(base64SignedPdf);
            string path = env.WebRootPath + "/ReadWriteData/Signed_Invoices/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = path + Bill_No + ".pdf";
            System.IO.File.WriteAllBytes(path, pdfBytes);

            var imagePath = "/ReadWriteData/Signed_Invoices/" + Bill_No + ".pdf";
            var result = allGeneratedBillsRepository.SaveUploadFile(imagePath, Bill_No);

            return Json(new { status = 1 });
        }

        public IActionResult Delete(string BILL_NO, string REGION_CODE)
        {
            try
            {
                string Region = REGION_CODE.Substring(0, 1);

                string FolderName = GetFolderNameByRegion(Region);

                var path = env.WebRootPath + "/ReadWriteData/" + FolderName;
                string pdfFilePath = Path.Combine(path, BILL_NO + ".pdf");

                bool fileExists = System.IO.File.Exists(pdfFilePath);

                if (fileExists)
                {
                    // If the file exists, delete it
                    System.IO.File.Delete(pdfFilePath);
                    AlertDeletedSuccess();
                    var result = allGeneratedBillsRepository.SaveUploadFile(null, BILL_NO);
                }
                else
                {
                    AlertDanger("This Bill NO PDF is not exists !!");
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AllGeneratedBills", "Delete", 1, GetIPAddress());
            }

            return RedirectToAction("Index");
        }
    }
}
