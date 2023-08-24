using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class RailwaysDirectoryController : BaseController
    {
        private readonly IRailwaysDirectoryRepository railwaysDirectoryRepository;

        public RailwaysDirectoryController(IRailwaysDirectoryRepository _railwaysDirectoryRepository)
        {
            railwaysDirectoryRepository = _railwaysDirectoryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            RailwaysDirectoryModel model = new();
            if (id > 0)
            {
                model = railwaysDirectoryRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Manage(RailwaysDirectoryModel model)
        {
            try
            {
                if (!railwaysDirectoryRepository.IsDuplicate(model))
                {
                    if (model.Id == 0)
                    {
                        model.Createdby = UserId;
                        model.UserId = USER_ID.Substring(0, 8);
                        railwaysDirectoryRepository.SaveDetails(model);
                        AlertAddSuccess("Record Added Successfully.");
                    }
                    else
                    {
                        model.Updatedby = UserId;
                        model.UserId = USER_ID.Substring(0, 8);
                        railwaysDirectoryRepository.SaveDetails(model);
                        AlertAddSuccess("Record Updated Successfully.");
                    }

                    return RedirectToAction("Index");
                }
                else
                    AlertAlreadyExist();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RailwaysDirectory", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<RailwaysDirectoryModel> dTResult = railwaysDirectoryRepository.GetRMList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                string RailwayCode = railwaysDirectoryRepository.IsExistsRailwayCode(id);
                if (RailwayCode != "")
                {
                    AlertDanger("You Cannot Delete this Raliway Code, because there is a record present for this Railway Code in " + RailwayCode + "!!!");
                }
                else
                {
                    if (railwaysDirectoryRepository.Remove(id))
                        AlertDeletedSuccess();
                    else
                        AlertDanger();
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RailwaysDirectory", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

    }
}
