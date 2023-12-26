using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class RegionalHRDataOfIEController : BaseController
    {
        #region Variables
        private readonly IRegionalHRDataOfIERepository regionalHRDataOfIERepository;
        #endregion
        public RegionalHRDataOfIEController(IRegionalHRDataOfIERepository _regionalHRDataOfIERepository)
        {
            regionalHRDataOfIERepository = _regionalHRDataOfIERepository;
        }

        [Authorization("RegionalHRDataOfIE", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorization("RegionalHRDataOfIE", "Index", "view")]
        public IActionResult Manage(string id)
        {
            string DecryptId = Common.DecryptQueryString(id);
            RegionalHRDataOfIEModel model = new();
            if (DecryptId != null && DecryptId != "")
            {
                model = regionalHRDataOfIERepository.FindByID(Convert.ToInt32(DecryptId));
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<RegionalHRDataOfIEModel> dTResult = regionalHRDataOfIERepository.GetList(dtParameters);
            return Json(dTResult);
        }
        [Authorization("RegionalHRDataOfIE", "Index", "delete")]
        public IActionResult Delete(string id)
        {
            try
            {
                string DecryptId = Common.DecryptQueryString(id);
                if (regionalHRDataOfIERepository.Remove(Convert.ToInt32(DecryptId), UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RegionalHRDataOfIE", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("RegionalHRDataOfIE", "Index", "edit")]
        public IActionResult InsertUpdate(RegionalHRDataOfIEModel model)
        {
            try
            {
                if (model.Id == 0)
                {
                    model.Createdby = UserId;
                    regionalHRDataOfIERepository.InsertUpdate(model);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Updatedby = UserId;
                    regionalHRDataOfIERepository.InsertUpdate(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RegionalHRDataOfIE", "RoleDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetIEData(int IeCd)
        {
            try
            {
                RegionalHRDataOfIEModel regional = regionalHRDataOfIERepository.GetIEDetails(IeCd);
                return Json(new { status = true, obj = regional });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RegionalHRDataOfIE", "GetIEData", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
