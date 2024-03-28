using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class RailwaysDesignationController : BaseController
    {
        private readonly IRailwaysDesignationRepository railwaysDesignationRepository;

        public RailwaysDesignationController(IRailwaysDesignationRepository _railwaysDesignationRepository)
        {
            railwaysDesignationRepository = _railwaysDesignationRepository;
        }

        [Authorization("RailwaysDesignation", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorization("RailwaysDesignation", "Index", "view")]
        public IActionResult Manage(int id)
        {
            Rly_Designation_FormModel model = new();

            if (id > 0)
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
        [Authorization("RailwaysDesignation", "Index", "edit")]
        public IActionResult Manage(Rly_Designation_FormModel model)
        {
            try
            {
                if (!railwaysDesignationRepository.IsDuplicate(model))
                {
                    if (model.ID == 0)
                    {
                        model.Createdby = UserId;
                        model.UserId = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                        railwaysDesignationRepository.SaveDetails(model);
                        AlertAddSuccess("Record Added Successfully.");
                    }
                    else
                    {
                        model.Updatedby = UserId;
                        model.UserId = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                        railwaysDesignationRepository.SaveDetails(model);
                        AlertAddSuccess("Record Updated Successfully.");
                    }

                    return RedirectToAction("Index");
                }
                else
                    AlertAlreadyExist("Designation Code already exists !!");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "RailwaysDirectory", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        [Authorization("RailwaysDesignation", "Index", "delete")]
        public IActionResult Delete(int id)
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