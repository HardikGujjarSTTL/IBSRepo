using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class Expenditure : BaseController
    {
        #region Variables
        private readonly IExpenditureRepository expenditureRepository;
        #endregion
        public Expenditure(IExpenditureRepository _expenditureRepository)
        {
            expenditureRepository = _expenditureRepository;
        }
        [Authorization("Expenditure", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("Expenditure", "Index", "view")]
        public IActionResult Manage(string _ExpPer)
        {
            ExpenditureModel model = new();
            if (_ExpPer != null)
            {
                model = expenditureRepository.FindByID(_ExpPer, GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ExpenditureModel> dTResult = expenditureRepository.GetExpenditureList(dtParameters, GetRegionCode);
            return Json(dTResult);
        }

        [Authorization("Expenditure", "Index", "delete")]
        public IActionResult Delete(string _ExpPer)
        {
            try
            {
                if (expenditureRepository.Remove(_ExpPer, GetRegionCode, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Expenditure", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("Expenditure", "Index", "edit")]
        public IActionResult ExpenditureDetailsSave(ExpenditureModel model)
        {
            try
            {
                string msg = "Expenditure Inserted Successfully.";

                if (model.ExpPer != null)
                {
                    msg = "Expenditure Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                model.UserId = Convert.ToString(UserId);
                model.RegionCode = GetRegionCode;
                var i = expenditureRepository.ExpenditureDetailsInsertUpdate(model);
                if (i != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Expenditure", "ExpenditureDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
