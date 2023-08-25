using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class RitesDesignationMasterController : BaseController
    {
        private readonly IRitesDesignationMasterRepository ritesDesignationMasterRepository;

        public RitesDesignationMasterController(IRitesDesignationMasterRepository _ritesDesignationMasterRepository)
        {

            ritesDesignationMasterRepository = _ritesDesignationMasterRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            RDMModel model = new();
            if (id > 0)
            {
                model = ritesDesignationMasterRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Manage(RDMModel model)
        {
            try
            {
                if (model.RDesigCd == 0)
                {
                    model.Createdby = UserId;
                    ritesDesignationMasterRepository.SaveDetails(model);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Updatedby = UserId;
                    ritesDesignationMasterRepository.SaveDetails(model);
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
            DTResult<RDMModel> dTResult = ritesDesignationMasterRepository.GetRDMList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (ritesDesignationMasterRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RitesDesignationMaster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

    }
}
