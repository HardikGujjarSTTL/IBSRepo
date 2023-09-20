using IBS.Interfaces;
using IBS.Models;
using IBS.Helper;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using IBS.Filters;
using System.Data;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Org.BouncyCastle.Asn1.X509;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Controllers
{
    public class AddRecieptVoucherController : BaseController
    {
        private readonly IAddRecieptVoucher addVoucherRepository;
        public AddRecieptVoucherController(IAddRecieptVoucher _addVoucherRepository)
        {
            addVoucherRepository = _addVoucherRepository;
        }
        [Authorization("AddRecieptVoucher", "Index", "view")]
        public IActionResult Index()
        {

            return View();
        }
        [Authorization("AddRecieptVoucher", "Index", "view")]
        public IActionResult AddRecieptVoucher(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT)
        {
            AddRecieptVoucherModel model = new();

            if (VCHR_NO != "" && VCHR_NO != null)
            {
                model = addVoucherRepository.FindByID(VCHR_NO, BANK_CD, CHQ_NO, CHQ_DT);
            }
            return View(model);
        }
        [Authorization("AddRecieptVoucher", "Index", "view")]
        public IActionResult VoucherList([FromBody] DTParameters dtParameters)
        {
            DTResult<AddRecieptVoucherModel> dTResult = addVoucherRepository.GetVoucherList(dtParameters);
            return Json(dTResult);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("AddRecieptVoucher", "AddRecieptVoucher", "edit")]
        public IActionResult VoucherDetailsSave(AddRecieptVoucherModel model)
        {
            try
            {
                string msg = "Voucher Inserted Successfully.";

                if (model.VCHR_NO != "")
                {
                    msg = "Voucher Updated Successfully.";

                }
                //model.Createdby = UserId;
                //int i = contractRepository.ContractDetailsInsertUpdate(model);
                string i = addVoucherRepository.VoucherDetailsSave(model, GetUserInfo.Region.ToString());
                if (i != "" || i != "")
                {
                    return Json(new { status = true, responseText = msg });
                }
            }

            catch (Exception ex)
            {
                // Common.AddException(ex.ToString(), ex.Message.ToString(), "Contract", "ContractDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public ActionResult ButtonClick(string AccCD, string txtBPO, string lstBPO, string txtCSNO)
        {
            AddRecieptVoucherModel bPOmodel = new AddRecieptVoucherModel();

            var list = GetDistinctBPOsByCaseNo(txtCSNO, bPOmodel);

            string Narrt = string.Empty;
            try
            {
                if (AccCD == "2709" || AccCD == "2210" || AccCD == "2212")
                {

                    var result = addVoucherRepository.ChkCSNO(txtCSNO, lstBPO, out Narrt);

                    if (result == "")
                    {
                        ViewBag.AlertMessage = "Invalid Case No.!!!";
                        ViewBag.FocusOnTxtCSNO = true;
                    }

                    else
                    {
                        Narrt = result;
                    }
                }
                else
                {
                    ViewBag.ShowBPO = true;

                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.Replace("\n", "");
                return RedirectToAction("Error", "Home", new { errMsg = errorMessage });
            }


            return Json(new { status = false, Narrt = Narrt, ((IBS.Models.AddRecieptVoucherModel)((Microsoft.AspNetCore.Mvc.ViewResult)list).Model).BPOList, responseText = "Oops Somthing Went Wrong !!" });
        }

        public ActionResult GetDistinctBPOsByCaseNo(string txtCSNO, AddRecieptVoucherModel bPOmodel)
        {
            string DropdownValues = "";
            var dropdownValues = addVoucherRepository.GetDistinctBPOsByCaseNo(txtCSNO);
            if (dropdownValues == null)
            {
                ViewBag.AlertMessage = "Their is No BPO Present For the given Case No,To enter BPO goto PO Update.";

            }
            else
            {

                bPOmodel.BPOList = dropdownValues;
                //List<BPOmodel> bpoList = dropdownValues;
                //ViewBag.BPOList = new SelectList(bpoList, "BPO_CD", "BPO_NAME");
            }

            return View(bPOmodel);
        }

        [HttpPost]
        public IActionResult UploadExcel(IFormFile file)
        {
            //DataTable dataTable = ReadExcelFile("E:\\2nd floor\\Github\\IBSRepo\\IBS\\wwwroot\\ExcelSamples\\AddVoucher.xls");
            using var package = new ExcelPackage("E:\\2nd floor\\Github\\IBSRepo\\IBS\\wwwroot\\ExcelSamples\\AddVoucher.xls");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var currentSheet = package.Workbook.Worksheets;
            var workSheet = currentSheet.First();
            if (file != null && file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    byte[] excelBytes = stream.ToArray();

                    // Call a method to process the Excel bytes and return a DataTable
                    DataTable dt = ProcessExcelBytes(excelBytes);

                    // You can use the 'dataTable' in your application as needed

                    return View("Result");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult OnPostUpload_old(IFormFile file)
        {



            //var file = files.FirstOrDefault();
            var inputstream = file.OpenReadStream();

            XSSFWorkbook workbook = new XSSFWorkbook(inputstream);

            //var FIRST_ROW_NUMBER = { { firstRowWithValue } };

            ISheet sheet = workbook.GetSheetAt(0);
            // Example: var firstCellRow = (int)sheet.GetRow(0).GetCell(0).NumericCellValue;

            return View();
            //business logic & saving data to DB                        
        }
        public ActionResult OnPostUpload(IFormFile file , AddRecieptVoucherModel model)
        {
            try
            {
                //var file = files.FirstOrDefault();
                var inputstream = file.OpenReadStream();

                XSSFWorkbook workbook = new XSSFWorkbook(inputstream);

                var FIRST_ROW_NUMBER = 1;

                ISheet sheet = workbook.GetSheetAt(0);
                // Example: var firstCellRow = (int)sheet.GetRow(0).GetCell(0).NumericCellValue;

                for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)
                {
                    IRow currentRow = sheet.GetRow(rowIdx);

                    if (currentRow == null || currentRow.Cells == null || currentRow.Cells.Count() < FIRST_ROW_NUMBER) break;
                    var df = new DataFormatter();

                    for (int cellNumber = 0; cellNumber < 3; cellNumber++)
                    {
                       
                        var VoucherDate = currentRow.Cells[0].ToString();
                        if (string.IsNullOrEmpty(VoucherDate))
                        {
                            break;
                        }
                        var Bank_code = currentRow.Cells[1].ToString();
                        if (string.IsNullOrEmpty(Bank_code))
                        {
                            break;
                        }
                            var VoucherType = currentRow.Cells[2].ToString();
                        if (string.IsNullOrEmpty(VoucherType))
                        {
                            break;
                        }
                       if(VoucherDate != "" && Bank_code != "" && VoucherType != "")
                       {
                            string Region = GetRegionCode;

                            var result = addVoucherRepository.Insert(model, VoucherDate, Bank_code,  VoucherType , Region);

                       }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return View();
        }










        public static DataTable ReadExcelFile(string filePath)
        {
            // Create a new DataTable to hold the data
            DataTable dataTable = new DataTable();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Assuming you're reading the first worksheet

                // Loop through rows and columns to populate the DataTable
                foreach (var cell in worksheet.Cells)
                {
                    // Assuming the first row contains column headers
                    if (cell.Start.Row == 1)
                    {
                        dataTable.Columns.Add(cell.Text);
                    }
                    else
                    {
                        // Add rows to the DataTable
                        if (cell.Start.Column == 1)
                        {
                            dataTable.Rows.Add();
                        }

                        dataTable.Rows[dataTable.Rows.Count - 1][cell.Start.Column - 1] = cell.Text;
                    }
                }
            }

            return dataTable;
        }

        private DataTable ProcessExcelBytes(byte[] excelBytes)
        {
            // Create a new DataTable to hold the data
            DataTable dataTable = new DataTable();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new MemoryStream(excelBytes)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Assuming you're reading the first worksheet

                // Loop through rows and columns to populate the DataTable
                foreach (var cell in worksheet.Cells)
                {
                    // Assuming the first row contains column headers
                    if (cell.Start.Row == 1)
                    {
                        dataTable.Columns.Add(cell.Text);
                    }
                    else
                    {
                        // Add rows to the DataTable
                        if (cell.Start.Column == 1)
                        {
                            dataTable.Rows.Add();
                        }

                        dataTable.Rows[dataTable.Rows.Count - 1][cell.Start.Column - 1] = cell.Text;
                    }
                }
            }

            return dataTable;
        }
    }
}
