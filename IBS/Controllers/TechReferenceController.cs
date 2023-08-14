using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class TechReferenceController : BaseController
    {
        #region Variables
        private readonly ITechReferenceRepository techReferenceRepository;
        #endregion
        public TechReferenceController(ITechReferenceRepository _techReferenceRepository)
        {
            techReferenceRepository = _techReferenceRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Manage(string _techId)
        {
            TechReferenceModel model = new();
            if (_techId != null)
            {
                model = techReferenceRepository.FindByID(_techId,GetRegionCode);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
            {
            DTResult<TechReferenceModel> dTResult = techReferenceRepository.GetTechReferenceList(dtParameters,GetRegionCode);
            return Json(dTResult);
        }
        public IActionResult Delete(string _techId)
        {
            try
            {
                if (techReferenceRepository.Remove(_techId, GetRegionCode,UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Contract", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TechRefDetailsSave(TechReferenceModel model)
        {
            try
            {
                string msg = "TechReference Inserted Successfully.";
                if (model.TechId != null)
                {
                    msg = "TechReference Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                string i = techReferenceRepository.TechRefDetailsInsertUpdate(model,GetRegionCode);
                if (i != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "TechReference", "TechReferenceDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
