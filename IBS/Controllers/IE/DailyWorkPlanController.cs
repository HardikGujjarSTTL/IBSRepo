using IBS.Interfaces.IE;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.IE;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult DailyWorkPlan(DailyWorkPlanModel model)
        {
            try
            {
                int i = 0;
                if (model.Reason != null)
                {
                    model.Createdby = Convert.ToString(UserName.Trim());
                    model.UserId = Convert.ToString(UserName.Trim());
                    model.IeCd = GetIeCd;
                    model.RegionCode = Region;
                    i = dailyRepository.DetailsInsertUpdate(model);
                    AlertAddSuccess("Record Added Successfully.");
                }
                return RedirectToAction("DailyWorkPlan");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DailyWorkPlan", "UserDetailsSave", 1, GetIPAddress());
            }
            return View(model);
        }

        //[HttpPost]
        //public IActionResult UpdateBillsFinalisation(DailyWorkPlanModel model)
        //{
        //    string i = dailyRepository.SaveDetails(model, Region);
        //    return Json(new { status = true, responseText = "Records Locking Successfully." });
        //}
    }
}
