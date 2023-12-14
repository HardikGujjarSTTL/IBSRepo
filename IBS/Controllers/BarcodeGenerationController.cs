using IBS.Interfaces;
using IBS.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Drawing.Imaging;
using System.Text;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace IBS.Controllers
{
    public class BarcodeGenerationController : BaseController
    {
        private readonly IBarcodeGeneration BarcodeGen;
        private readonly IWebHostEnvironment env;
        private readonly int barcodeWidth = 400;
        private readonly int barcodeHeight = 120;

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

        public IActionResult GenerateBarcode(string Barcode, int quantity,string caseno,int callsno,string calldate)
        {
            SaveBarcodeGenerated(Barcode, quantity, caseno, callsno, calldate);
            List<System.Drawing.Image> images = new List<System.Drawing.Image>();

            for (int i = 0; i < quantity; i++)
            {
                string uniqueBarcode = $"{Barcode}\n{DateTime.Now.ToString("dd-MM-yyyy")}";

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128B;

                b.Alignment = BarcodeLib.AlignmentPositions.LEFT;
                b.IncludeLabel = true;
                b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;

                int widthPixel = (int)ConvertPointToPixel(barcodeWidth);
                int HeightPixel = (int)ConvertPointToPixel(barcodeHeight);

                System.Drawing.Image img = b.Encode(type, Barcode, System.Drawing.Color.Black, System.Drawing.Color.White, widthPixel, HeightPixel);

                images.Add(img);

                string fileName = $"barcode_{i + 1}.png";
                string filePath = Path.Combine(env.WebRootPath, "GeneratedBarcode", fileName);

                img.Save(filePath, ImageFormat.Png);
            }

            byte[] document = ImagesToPdf(images);

            return File(document, "application/pdf", "barcode.pdf");
        }
        [HttpPost]
        public IActionResult SaveBarcodeGenerated(string Barcode, int quantity, string caseno, int callsno, string calldate)
        {
            string IPADDRESS = this.HttpContext.Connection.RemoteIpAddress.ToString();
            string USERID = Convert.ToString(UserId);
            string CREATEDBY = UserName.Trim();
            
            try
            {
                bool dTResult = BarcodeGen.SaveBarcodeGenerated(Barcode, quantity, caseno, callsno, calldate, IPADDRESS, USERID, CREATEDBY);
                if (dTResult == true)
                {
                    return Json(new { status = true, Id = dTResult });
                }
                else
                {
                    return Json(new { status = false, Id = dTResult });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BarcodeGeneration", "SaveBarcodeGenerated", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
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

        private float ConvertPointToPixel(float ValueInPoint)
        {
            return (ValueInPoint * 96) / 72;
        }

        public byte[] ImagesToPdf(List<System.Drawing.Image> images)
        {
            using (var ms = new MemoryStream())
            {
                //iTextSharp.text.Rectangle pageSize = new iTextSharp.text.Rectangle(0, 0, barcodeWidth, barcodeHeight);
                //var document = new Document(pageSize, 0, 0, 0, 0);

                //PdfWriter.GetInstance(document, ms).SetFullCompression();

                //document.Open();
                var document = new iTextSharp.text.Document();
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, ms);

                document.Open();

                foreach (System.Drawing.Image image in images)
                {                    
                    var img = iTextSharp.text.Image.GetInstance(ImageToByteArray(image));
                    document.Add(img);
                    document.NewPage(); 
                    
                }

                if (document.IsOpen()) document.Close();
                return ms.ToArray();
            }
        }
        
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

    }
}
