using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace IBS.Controllers
{
    public class InterUnitRecieptController : BaseController
    {


        private readonly IInterUnitRecieptRepository interunitrecieptrepository;
        public InterUnitRecieptController(IInterUnitRecieptRepository _interunitrecieptrepository)
        {
            interunitrecieptrepository = _interunitrecieptrepository;
        }


        [Authorization("InterUnitReciept", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       

        public IActionResult InterUnitRecieptSave(InterUnitRecieptModel model)
        {
            string VCHR_NO = "";
            string VCHR_DT = "";
            try
            {
                string msg = "Voucher Inserted Successfully.";
                if (model.VCHR_NO != "")
                {
                    msg = "Voucher Updated Successfully.";

                }
                //model.Createdby = UserId;
                //int i = contractRepository.ContractDetailsInsertUpdate(model);
                string i = interunitrecieptrepository.InterUnitRecieptSave(model, GetUserInfo.Region.ToString());
                if (i != "" || i != "")
                {
                    VCHR_NO = i;
                    VCHR_DT = i;
                    return Json(new { status = true, responseText = msg, vchrNo = VCHR_NO , vchr_dt = VCHR_DT });
                }
            }

            catch (Exception ex)
            {
                // Common.AddException(ex.ToString(), ex.Message.ToString(), "Contract", "ContractDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, VCHR_NO = VCHR_NO , responseText = "Oops Somthing Went Wrong !!" });
        }

      

        public IActionResult Manage(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT, string VCHR_DT)
        {
            InterUnitRecieptModel model = new();
            if (VCHR_NO != "" && VCHR_NO != null)
            {
                 model.Action= "M";
                model = interunitrecieptrepository.FindByID(VCHR_NO, BANK_CD, CHQ_NO, CHQ_DT,VCHR_DT);
            }
            else
            {
                model.Action = "A";
            }
            return View(model);


            
        }


        

        public IActionResult RecieptList([FromBody] DTParameters dtParameters)
        {
            var Region = GetRegionCode;
            DTResult<InterUnitRecieptModel> dTResult = interunitrecieptrepository.RecieptList(dtParameters,Region);
            return Json(dTResult);
        }





        [HttpPost]
        public ActionResult ButtonClick(string AccCD, string txtBPO, string lstBPO, string txtCSNO, string txtBPOtype, string BPOCD)
        {

            InterUnitRecieptModel bPOmodel = new InterUnitRecieptModel();

            var list = GetDistinctBPOsByCaseNo(txtCSNO, bPOmodel, txtBPOtype, BPOCD);


            string Narrt = string.Empty;
            try
            {
                if (AccCD == "2709" || AccCD == "2210" || AccCD == "2212")
                {
                    var result = interunitrecieptrepository.ChkCSNO(txtCSNO);
                }
                else
                {
                    if (AccCD == "2201" || AccCD == "2202" || AccCD == "2203" || AccCD == "2204" || AccCD == "2205")
                    {
                        var result = interunitrecieptrepository.fill_BPO(txtCSNO, lstBPO, txtBPOtype);
                    }
                    else
                    {
                        var result = interunitrecieptrepository.fill_BPO01(txtCSNO, lstBPO, txtBPOtype);
                        if (result != null)
                        {
                            Narrt = Convert.ToString(result);
                        }
                    }
                }





            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.Replace("\n", "");
                return RedirectToAction("Error", "Home", new { errMsg = errorMessage });
            }


            return Json(new { status = false, Narrt = Narrt, ((IBS.Models.InterUnitRecieptModel)((Microsoft.AspNetCore.Mvc.ViewResult)list).Model).BPOList, responseText = "Oops Somthing Went Wrong !!" });
        }

        public ActionResult GetDistinctBPOsByCaseNo(string txtCSNO, InterUnitRecieptModel bPOmodel, string txtBPOtype, string BPOCD)
        {
            string DropdownValues = "";
            var dropdownValues = interunitrecieptrepository.GetDistinctBPOsByCaseNo(txtCSNO, txtBPOtype, BPOCD);
            if (dropdownValues == null)
            {
                ViewBag.AlertMessage = "Their is No BPO Present For the given Case No,To enter BPO goto PO Update.";

            }
            else
            {

                bPOmodel.BPOList = dropdownValues;
                //List<BPOmodel> bpoList = dropdownValues;
                //ViewBag.BPOList = new SelectList(bpoList, "BPO_CD", "BPO_NAME");
            }

            return View(bPOmodel);
        }

            public IActionResult Delete(string VCHR_NO, string CHQ_NO, string CHQ_DT, int BANK_CD)
            {
                try
                {
                    int U_ID = Convert.ToInt32(UserId);
                    if (interunitrecieptrepository.Remove(VCHR_NO, CHQ_NO , CHQ_DT , BANK_CD, U_ID))
                        AlertDeletedSuccess();
                    else
                        AlertDanger();
                }
                catch (Exception ex)
                {
                    Common.AddException(ex.ToString(), ex.Message.ToString(), "InterUnitReceipt", "Delete", 1, GetIPAddress());
                    AlertDanger();
                }
                return RedirectToAction("Index");
            }



    }
}
