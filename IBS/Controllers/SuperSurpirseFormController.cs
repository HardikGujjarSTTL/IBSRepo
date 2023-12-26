//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class SuperSurpirseFormController : BaseController
    {
        #region Variables
        private readonly ISuperSurpirseFormRepository SuperSurpirseFormRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ModelContext context;
        #endregion
        public SuperSurpirseFormController(ISuperSurpirseFormRepository _SuperSurpirseFormRepository, IWebHostEnvironment webHostEnvironment, ModelContext context)
        {
            SuperSurpirseFormRepository = _SuperSurpirseFormRepository;
            _webHostEnvironment = webHostEnvironment;
            this.context = context;
        }

        [Authorization("SuperSurpirseForm", "Index", "view")]
        public IActionResult Index()
        {

            return View();
        }
        [Authorization("SuperSurpirseForm", "Index", "view")]
        public IActionResult SuperSurpirseManage(string CaseNo, string CallDt, string CallSNo, int Count)
        {
            SuperSurpirseFormModel SuperSurpirseFormModel = new SuperSurpirseFormModel();
            try
            {
                SuperSurpirseFormModel.Regin = GetRegionCode;
                SuperSurpirseFormModel = SuperSurpirseFormRepository.LoadSuperData(SuperSurpirseFormModel, CaseNo, CallDt, CallSNo);
                SuperSurpirseFormModel.Regin = GetRegionCode;
                if (Count == 1)
                {
                    ViewBag.Count = 1;
                }
                else
                {
                    ViewBag.Count = 0;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions or errors here
                Common.AddException(ex.ToString(), ex.Message.ToString(), "SuperSurpirseForm", "SuperSurpirseManage", 1, GetIPAddress());
            }
            return View(SuperSurpirseFormModel);
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<SuperSurpirseFormModel> dTResult = new DTResult<SuperSurpirseFormModel>();
            try
            {
                string Regin = GetRegionCode;
                dTResult = SuperSurpirseFormRepository.GetSuperFormData(dtParameters, Regin);
                var Caseno = dtParameters.AdditionalValues?.GetValueOrDefault("CaseNo");
                var CallDt = dtParameters.AdditionalValues?.GetValueOrDefault("CallDate");
                var CallSNo = dtParameters.AdditionalValues?.GetValueOrDefault("CallSNo");
                //var Exists = context.T44SuperSurprises.Find(Convert.ToString(Caseno), Convert.ToDateTime(CallDt), Convert.ToInt32(CallSNo));
                //var result = new
                //{
                //    Data = dTResult,
                //    Count = (Exists == null) ? 0 : 1
                //};
            }
            catch (Exception ex)
            {
                // Handle any exceptions or errors here
                Common.AddException(ex.ToString(), ex.Message.ToString(), "SuperSurpirseForm", "LoadTable", 1, GetIPAddress());
            }
            return Json(dTResult);
        }
        [HttpPost]
        [Authorization("SuperSurpirseForm", "Index", "edit")]
        public IActionResult Save(SuperSurpirseFormModel SuperSurpirseFormModel)
        {
            SuperSurpirseFormModel.Regin = GetRegionCode;
            SuperSurpirseFormModel.UserName = Convert.ToString(UserId);
            //bool dTResult = ETrainingDetailsRepository.Save(iETrainingDetailsModel);
            //if(dTResult == true)
            //{
            //    AlertAddSuccess();
            //}
            //return RedirectToAction("IETrainingDetails","Index");
            try
            {
                string msg = "Data Inserted Successfully.";

                string msg1 = "Registration Details not available";
                bool dTResult = SuperSurpirseFormRepository.Save(SuperSurpirseFormModel);
                if (dTResult == true)
                {
                    return Json(new { status = true, responseText = msg, Id = dTResult });
                }
                else
                {
                    return Json(new { status = false, responseText = msg1, Id = dTResult });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "SuperSurpirseForm", "Save", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
