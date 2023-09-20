using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class pfrmFromToDateController : Controller
    {
        private readonly IpfrmFromToDateRepository ipfrmFromToDateRepository;
        public pfrmFromToDateController(IpfrmFromToDateRepository _ipfrmFromToDateRepository)
        {
            ipfrmFromToDateRepository = _ipfrmFromToDateRepository;
        }
        [Authorization("pfrmFromToDate", "Index", "view")]
        public IActionResult Index()
        {
            var action = Request.Query["action"];
            ViewBag.Action = action;
            var partialView = "";
            if (action == "CMWISEICNI")
            {
                partialView = "../ICAction/IC_CMWISEICNI_Partial";
            }
            ViewBag.PartialView = partialView;
            return View();
        }


        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ICIsuued> dTResult = ipfrmFromToDateRepository.GetDataList(dtParameters);
            return Json(dTResult);
        }
    }
}
