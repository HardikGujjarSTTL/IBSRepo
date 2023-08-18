using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class IC_Bookset_FormController : BaseController
    {
        #region Variables
        private readonly I_IC_Bookset_Form iC_Bookset_Form;
        #endregion
        public IC_Bookset_FormController(I_IC_Bookset_Form _iC_Bookset_Form)
        {
            iC_Bookset_Form = _iC_Bookset_Form;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            IC_Bookset_FormModel model = new();
            if (id > 0)
            {
                model = iC_Bookset_Form.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<IC_Bookset_FormModel> dTResult = iC_Bookset_Form.GetBooksetList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (iC_Bookset_Form.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Bookset_Form", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IC_Bookset_FormDetailsSave(IC_Bookset_FormModel model)
        {
            try
            {
                string msg = "IC Bookset Form Inserted Successfully.";

               // if (model.BkNo > 0)
                {
                    msg = "IC Bookset Form Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = iC_Bookset_Form.BooksetDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IC_Bookset_Form", "IC_Bookset_FormDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}

