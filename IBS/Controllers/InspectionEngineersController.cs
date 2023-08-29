using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers
{
    public class InspectionEngineersController : BaseController
    {
        #region Variables
        private readonly IInspectionEngineers inspectionEngineers;
        #endregion
        public InspectionEngineersController(IInspectionEngineers _inspectionEngineers)
        {
            inspectionEngineers = _inspectionEngineers;
        }
        public IActionResult Index(int IeCd)
        {
            InspectionEngineersModel model = new();
            model.IeRegion = GetRegionCode;
            if (IeCd > 0)
            {
                model = inspectionEngineers.FindByID(IeCd);
            }

            return View(model);
        }

        public IActionResult Manage(int IeCd, string ActionType)
        {
            InspectionEngineersModel model = new();
            model.IeRegion = GetRegionCode;
            if (IeCd > 0)
            {
                model = inspectionEngineers.FindManageByID(IeCd, ActionType, GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<InspectionEngineersModel> dTResult = inspectionEngineers.GetInspectionEngineersList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (inspectionEngineers.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsSave(InspectionEngineersModel model)
        {
            try
            {
                string msg = "Inserted Successfully.";

                if (model.IeCd > 0)
                {
                    msg = "Your Record Has Been Updated And IE Password Has Been Reset To His Employee No. so, Plz Inform Him!!!";
                    model.Updatedby = UserId;
                    model.UserId = Convert.ToString(UserId);
                }
                model.Createdby = UserId;
                model.UserId = Convert.ToString(UserId);
                string i = inspectionEngineers.DetailsInsertUpdate(model);
                if (i != "Exists")
                {
                    return Json(new { status = true, responseText = msg });
                }
                else if (i == "0")
                {
                    msg = "No Record Found!!! The Record has been  Deleted by other User While you were Modifying the Data";
                    return Json(new { status = false, responseText = msg });
                }
                else
                {
                    msg = "IE with Same Employee Short Name or Employee No. Already Exists!!!";
                    return Json(new { status = false, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetIeCity(int IeCityId)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetIeCity(IeCityId);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "GetIeCity", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetMatch(int IeCd)
        {
            try
            {
                string MCode = inspectionEngineers.GetMatch(IeCd, GetRegionCode);
                return Json(new { status = true, MCode = MCode });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "GetMatch", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
