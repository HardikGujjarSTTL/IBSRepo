using IBS.Interfaces;
using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Vendor
{
    public class PurchesOrder1LOAController : BaseController
    {

        #region Variables
        private readonly IPurchesOrder1LOARepository purchesorderRepository;
        #endregion
        public PurchesOrder1LOAController(IPurchesOrder1LOARepository _purchesorderRepository)
        {
            purchesorderRepository = _purchesorderRepository;
        }


        public IActionResult Index(string CaseNo)
        {
            PurchesOrder1LOAModel model = new();

            if (CaseNo != null)
            {
                model = purchesorderRepository.FindByID(CaseNo);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<PurchesOrder1LOAModel> dTResult = purchesorderRepository.GetDataList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Manage(string CaseNo, byte ItemSrno, string type, string PoDt,int lstItemDesc)
        {
            PurchesOrder1LOAModel model = new();
            if (CaseNo != null)
            {
                //model = purchesorderRepository.FindByID(CaseNo);
                model = purchesorderRepository.FindByDetail(CaseNo, ItemSrno, type, PoDt, lstItemDesc);
            }

            return View(model);
        }

        public IActionResult GetUOMChanged(decimal id)
        {
            DTResult<PurchesOrder1LOAModel> dTResult = purchesorderRepository.FindByUOMDetail(id);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTableItemDetails([FromBody] DTParameters dtParameters)
        {
            DTResult<PurchesOrder1LOAModel> dTResult = purchesorderRepository.GetPODataList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsSave(PurchesOrder1LOAModel model)
        {
            try
            {
                string msg = "Inserted Successfully.";

                if (model.CaseNo != null && model.ItemSrno != null)
                {
                    msg = "Updated Successfully.";
                    model.UserId = Convert.ToString(UserName);
                    model.Updatedby = Convert.ToString(UserName);
                }
                model.UserId = Convert.ToString(UserName);
                model.Createdby = Convert.ToString(UserName);

                int i = purchesorderRepository.DetailsUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MA APPROVE FORM ", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
