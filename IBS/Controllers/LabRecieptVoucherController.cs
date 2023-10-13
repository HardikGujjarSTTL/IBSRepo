using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class LabRecieptVoucherController : BaseController
    {
        #region Variables
        private readonly ILabRecieptVoucherRepository LabRecieptVoucherRepository;
        #endregion
        public LabRecieptVoucherController(ILabRecieptVoucherRepository _LabRecieptVoucherRepository)
        {
            LabRecieptVoucherRepository = _LabRecieptVoucherRepository;
        }
        [Authorization("LabRecieptVoucher", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("LabRecieptVoucher", "Index", "view")]
        public IActionResult AddRecieptVoucher(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT)
        {
            var regin = GetRegionCode;
            ViewBag.RolCD = GetAuthType;
            ViewBag.regin = regin;
            LabRecieptVoucherModel model = new();

            if (VCHR_NO != "" && VCHR_NO != null)
            {
                model = LabRecieptVoucherRepository.FindByID(VCHR_NO, BANK_CD, CHQ_NO, CHQ_DT);
            }
            return View(model);
        }

        public IActionResult VoucherList([FromBody] DTParameters dtParameters)
        {
            DTResult<LabRecieptVoucherModel> dTResult = LabRecieptVoucherRepository.GetVoucherList(dtParameters);
            return Json(dTResult);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("LabRecieptVoucher", "Index", "edit")]
        public IActionResult VoucherDetailsSave(LabRecieptVoucherModel model)
        {
            try
            {
                string msg = "Voucher Inserted Successfully.";

                if (model.VCHR_NO != "" && model.VCHR_NO != null)
                {
                    msg = "Voucher Updated Successfully.";

                }
                //model.Createdby = UserId;
                //int i = contractRepository.ContractDetailsInsertUpdate(model);
                string i = LabRecieptVoucherRepository.VoucherDetailsSave(model, GetUserInfo.Region.ToString());
                if (i != "0")
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
        [Authorization("LabRecieptVoucher", "Index", "edit")]
        public ActionResult ButtonClick(string AccCD, string txtBPO, string lstBPO, string txtCSNO)
        {
            LabRecieptVoucherModel bPOmodel = new LabRecieptVoucherModel();

            var list = GetDistinctBPOsByCaseNo(txtCSNO, bPOmodel);

            string Narrt = string.Empty;
            try
            {
                if (AccCD == "2709" || AccCD == "2210" || AccCD == "2212")
                {
                    var result = "";
                    if (txtCSNO != "" && txtCSNO != null)
                    {
                         result = LabRecieptVoucherRepository.ChkCSNO(txtCSNO, lstBPO, out Narrt);
                    }
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


            return Json(new { status = false, Narrt = Narrt, ((IBS.Models.LabRecieptVoucherModel)((Microsoft.AspNetCore.Mvc.ViewResult)list).Model).BPOList, responseText = "Oops Somthing Went Wrong !!" });
        }

        public ActionResult GetDistinctBPOsByCaseNo(string txtCSNO, LabRecieptVoucherModel bPOmodel)
        {
            var dropdownValues = LabRecieptVoucherRepository.GetDistinctBPOsByCaseNo(txtCSNO);
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


    }
}
