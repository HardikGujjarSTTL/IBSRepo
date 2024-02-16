using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class IE_CO_FormController : BaseController
    {
        private readonly I_IE_CO_FormRepository iE_CO_FormRepository;

        public IE_CO_FormController(I_IE_CO_FormRepository _iE_CO_FormRepository)
        {
            iE_CO_FormRepository = _iE_CO_FormRepository;
        }

        [Authorization("IE_CO_Form", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorization("IE_CO_Form", "Index", "view")]
        public IActionResult Manage(int id)
        {
            IE_CO_FormModel model = new();
            if (id > 0)
            {
                model = iE_CO_FormRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<IE_CO_FormModel> dTResult = iE_CO_FormRepository.GetCOList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        [Authorization("IE_CO_Form", "Index", "edit")]
        public IActionResult Manage(IE_CO_FormModel model)
        {
            try
            {
                if (model.CoCd == 0)
                {
                    model.Createdby = UserId;
                    model.CoRegion = Region;
                    var res = iE_CO_FormRepository.SaveDetails(model);
                    if(res < 0)
                    {
                        AlertAlreadyExist("Record already exists !!");
                        return View(model);
                    }
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Updatedby = UserId;
                    iE_CO_FormRepository.SaveDetails(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IE_CO_Form", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        [Authorization("IE_CO_Form", "Index", "delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (iE_CO_FormRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "iE_CO_FormRepository", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

    }
}