using IBS.Interfaces;
using IBS.Interfaces.IE;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class CMDailyWorkPlanController : BaseController
    {
        #region Variables
        private readonly ICMDailyWorkPlanRepository dailyRepository;
        #endregion

        public CMDailyWorkPlanController(ICMDailyWorkPlanRepository _dailyRepository)
        {
            dailyRepository = _dailyRepository;
        }
        public IActionResult Index()
        {
            CMDailyWorkPlanModel model = new();
            model.FromDt = DateTime.Now.Date;
            model.ToDt = DateTime.Now.Date.AddDays(1);
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadDtlTable([FromBody] DTParameters dtParameters)
        {
            DTResult<CMDailyWorkPlanModel> dTResult = dailyRepository.GetLoadTable(dtParameters, Region);
            return Json(dTResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CMDailyWorkPlan(CMDailyWorkPlanModel model, IFormCollection formCollection)
        {
            try
            {
                if (model.IsUrgencyUpdate == "True")
                {
                    if (formCollection.Keys.Contains("Urgency"))
                    {
                        model.Urgency = formCollection["Urgency"];
                        int i = 0;
                        model.Createdby = Convert.ToString(UserName.Trim());
                        model.UserId = Convert.ToString(UserName.Trim());
                        model.RegionCode = Region;
                        i = dailyRepository.UpdateUrgency(model, Region);
                        if (i != 0)
                        {
                            AlertAddSuccess("Updated Successfully.");
                        }
                    }
                }
                else
                {
                    if (formCollection.Keys.Contains("checkedWork"))
                    {
                        model.checkedWork = formCollection["checkedWork"];
                    }
                    int i = 0;
                    model.Createdby = Convert.ToString(UserName.Trim());
                    model.UserId = Convert.ToString(UserName.Trim());
                    model.RegionCode = Region;

                    i = dailyRepository.SaveApproval(model, Region);
                    if (i != 0)
                    {
                        AlertAddSuccess("Record Added Successfully.");
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DailyWorkPlan", "UserDetailsSave", 1, GetIPAddress());
            }
            return View(model);
        }
    }
}
