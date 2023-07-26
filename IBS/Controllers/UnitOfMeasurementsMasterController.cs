using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
	public class UnitOfMeasurementsMasterController : BaseController
    {
		#region Variables
		private readonly IUnitOfMeasurements unitOfMeasurements;
		#endregion
		public UnitOfMeasurementsMasterController(IUnitOfMeasurements _unitOfMeasurements)
		{
			unitOfMeasurements = _unitOfMeasurements;
		}
		public IActionResult Index()
        {
            return View();
        }

		public IActionResult Manage(int id)
		{
			UOMModel model = new();
			if (id > 0)
			{
				model = unitOfMeasurements.FindByID(id);
			}
			return View(model);
		}

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<UOMModel> dTResult = unitOfMeasurements.GetUOMList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (unitOfMeasurements.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "UnitOfMeasurementsMaster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UnitOfMeasurementsDetailsSave(UOMModel model)
        {
            try
            {
                string msg = "Unit Of Measurement Inserted Successfully.";

                if (model.UomCd > 0)
                {
                    msg = "Unit Of Measurement Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = unitOfMeasurements.UOMDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "UnitOfMeasurementsMaster", "UnitOfMeasurementsDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
