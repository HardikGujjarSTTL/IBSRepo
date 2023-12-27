using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class UnregisteredCallsController : BaseController
    {
        private readonly IUnregisteredCallsRepository unregisteredCallsRepository;

        public UnregisteredCallsController(IUnregisteredCallsRepository _unregisteredCallsRepository)
        {
            this.unregisteredCallsRepository = _unregisteredCallsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            dtParameters.AdditionalValues.Add("Region", Region);
            DTResult<UnregisteredCallsModel> dTResult = unregisteredCallsRepository.GetUnregisteredCallsList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Manage(int id)
        {
            UnregisteredCallsModel model = new() { Region = Region };

            if (id > 0)
            {
                model = unregisteredCallsRepository.FindByID(id);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Manage(UnregisteredCallsModel model)
        {
            try
            {
                if (model.IsNew)
                {
                    if (!unregisteredCallsRepository.IsExists(model.IeCd))
                    {
                        unregisteredCallsRepository.SaveDetails(model);
                        AlertAddSuccess();
                        return RedirectToAction("Index");
                    }
                    else
                        AlertAlreadyExist();
                }
                else
                {
                    unregisteredCallsRepository.SaveDetails(model);
                    AlertUpdateSuccess();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "UnregisteredCalls", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (unregisteredCallsRepository.Remove(id))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "UnregisteredCalls", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }
    }
}
