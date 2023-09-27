using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class Calls_Marked_For_Specific_POController : BaseController
    {
        private readonly ICalls_Marked_For_Specific_PORepository callmarkedforspecificpo;
        public Calls_Marked_For_Specific_POController(ICalls_Marked_For_Specific_PORepository _callmarkedforspecificpo)
        {
            callmarkedforspecificpo = _callmarkedforspecificpo;
        }

        [HttpPost]
        public IActionResult Dropdown(string selectedValue)
        {
            if(selectedValue == "R")
            {
                List<railway_dropdown> result = callmarkedforspecificpo.GetValue(selectedValue);
                return Json(result);
            }
            else
            {
                List<railway_dropdown> result = callmarkedforspecificpo.GetValue2(selectedValue);
                return Json(result);
            }

        }

        [HttpPost]
        public IActionResult gridData([FromBody] DTParameters dtParameters)
        {
            DTResult<Calls_Marked_For_Specific_POModel> dTResult = callmarkedforspecificpo.gridData(dtParameters);
            return Json(dTResult);
        }

        public IActionResult edit(string PO_NO , string PO_DT, string RLY_NONRLY, string RLY_CD)
        {
           Calls_Marked_For_Specific_POModel dTResult = callmarkedforspecificpo.edit(PO_NO, PO_DT, RLY_NONRLY, RLY_CD);
            return View(dTResult);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
