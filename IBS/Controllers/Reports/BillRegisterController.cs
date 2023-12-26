using IBS.Interfaces.Reports;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Reports
{
    public class BillRegisterController : BaseController
    {

        #region Variables
        private readonly IBillRegisterRepository billrepository;
        #endregion
        public BillRegisterController(IBillRegisterRepository _billrepository)
        {
            billrepository = _billrepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<BillRegisterModel> dTResult = billrepository.GetDataList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult ProcessDataTable(BillRegisterModel model, string commaSeparatedString, string Region)
        {
            try
            {
                int i = 0;
                string msg = "Inserted Successfully.";
                if (commaSeparatedString != null)
                {
                    i = billrepository.DetailsSave(model, commaSeparatedString, Region);
                }
                if (i > 0)
                {
                    return Json(new { success = true, responseText = msg, Status = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Bill Register", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { success = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
