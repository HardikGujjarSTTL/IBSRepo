using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class SummaryConsigneeWiseInspController : BaseController
    {
        #region Variables
        private readonly ISummaryConsigneeWiseInspRepository SummaryConsigneeWiseInspRepository;
        #endregion
        public SummaryConsigneeWiseInspController(ISummaryConsigneeWiseInspRepository _SummaryConsigneeWiseInspRepository)
        {
            SummaryConsigneeWiseInspRepository = _SummaryConsigneeWiseInspRepository;
        }

        public IActionResult SummaryConsigneeWiseInsp()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<SummaryConsigneeWiseInspModel> dTResult = SummaryConsigneeWiseInspRepository.SummaryConsigneeWiseInsp(dtParameters, Regin);
            return Json(dTResult);
        }
        

    }
}
