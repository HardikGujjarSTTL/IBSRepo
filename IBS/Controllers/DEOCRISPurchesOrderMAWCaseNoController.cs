using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;

namespace IBS.Controllers
{
    [Authorization]
    public class DEOCRISPurchesOrderMAWCaseNoController : BaseController
    {
        #region Variables
        private readonly IDEOCRISPurchesOrderWCaseNoRepository purchesorderRepository;
        #endregion
        public DEOCRISPurchesOrderMAWCaseNoController(IDEOCRISPurchesOrderWCaseNoRepository _purchesorderRepository)
        {
            purchesorderRepository = _purchesorderRepository;
        }
        [Authorization("DEOCRISPurchesOrderMAWCaseNo", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<DEO_CRIS_PurchesOrderListModel> dTResult = purchesorderRepository.GetDataList(dtParameters, Region);
            return Json(dTResult);
        }

        [Authorization("DEOCRISPurchesOrderMAWCaseNo", "Index", "view")]
        public IActionResult Manage(string ImmsPokey, string ImmsRlyCd)
        {
            DEO_CRIS_PurchesOrderModel model = new();

            if (ImmsPokey != null && ImmsRlyCd != null)
            {
                model = purchesorderRepository.FindByID(ImmsPokey, ImmsRlyCd);
                if (model != null)
                {
                    if (model.VEND_CD != null)
                    {
                        List<SelectListItem> selectListItems = Common.GetVendor_City(Convert.ToInt32(model.VEND_CD));
                        //model.selectVend_CDListItems = Common.GetVendor_City(Convert.ToInt32(model.VEND_CD)); 
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult GetVend_CD(string VEND_CD, string IMMS_VENDOR_CD,string VENDOR)
        {
            try
            {
                List<SelectListItem> agencyClient = new List<SelectListItem>();
                int? VendCd=0;
                if (VEND_CD != null)
                {
                    agencyClient = Common.GetVendor_City(Convert.ToInt32(VEND_CD));
                }
                else
                {
                    VendCd = Common.GetVEND_CD(IMMS_VENDOR_CD);
                    if(VendCd != null && VendCd != 0)
                    {
                        agencyClient = Common.GetVendor_City(Convert.ToInt32(VendCd));
                    }
                    else
                    {
                        agencyClient = Common.GetVendorUsingText(VENDOR);
                    }
                }
                return Json(new { status = true, list = agencyClient, Vend_CdID = VendCd });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOCRISPurchesOrderMAWCaseNo", "GetAgencyClient", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult GetPurchaserCdusingConsigneeCd(string ConsigneeCd)
        {
            try
            {
                List<SelectListItem> Objlist = Common.GetPurchaserCdusingConsigneeCd(Convert.ToInt32(ConsigneeCd));
                return Json(new { status = true, list = Objlist });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOCRISPurchesOrderMAWCaseNo", "GetPurchaserCdusingConsigneeCd", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult SearchPurchaserOnLoad(string RLY_CD, string IMMS_PURCHASER_CD)
        {
            try
            {
                List<SelectListItem> agencyClient = new List<SelectListItem>();
                int? PurchaserCd = 0;
                PurchaserCd = Common.GetVEND_CDusingRLY_CD(RLY_CD, IMMS_PURCHASER_CD);
                if (PurchaserCd != null && PurchaserCd != 0)
                {
                    agencyClient = Common.GetPurchaserCdusingConsigneeCd(Convert.ToInt32(PurchaserCd));
                }
                return Json(new { status = true, list = agencyClient, Vend_CdID = PurchaserCd });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOCRISPurchesOrderMAWCaseNo", "GetAgencyClient", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("DEOCRISPurchesOrderMAWCaseNo", "Index", "edit")]
        public IActionResult DetailsSave(DEOCRISPurchesOrderMAModel model)
        {
            try
            {
                string msg = "Inserted Successfully.";

                if (model.Rly != null && model.Makey != null && model.Slno != null)
                {
                    msg = "Updated Successfully.";
                    model.ApprovedBy = Convert.ToString(UserId);
                }

                int i = purchesorderRepository.DetailsUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "CRIS PURCHASE ORDERS REGISTERED", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
