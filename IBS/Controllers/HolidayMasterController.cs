using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Authorization;
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
            DTResult<HolidayMasterModel> dTResult = holidayMasterRepository.GetHolidayMasterList(dtParameters);
            return Json(dTResult);
        }

        [Authorization("HolidayMaster", "Index", "edit")]
        public IActionResult Manage(int id)
        {
            HolidayMasterModel model =new HolidayMasterModel();
            if (id > 0)
            {
                model = holidayMasterRepository.FindByID(id);
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
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HolidayMaster", "Manage", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }
    }
}
