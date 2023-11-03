using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class VendorLabSampleFormController : BaseController
    {
        #region Variables
        private readonly IVendorLabSampleInfoRepository VendorLabSampleInfoRepository;
        #endregion
        public VendorLabSampleFormController(IVendorLabSampleInfoRepository _VendorLabSampleInfoRepository)
        {
            VendorLabSampleInfoRepository = _VendorLabSampleInfoRepository;
        }

        #region Lab Sample Information
        [Authorization("VendorLabSampleForm", "LabSampleInfo", "view")]
        public IActionResult LabSampleInfo()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LapSampleIndex([FromBody] DTParameters dtParameters)
        {
            string VenCod = UserName;
            DTResult<LabSampleInfoModel> dTResult = new DTResult<LabSampleInfoModel>();
            try
            {
                dTResult = VendorLabSampleInfoRepository.LapSampleIndex(dtParameters, VenCod);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorLabSampleForm", "LapSampleIndex", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [Authorization("VendorLabSampleForm", "LabSampleInfo", "view")]
        public IActionResult LabPaymentRecieptForm(string CaseNo, string CallRdt, string CallSno, string Flag)
        {
            ViewBag.CaseNo = CaseNo;
            ViewBag.CallRdt = CallRdt;
            ViewBag.Sno = CallSno;
            ViewBag.Flag = Flag;
            return View();
        }
        [HttpPost]
        [Authorization("VendorLabSampleForm", "LabSampleInfo", "view")]
        public IActionResult SampleDtlData(string CaseNo, string CallRdt, string CallSno)
        {
            string Regin = GetRegionCode;
            LabSampleInfoModel dTResult = new LabSampleInfoModel();
            try
            {
                dTResult = VendorLabSampleInfoRepository.SampleDtlData(CaseNo, CallRdt, CallSno, Regin);
                if (dTResult.File != "False")
                {
                    //string mdtEx = dateconcate1(CallRdt.Trim());
                    //string myFileEx = $"{CaseNo.Trim()}_{CallSno.Trim()}_{mdtEx}";

                    //string fpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Payment", $"{myFileEx}.PDF");
                    string fn = "", MyFile = "", fx = "", fl = "";
                    string mdt = dateconcate(CallRdt.Trim());
                    MyFile = CaseNo.Trim() + '_' + CallSno.Trim() + '_' + mdt;
                    String filePath = null;
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB", "PReciept", MyFile + ".PDF");
                    dTResult.FileNm = MyFile;
                    dTResult.FileLink = filePath;
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorLabSampleForm", "SampleDtlData", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        string dateconcate(string dt)
        {
            string myYear, myMonth, myDay;

            myYear = dt.Substring(6, 4);
            myMonth = dt.Substring(3, 2);
            myDay = dt.Substring(0, 2);
            string dt1 = myYear + myDay + myMonth;
            return (dt1);
        }
        string dateconcate1(string dt)
        {
            string myYear, myMonth, myDay;

            myYear = dt.Substring(6, 4);
            myMonth = dt.Substring(3, 2);
            myDay = dt.Substring(0, 2);
            string dt1 = myYear + myMonth + myDay;
            return (dt1);
        }
        [HttpPost]
        [Authorization("VendorLabSampleForm", "LabSampleInfo", "edit")]
        public JsonResult SaveDataDetails()
        {
            LabSampleInfoModel LabSampleInfoModel = new LabSampleInfoModel();
            try
            {
                LabSampleInfoModel.UName = UserName;
                LabSampleInfoModel.CaseNo = Request.Form["CaseNo"];
                LabSampleInfoModel.CallRecDt = Request.Form["CallRecDt"];
                LabSampleInfoModel.CallSNO = Request.Form["CallSNO"];
                LabSampleInfoModel.NetTesting = Request.Form["NetTesting"];
                LabSampleInfoModel.TDS = Request.Form["TDS"];
                LabSampleInfoModel.UTRNO = Request.Form["UTRNO"];
                LabSampleInfoModel.UTRDT = Request.Form["UTRDT"];
                var file = Request.Form.Files["UploadPayment"];
                if (file != null && file.Length > 0)
                {
                    // Save the file or process it as needed
                    // For example:
                    //var fileName = Path.GetFileName(file.FileName);
                    //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Payment", fileName);
                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    file.CopyTo(stream);
                    //}
                    //string mdtEx = dateconcate(LabSampleInfoModel.CallRecDt.Trim());
                    //string myFileEx = $"{LabSampleInfoModel.CaseNo.Trim()}_{LabSampleInfoModel.CallSNO.Trim()}_{mdtEx}";

                    //string fpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "Payment", $"{myFileEx}.PDF");
                    //using (var stream = new FileStream(fpath, FileMode.Create))
                    //{
                    //    file.CopyTo(stream);
                    //}
                    string fn = "", MyFile = "", fx = "", fl = "";
                    string mdt = dateconcate1(LabSampleInfoModel.CallRecDt.Trim());
                    MyFile = LabSampleInfoModel.CaseNo.Trim() + '_' + LabSampleInfoModel.CallSNO.Trim() + '_' + mdt;
                    fn = Path.GetFileName(file.FileName);
                    String SaveLocation = null;
                    SaveLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB", "PReciept", MyFile + ".PDF");
                    using (var stream = new FileStream(SaveLocation, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                bool result;
                result = VendorLabSampleInfoRepository.SaveDataDetails(LabSampleInfoModel);
                if (result == false)
                {
                    return Json(false);
                }
                else
                {
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorLabSampleForm", "SaveDataDetails", 1, GetIPAddress());
            }
            return Json(false);
        }
        [HttpPost]
        [Authorization("VendorLabSampleForm", "LabSampleInfo", "edit")]
        public JsonResult UpdateDetails()
        {
            LabSampleInfoModel LabSampleInfoModel = new LabSampleInfoModel();
            try
            {
                LabSampleInfoModel.UName = UserName;
                LabSampleInfoModel.CaseNo = Request.Form["CaseNo"];
                LabSampleInfoModel.CallRecDt = Request.Form["CallRecDt"];
                LabSampleInfoModel.CallSNO = Request.Form["CallSNO"];
                LabSampleInfoModel.NetTesting = Request.Form["NetTesting"];
                LabSampleInfoModel.TDS = Request.Form["TDS"];
                LabSampleInfoModel.UTRNO = Request.Form["UTRNO"];
                LabSampleInfoModel.UTRDT = Request.Form["UTRDT"];
                var file = Request.Form.Files["UploadPayment"];
                if (file != null && file.Length > 0)
                {
                    // Save the file or process it as needed
                    // For example:
                    //var fileName = Path.GetFileName(file.FileName);
                    //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Payment", fileName);
                    //string mdtEx = dateconcate(LabSampleInfoModel.CallRecDt.Trim());
                    //string myFileEx = $"{LabSampleInfoModel.CaseNo.Trim()}_{LabSampleInfoModel.CallSNO.Trim()}_{mdtEx}";

                    //string fpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "Payment", $"{myFileEx}.PDF");
                    //using (var stream = new FileStream(fpath, FileMode.Create))
                    //{
                    //    file.CopyTo(stream);
                    //}
                    string fn = "", MyFile = "", fx = "", fl = "";
                    string mdt = dateconcate1(LabSampleInfoModel.CallRecDt.Trim());
                    MyFile = LabSampleInfoModel.CaseNo.Trim() + '_' + LabSampleInfoModel.CallSNO.Trim() + '_' + mdt;
                    fn = Path.GetFileName(file.FileName);
                    String SaveLocation = null;
                    SaveLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB", "PReciept", MyFile + ".PDF");
                    using (var stream = new FileStream(SaveLocation, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                bool result;
                result = VendorLabSampleInfoRepository.UpdateDetails(LabSampleInfoModel);
                if (result == false)
                {
                    return Json(false);
                }
                else
                {
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorLabSampleForm", "UpdateDetails", 1, GetIPAddress());
            }
            return Json(false);
        }
        [HttpPost]
        public IActionResult CheckExist(string CaseNo, string CallRdt, string CallSno)
        {
            string Regin = GetRegionCode;
            string dTResult = VendorLabSampleInfoRepository.CheckExist(CaseNo, CallRdt, CallSno, Regin);
            return Json(dTResult);
        }
        public IActionResult DownloadFile(string caseno, string calldt, string csno, string filename)
        {
            try { 
            string fn = "", MyFile = "", fx = "", fl = "";
            string mdt = dateconcate(calldt.Trim());
            MyFile = caseno.Trim() + '_' + csno.Trim() + '_' + mdt;
            fn = Path.GetFileName(filename);
            String filePath = null;
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB", "PReciept", MyFile + ".PDF");

            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                return File(fileBytes, "application/pdf", fn);
            }
            else
            {
                return NotFound(); // Or return another result indicating the file does not exist
            }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorLabSampleForm", "DownloadFile", 1, GetIPAddress());
            }
            return NotFound();
        }
        #endregion


    }
}
