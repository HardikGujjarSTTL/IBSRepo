using IBS.Interfaces.IE;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.IE;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.IE
{
    public class DailyWorkPlanController : BaseController
    {
        #region Variables
        private readonly IDailyWorkPlanRepository dailyworkplanRepository;
        #endregion

        public DailyWorkPlanController(IDailyWorkPlanRepository _dailyworkplanRepository)
        {
            dailyworkplanRepository = _dailyworkplanRepository;
        }

        public IActionResult DailyWorkPlan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<DailyWorkPlanModel> dTResult = dailyworkplanRepository.GetMessageList(dtParameters, GetIeCd);
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
                    i = dailyworkplanRepository.DetailsInsertUpdate(model);
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
    }
}
