using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers.Vendor
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
        public IActionResult Manage(string CaseNo)
        {
            PO_MasterModel model = new();
            if (CaseNo != null)
            {
                model = pOMasterRepository.FindByID(CaseNo);
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
        public IActionResult Delete(string CaseNo)
        {
            try
            {
                if (pOMasterRepository.Remove(CaseNo, UserId))
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
        public IActionResult POMasterSave(PO_MasterModel model)
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                string msg = "PO Master Inserted Successfully.";
                if (model.CaseNo != null)
                {
                    msg = "PO Master Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                model.VendCd = VendCd;
                if (model.PoiCd == null || model.PoiCd == 0)
                {
                    model.PoiCd = VendCd;
                }

                string i = pOMasterRepository.POMasterDetailsInsertUpdate(model);
                if (i != "" && i != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "POMasterSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetLOADetails(string CaseNo)
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                var obj = pOMasterRepository.FindCaseNo(CaseNo, VendCd);
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
        public IActionResult GetVendor(int id = 0)
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                if (id > 0)
                {
                    VendCd = id;
                }
                List<SelectListItem> agencyClient = Common.GetVendor(VendCd);
                foreach (var item in agencyClient.Where(x => x.Value == Convert.ToString(VendCd)).ToList())
                {
                    if (item.Value == Convert.ToString(VendCd))
                    {
                        item.Selected = true;
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
        public IActionResult Getvendor_status()
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
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
        public IActionResult GetManufVEND(int id = 0)
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                if (id > 0)
                {
                    VendCd = id;
                }
                VendorModel getvendor = Common.GetManufVEND(VendCd);
                return Json(new { status = true, getvendor = getvendor });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetManufVEND", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult ManageVendorDetails(string CaseNo)
        {
            PO_MasterModel model = new();
            if (CaseNo != null)
            {
                model = pOMasterRepository.FindByID(CaseNo);
            }
            return View(model);
        }

        public IActionResult PODetails(string CaseNo)
        {
            PO_MasterModel model = new();
            if (CaseNo != null)
            {
                model = pOMasterRepository.FindByID(CaseNo);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTableForPODetails([FromBody] DTParameters dtParameters)
        {
            DTResult<PO_MasterDetailListModel> dTResult = pOMasterRepository.GetPOMasterDetailsList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult DeletePODetails(string CASE_NO, string ITEM_SRNO)
        {
            try
            {
                if (pOMasterRepository.RemovePODetails(CASE_NO, ITEM_SRNO, UserId))
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

        public IActionResult PoDetailManage(string CASE_NO, string ITEM_SRNO)
        {
            PO_MasterDetailsModel model = new();
            PO_MasterModel modelPM = new();
            if (CASE_NO != null)
            {
                modelPM = pOMasterRepository.FindByID(CASE_NO);
            }
            if (CASE_NO != null && ITEM_SRNO != null && ITEM_SRNO != "0")
            {
                model = pOMasterRepository.FindPODetailsByID(CASE_NO, ITEM_SRNO);
            }
            else
            {
                model.CaseNo = modelPM.CaseNo;
                model.ItemSrno = Convert.ToByte(pOMasterRepository.GenerateITEM_SRNO(CASE_NO));
            }
            model.RlyCd = modelPM.RlyCd;
            model.RlyNonrly = modelPM.RlyNonrly;
            model.PoDt = modelPM.PoDt;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult POMasterDetailsSave(PO_MasterDetailsModel model)
        {
            try
            {
                int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
                string msg = "PO Master Details Inserted Successfully.";
                if (model.CaseNo != null)
                {
                    msg = "PO Master Details Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = pOMasterRepository.POMasterSubDetailsInsertUpdate(model);
                if (i >0)
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

        [HttpGet]
        public IActionResult GetBillPayingOfficer(string SBPO)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetBillPayingOfficerUsingSBPO(SBPO);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetBillPayingOfficer", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpGet]
        public IActionResult GetConsigneeUsingConsignee(int ConsigneeSearch)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetConsigneeUsingConsignee(ConsigneeSearch);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "POMaster", "GetConsigneeUsingConsignee", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetUOMChanged(decimal id)
        {
            DTResult<PO_MasterDetailsModel> dTResult = pOMasterRepository.FindByUOMDetail(id);
            return Json(dTResult);
        }
    }
}
