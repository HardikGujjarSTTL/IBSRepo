using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class Bill_Paying_Officer_FormController : BaseController
    {
        #region Variables
        private readonly IBill_Paying_Officer_Form bill_Paying_Officer_Form;
        #endregion
        public Bill_Paying_Officer_FormController(IBill_Paying_Officer_Form _bill_Paying_Officer_Form)
        {
           bill_Paying_Officer_Form  = _bill_Paying_Officer_Form;
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
                model = bill_Paying_Officer_Form.FindByID(BpoCd);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<Bill_Paying_Officer_FormModel> dTResult = bill_Paying_Officer_Form.GetBPOList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (bill_Paying_Officer_Form.Remove(id, UserId))
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Bill_Paying_Officer_FormDetailsSave(Bill_Paying_Officer_FormModel model)
        {
            try
            {
                string msg = "Bill Paying Officer Inserted Successfully.";

              //  if (model.BpoCd > 0)
                {
                    msg = "Bill Paying Officer Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = bill_Paying_Officer_Form.BPODetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Bill_Paying_Officer_Form", "Bill_Paying_Officer_FormDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
