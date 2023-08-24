using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using MessagePack;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class RailwaysDesignationController : BaseController
    {
        #region Variables
        private readonly IRailwaysDesignationRepository railwaysDesignationRepository;
       #endregion
        public RailwaysDesignationController(IRailwaysDesignationRepository _railwaysDesignationRepository)
        {
            railwaysDesignationRepository = _railwaysDesignationRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(string id)
        {
            Rly_Designation_FormModel model = new();

            if (!string.IsNullOrEmpty(id))
            {
                model = railwaysDesignationRepository.FindByID(id);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<Rly_Designation_FormModel> dTResult = railwaysDesignationRepository.GetRailwaysDesignationList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult Manage(Rly_Designation_FormModel model)
        {
            try
            {
                if (!railwaysDesignationRepository.IsDuplicate(model))
                {
                    if (model.IsNew)
                    {
                        model.Createdby = UserId;
                        model.UserId = USER_ID.Substring(0, 8);
                        railwaysDesignationRepository.SaveDetails(model);
                        AlertAddSuccess("Record Added Successfully.");
                    }
                    else
                    {
                        model.Updatedby = UserId;
                        model.UserId = USER_ID.Substring(0, 8);
                        railwaysDesignationRepository.SaveDetails(model);
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

        public IActionResult Delete(string id)
        {
            try
            {
                string RailwayDesignationCode = railwaysDesignationRepository.IsExistsRailwayDesignationCode(id);
                if (RailwayDesignationCode != "")
                {
                    AlertDanger("You Cannot Delete this Raliway Code, because there is a record present for this Railway Code in " + RailwayDesignationCode + "!!!");
                }
                else
                {
                    if (railwaysDesignationRepository.Remove(id))
                        AlertDeletedSuccess();
                    else
                        AlertDanger();
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RailwaysDesignation", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

    }
}