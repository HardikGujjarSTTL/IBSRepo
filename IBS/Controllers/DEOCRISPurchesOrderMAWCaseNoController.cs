using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
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
        private readonly ISendMailRepository pSendMailRepository;
        #endregion
        public DEOCRISPurchesOrderMAWCaseNoController(IDEOCRISPurchesOrderWCaseNoRepository _purchesorderRepository, ISendMailRepository _pSendMailRepository)
        {
            purchesorderRepository = _purchesorderRepository;
            pSendMailRepository = _pSendMailRepository;
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
        public IActionResult GetVend_CD(string VEND_CD, string IMMS_VENDOR_CD, string VENDOR)
        {
            try
            {
                List<SelectListItem> agencyClient = new List<SelectListItem>();
                int? VendCd = 0;
                if (VEND_CD != null)
                {
                    agencyClient = Common.GetVendor_City(Convert.ToInt32(VEND_CD));
                }
                else
                {
                    VendCd = Common.GetVEND_CD(IMMS_VENDOR_CD);
                    if (VendCd != null && VendCd != 0)
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOCRISPurchesOrderMAWCaseNo", "SearchPurchaserOnLoad", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpGet]
        public IActionResult getBPOOnLoad(string RLY_CD, string IMMS_BPO_CD)
        {
            try
            {
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                string? BpoCd = "";
                BpoCd = Common.GetBPO_CDusingRLY_CD(RLY_CD, IMMS_BPO_CD);
                if (BpoCd != null && BpoCd != "0")
                {
                    selectListItems = Common.GetBPOList(BpoCd);
                }
                return Json(new { status = true, list = selectListItems, BpoCdID = BpoCd });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOCRISPurchesOrderMAWCaseNo", "getBPOOnLoad", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetBPO(string RLY_CD)
        {
            try
            {
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                selectListItems = Common.GetBPOListUsingBpoRly(RLY_CD);
                return Json(new { status = true, list = selectListItems });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOCRISPurchesOrderMAWCaseNo", "GetBPO", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetVendorUsingMFG(string MFG)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetVendorUsingText(MFG);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOCRISPurchesOrderMAWCaseNo", "GetVendorUsingMFG", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("DEOCRISPurchesOrderMAWCaseNo", "Index", "edit")]
        public IActionResult DeoCrisPOMasterSave(DEO_CRIS_PurchesOrderModel model)
        {
            try
            {
                string msg = "Record is updated!!!";
                model.UserId = Convert.ToString(UserId);
                bool objRet = purchesorderRepository.DetailsUpdate(model);
                if (objRet == true)
                {
                    return Json(new { status = true, responseText = msg});
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOCRISPurchesOrderMAWCaseNo", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]

        public IActionResult UpdateREMARKS(string REMARKS, int IMMS_POKEY, string IMMS_RLY_CD)
        {
            try
            {
                string msg = "Record is updated!!!";
                bool i = purchesorderRepository.UpdateREMARKS(REMARKS, IMMS_POKEY, IMMS_RLY_CD);
                if (i == true)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOCRISPurchesOrderMAWCaseNo", "UpdateREMARKS", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult AcceptPO(string REGION_CODE, string IMMS_POKEY, string IMMS_RLY_CD, string PoNo, string PoDt)
        {
            try
            {
                string retMsg = "";
                int err_code = 0;
                string RealCaseNo = "";
                string msg = "Successfully.";
                var Region = IBS.Helper.SessionHelper.UserModelDTO.Region;
                var UserID = IBS.Helper.SessionHelper.UserModelDTO.UserID;
                string[] result = new string[2];
                result = purchesorderRepository.GenerateRealCaseNoCRIS(REGION_CODE, IMMS_POKEY, IMMS_RLY_CD, Convert.ToString(UserID));
                if (result.Length > 0)
                {
                    if (result[0] != null)
                    {
                        err_code = Convert.ToInt32(result[0]);
                    }
                    if (result[1] != null)
                    {
                        RealCaseNo = result[1].ToString();
                    }
                }
                if (err_code != 0 && err_code <= 7)
                {
                    retMsg = HandleErrorCodes(err_code);
                    return Json(new { status = false, responseText = retMsg });
                }
                else
                {
                    if (result[1] != null)
                    {
                        //SendMail(IMMS_POKEY, PoNo, PoDt, RealCaseNo);
                        return Json(new { status = true, OUT_CASE_NO = RealCaseNo, responseText = msg });
                    }
                }
                return Json(new { status = false, responseText = "PO is not accepted" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "DEOCRISPurchesOrderMAWCaseNo", "EditCaseNo", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public bool SendMail(string CaseNo, string PoNo, string PoDt, string RealCaseNo)
        {
            var Region = IBS.Helper.SessionHelper.UserModelDTO.Region;
            string vendorEmail = purchesorderRepository.getVendorEmail(CaseNo);
            string wRegion = "";
            wRegion = GetRegionInfo(Region);
            string mail_body = "Dear Sir/Madam,\n\n In Reference to your PO: No. " + PoNo + " dated.  " + PoDt + " the Case No. allocated is  -  " + RealCaseNo + ". Kindly mention this Case No. while placing call on RITES. Thanks for using RITES Inspection Services. \n NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE). \n\n" + wRegion + ".";
            string sender = "";
            if (Region == "N")
            {
                sender = "nrinspn@rites.com";
            }
            else if (Region == "W")
            {
                sender = "wrinspn@rites.com";
            }
            else if (Region == "E")
            {
                sender = "erinspn@rites.com";
            }
            else if (Region == "S")
            {
                sender = "srinspn@rites.com";
            }
            else if (Region == "Q")
            {
                sender = "ritescqa@rites.com";
            }
            SendMailModel sendMailModel = new SendMailModel();
            // sender for local mail testing
            sender = "hardiksilvertouch007@outlook.com";
            sendMailModel.From = sender;
            sendMailModel.To = vendorEmail;
            sendMailModel.Subject = "Case No. allocated against PO registered by you on our Portal.";
            sendMailModel.Message = mail_body;
            bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
            return isSend;
        }

        public string GetRegionInfo(string region)
        {
            string wRegion = "";
            if (string.IsNullOrEmpty(region))
            {
                return wRegion;
            }
            switch (region)
            {
                case "N":
                    wRegion = "NORTHERN REGION \n 12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 \n Phone : +918800018691-95 \n Fax : 011-22024665";
                    break;
                case "S":
                    wRegion = "SOUTHERN REGION \n CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 \n Phone : 044-28292807/044- 28292817 \n Fax : 044-28290359";
                    break;
                case "E":
                    wRegion = "EASTERN REGION \n CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  \n Fax : 033-22348704";
                    break;
                case "W":
                    wRegion = "WESTERN REGION \n 5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 \n Phone : 022-68943400/68943445";
                    break;
                case "C":
                    wRegion = "Central Region";
                    break;
                case "Q":
                    wRegion = "CO QA Division";
                    break;
                default:
                    break;
            }

            return wRegion;
        }
        public string HandleErrorCodes(int err_code)
        {
            string msg = "";
            if (err_code == 1)
            {
                msg = "No Purchase Order Registered by the Vendor.";
            }
            else if (err_code == 2)
            {
                msg = "Either Agent/Client or Purchaser is Missing. The value [Others] is not acceptable for Agent/Client or Purchaser";
            }
            else if (err_code == 3)
            {
                msg = "Either Consignee or BPO is Missing in Item Details. The value [Others] is not acceptable in Consignee/BPO.";
            }
            else if (err_code == 4)
            {
                msg = "Unable to Insert data in PO Master (T13). Contact System Administrator.";
            }
            else if (err_code == 5)
            {
                msg = "Unable to Insert data in PO-BPO Master (T14). Contact System Administrator.";
            }
            else if (err_code == 6)
            {
                msg = "Unable to Insert data in PO Details (T15). Contact System Administrator.";
            }
            else if (err_code == 7)
            {
                msg = "Unable to Update Case No. in T80. Contact System Administrator.";
            }
            return msg;
        }




    }
}
