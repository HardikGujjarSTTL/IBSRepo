using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using IBS.Helper;

namespace IBS.Controllers
{
    [Authorization]
    public class ManPowerQAController : BaseController
    {
        private readonly IManPowerQARepository manPowerQARepository;
        SessionHelper objSessionHelper = new SessionHelper();

        public ManPowerQAController(IManPowerQARepository _manPowerQARepository)
        {
            manPowerQARepository = _manPowerQARepository;
        }

        [Authorization("ManPowerQA", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ManpowerModel> dTResult = manPowerQARepository.GetMasterList(dtParameters);
            return Json(dTResult);
        }

        [Authorization("ManPowerQA", "Index", "view")]
        public IActionResult Manage(int id)
        {
            ManpowerModel model = new();
            if (id > 0)
            {
                model = manPowerQARepository.FindByID(id);
                objSessionHelper.lstManpowerDetailModel = model.lstManpowerDetailModel;
            }
            else
            {
                objSessionHelper.lstManpowerDetailModel = null;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ManPowerQA", "Index", "edit")]
        public IActionResult ManPowerSave(ManpowerModel model)
        {
            try
            {
                string msg = "";
                if (model.ID == 0)
                {
                    model.UserID = UserId;
                    model.UserName = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                    if (objSessionHelper.lstManpowerDetailModel != null)
                    {
                        model.lstManpowerDetailModel = objSessionHelper.lstManpowerDetailModel;
                    }
                    var res = manPowerQARepository.SaveMaster(model);
                    if (res < 0)
                    {
                        msg = "Record already exists !!";
                        return View(model);
                    }
                    msg = "Record Added Successfully.";
                }
                else
                {
                    model.UserID = UserId;
                    model.UserName = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                    manPowerQARepository.SaveMaster(model);
                    msg = "Record Updated Successfully.";
                }
                return Json(new { status = true, responseText = msg });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ManPowerQA", "Manage", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult SaveManpowerDetail(ManpowerDetailModel model)
        {
            try
            {
                var DetailsID = 0;
                if (model.ManpowerID > 0)
                {
                    model.UserID = UserId;
                    DetailsID = manPowerQARepository.SaveDetails(model);
                }
                List<ManpowerDetailModel> lstManpowerDetailModel = objSessionHelper.lstManpowerDetailModel == null ? new List<ManpowerDetailModel>() : objSessionHelper.lstManpowerDetailModel;
                lstManpowerDetailModel.RemoveAll(x => x.DetailID == Convert.ToInt32(model.DetailID));
                if (model.DetailID > 0)
                {
                    model.DetailID = model.DetailID;
                }
                else
                {
                    model.DetailID = lstManpowerDetailModel.Count > 0 ? (lstManpowerDetailModel.OrderByDescending(a => a.DetailID).FirstOrDefault().DetailID) + 1 : 1;
                }
                if (model.ManpowerID > 0 && DetailsID > 0)
                {
                    model.DetailID = DetailsID;
                }
                lstManpowerDetailModel.Add(model);
                objSessionHelper.lstManpowerDetailModel = lstManpowerDetailModel;
                return Json(new { status = true, responseText = "Manpower Detail Added Successfully." });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ManPowerQA ", "SaveManpowerDetail", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult LoadManpowerDetailTable([FromBody] DTParameters dtParameters)
        {
            List<ManpowerDetailModel> lstManpowerDetailModel = new List<ManpowerDetailModel>();
            if (objSessionHelper.lstManpowerDetailModel != null)
            {
                lstManpowerDetailModel = objSessionHelper.lstManpowerDetailModel;
            }
            DTResult<ManpowerDetailModel> dTResult = manPowerQARepository.GetManpowerDetailList(dtParameters, lstManpowerDetailModel);
            return Json(dTResult);
        }

        [HttpGet]
        public IActionResult EditManpowerDetail(string id)
        {
            try
            {
                ManpowerDetailModel manpowerDetail = objSessionHelper.lstManpowerDetailModel.Where(x => x.DetailID == Convert.ToInt32(id)).FirstOrDefault();
                return Json(new { status = true, data = manpowerDetail });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ManPowerQA", "EditManpowerDetail", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult DeleteManpowerDetail(int DetailID, int ManpowerID)
        {
            try
            {
                if (ManpowerID > 0)
                {
                    var res = manPowerQARepository.DeleteManpowerDetail(DetailID,ManpowerID);
                }
                List<ManpowerDetailModel> manpowerDetail = objSessionHelper.lstManpowerDetailModel == null ? new List<ManpowerDetailModel>() : objSessionHelper.lstManpowerDetailModel;
                manpowerDetail.RemoveAll(x => x.DetailID == DetailID);
                objSessionHelper.lstManpowerDetailModel = manpowerDetail;
                return Json(new { status = true, responseText = "Manpower Detail Deleted Successfully" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ManPowerQA", "DeleteManpowerDetail", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult DeleteManpower(int ID)
        {
            try
            {
                if (ID > 0)
                {
                    var res = manPowerQARepository.DeleteManpower(ID, UserId);
                    if (res>0)
                    {

                    }
                    return Json(new { status = true, responseText = "Manpower Deleted Successfully" });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ManPowerQA", "DeleteManpower", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
