using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class IC_Bookset_FormController : BaseController
    {
        private readonly I_ICBooksetFormRepository iCBooksetFormRepository;

        public IC_Bookset_FormController(I_ICBooksetFormRepository _iCBooksetFormRepository)
        {
            iCBooksetFormRepository = _iCBooksetFormRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            IC_Bookset_FormModel model = new() { BkSubmitted = "N", Region = Region };

            if (id > 0)
            {
                model = iCBooksetFormRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<IC_Bookset_FormModel> dTResult = iCBooksetFormRepository.GetBooksetList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult Manage(IC_Bookset_FormModel model)
        {
            try
            {
                ModelState.Remove("IssueDt");
                ModelState.Remove("BkSubmitDt");
                ModelState.Remove("CutOffDt");

                if (ModelState.IsValid)
                {
                    string returnMsg = iCBooksetFormRepository.IsExists(model);

                    if (string.IsNullOrEmpty(returnMsg))
                    {
                        if (model.Id == 0)
                        {
                            model.Createdby = UserId;
                            model.UserId = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                            iCBooksetFormRepository.SaveDetails(model);
                            AlertAddSuccess("Record Added Successfully.");
                        }
                        else
                        {
                            model.Updatedby = UserId;
                            model.UserId = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                            iCBooksetFormRepository.SaveDetails(model);
                            AlertAddSuccess("Record Updated Successfully.");
                        }
                        return RedirectToAction("Index");
                    }
                    else AlertAlreadyExist(returnMsg);
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Bookset_Form", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (iCBooksetFormRepository.Remove(id))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "iCBooksetFormRepository", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

    }
}

