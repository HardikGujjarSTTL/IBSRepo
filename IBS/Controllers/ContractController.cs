using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ContractController : BaseController
    {
        #region Variables
        private readonly IContractRepository contractRepository;
        #endregion
        public ContractController(IContractRepository _contractRepository)
        {
            contractRepository = _contractRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Manage(int id)
        {
            ContractModel model = new();
            if (id > 0)
            {
                model = contractRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ContractModel> dTResult = contractRepository.GetContractList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (contractRepository.Remove(id, UserId))
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
        public IActionResult ContractDetailsSave(ContractModel model)
        {
            try
            {
                string msg = "Contract Inserted Successfully.";

                if (model.ContractId > 0)
                {
                    msg = "Contract Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = contractRepository.ContractDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Contract", "ContractDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
