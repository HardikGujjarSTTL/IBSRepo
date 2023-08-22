using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.IE_Reports
{
    public class IEWorkPlanController : BaseController
    {
        public IActionResult Index(string actiontype)
        {
           
            ViewBag.Region = Convert.ToString(GetUserInfo.Region);
            ViewBag.Action = actiontype;
            var IEName = Common.GetIENameIsStatusNull(Convert.ToString(GetUserInfo.Region));
            var COName = Common.GetOfficerIsCoStatusIsNull(Convert.ToString(GetUserInfo.Region));
            if(actiontype == "E")
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
