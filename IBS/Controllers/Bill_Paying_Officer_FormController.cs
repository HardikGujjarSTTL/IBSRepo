using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers
{
    public class Bill_Paying_Officer_FormController : BaseController
    {
        #region Variables
        private readonly IBill_Paying_Officer_Form billrepository;
        #endregion
        public Bill_Paying_Officer_FormController(IBill_Paying_Officer_Form _billrepository)
        {
            billrepository = _billrepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string BpoCd)
        {
            Bill_Paying_Officer_FormModel model = new();
            if (BpoCd != null)
            {
                model = billrepository.FindByID(BpoCd);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<Bill_Paying_Officer_FormModel> dTResult = billrepository.GetBPOList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult Manage(Bill_Paying_Officer_FormModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.BpoCd))
                {
                    model.Createdby = UserId;
                    model.UserId = UserName.Substring(0, 8);
                    billrepository.BPOSave(model);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Updatedby = UserId;
                    model.UserId = UserName.Substring(0, 8);
                    billrepository.BPOSave(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Bill_Paying_Officer_Form", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (billrepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Bill_Paying_Officer_Form", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        public IActionResult GetState(int BpoCityCd)
        {
            try
            {
                string state = billrepository.GetState(BpoCityCd);
                return Json(state); // Return the state directly
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InspectionEngineers", "GetIeCity", 1, GetIPAddress());
            }
            return Json("Oops Somthing Went Wrong !!"); // Return an error message
        }


    }
}
