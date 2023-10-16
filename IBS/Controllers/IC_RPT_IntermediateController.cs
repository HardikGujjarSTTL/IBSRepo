﻿using IBS.DataAccess;
using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class IC_RPT_IntermediateController : BaseController
    {
        #region Varible
        private readonly IIC_RPT_IntermediateRepository iC_RPT_IntermediateRepository;
        SessionHelper objSessionHelper = new SessionHelper();
        public IC_RPT_IntermediateController(IIC_RPT_IntermediateRepository _iC_RPT_IntermediateRepository)
        {
            iC_RPT_IntermediateRepository = _iC_RPT_IntermediateRepository;
        }
        #endregion

        [Authorization("IC_RPT_Intermediate", "Index", "view")]
        public IActionResult Index()
        {
            IC_RPT_IntermediateModel model = new();
            //model.CASE_NO = "N22061024";
            //model.Call_Recv_dt = "23/08/2022";
            //model.Call_SNO = "99";
            //model.CONSIGNEE_CD = "3895";
            //model.ACTIONAR = "A";

            var CASE_NO = "N21111089";
            var Call_Recv_dt = Convert.ToString("13/08/2022");
            var Call_SNO = "3";
            var CONSIGNEE_CD = "39";
            var ACTIONAR = "A";

            model = iC_RPT_IntermediateRepository.AcceptedFun(CASE_NO, Call_Recv_dt, Call_SNO, CONSIGNEE_CD);
            model.ACTIONAR = ACTIONAR;
            model.CONSIGNEE_CD = CONSIGNEE_CD;

            //model = iC_RPT_IntermediateRepository.GetDetails(model.CASE_NO, model.Display_Call_Recv_dt, model.Call_SNO, model.ITEM_SRNO_PO, model.CONSIGNEE_CD);

            model.Region = Region;
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadPOAmendmentTable([FromBody] DTParameters dtParameters)
        {
            DTResult<PO_Amendments> dTResult = iC_RPT_IntermediateRepository.GetPOAmendment(dtParameters);
            objSessionHelper.lstPoAmendments = dTResult.data.ToList();
            return Json(dTResult);
        }

        public IActionResult AcceptedFun(string Case_No, string Call_Recv_Dt, string Call_SNo, string Consignee_Cd)
        {
            var data = iC_RPT_IntermediateRepository.AcceptedFun(Case_No, Call_Recv_Dt, Call_SNo, Consignee_Cd);
            return Json(data);
        }

        public IActionResult GetVisitsChanges(string Case_No, string Call_Recv_Dt, string Call_SNo, string VisitDate)
        {
            VisitDate = iC_RPT_IntermediateRepository.GetVisitsChanges(Case_No, Call_Recv_Dt, Call_SNo, VisitDate);
            return Json(VisitDate);
        }


        public IActionResult FillItems(string Case_No, string Call_Recv_Dt, string Call_SNo, string Consignee_Cd)
        {
            var model = iC_RPT_IntermediateRepository.FillItems(Case_No, Call_Recv_Dt, Call_SNo, Consignee_Cd);
            return Json(model);
        }

        public IActionResult SetItemVal(string Case_No, string Call_Recv_Dt, string Call_SNo, string ITEM_SRNO_PO, string Consignee_Cd)
        {
            var model = iC_RPT_IntermediateRepository.GetDetails(Case_No, Call_Recv_Dt, Call_SNo, ITEM_SRNO_PO, Consignee_Cd);
            return Json(model);
        }


        public IActionResult SetAccepted(string Case_No, string Call_Recv_Dt, string Call_SNo, string Consignee_Cd)
        {
            var data = iC_RPT_IntermediateRepository.SetAccepted(Case_No, Call_Recv_Dt, Call_SNo, Consignee_Cd);
            return Json(data);
        }

        public IActionResult FillItemDropDown(string Case_No, string Call_Recv_Dt, string Call_SNo, string Consignee_Cd)
        {
            var data = iC_RPT_IntermediateRepository.GetItems(Case_No, Call_Recv_Dt, Call_SNo, Consignee_Cd);
            return Json(data);
        }

        [HttpPost]
        public IActionResult SaveDetails(IC_RPT_IntermediateModel model)
        {
            try
            {
                var result = true;
                result = iC_RPT_IntermediateRepository.SaveDetail(model, GetUserInfo);

                if (result)
                    AlertAddSuccess("Record Added Successfully.");
                else
                    AlertDanger("Looks Like Something Went Wrong. Some Error Occurs...");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_RPT_Intermediate", "SaveDetails", 1, GetIPAddress());
            }
            return RedirectToAction("Index", model);
        }

        public IActionResult RefreshDetail(IC_RPT_IntermediateModel model)
        {
            try
            {
                var data = iC_RPT_IntermediateRepository.RefreshDetail(model, GetUserInfo);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_RPT_Intermediate", "RefereDetail", 1, GetIPAddress());
            }
            return Json(null);
        }

        public IActionResult SaveAmendment(string CaseNo, string Po_No,PO_Amendments model)
        {
            int res = 0;
            var Iecd = GetUserInfo.IeCd;
            try
            {
                List<PO_Amendments> lstPoAhm = objSessionHelper.lstPoAmendments;
                lstPoAhm.ForEach(x => x.IECD = Iecd);
                res = iC_RPT_IntermediateRepository.SaveAmendment( CaseNo,  Po_No,  model, lstPoAhm);
                if(res > 0)
                {
                    return Json(new { status = true, responseText = "PO Amendment Record Added Successfully." });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_RPT_Intermediate", "SaveAmendment", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Looks Like Something Went Wrong. Some Error Occurs..." });
        }
    }
}
