
using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
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
        public IActionResult GenerateBarcode(string Barcode)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Bitmap bitMap = new Bitmap(Barcode.Length * 40, 80))
                {
                    using (Graphics graphics = Graphics.FromImage(bitMap))
                    {
                        Font oFont = new Font("IDAutomationHC39M", 16);
                        PointF point = new PointF(2f, 2f);
                        SolidBrush whiteBrush = new SolidBrush(Color.White); // Use a white background
                        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                        graphics.DrawString("*" + Barcode + "*", oFont, blackBrush, point);
                    }
                    bitMap.Save(memoryStream, ImageFormat.Png); // Save as PNG
                    ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            return View();
        }
        
    }
}
