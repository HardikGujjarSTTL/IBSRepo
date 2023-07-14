﻿using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class POMasterController : BaseController
    {
        #region Variables
        private readonly IPOMasterRepository pOMasterRepository;
        #endregion
        public POMasterController(IPOMasterRepository _pOMasterRepository)
        {
            pOMasterRepository = _pOMasterRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Manage(int id)
        {
            PO_MasterModel model = new();
            if (id > 0)
            {
                model = pOMasterRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<PO_MasterModel> dTResult = pOMasterRepository.GetPOMasterList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (pOMasterRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult POMasterDetailsSave(PO_MasterModel model)
        {
            try
            {
                string msg = "PO Master Inserted Successfully.";
                if (model.Id > 0)
                {
                    msg = "PO Master Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = pOMasterRepository.POMasterDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "POMasterDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
