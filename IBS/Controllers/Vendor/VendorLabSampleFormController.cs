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
            DTResult<LabSampleInfoModel> dTResult = VendorLabSampleInfoRepository.LapSampleIndex(dtParameters, VenCod);
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
            LabSampleInfoModel dTResult = VendorLabSampleInfoRepository.SampleDtlData(CaseNo, CallRdt, CallSno, Regin);
            if(dTResult.File != "False")
            {
                string mdtEx = dateconcate1(CallRdt.Trim());
                string myFileEx = $"{CaseNo.Trim()}_{CallSno.Trim()}_{mdtEx}";

                string fpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Payment", $"{myFileEx}.PDF");
                dTResult.FileNm = myFileEx;
                dTResult.FileLink = fpath;
            }
            
            return Json(dTResult);
        }
        string dateconcate(string dt)
        {
            string myYear, myMonth, myDay;

            myYear = dt.Substring(6, 4);
            myMonth = dt.Substring(3, 2);
            myDay = dt.Substring(0, 2);
            string dt1 = myYear  + myDay +myMonth;
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
                string mdtEx = dateconcate(LabSampleInfoModel.CallRecDt.Trim());
                string myFileEx = $"{LabSampleInfoModel.CaseNo.Trim()}_{LabSampleInfoModel.CallSNO.Trim()}_{mdtEx}";

                string fpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Payment", $"{myFileEx}.PDF");
                using (var stream = new FileStream(fpath, FileMode.Create))
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
        [HttpPost]
        [Authorization("VendorLabSampleForm", "LabSampleInfo", "edit")]
        public JsonResult UpdateDetails()
        {
            LabSampleInfoModel LabSampleInfoModel = new LabSampleInfoModel();
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
                string mdtEx = dateconcate(LabSampleInfoModel.CallRecDt.Trim());
                string myFileEx = $"{LabSampleInfoModel.CaseNo.Trim()}_{LabSampleInfoModel.CallSNO.Trim()}_{mdtEx}";

                string fpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Payment", $"{myFileEx}.PDF");
                using (var stream = new FileStream(fpath, FileMode.Create))
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
        [HttpPost]
        public IActionResult CheckExist(string CaseNo, string CallRdt, string CallSno)
        {
            string Regin = GetRegionCode;
            string dTResult = VendorLabSampleInfoRepository.CheckExist(CaseNo, CallRdt, CallSno, Regin);
            return Json(dTResult);
        }
        #endregion


    }
}
