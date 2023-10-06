using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.InspectionBilling;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace IBS.Controllers.InspectionBilling
{
    public class BillAdjustmentsNewController : BaseController
    {
        #region Variables
        private readonly IBillAdjustmentsNewRepository inpsRepository;

        #endregion
        public BillAdjustmentsNewController(IBillAdjustmentsNewRepository _inpsRepository)
        {
            inpsRepository = _inpsRepository;
        }

        public IActionResult Index()
        {
            //InspectionCertModel model = new();
            //if (CaseNo != "" && CallRecvDt != null && CallSno > 0)
            //{
            //    model = inpsRepository.FindByInspDetailsID(CaseNo, CallRecvDt, CallSno, Bkno, Setno, ActionType, Region, RoleId);
            //}
            //model.ActionType = ActionType;

            //return View(model);
            return View();
        }
    }
}
