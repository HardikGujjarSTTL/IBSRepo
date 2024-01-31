﻿using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Xml;

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
            labInvoicelst model = new labInvoicelst();
            try
            {
                model = labInvoiceRepository.GetLabInvoice(FromDate, ToDate, Region);
                var RelativePath = "/ReadWriteData/Signed_Lab_Invoices/";
                model.pdfFolder = RelativePath;
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabInvoice", "LabInvoiceList", 1, GetIPAddress());
            }
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetLabInvoiceList(string FromDate, string ToDate, string Region)
        {
            List<labInvoicelst> lstGenBill = new List<labInvoicelst>();
            try
            {
                labInvoicelst model = new labInvoicelst();
                model = labInvoiceRepository.GetLabInvoice(FromDate, ToDate, Region);
                if (model.lstlabInvoicelst.Count > 0)
                    lstGenBill = model.lstlabInvoicelst;
                else
                    return Json(new { status = 0, list = lstGenBill });
                return Json(new { status = 1, list = lstGenBill });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabInvoice", "GetLabInvoiceList", 1, GetIPAddress());
                return Json(new { status = 0, list = lstGenBill });
            }
        }

        #region Generate PDF with Digital Signature
        [HttpPost]
        public async Task<IActionResult> GeneratePDF(labInvoicelst model)
        {
            string htmlContent = "";
            model.items = labInvoiceRepository.GetBillItems(model.InvoiceNo);
            decimal? totalTestingCharges = model.items.Sum(billItem => billItem.TESTING_CHARGES);
            decimal? SubTotal = model.items.Sum(billItem => billItem.IGST + totalTestingCharges);
            model.TotalTESTING_CHARGES = totalTestingCharges;
            model.GrandTotal = SubTotal;

            if (!string.IsNullOrEmpty(model.qr_code))
            {
                model.qr_code = Common.QRCodeGenerate(model.qr_code);
            }

            string paths = env.WebRootPath + "/images/";
            var imagePath = Path.Combine(paths, "rites-logo.png");
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            model.base64Logo = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

            var path = env.WebRootPath + "/ReadWriteData/" + "Signed_Lab_Invoices";
            var RelativePath = "/ReadWriteData/Signed_Lab_Invoices/";
            model.pdfFolder = RelativePath;

            //check if the PDF file exists
            string pdfFilePath = Path.Combine(path, model.InvoiceBillNo + ".pdf");
            bool fileExists = System.IO.File.Exists(pdfFilePath);
            var PDFNamee = model.InvoiceBillNo + ".pdf";

            htmlContent = await this.RenderViewToStringAsync("/Views/LabInvoice/LabInvoicePDF.cshtml", model);

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
                pdfStream.Position = 0;
                int pageCount = CountPdfPages(pdfStream);

                string xmlData = GenerateDigitalSignatureXML(base64String, pageCount);

                return Json(new { status = 1, xmlDetail = xmlData, InvoiceNo = model.InvoiceNo, InvoiceBillNo = model.InvoiceBillNo });
            }
            return Json(new { status = 0, xmlDetail = "", InvoiceNo = model.InvoiceNo, InvoiceBillNo = model.InvoiceBillNo });
        }
        #endregion

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
        public IActionResult UploadSignedPdf1(string base64SignedPdf, string InvoiceNo,string InvoiceBillNo)
        {
            byte[] pdfBytes = Convert.FromBase64String(base64SignedPdf);
            string path = env.WebRootPath + "/ReadWriteData/Signed_Lab_Invoices/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = path + InvoiceBillNo + ".pdf";
            System.IO.File.WriteAllBytes(path, pdfBytes);
            var PDFName = InvoiceBillNo + ".pdf";
            var imagePath = "/ReadWriteData/Signed_Lab_Invoices/" + InvoiceBillNo + ".pdf";
            string msg = labInvoiceRepository.UpdatePDFDetails(InvoiceNo, PDFName, imagePath);

            return Json(new { status = 1 });
        }
    }
}
