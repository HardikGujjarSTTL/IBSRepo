using IBS.Filters;
using IBS.Interfaces;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers.InspectionBilling
{
    [Authorization]
    public class AdministratorPurchaseOrderController : BaseController
    {
        #region Variables
        private readonly IAdministratorPurchaseOrderRepository pIAdministratorPurchaseOrderRepository;
        #endregion
        public AdministratorPurchaseOrderController(IAdministratorPurchaseOrderRepository _pIAdministratorPurchaseOrderRepository)
        {
            pIAdministratorPurchaseOrderRepository = _pIAdministratorPurchaseOrderRepository;
        }

        [Authorization("AdministratorPurchaseOrder", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string region_code = Convert.ToString(IBS.Helper.SessionHelper.UserModelDTO.Region);
            DTResult<AdministratorPurchaseOrderListModel> dTResult = pIAdministratorPurchaseOrderRepository.GetPOMasterList(dtParameters, region_code);
            return Json(dTResult);
        }

        [Authorization("AdministratorPurchaseOrder", "Index", "view")]
        public IActionResult Manage(string PO_TYPE,string RLY_CD,string CaseNo)
        {
            AdministratorPurchaseOrderModel model = new();
            if (CaseNo != null)
            {
                model = pIAdministratorPurchaseOrderRepository.FindByID(CaseNo);
                string[] types = getType(model.RlyNonrly, model.RlyCd);
                if (model.VendCd != null && model.PoiCd != null)
                {
                    VendorModel getvendor = Common.GetManufVEND(Convert.ToInt32(model.VendCd));
                    model.MPOI = getvendor.VendAdd1;
                }
                ViewBag.Railway = types[0];
                ViewBag.RLY_CD = types[1];
            }
            else
            {
                string[] types = getType(PO_TYPE, RLY_CD);
                ViewBag.Railway = types[0];
                ViewBag.RLY_CD = types[1];
                model.RlyNonrly = PO_TYPE;
                model.RlyCd = RLY_CD;
            }
            return View(model);
        }

        public string[] getType(string ptype, string rtype)
        {
            string[] strings = new string[2];
            if (ptype == "P" || ptype == "F" || ptype == "U" || ptype == "S")
            {
                string rcd = ptype == "P" ? "Private" :
                                     ptype == "F" ? "Foreign Railway" :
                                     ptype == "U" ? "PSU" :
                                     ptype == "S" ? "State Government" : "";
                strings[0]= rcd;
                strings[1] = " (" + rtype + ") ";
                return strings;
            }
            else
            {
                string rcd = Common.GetRailway(rtype);
                strings[0] = "Railways";
                strings[1] = " (" + rcd + ") ";
                return strings;
            }
        }
        [HttpGet]
        public IActionResult GetRailwayCode(string type)
        {
            try
            {
                List<SelectListItem> objList = Common.GetRailwayCode(type);
                return Json(new { status = true, list = objList });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "GetRailwayCode", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult getVendor(string vend_cd)
        {
            try
            {
                List<SelectListItem> objList = Common.GetVendCd(vend_cd);
                return Json(new { status = true, list = objList });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "getVendor", 1, GetIPAddress());
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "Getvendor_status", 1, GetIPAddress());
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "GetPurchaserCd", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetVendor(int id = 0)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetVendor(id);
                foreach (var item in agencyClient.Where(x => x.Value == Convert.ToString(id)).ToList())
                {
                    if (item.Value == Convert.ToString(id))
                    {
                        item.Selected = true;
                    }
                }
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "GetVendor", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetManufVEND(int id = 0)
        {
            try
            {
                VendorModel getvendor = Common.GetManufVEND(id);
                return Json(new { status = true, getvendor = getvendor });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "GetManufVEND", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("AdministratorPurchaseOrder", "Index", "edit")]
        public IActionResult POMasterSave(AdministratorPurchaseOrderModel model)
        {
            try
            {
                string RegionCode = IBS.Helper.SessionHelper.UserModelDTO.Region;
                string msg = "Administrator Purchase Order Inserted Successfully.";
                if (model.CaseNo != null)
                {
                    msg = "Administrator Purchase Order Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                model.UserId = Convert.ToString(UserId);
                model.RegionCode = RegionCode;
                string i = pIAdministratorPurchaseOrderRepository.POMasterDetailsInsertUpdate(model);
                if (i != "" && i != null)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "POMasterSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult ConsigneeLoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ConsigneeListModel> dTResult = pIAdministratorPurchaseOrderRepository.GetConsigneeDetaisList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult ConsigneeDelete(string CASE_NO, string CONSIGNEE_CD,string BPO_CD)
        {
            try
            {
                if (pIAdministratorPurchaseOrderRepository.ConsigneeDelete(CASE_NO, CONSIGNEE_CD, BPO_CD))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "AdministratorPurchaseOrder", "ConsigneeDelete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }
    }
}
