using IBS.Filters;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class InterUnit_TransferController : BaseController
    {
        private readonly IInterUnit_TransferRepository interunittransferrepository;
        SessionHelper objSessionHelper = new SessionHelper();
        public InterUnit_TransferController(IInterUnit_TransferRepository _interunittransferrepository)
        {
            interunittransferrepository = _interunittransferrepository;
        }

        [Authorization("InterUnit_Transfer", "Index", "view")]

        public IActionResult Index()
        {
            return View();
        }

        [Authorization("InterUnit_Transfer", "Index", "view")]
        public IActionResult Manage(string Bank, string ChqNo, string ChqDT)
        {
            InterUnit_TransferModel data = interunittransferrepository.Get_Inter_Unit_Transfer(Bank, ChqNo, ChqDT, Region);
            if (!string.IsNullOrEmpty(data.ErrorMsg))
            {
                AlertDanger(data.ErrorMsg);
                return RedirectToAction("Index");

            }
            else
            {
                objSessionHelper.lstInterUnitTransferRegionModel = data.lstUnitTransfer;
                //objSessionHelper.lstInterUnitTransferRegionModel = null;
            }

            if (Region == "N") { data.Region_ID = "3007"; }
            else if (Region == "E") { data.Region_ID = "3008"; }
            else if (Region == "S") { data.Region_ID = "3009"; }
            else if (Region == "W") { data.Region_ID = "3006"; }
            else if (Region == "C") { data.Region_ID = "3066"; }
            return View(data);
        }

        [HttpPost]
        [Authorization("InterUnit_Transfer", "Index", "edit")]
        public IActionResult DetailsSave(InterUnit_TransferModel model)
        {
            if (objSessionHelper.lstInterUnitTransferRegionModel != null)
            {
                model.lstUnitTransfer = objSessionHelper.lstInterUnitTransferRegionModel;
            }
            var result = interunittransferrepository.DetailsInsertUpdate(model, GetUserInfo);

            var msg = "";
            if (result) { msg = "Record Insert Successfully"; }
            else { msg = "Oops Somthing Went Wrong !!"; }
            return Json(new { status = result, responseText = msg }); ;
        }

        [HttpPost]
        public IActionResult LoadUnitTransferTable([FromBody] DTParameters dtParameters)
        {
            List<InterUnitTransferRegionModel> lstInterUnitTransferRegionModel = new List<InterUnitTransferRegionModel>();
            if (objSessionHelper.lstInterUnitTransferRegionModel != null)
            {
                lstInterUnitTransferRegionModel = objSessionHelper.lstInterUnitTransferRegionModel;
            }

            DTResult<InterUnitTransferRegionModel> dTResult = interunittransferrepository.GetInterUnitTransferRegion(dtParameters, lstInterUnitTransferRegionModel);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult SaveInterUnitTransfer(InterUnitTransferRegionModel model)
        {
            try
            {
                if (model.ACC_CD == "3007") { model.ACC_DESC = "Northern"; }
                else if (model.ACC_CD == "3008") { model.ACC_DESC = "Eastern"; }
                else if (model.ACC_CD == "3009") { model.ACC_DESC = "Southern"; }
                else if (model.ACC_CD == "3006") { model.ACC_DESC = "Western"; }
                else if (model.ACC_CD == "3066") { model.ACC_DESC = "Central"; }
                else if (model.ACC_CD == "9999") { model.ACC_DESC = "Bill Adjustment of Old System"; }
                else if (model.ACC_CD == "9998") { model.ACC_DESC = "Miscelleanous Adjustments"; }
                else if (model.ACC_CD == "9979") { model.ACC_DESC = "Refund"; }
                if (string.IsNullOrEmpty(model.ACTION))
                {
                    var res = objSessionHelper.lstInterUnitTransferRegionModel.Where(x => x.ACC_CD == model.ACC_CD).FirstOrDefault();

                    if (res != null)
                    {
                        return Json(new { status = false, responseText = model.ACC_DESC + " this region already exists." }); ;
                    }
                }
                List<InterUnitTransferRegionModel> lstInterUnitTransferRegionModel = objSessionHelper.lstInterUnitTransferRegionModel == null ? new List<InterUnitTransferRegionModel>() : objSessionHelper.lstInterUnitTransferRegionModel;
                lstInterUnitTransferRegionModel.RemoveAll(x => x.ID == Convert.ToInt32(model.ID));
                if (model.ID > 0)
                {
                    model.ID = model.ID;
                }
                else
                {
                    model.ID = lstInterUnitTransferRegionModel.Count > 0 ? (lstInterUnitTransferRegionModel.OrderByDescending(a => a.ID).FirstOrDefault().ID) + 1 : 1;
                }
                lstInterUnitTransferRegionModel.Add(model);
                objSessionHelper.lstInterUnitTransferRegionModel = lstInterUnitTransferRegionModel;
                return Json(new { status = true, responseText = "Inter Unit Transfer Added Successfully." });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InterUnit_Transfer ", "SaveInterUnitTransfer", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }


        [HttpGet]
        public IActionResult EditInterUnitTransfer(string id)
        {
            try
            {
                InterUnitTransferRegionModel InUniTrans = objSessionHelper.lstInterUnitTransferRegionModel.Where(x => x.ID == Convert.ToInt32(id)).FirstOrDefault();
                return Json(new { status = true, list = InUniTrans });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InterUnit_Transfer", "EditInterUnitTransfer", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        [Authorization("InterUnit_Transfer", "Index", "delete")]
        public IActionResult DetailDelete(string BANK_CD, string CHQ_NO, string CHQ_DT, string JV_NO, string DelID)
        {
            try
            {
                List<InterUnitTransferRegionModel> lstInterUnitTransferRegionModel = objSessionHelper.lstInterUnitTransferRegionModel == null ? new List<InterUnitTransferRegionModel>() : objSessionHelper.lstInterUnitTransferRegionModel;

                var model = lstInterUnitTransferRegionModel.Where(x => x.ID == Convert.ToInt32(DelID)).FirstOrDefault();
                var result = interunittransferrepository.DetailDelete(BANK_CD, CHQ_NO, CHQ_DT, JV_NO, DelID, model, GetUserInfo);
                if (result)
                {
                    return Json(new { status = true, responseText = "Inter Unit Transfer Deleted Successfully" });
                }
                //objSessionHelper.lstInterUnitTransferRegionModel = lstInterUnitTransferRegionModel;
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "InterUnit_Transfer", "DetailDelete", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
