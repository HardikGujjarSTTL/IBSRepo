using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using System;
using System.Drawing;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using IBS.Interfaces.Reports;
using System.Data;
using OfficeOpenXml;
using System.IO;
//using ClosedXML.Excel;

namespace IBS.Controllers.Reports
{
    public class BPOWiseOutstandingBillsController : BaseController
    {

        #region Variables
        private readonly IBPOWiseOutstandingBillsRepository BPOWiseOutstandingBillsRepository;
        #endregion
        public BPOWiseOutstandingBillsController(IBPOWiseOutstandingBillsRepository _BPOWiseOutstandingBillsRepository)
        {
            BPOWiseOutstandingBillsRepository = _BPOWiseOutstandingBillsRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BPOWiseOutBills()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetBPO(string BPO)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetBillPayingOfficerUsingSBPO(BPO);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BPOWiseOutstandingBills", "GetBPO", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpGet]
        public IActionResult GetRlyCode(string BpoType)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetBPORLY(BpoType);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BPOWiseOutstandingBills", "GetRlyCode", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        //public IActionResult GenerateReport([FromBody] BPOWiseOutstandingBillsModel BPOWiseOutstandingBillsModel)
        //{
        //    BPOWiseOutstandingBillsModel.Region = GetRegionCode;
        //    try
        //    {
        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            DataSet dTResult = BPOWiseOutstandingBillsRepository.GenerateReport(BPOWiseOutstandingBillsModel);
        //            wb.Worksheets.Add(dTResult);

        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                wb.SaveAs(stream);
        //                stream.Position = 0; 

        //                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.AddException(ex.ToString(), ex.Message.ToString(), "BPOWiseOutstandingBills", "BPOWiseOutBills", 1, GetIPAddress());
        //        return Content("An error occurred while generating the Excel file.");
        //    }
        //}
        [HttpPost]
        public ActionResult GenerateReport(BPOWiseOutstandingBillsModel BPOWiseOutstandingBillsModel)
        {
            BPOWiseOutstandingBillsModel.Region = GetRegionCode;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                DataSet dTResult = BPOWiseOutstandingBillsRepository.GenerateReport(BPOWiseOutstandingBillsModel);

                // Create a new ExcelPackage (you'll need the EPPlus library for this)
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("WorksheetName"); // Change the worksheet name as needed

                    // Loop through the DataSet and populate the Excel worksheet with data
                    for (int i = 0; i < dTResult.Tables[0].Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dTResult.Tables[0].Columns[i].ColumnName;
                    }
                    for (int row = 0; row < dTResult.Tables[0].Rows.Count; row++)
                    {
                        for (int col = 0; col < dTResult.Tables[0].Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dTResult.Tables[0].Rows[row][col];
                        }
                    }

                    // Prepare the Excel file for download
                    byte[] excelBytes = excelPackage.GetAsByteArray();
                    HttpContext.Response.Clear();
                    //HttpContext.Response.Buffer = true;
                    HttpContext.Response.Headers.Add("content-disposition", "attachment; filename=ExportByExcel.xlsx"); // Changed to .xlsx for better compatibility
                    HttpContext.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Response.Headers.Add("Charset", "");
                    HttpContext.Response.Body.WriteAsync(excelBytes, 0, excelBytes.Length);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BPOWiseOutstandingBills", "BPOWiseOutBills", 1, GetIPAddress());
                return Content("An error occurred while generating the Excel file.");
            }

            return Json(new { status = true, responseText = "" }); // You might want to return a view or a different result here.
        }

    }
}
