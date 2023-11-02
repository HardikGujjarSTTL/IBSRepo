using IBS.Interfaces.IE;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.IE;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;

namespace IBS.Controllers.IE
{
    public class DailyWorkPlanController : BaseController
    {
        #region Variables
        private readonly IDailyWorkPlanRepository dailyRepository;
        #endregion

        public DailyWorkPlanController(IDailyWorkPlanRepository _dailyRepository)
        {
            dailyRepository = _dailyRepository;
        }

        public IActionResult DailyWorkPlan()
        {
            DailyWorkPlanModel model = new();
            model.IeCd = GetIeCd;

            if (model.IeCd > 0)
            {
                model = dailyRepository.FindByDetails(model, Region);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<DailyWorkPlanModel> dTResult = dailyRepository.GetLoadTable(dtParameters, Region, GetIeCd);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTableCurrentDay([FromBody] DTParameters dtParameters)
        {
            DTResult<DailyWorkPlanModel> dTResult = dailyRepository.GetLoadTableCurrentDay(dtParameters, Region, GetIeCd);
            return Json(dTResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DailyWorkPlan(DailyWorkPlanModel model, IFormCollection formCollection)
        {
            try
            {
                if (formCollection.Keys.Contains("checkedWork"))
                {
                    model.checkedWork = formCollection["checkedWork"];
                }
                int i = 0;
                model.Createdby = Convert.ToString(UserName.Trim());
                model.UserId = Convert.ToString(UserName.Trim());
                model.IeCd = GetIeCd;
                model.RegionCode = Region;
                if (model.ActionType == "S")
                {
                    i = dailyRepository.DetailsInsertUpdate(model, Region, GetIeCd);
                    if (i == 0)
                    {
                        AlertDanger("Your Work Plan Cannot be Saved due to  one or both of the following \n1. You have selected more then 3 different vendors. \n2. You have Selected more then 6 calls of a particular vendor.");
                    }
                    else
                    {
                        AlertAddSuccess("Record Added Successfully.");
                    }

                }
                else
                {
                    i = dailyRepository.DetailsDelete(model, Region, GetIeCd);
                    AlertAddSuccess("Record Deleted Successfully.");
                }

                return RedirectToAction("DailyWorkPlan");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DailyWorkPlan", "UserDetailsSave", 1, GetIPAddress());
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult NonInspectionSave(DailyWorkPlanModel model)
        {
            try
            {
                string msg = "Record Inserted Successfully.";
                if (model.NIWorkType != null && model.FromDt != null && model.ToDt != null)
                {
                    msg = "Record Updated Successfully.";
                    model.Updatedby = Convert.ToString(UserId);
                }
                model.Createdby = Convert.ToString(UserId);

                int i = dailyRepository.NonInspectionSave(model, Region, GetIeCd);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "NonInspectionSave", "DailyWorkPlan", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });

        }

        [HttpPost]
        public IActionResult LoadTableNonInspection([FromBody] DTParameters dtParameters)
        {
            DTResult<DailyWorkPlanModel> dTResult = dailyRepository.GetLoadTableNonInspection(dtParameters, Region, GetIeCd);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult ReasonSave(DateTime? NwpDt, string Reason)
        {
            try
            {
                string Dtl = "";
                string msg = "Record Inserted Successfully.";
                if (NwpDt != null && Reason != null)
                {
                    Dtl = dailyRepository.ReasonSave(NwpDt, Reason, GetIeCd, Region, UserName.Trim());
                }

                if (Dtl != "")
                {
                    return Json(new { status = true, responseText = msg, NwpDt = Dtl });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ReasonSave", "DailyWorkPlan", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });

        }
    }
}
