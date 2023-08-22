using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Dynamic;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace IBS.Controllers
{
    public class LabRegisterFormController : BaseController
    {
        #region Variables
        private readonly ILabRegFormRepository LabRegFormRepository;
        #endregion
        public LabRegisterFormController(ILabRegFormRepository _LabRegFormRepository)
        {
            LabRegFormRepository = _LabRegFormRepository;
        }

        #region Lab_Register_Form
        public IActionResult Index()
        {
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            
            return View(lABREGISTERModel);
        }
        public IActionResult LabRegisterForm(string RegNo)
        {
            ViewBag.RegNo = RegNo;
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            lABREGISTERModel = LabRegFormRepository.LoaddataModify(RegNo);
            //lABREGISTERModel.TestingType= "Normal";
            //lABREGISTERModel.CaseNo = "N12081010";
            lABREGISTERModel.CallRecDt = lABREGISTERModel.CallDateAndSno;
            //lABREGISTERModel.CallSNO = "70";
            return View(lABREGISTERModel);
        }
        [HttpPost]
        public IActionResult LoaddataModify(string RegNo,string CaseNo,string CallDt,string CallSno)
        {           
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            lABREGISTERModel.CaseNo = CaseNo;
            lABREGISTERModel.CallRecDt = CallDt;
            lABREGISTERModel.CallSNO = CallSno;
            lABREGISTERModel = LabRegFormRepository.LoaddataModify(RegNo);
            ViewBag.srno = lABREGISTERModel.SNO;
            return Json(lABREGISTERModel);
        }
        [HttpPost]
        public IActionResult LoadTable(string RegNo,string SNO)
        {
            List<LABREGISTERModel> dTResult = LabRegFormRepository.GetLabRegDtl(RegNo,SNO);
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult LabDtlModify(string RegNo, string SNO)
        {
            //DTResult<LABREGISTERModel> dTResult = LabRegFormRepository.LabDtlModify();
            LABREGISTERModel dTResult = LabRegFormRepository.LabDtlModify(RegNo, SNO);
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult LabPaymentModify(string CaseNo, string VCode)
        {
            List<LABREGISTERModel> dTResult = LabRegFormRepository.LabPaymentModify(CaseNo, VCode);
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult LapIndexData(string CaseNo, string CallRdt , string RegNo)
        {
            List<LABREGISTERModel> dTResult = LabRegFormRepository.LapIndexData(CaseNo, CallRdt, RegNo);
            return Json(dTResult);
        }
        public IActionResult LabRegisterFormNew(string CaseNo,string CallDt,string CallSno)
        {
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            lABREGISTERModel.CaseNo = CaseNo;
            lABREGISTERModel.CallRecDt = CallDt;
            lABREGISTERModel.CallSNO = CallSno;
            ViewBag.CN = CaseNo;
            ViewBag.CDT = CallDt;
            ViewBag.CSN = CallSno;
            return View(lABREGISTERModel);
        }
        [HttpPost]
        public IActionResult FormNew(string CaseNo, string CallDt, string CallSno)
        {
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            lABREGISTERModel = LabRegFormRepository.LabRegisterFormNew(CaseNo, CallDt, CallSno);
            ViewBag.srno = lABREGISTERModel.SNO;
            ViewBag.IEC = lABREGISTERModel.IECode;
                ViewBag.VC = lABREGISTERModel.VendorCode;
            return Json(lABREGISTERModel);
        }
        [HttpPost]
        public bool SaveDataDetails([FromBody]LABREGISTERModel LABREGISTERModel)
        {
            LABREGISTERModel.UName = UserId.ToString();
            bool result;
            result = LabRegFormRepository.SaveDataDetails(LABREGISTERModel);
            if(result == false)
            {
                return false;
            }
            else
            {
                return true;
            }
           
        }
        [HttpPost]
        public bool InsertLabReg([FromBody] LABREGISTERModel LABREGISTERModel)
        {
            LABREGISTERModel.UName = UserId.ToString();
            LABREGISTERModel.Region = GetRegionCode;
            bool result;
            //if(LABREGISTERModel.SampleRegNo == "" && LABREGISTERModel.CodeNo != "" && LABREGISTERModel.CodeDate != "" ) {
                result = LabRegFormRepository.InsertLabReg(LABREGISTERModel);
                if (result == false)
                {
                    //ViewBag.regnoblank = "Registration Details not available";
                    return false;
                }
                else
                {
                    return true;
                }
            //}
            //else
            //{
            //    ViewBag.Message = "Code No. & Date cannot be left blank."; 
            //    return false;
            //}
          
        }
        [HttpPost]
        public IActionResult PrintInvoice(string RegNo)
        {
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            lABREGISTERModel.Region = GetRegionCode;
            lABREGISTERModel.UName = UserId.ToString();
            bool Results = LabRegFormRepository.PrintInvoice(RegNo, lABREGISTERModel);
            return Json(Results);
        }
        [HttpPost]
        public IActionResult PostAmount([FromBody] LABREGISTERModel LABREGISTERModel)
        {
            //LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            LABREGISTERModel.Region = GetRegionCode;
            LABREGISTERModel.UName = UserId.ToString();
            bool Results = LabRegFormRepository.PostAmount(LABREGISTERModel);
            return Json(Results);
        }
        #endregion


    }
}
