using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class UnitOfMeasurementsMasterController : BaseController
    {
        private readonly IUnitOfMeasurementsRepository unitOfMeasurementsRepository;

        public UnitOfMeasurementsMasterController(IUnitOfMeasurementsRepository _unitOfMeasurementsRepository)
        {
            unitOfMeasurementsRepository = _unitOfMeasurementsRepository;
        }

        [Authorization("UnitOfMeasurementsMaster", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorization("UnitOfMeasurementsMaster", "Index", "view")]
        public IActionResult Manage(int id)
        {
            UOMModel model = new();
            if (id > 0)
            {
                model = unitOfMeasurementsRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        [Authorization("UnitOfMeasurementsMaster", "Index", "edit")]
        public IActionResult Manage(UOMModel model)
        {
            try
            {
                if (model.UomCd == 0)
                {
                    model.Createdby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    unitOfMeasurementsRepository.SaveDetails(model);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Updatedby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    unitOfMeasurementsRepository.SaveDetails(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "UnitOfMeasurements", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<UOMModel> dTResult = unitOfMeasurementsRepository.GetUOMList(dtParameters);
            return Json(dTResult);
        }

        [Authorization("UnitOfMeasurementsMaster", "Index", "delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (unitOfMeasurementsRepository.Remove(id, UserId))
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

    }
}
