using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ClientEntryFormController : BaseController
    {
        private readonly IClientEntryForm clientEntryForm;

        public ClientEntryFormController(IClientEntryForm _clientEntryForm)
        {
            clientEntryForm = _clientEntryForm;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            ClientEntryFormModel model = new();
            if (id > 0)
            {
                model = clientEntryForm.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ClientEntryFormModel> dTResult = clientEntryForm.GetClientEntryFormList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (clientEntryForm.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClientEntryForm", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClientEntryFormDetailsSave(ClientEntryFormModel model)
        {
            try
            {
                string msg = "Client Entry Form Inserted Successfully.";

                // if (model.Mobile > 0) 
                {
                    msg = "Client Entry Form Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = clientEntryForm.ClientEntryFormDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClientEntryForm", "ClientEntryFormDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
