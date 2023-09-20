using IBS.Filters;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.IE_Reports
{
    [Authorization]
    public class IEWorkPlanController : BaseController
    {
        [Authorization("IEWorkPlan", "Index", "view")]
        public IActionResult Index()
        {
            var Action = Request.Query["Action"];
            ViewBag.Region = Convert.ToString(GetUserInfo.Region);
            ViewBag.Action = Action;
            var IEName = Common.GetIENameIsStatusNull(Convert.ToString(GetUserInfo.Region));
            var COName = Common.GetOfficerIsCoStatusIsNull(Convert.ToString(GetUserInfo.Region));
            if(Action == "E")
            {
                ViewBag.HeaderTitle = "IE DAILY WORK PLAN EXCEPTION REPORT";
            }
            else
            {
                ViewBag.HeaderTitle = "IE DAILY WORK PLAN REPORT";
            }

            return View();
        }
    }
}
