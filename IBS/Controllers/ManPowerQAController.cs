using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class ManPowerQAController : BaseController
    {
        private readonly IManPowerQARepository manPowerQARepository;

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
            }
            return View(model);
        }

        [Authorization("ManPowerQA", "Index", "edit")]
        [HttpPost]
        public IActionResult Manage(ManpowerModel model)
        {
            try
            {
                if (model.ID == 0)
                {
                    model.UserID = UserId;
                    model.UserName = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                    var res = manPowerQARepository.SaveMaster(model);
                    if (res < 0)
                    {
                        AlertAlreadyExist("Record already exists !!");
                        return View(model);
                    }
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.UserID = UserId;
                    model.UserName = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                    manPowerQARepository.SaveMaster(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ManPowerQA", "Manage", 1, GetIPAddress());
            }
            return RedirectToAction("Index");
        }

        [Authorization("ManPowerQA", "Index", "view")]
        public IActionResult Detail()
        {            
            return View();            
        }

        [Authorization("ManPowerQA", "Index", "view")]
        public IActionResult ManageDetail(int id)
        {
            ManpowerDetailModel model = new();
            if (id > 0)
            {
                model = manPowerQARepository.DetailFindByID(id);
            }
            return View(model);
        }

        [Authorization("ManPowerQA", "Index", "edit")]
        [HttpPost]
        public IActionResult ManageDetail(ManpowerDetailModel model)
        {
            try
            {
                if (model.ID == 0)
                {
                    model.UserID = UserId;
                    model.UserName = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                    var res = manPowerQARepository.SaveDetails(model);
                    if (res < 0)
                    {
                        AlertAlreadyExist("Record already exists !!");
                        return View(model);
                    }
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.UserID = UserId;
                    model.UserName = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                    manPowerQARepository.SaveDetails(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ManPowerQA", "ManageDetail", 1, GetIPAddress());
            }
            return RedirectToAction("Detail");
        }

        [HttpPost]
        public IActionResult LoadTableDetail([FromBody] DTParameters dtParameters)
        {
            DTResult<ManpowerDetailModel> dTResult = manPowerQARepository.GetDetailList(dtParameters);
            return Json(dTResult);
        }
    }
}
