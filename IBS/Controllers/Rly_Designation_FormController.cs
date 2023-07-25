using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using MessagePack;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class Rly_Designation_FormController : BaseController
    {
        #region Variables
        private readonly IRly_Designation_Form rly_Designation_Form;
       #endregion
        public Rly_Designation_FormController(IRly_Designation_Form _rly_Designation_Form)
        {
            rly_Designation_Form = _rly_Designation_Form;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string id)
        {
            Rly_Designation_FormModel model = new();
            if (id != null)
            {
                model = rly_Designation_Form.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<Rly_Designation_FormModel> dTResult = rly_Designation_Form.GetRly_Designation_FormList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(String id)
        {
            try
            {
                if (rly_Designation_Form.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Rly_Designation_Form", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rly_Designation_FormDetailsSave(Rly_Designation_FormModel model)
        {
            try
            {
                string msg = "Railway Designation Master Inserted Successfully.";

               // if (model.RlyDesigCd > 0)
                {
                    msg = "Railway Designation Master Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                string i = rly_Designation_Form.Rly_Designation_FormInsertUpdate(model);
                if (i != "" && i != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Rly_Designation_Form", "Rly_Designation_FormDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}