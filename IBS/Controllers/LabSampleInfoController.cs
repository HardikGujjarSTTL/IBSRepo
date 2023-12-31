﻿using IBS.DataAccess;
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
        public IActionResult LapSampleIndex([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabSampleInfoModel> dTResult = new DTResult<LabSampleInfoModel>();
            try
            {
                dTResult = LabSampleInfoRepository.LapSampleIndex(dtParameters, Regin);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabSampleInfo", "LapSampleIndex", 1, GetIPAddress());
            }
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
            LabSampleInfoModel dTResult = new LabSampleInfoModel();
            try
            {
                dTResult = LabSampleInfoRepository.SampleDtlData(CaseNo, CallRdt, CallSno, Regin);
                ViewBag.Hyperlink = dTResult.Hyperlink2;
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabSampleInfo", "SampleDtlData", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [HttpPost]
        [Authorization("LabSampleInfo", "LabSampleInfo", "edit")]
        public JsonResult SaveDataDetails()
        {
            LabSampleInfoModel LabSampleInfoModel = new LabSampleInfoModel();
            try
            {
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
                bool result;
                if (file != null && file.Length > 0)
                {
                    // Save the file or process it as needed
                    // For example:
                    //var fileName = Path.GetFileName(file.FileName);
                    //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData","LAB", fileName);
                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    file.CopyTo(stream);
                    //}
                    string fn = "", MyFile = "", fx = "", fl = "";
                    string mdt = dateconcate(LabSampleInfoModel.CallRecDt.Trim());
                    MyFile = LabSampleInfoModel.CaseNo.Trim() + '_' + LabSampleInfoModel.CallSNO.Trim() + '_' + mdt;
                    fn = Path.GetFileName(file.FileName);
                    String SaveLocation = null;
                    SaveLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB", MyFile + ".PDF");
                    using (var stream = new FileStream(SaveLocation, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        result = LabSampleInfoRepository.UploadDate(LabSampleInfoModel);
                    }
                }
               
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
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabSampleInfo", "SaveDataDetails", 1, GetIPAddress());
            }
            return Json(false);
        }
        [HttpPost]
        [Authorization("LabSampleInfo", "LabSampleInfo", "edit")]
        public JsonResult UpdateDetails()
        {
            LabSampleInfoModel LabSampleInfoModel = new LabSampleInfoModel();
            try
            {
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
                bool result;
                if (file != null && file.Length > 0)
                {
                    // Save the file or process it as needed
                    // For example:
                    //var fileName = Path.GetFileName(file.FileName);
                    //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB", fileName);
                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    file.CopyTo(stream);
                    //}
                    string fn = "", MyFile = "", fx = "", fl = "";
                    string mdt = dateconcate(LabSampleInfoModel.CallRecDt.Trim());
                    MyFile = LabSampleInfoModel.CaseNo.Trim() + '_' + LabSampleInfoModel.CallSNO.Trim() + '_' + mdt;
                    fn = Path.GetFileName(file.FileName);
                    String SaveLocation = null;
                    SaveLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB", MyFile + ".PDF");
                    using (var stream = new FileStream(SaveLocation, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        result = LabSampleInfoRepository.UploadDate(LabSampleInfoModel);
                    }

                }
                
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
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabSampleInfo", "UpdateDetails", 1, GetIPAddress());
            }
            return Json(false);

        }
        string dateconcate(string dt)
        {
            string myYear, myMonth, myDay;

            myYear = dt.Substring(6, 4);
            myMonth = dt.Substring(3, 2);
            myDay = dt.Substring(0, 2);
            string dt1 = myYear + myMonth + myDay;
            return (dt1);
        }
        [HttpPost]
        public IActionResult CheckExist(string CaseNo, string CallRdt, string CallSno)
        {
            string Regin = GetRegionCode;
            string dTResult = LabSampleInfoRepository.CheckExist(CaseNo, CallRdt, CallSno, Regin);
            return Json(dTResult);
        }
        string dateconcate2(string dt)
        {
            string myYear, myMonth, myDay;

            myYear = dt.Substring(6, 4);
            myMonth = dt.Substring(3, 2);
            myDay = dt.Substring(0, 2);
            string dt1 = myYear + myDay + myMonth;
            return (dt1);
        }
        public IActionResult DownloadFile(string caseno,string calldt,string csno,string filename)
        {
            try
            {
                string fn = "", MyFile = "", fx = "", fl = "";
                string mdt = dateconcate2(calldt.Trim());
                MyFile = caseno.Trim() + '_' + csno.Trim() + '_' + mdt;
                fn = Path.GetFileName(filename);
                String filePath = null;
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB", MyFile + ".PDF");

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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabSampleInfo", "DownloadFile", 1, GetIPAddress());
            }
            return NotFound();
        }
        #endregion


    }
}
