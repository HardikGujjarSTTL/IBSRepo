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
    public class LabSampleInfoController : BaseController
    {
        #region Variables
        private readonly ILabSampleInfoRepository LabSampleInfoRepository;
        #endregion
        public LabSampleInfoController(ILabSampleInfoRepository _LabSampleInfoRepository)
        {
            LabSampleInfoRepository = _LabSampleInfoRepository;
        }

        #region Lab Sample Information
        [Authorization("LabSampleInfo", "LabSampleInfo", "view")]
        public IActionResult LabSampleInfo()
        {

            return View();
        }
        [HttpPost]
        [Authorization("LabSampleInfo", "LabSampleInfo", "view")]
        public IActionResult LapSampleIndex(string CaseNo, string CallRdt, string CallSno)
        {
            string Regin = GetRegionCode;
            List<LabSampleInfoModel> dTResult = LabSampleInfoRepository.LapSampleIndex(CaseNo, CallRdt, CallSno, Regin);
            return Json(dTResult);
        }
        public IActionResult LabSampleDtl(string CaseNo, string CallRdt, string CallSno, string Flag)
        {
            ViewBag.CaseNo = CaseNo; 
            ViewBag.CallRdt = CallRdt;
            ViewBag.Sno = CallSno;
            ViewBag.Flag = Flag;
            return View();
        }
        [HttpPost]
        public IActionResult SampleDtlData(string CaseNo, string CallRdt, string CallSno)
        {
            string Regin = GetRegionCode;
            LabSampleInfoModel dTResult = LabSampleInfoRepository.SampleDtlData(CaseNo, CallRdt, CallSno, Regin);
            return Json(dTResult);
        }
        [HttpPost]
        [Authorization("LabSampleInfo", "LabSampleInfo", "add")]
        public JsonResult SaveDataDetails()
        {
            LabSampleInfoModel LabSampleInfoModel = new LabSampleInfoModel();
            LabSampleInfoModel.UName = UserId.ToString();
            LabSampleInfoModel.CaseNo = Request.Form["CaseNo"];
            LabSampleInfoModel.CallRecDt = Request.Form["CallRecDt"];
            LabSampleInfoModel.CallSNO = Request.Form["CallSNO"];
            LabSampleInfoModel.IE = Request.Form["IE"];
            LabSampleInfoModel.Status = Request.Form["Status"];
            LabSampleInfoModel.DateofRecSample = Request.Form["DateofRecSample"];
            LabSampleInfoModel.TotalTFee = Request.Form["TotalTFee"];
            LabSampleInfoModel.LikelyDt = Request.Form["LikelyDt"];
            LabSampleInfoModel.Remarks = Request.Form["Remarks"];
            var file = Request.Form.Files["UploadLab"];
            if (file != null && file.Length > 0)
            {
                // Save the file or process it as needed
                // For example:
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LAB", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            bool result;
            result = LabSampleInfoRepository.SaveDataDetails(LabSampleInfoModel);
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
        [Authorization("LabSampleInfo", "LabSampleInfo", "add")]
        public JsonResult UpdateDetails()
        {
            LabSampleInfoModel LabSampleInfoModel = new LabSampleInfoModel();
            LabSampleInfoModel.UName = UserId.ToString();
            LabSampleInfoModel.CaseNo = Request.Form["CaseNo"];
            LabSampleInfoModel.CallRecDt = Request.Form["CallRecDt"];
            LabSampleInfoModel.CallSNO = Request.Form["CallSNO"];
            LabSampleInfoModel.Status = Request.Form["Status"];
            LabSampleInfoModel.DateofRecSample = Request.Form["DateofRecSample"];
            LabSampleInfoModel.TotalTFee = Request.Form["TotalTFee"];
            LabSampleInfoModel.LikelyDt = Request.Form["LikelyDt"];
            LabSampleInfoModel.Remarks = Request.Form["Remarks"];
            var file = Request.Form.Files["UploadLab"];
            if (file != null && file.Length > 0)
            {
                // Save the file or process it as needed
                // For example:
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LAB", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            bool result;
            result = LabSampleInfoRepository.UpdateDetails(LabSampleInfoModel);
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
            string dTResult = LabSampleInfoRepository.CheckExist(CaseNo, CallRdt, CallSno, Regin);
            return Json(dTResult);
        }
        #endregion


    }
}
