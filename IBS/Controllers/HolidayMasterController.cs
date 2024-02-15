using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class HolidayMasterController : BaseController
    {
        #region Variables
        private readonly IHolidayMasterRepository holidayMasterRepository;
        #endregion
        public HolidayMasterController(IHolidayMasterRepository _holidayMasterRepository)
        {
            holidayMasterRepository = _holidayMasterRepository;
        }

        [Authorization("HolidayMaster", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<HolidayMasterModel> dTResult = holidayMasterRepository.GetHolidayMasterList(dtParameters, Region);
            return Json(dTResult);
        }

        [Authorization("HolidayMaster", "Index", "edit")]
        public IActionResult Manage(int id)
        {
            HolidayMasterModel model = new HolidayMasterModel();
            if (id > 0)
            {
                model = holidayMasterRepository.FindByID(id);
            }
            else
            {
                model.Region = Region;
            }
            return View(model);
        }

        [HttpPost]
        [Authorization("HolidayMaster", "Index", "edit")]
        public IActionResult ManageSave(HolidayMasterModel model)
        {
            int result = 0;
            try
            {
                if (model.ID == 0 || model.ID == null)
                {
                    model.CreatedBy = UserId;
                    model.User_Name = UserName.Length >= 8 ? UserName.Substring(0, 8) : UserName;
                    result = holidayMasterRepository.HolidayMasterSave(model);
                    if (result > 0)
                        AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.UpdatedBy = UserId;
                    model.User_Name = UserName.Length >= 8 ? UserName.Substring(0, 8) : UserName;
                    result = holidayMasterRepository.HolidayMasterSave(model);
                    if (result > 0)
                        AlertAddSuccess("Record Updated Successfully.");
                }
                if (result <= 0)
                    AlertDanger("Record Added/Updated Not Successfully.");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HolidayMaster", "ManageSave", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [Authorization("HolidayMaster", "Index", "delete")]
        public IActionResult HolidayMasterDelete(int id)
        {
            try
            {
                var model = new HolidayMasterModel();
                model.User_Name = UserName;
                model.UpdatedBy = UserId;
                var res = holidayMasterRepository.HolidayMasterDelete(id, model);
                if (res > 0)
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HolidayMaster", "HolidayMasterDelete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        public IActionResult HolidayDetail(int id)
        {
            HolidayDetailModel model = new HolidayDetailModel();
            model.HOLIDAY_ID = id;
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTableDetail([FromBody] DTParameters dtParameters)
        {
            DTResult<HolidayDetailModel> dTResult = holidayMasterRepository.GetHolidayDetailList(dtParameters, Region);
            return Json(dTResult);
        }

        [Authorization("HolidayMaster", "Index", "edit")]
        public IActionResult ManageDetail(int id)
        {
            HolidayDetailModel model = new HolidayDetailModel();
            if (id > 0)
            {
                model = holidayMasterRepository.Detail_FindByID(id);
                model.REGION = Region;
            }
            else
            {
                model.REGION = Region;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult HolidayDetailSave(HolidayDetailModel model)
        {
            int result = 0;
            try
            {
                if (model.ID == 0 || model.ID == null)
                {
                    model.USER_ID = UserId;
                    model.USER_NAME = UserName.Length >= 8 ? UserName.Substring(0, 8) : UserName;
                    result = holidayMasterRepository.HolidayDetailSave(model);
                    if (result > 0)
                        AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.USER_ID = UserId;
                    model.USER_NAME = UserName.Length >= 8 ? UserName.Substring(0, 8) : UserName;
                    result = holidayMasterRepository.HolidayDetailSave(model);
                    if (result > 0)
                        AlertAddSuccess("Record Updated Successfully.");
                }
                if (result <= 0)
                    AlertDanger("Record Added/Updated Not Successfully.");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HolidayMaster", "HolidayDetailSave", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
            return View(model);
        }

        [Authorization("HolidayMaster", "Index", "delete")]
        public IActionResult HolidayDetailDelete(int id)
        {
            try
            {
                var model = new HolidayDetailModel();
                model.USER_NAME = UserName;
                model.USER_ID = UserId;
                var res = holidayMasterRepository.HolidayDetailDelete(id, model);
                if (res > 0)
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HolidayMaster", "HolidayDetailDelete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

    }
}
