    using IBS.Interfaces;
using IBS.Models;
using IBS.Helper;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers
{
    public class AddRecieptVoucherController : BaseController
    {
        private readonly IAddRecieptVoucher addVoucherRepository;
        public AddRecieptVoucherController(IAddRecieptVoucher _addVoucherRepository)
        {
            addVoucherRepository = _addVoucherRepository;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult AddRecieptVoucher(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT)
        {
            AddRecieptVoucherModel model = new();

            if (VCHR_NO != "" && VCHR_NO != null)
            {
                model = addVoucherRepository.FindByID(VCHR_NO, BANK_CD, CHQ_NO, CHQ_DT);
            }
            return View(model);
        }

        public IActionResult VoucherList([FromBody] DTParameters dtParameters)
        {
            DTResult<AddRecieptVoucherModel> dTResult = addVoucherRepository.GetVoucherList(dtParameters);
            return Json(dTResult);
        }

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VoucherDetailsSave(AddRecieptVoucherModel model)
        {
            try
            {
                string msg = "Voucher Inserted Successfully.";

                if (model.VCHR_NO != "")
                {
                    msg = "Voucher Updated Successfully.";

                }
                //model.Createdby = UserId;
                //int i = contractRepository.ContractDetailsInsertUpdate(model);
                string i = addVoucherRepository.VoucherDetailsSave(model, GetUserInfo.Region.ToString());
                if (i != "" || i != "")
                {
                    return Json(new { status = true, responseText = msg });
                }
            }

            catch (Exception ex)
            {
                // Common.AddException(ex.ToString(), ex.Message.ToString(), "Contract", "ContractDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public ActionResult ButtonClick(string AccCD, string txtBPO, string lstBPO, string txtCSNO )
        {
            AddRecieptVoucherModel bPOmodel = new AddRecieptVoucherModel();
         
           var list = GetDistinctBPOsByCaseNo(txtCSNO, bPOmodel);
            
            string Narrt = string.Empty;
            try
            {
                if (AccCD == "2709" || AccCD == "2210" || AccCD == "2212")
                {
                  
                    var  result = addVoucherRepository.ChkCSNO(txtCSNO, lstBPO,out Narrt);

                    if (result == "")
                    {
                            ViewBag.AlertMessage = "Invalid Case No.!!!";
                        ViewBag.FocusOnTxtCSNO = true;  
                    }
                    
                    else 
                    {
                        Narrt = result;
                    }
                }
                else
                {
                    ViewBag.ShowBPO = true;
                  
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.Replace("\n", "");
                return RedirectToAction("Error", "Home", new { errMsg = errorMessage });
            }


            return Json(new { status = false, Narrt= Narrt, ((IBS.Models.AddRecieptVoucherModel)((Microsoft.AspNetCore.Mvc.ViewResult)list).Model).BPOList, responseText = "Oops Somthing Went Wrong !!" });
        }

        public ActionResult GetDistinctBPOsByCaseNo(string txtCSNO, AddRecieptVoucherModel bPOmodel)
        {
            string DropdownValues = "";
            var dropdownValues = addVoucherRepository.GetDistinctBPOsByCaseNo(txtCSNO);
            if(dropdownValues == null)
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


    }
}
