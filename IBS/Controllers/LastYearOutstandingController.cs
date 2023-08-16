using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class LastYearOutstanding : BaseController
    {
        #region Variables
        private readonly ILastYearOutstandingRepository LastYearOutstandingRepository;
        #endregion
        public LastYearOutstanding(ILastYearOutstandingRepository _lastYearOutstandingRepository)
        {
            LastYearOutstandingRepository = _lastYearOutstandingRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Manage(string _LyPer)
        {
            LastYearOutstandingModel model = new();
            if (_LyPer != null)
            {
                model = LastYearOutstandingRepository.FindByID(_LyPer, GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<LastYearOutstandingModel> dTResult = LastYearOutstandingRepository.GetLastYearOutstandingList(dtParameters,GetRegionCode);
            return Json(dTResult);
        }

        public IActionResult Delete(string _LyPer)
        {
            try
            {
                if(LastYearOutstandingRepository.Remove(_LyPer, GetRegionCode))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LastYearOutstanding", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LastYearOutstandingDetailsSave(LastYearOutstandingModel model)
        {
            try
            {
                string msg = "Last Year Outstanding Amount Inserted Successfully.";

                if (model.Ly_Per != null)
                {
                    msg = "Last Year Outstanding Amount Updated Successfully.";
                    //model.Updatedby = UserId;
                }
                //model.Createdby = UserId;
                model.Region_Code = GetRegionCode;
                var i = LastYearOutstandingRepository.LastYearOutstandingDetailsInsertUpdate(model);
                if (i != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LastYearOutstanding", "LastYearOutstandingDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
