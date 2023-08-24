using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class Highlights : BaseController
    {
        #region Variables
        private readonly IHighlightsRepository highlightsRepository;
        #endregion
        public Highlights(IHighlightsRepository _highlightsRepository)
        {
            highlightsRepository = _highlightsRepository;
        }
        [Authorization("Highlights", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("Highlights", "Index", "view")]
        public IActionResult Manage(string _highDt)
        {
            HighlightsModel model = new();
            if (_highDt != null)
            {
                model = highlightsRepository.FindByID(_highDt, GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<HighlightsModel> dTResult = highlightsRepository.GetHighlightsList(dtParameters,GetRegionCode);
            return Json(dTResult);
        }
        [Authorization("Highlights", "Index", "delete")]
        public IActionResult Delete(string _highDt,string RegionCode)
        {
            try
            {
                if(highlightsRepository.Remove(_highDt, GetRegionCode, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Highlights", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("Highlights", "Index", "add")]
        public IActionResult HighlightsDetailsSave(HighlightsModel model)
        {
            try
            {
                string msg = "Highlights Inserted Successfully.";

                if (model.High_Dt != null)
                {
                    msg = "Highlights Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                model.User_Id =Convert.ToString(UserId);
                model.Region_Code = GetRegionCode;
                var i = highlightsRepository.HighlightsDetailsInsertUpdate(model);
                if (i != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Highlights", "HighlightsDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
