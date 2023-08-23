using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace IBS.Controllers
{
    public class LabPaymentListController : BaseController
    {
        #region Variables
        private readonly ILabPaymentListRepository LabPaymentListRepository;
        #endregion
        public LabPaymentListController(ILabPaymentListRepository _LabPaymentListRepository)
        {
            LabPaymentListRepository = _LabPaymentListRepository;
        }

       
        public IActionResult LabPaymentList()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabPaymentListModel> dTResult = LabPaymentListRepository.GetLapPaymentList(dtParameters, Regin);
            return Json(dTResult);
        }
        public IActionResult LabPaymentApproval(string CaseNo,string CallSno,string CallRecvDt)
        {
            ViewBag.CaseNo = CaseNo;
            ViewBag.CallSno = CallSno;
            ViewBag.CallRecvDt = CallRecvDt;
            return View();
        }
        [HttpPost]
        public IActionResult LoadPayment(string CaseNo, string CallSno, string CallRecvDt)
        {
            string Regin = GetRegionCode;
            LabPaymentListModel dTResult = LabPaymentListRepository.LoadPayment(CaseNo, CallSno, CallRecvDt, Regin);
            return Json(dTResult);
        }
        [HttpPost]
        public JsonResult SaveData()
        {
            LabPaymentListModel LabPaymentListModel = new LabPaymentListModel();
            LabPaymentListModel.UName = UserId.ToString();
            LabPaymentListModel.CaseNo = Request.Form["CaseNo"];
            LabPaymentListModel.CallRecvDt = Request.Form["CallRecvDt"];
            LabPaymentListModel.CallSno = Request.Form["CallSno"];
            LabPaymentListModel.DocStatusFin = Request.Form["DocStatusFin"];
            LabPaymentListModel.Remarks = Request.Form["Remarks"];
            
            bool result;
            result = LabPaymentListRepository.SaveData(LabPaymentListModel);
            if (result == false)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }

        }
    }
}
