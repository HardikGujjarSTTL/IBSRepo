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
            DTResult<DailyWorkPlanModel> dTResult = dailyworkplanRepository.GetMessageList(dtParameters,GetIeCd);
            return Json(dTResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsSave(DailyWorkPlanModel model)
        {
            try
            {
                string msg = "Message Inserted Successfully.";

                if (model.Reason  != null)
                {
                    msg = "Message Updated Successfully.";
                    //model.Updatedby = Convert.ToString(UserId);
                }
                //model.Createdby = Convert.ToString(UserId);
                int i = dailyworkplanRepository.DetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "User", "UserDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
