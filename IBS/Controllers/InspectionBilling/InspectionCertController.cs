using IBS.Interfaces.InspectionBilling;
using IBS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IBS.Models;
using IBS.Repositories.Vendor;
using IBS.Repositories;

namespace IBS.Controllers.InspectionBilling
{
    public class InspectionCertController : BaseController
    {
        #region Variables
        private readonly IInspectionCertRepository inpsRepository;

        #endregion
        public InspectionCertController(IInspectionCertRepository _inpsRepository)
        {
            inpsRepository = _inpsRepository;
        }

        public IActionResult Index(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno)
        {
            InspectionCertModel model = new();
            if (CaseNo != "" && CallRecvDt != null && CallSno > 0)
            {
                model = inpsRepository.FindByID(CaseNo, CallRecvDt, CallSno, Bkno, Setno, GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<InspectionCertModel> dTResult = inpsRepository.GetDataList(dtParameters, GetRegionCode);
            return Json(dTResult);
        }

        public IActionResult InspectionDetails(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno, string ActionType)
        {
            InspectionCertModel model = new();
            if (CaseNo != "" && CallRecvDt != null && CallSno > 0)
            {
                model = inpsRepository.FindByInspDetailsID(CaseNo, CallRecvDt, CallSno, Bkno, Setno, ActionType, GetRegionCode, RoleId);
            }
            return View(model);
        }

        public IActionResult GetBillDetails(string BillNo)
        {
            DTResult<InspectionCertModel> dTResult = inpsRepository.GetBillDetails(BillNo);
            return Json(dTResult);
        }

        public IActionResult GetConsignee(int ConsigneeCd)
        {
            DTResult<InspectionCertModel> dTResult = inpsRepository.GetConsignee(ConsigneeCd);
            return Json(dTResult);
        }

        public IActionResult GetBPO(string BPOCd)
        {
            DTResult<InspectionCertModel> dTResult = inpsRepository.GetBPO(BPOCd);
            return Json(dTResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateGST(InspectionCertModel model)
        {
            try
            {
                string msg = "";
                if (model.GstinNo != null && model.LegalName != null)
                {
                    msg = "GSTIN NO. & LEGAL NAME HAS BEEN UPDATED!!!";
                    model.Updatedby = UserId;
                    model.UserId = UserName;
                }

                int i = inpsRepository.UpdateGSTDetails(model, UserName);
                if (i > 0)
                {
                    return Json(new { success = true, responseText = msg, Status = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CallRegisterIB", "CallDetailsSave", 1, GetIPAddress());
            }
            return Json(new { success = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetBPOList(string BpoCd)
        {
            return Json(Common.GetBPOList(BpoCd));
        }

        int CheckDateDiff(string dt1, string dt2, int diff)
        {
            DateTime w_dt1 = new(Convert.ToInt32(dt1.Substring(6, 4)), Convert.ToInt32(dt1.Substring(3, 2)), Convert.ToInt32(dt1.Substring(0, 2)));
            DateTime w_dt2 = new(Convert.ToInt32(dt2.Substring(6, 4)), Convert.ToInt32(dt2.Substring(3, 2)), Convert.ToInt32(dt2.Substring(0, 2)));
            TimeSpan ts = w_dt1 - w_dt2;
            int differenceInDays = ts.Days;

            if (differenceInDays > diff)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        [HttpPost]
        public IActionResult InspectionCertSave(InspectionCertModel model)
        {
            try
            {
                string i = "";
                string msg = "Save Successfully.";

                if (GetRegionCode == "N")
                {
                    string mess = "";
                    int FinspCdtdiff = CheckDateDiff(Convert.ToString(model.FirstInspDt), Convert.ToString(model.DtInspDesire), 7);
                    int ICdtLinspdiff = CheckDateDiff(Convert.ToString(model.CertDt), Convert.ToString(model.LastInspDt), 3);
                    if (FinspCdtdiff == 1)
                    {
                        mess = "First Inspection Date - Call Date is greater then 7 Days!!!";
                    }
                    if (ICdtLinspdiff == 1)
                    {
                        if (mess == "")
                        {
                            mess = "IC Date - Last Inspection Date is greater then 3 Days!!!";
                        }
                        else
                        {
                            mess = mess + " & IC Date - Last Inspection Date is greater then 3 Days!!!";
                        }
                    }
                    AlertDanger(mess);
                }
                if (model.Caseno != null && model.Callrecvdt != null && model.Callsno > 0)
                {
                    model.UserId = UserName;
                    model.Createdby = UserName;
                    i = inpsRepository.InspectionCertSave(model, GetRegionCode);
                }
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
                else
                {
                    msg = "This Call cannot be deleted. because IC is present for this call!!!";
                    return Json(new { status = false, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VenderCallRegisterModel", "DetailsDelete", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult ReturnBillSubmit(InspectionCertModel model)
        {
            try
            {
                string str = "";
                string msg = "Update Successfully.";
                if (model.BillNo != null && model.BillDt != null)
                {
                    model.UserId = UserName;
                    model.Createdby = UserName;
                    str = inpsRepository.ReturnBillSubmit(model, GetRegionCode);
                }
                if (str != "")
                {
                    return Json(new { status = true, responseText = msg, Id = str });
                }
                else
                {
                    msg = "This bill is not returned from railways";
                    return Json(new { status = false, responseText = msg, Id = str });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "ReturnBillSubmit", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult Validation(InspectionCertModel model)
        {
            try
            {
                string str = "";
                str = inpsRepository.Validation(model, GetRegionCode);
                if (str != "")
                {
                    return Json(new { status = true, responseText = str });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionCert", "Validation", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult LoadTableDetails([FromBody] DTParameters dtParameters)
        {
            DTResult<InspectionCertModel> dTResult = inpsRepository.GetLoadTableDetails(dtParameters, GetRegionCode);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult BillUpdate(InspectionCertModel model)
        {
            try
            {
                string i = "";
                string msg = "Update Successfully.";

                string myYear, myMonth, myDay;
                myYear = Convert.ToString(model.BillDt).Substring(6, 4);
                myMonth = Convert.ToString(model.BillDt).Substring(3, 2);
                myDay = Convert.ToString(model.BillDt).Substring(0, 2);
                string dt1 = myYear + myMonth + myDay;
                int idt = dt1.CompareTo(DateTime.Now.Date);
                model.Regioncode = GetRegionCode;
                int fyr = inpsRepository.financial_year_check(model);
                if (fyr == 1)
                {
                    AlertDanger("Bill must be generated within the same financial year in which IC was Issued!!!" + ". \\n(ie. Certificate Date & Bill Date shoud be in same financial year)");
                }
                else if (idt > 0)
                {
                    AlertDanger("Bill Date Cannot be greater then Current Date!!!");
                }
                else if (model.BpoType == "R" && model.Au == "" && !(model.BpoRly == "RCF" || model.BpoRly == "ICF" || model.BpoRly == "RWF"))
                {
                    AlertDanger("AU Cannot be Blank For Railways Bills, Kindly Update the AU for the BPO and then Generate the Bill!!!");
                }
                else
                {
                    
                    i = inpsRepository.BillUpdate(model, GetRegionCode);
                }



                //if (model.Caseno != null && model.Callrecvdt != null && model.Callsno > 0)
                //{
                //    model.UserId = UserName;
                //    model.Createdby = UserName;
                //    i = inpsRepository.InspectionCertSave(model, GetRegionCode);
                //}
                if (i != "")
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
                else
                {
                    msg = "This Call cannot be deleted. because IC is present for this call!!!";
                    return Json(new { status = false, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VenderCallRegisterModel", "DetailsDelete", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
