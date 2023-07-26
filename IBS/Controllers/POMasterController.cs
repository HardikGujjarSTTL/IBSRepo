﻿using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
            DTResult<PO_MasterModel> dTResult = pOMasterRepository.GetPOMasterList(dtParameters, VendCd);
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

        public IActionResult GetLOADetails(string CaseNo)
        {
            try
            {
                var obj = pOMasterRepository.FindCaseNo(CaseNo);
                return Json(new { status = true, poMaster = obj });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetLOADetails", 1, GetIPAddress());
            }

            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetAgencyClient(string RlyNonrly)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetAgencyClient(RlyNonrly);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetAgencyClient", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult Getfill_consignee_purcher(string RlyNonrlyValue, string RlyNonrlyText, string RlyCd)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.Getfill_consignee_purcher(RlyNonrlyValue, RlyNonrlyText, RlyCd);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "Getfill_consignee_purcher", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetPurchaserCd(string consignee)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetPurchaserCd(consignee);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetPurchaserCd", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpGet]
        public IActionResult GetVendor()
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                List<SelectListItem> agencyClient = Common.GetVendor(VendCd);
                foreach (var item in agencyClient.Where(x=>x.Value == Convert.ToString(VendCd)).ToList())
                {
                    if(item.Value == Convert.ToString(VendCd))
                    {
                        item.Selected=true;
                    }
                }
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetVendor", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult Getvendor_status(int VendCd)
        {
            try
            {
                VendorModel getvendor_status = Common.Getvendor_status(VendCd);
                if (getvendor_status != null)
                {
                    if (getvendor_status.VendStatus == "B")
                    {
                        return Json(new { status = true, responseText = "This Vendor is Banned/Blacklisted From  " + getvendor_status.VendStatusDtFr + " To " + getvendor_status.VendStatusDtTo });
                    }
                }
                return Json(new { status = false, responseText = "" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "Getvendor_status", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetManufVEND(int VendCd)
        {
            try
            {
                VendorModel getvendor = Common.GetManufVEND(VendCd);
                return Json(new { status = true, getvendor = getvendor });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetManufVEND", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
