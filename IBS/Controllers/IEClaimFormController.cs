using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class IEClaimFormController : BaseController
    {
        private readonly IIEClaimFormRepository ieclaimrepository;
        public IEClaimFormController(IIEClaimFormRepository _ieclaimrepository)
        {
            ieclaimrepository = _ieclaimrepository;
        }

        [Authorization("IEClaimForm", "Manage", "view")]

        public IActionResult Manage(string CLAIM_NO, string ACTION, decimal ID)
        {
            IECliamFormModel model = new();

            if (CLAIM_NO != "" && CLAIM_NO != null)
            {
                model = ieclaimrepository.FindByID(CLAIM_NO, ACTION, ID);
            }
            return View(model);
        }


        [Authorization("IEClaimForm", "Insert_IE", "edit")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Insert_IE(IECliamFormModel model)
        {
            string result = "";
            try
            {
                string msg = "record Inserted Successfully.";

                if (model.CLAIM_NO != "")
                {
                    msg = "record Updated Successfully.";

                }
                //model.Createdby = UserId;
                //int i = contractRepository.ContractDetailsInsertUpdate(model);
                string Region = GetRegionCode;
                int uname = UserId;
                string i = ieclaimrepository.InsertIE(model, Region, uname);
                result = i;
                if (i != "" || i != "")
                {

                    return Json(new { status = true, result = result, responseText = msg });
                }
            }

            catch (Exception ex)
            {
                // Common.AddException(ex.ToString(), ex.Message.ToString(), "Contract", "ContractDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, result = result, responseText = "Oops Somthing Went Wrong !!" });
        }

        [Authorization("IEClaimForm", "Index", "view")]

        [HttpPost]
        public IActionResult IEList([FromBody] DTParameters dtParameters)
        {

            var Region = GetRegionCode;
            DTResult<IECliamFormModel> dTResult = ieclaimrepository.IE_List(dtParameters);
            return Json(dTResult);
        }


        [HttpPost]
        public IActionResult Manage_grid([FromBody] DTParameters dtParameters)
        {


            DTResult<IECliamFormModel> dTResult = ieclaimrepository.Manage_grid(dtParameters);
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult Payment_Save(string CLAIM_NO ,string VCHR_NO , string VCHR_DT)
        {

            string msg = "";
            try
            {
                string i = ieclaimrepository.Payment_Save(CLAIM_NO , VCHR_NO , VCHR_DT);
                if(i != null || i!= "")
                {
                    msg = "Payment Details Saved";
                }
                else
                {   
                    msg = "Something went wrong";
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { status = false , responseText = msg });
        }

        [Authorization("IEClaimForm", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
