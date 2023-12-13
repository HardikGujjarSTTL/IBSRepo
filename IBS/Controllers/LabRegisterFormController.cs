using IBS.DataAccess;
using IBS.Filters;
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
        [Authorization("LabRegisterForm", "Index", "view")]
        public IActionResult Index()
        {
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            
            return View(lABREGISTERModel);
        }
        [Authorization("LabRegisterForm", "Index", "view")]
        public IActionResult LabRegisterForm(string RegNo)
        {
            ViewBag.RegNo = RegNo;
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            try
            {
                lABREGISTERModel = LabRegFormRepository.LoaddataModify(RegNo);
                //lABREGISTERModel.TestingType= "Normal";
                //lABREGISTERModel.CaseNo = "N12081010";
                lABREGISTERModel.CallRecDt = lABREGISTERModel.CallDateAndSno;
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabRegisterForm", "LabRegisterForm", 1, GetIPAddress());
            }
            return View(lABREGISTERModel);
        }
        [HttpPost]
        public IActionResult LoaddataModify(string RegNo,string CaseNo,string CallDt,string CallSno)
        {           
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            try
            {
                lABREGISTERModel.CaseNo = CaseNo;
                lABREGISTERModel.CallRecDt = CallDt;
                lABREGISTERModel.CallSNO = CallSno;
                lABREGISTERModel = LabRegFormRepository.LoaddataModify(RegNo);
                ViewBag.srno = lABREGISTERModel.SNO;
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabRegisterForm", "LoaddataModify", 1, GetIPAddress());
            }
            return Json(lABREGISTERModel);
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            //List<LABREGISTERModel> dTResult = LabRegFormRepository.GetLabRegDtl(RegNo,SNO);
            DTResult<LABREGISTERModel> dTResult = new DTResult<LABREGISTERModel>();
            try
            {
                dTResult = LabRegFormRepository.GetLabRegDtl(dtParameters);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabRegisterForm", "LoadTable", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult LabDtlModify(string RegNo, string SNO)
        {
            //DTResult<LABREGISTERModel> dTResult = LabRegFormRepository.LabDtlModify();
            LABREGISTERModel dTResult = new LABREGISTERModel();
            try
            {
                dTResult = LabRegFormRepository.LabDtlModify(RegNo, SNO);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabRegisterForm", "LabDtlModify", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult LabPaymentModify(string CaseNo, string VCode)
        {
            List<LABREGISTERModel> dTResult = new List<LABREGISTERModel>();
            try
            {
                dTResult = LabRegFormRepository.LabPaymentModify(CaseNo, VCode);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabRegisterForm", "LabPaymentModify", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult LapIndexData([FromBody] DTParameters dtParameters)
        {
            DTResult<LABREGISTERModel> dTResult = new DTResult<LABREGISTERModel>();
            try
            {
                dTResult = LabRegFormRepository.LapIndexData(dtParameters);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabRegisterForm", "LapIndexData", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [Authorization("LabRegisterForm", "Index", "view")]
        public IActionResult LabRegisterFormNew(string CaseNo,string CallRdt, string CallSno)
        {
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            lABREGISTERModel.CaseNo = CaseNo;
            lABREGISTERModel.CallRecDt = CallRdt;
            lABREGISTERModel.CallSNO = CallSno;
            ViewBag.CN = CaseNo;
            ViewBag.CDT = CallRdt;
            ViewBag.CSN = CallSno;
            return View(lABREGISTERModel);
        }
        [HttpPost]
        public IActionResult FormNew(string CaseNo, string CallDt, string CallSno)
        {
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            try
            {
                lABREGISTERModel = LabRegFormRepository.LabRegisterFormNew(CaseNo, CallDt, CallSno);
                ViewBag.srno = lABREGISTERModel.SNO;
                ViewBag.IEC = lABREGISTERModel.IECode;
                ViewBag.VC = lABREGISTERModel.VendorCode;
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabRegisterForm", "FormNew", 1, GetIPAddress());
            }
            return Json(lABREGISTERModel);
        }
        [HttpPost]
        [Authorization("LabRegisterForm", "Index", "edit")]
        public bool SaveDataDetails([FromBody]LABREGISTERModel LABREGISTERModel)
        {
            try
            {
                LABREGISTERModel.UName = UserId.ToString();
                var RegNo = LABREGISTERModel.SampleRegNo;
                bool result;
                var testing = LABREGISTERModel.DTestingFee;
                var tax = LABREGISTERModel.DServiceTax;
                var hand = LABREGISTERModel.DHandlingCharges;
                var defaultTesting = 0;
                var defaultTax = 0;
                var defaultHand = 0;

                if (testing != null && int.TryParse(testing, out int testingValue))
                {
                    defaultTesting = testingValue;
                }

                if (tax != null && int.TryParse(tax, out int taxValue))
                {
                    defaultTax = taxValue;
                }

                if (hand != null && int.TryParse(hand, out int handValue))
                {
                    defaultHand = handValue;
                }

                var total = defaultTesting + defaultTax + defaultHand;
                //var total = Convert.ToInt32(testing) + Convert.ToInt32(tax) + Convert.ToInt32(hand);
                LABREGISTERModel.TotalLabCharges = Convert.ToString(total);
                if (LABREGISTERModel.Flag == "1")
                {
                    result = LabRegFormRepository.SaveDataDetails(LABREGISTERModel);
                }
                else
                {
                    result = LabRegFormRepository.InsertDataDetails(LABREGISTERModel);
                }

                if (result == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabRegisterForm", "SaveDataDetails", 1, GetIPAddress());
            }
            return false;
        }
        [HttpPost]
        [Authorization("LabRegisterForm", "Index", "edit")]
        public JsonResult InsertLabReg([FromBody] LABREGISTERModel LABREGISTERModel)
        {
            try
            {
                var testing = LABREGISTERModel.DTestingFee;
                var tax = LABREGISTERModel.DServiceTax;
                var hand = LABREGISTERModel.DHandlingCharges;
                //var total = Convert.ToInt32(testing) + Convert.ToInt32(tax) + Convert.ToInt32(hand);
                var defaultTesting = 0;
                var defaultTax = 0;
                var defaultHand = 0;

                if (testing != null && int.TryParse(testing, out int testingValue))
                {
                    defaultTesting = testingValue;
                }

                if (tax != null && int.TryParse(tax, out int taxValue))
                {
                    defaultTax = taxValue;
                }

                if (hand != null && int.TryParse(hand, out int handValue))
                {
                    defaultHand = handValue;
                }

                var total = defaultTesting + defaultTax + defaultHand;
                LABREGISTERModel.TotalLabCharges = Convert.ToString(total);

                LABREGISTERModel.UName = UserId.ToString();
                LABREGISTERModel.Region = GetRegionCode;
                bool result;
                //if(LABREGISTERModel.SampleRegNo == "" && LABREGISTERModel.CodeNo != "" && LABREGISTERModel.CodeDate != "" ) {
                result = LabRegFormRepository.InsertLabReg(LABREGISTERModel);
                if (result == false)
                {
                    //ViewBag.regnoblank = "Registration Details not available";
                    //return false;
                    return Json(new { success = false, message = "Registration Details not available" });
                }
                else
                {
                    return Json(new { success = true, message = LABREGISTERModel.SampleRegNo });
                    //return true;
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabRegisterForm", "InsertLabReg", 1, GetIPAddress());
            }
            //return false;
            return Json(new { success = false, message = "Registration Details not available" });
        }
        [HttpPost]
        public IActionResult PrintInvoice(string RegNo)
        {
            LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            bool Results = false;
            try
            {
                lABREGISTERModel.Region = GetRegionCode;
                lABREGISTERModel.UName = UserId.ToString();
                Results = LabRegFormRepository.PrintInvoice(RegNo, lABREGISTERModel);
                
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabRegisterForm", "PrintInvoice", 1, GetIPAddress());
            }
            return Json(Results);
        }
        [HttpPost]
        public bool PostAmount([FromBody] LABREGISTERModel LABREGISTERModel)
        {
            //LABREGISTERModel lABREGISTERModel = new LABREGISTERModel();
            try
            {
                LABREGISTERModel.Region = GetRegionCode;
                LABREGISTERModel.UName = UserId.ToString();
                bool Results = LabRegFormRepository.PostAmount(LABREGISTERModel);
                if (Results == true)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabRegisterForm", "PostAmount", 1, GetIPAddress());
            }
            return false;
        }
        #endregion


    }
}
