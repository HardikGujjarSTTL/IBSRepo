using DocumentFormat.OpenXml.Office2010.Excel;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ClientRailwayController : BaseController
    {
        #region Variables
        private readonly IClientRailwayRepository railwayRepository;
        #endregion
        public ClientRailwayController(IClientRailwayRepository _railwayRepository)
        {
            railwayRepository = _railwayRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<Railway> dTResult = railwayRepository.GetRailways(dtParameters);
            return Json(dTResult);
        }

        public IActionResult Manage(string id,string Type)
        {
            Railway model = new();
            if (!string.IsNullOrEmpty(id))
            {
                model.Type = Type;
                model = railwayRepository.FindRailwayByID(id);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("ClientRailway", "Index", "edit")]
        public IActionResult RailwaySave(Railway model)
        {
            try
            {
                model.USERID = USER_ID.Length > 8 ? USER_ID.Substring(0, 8) : USER_ID;
                if (model.Type == "U")
                {
                    model.Updatedby = UserId; 
                    int i = railwayRepository.RailwayInsertUpdate(model);
                    if (i == 2)
                    {
                        AlertAddSuccess("Record Updated Successfully.");
                    }
                    //else if(i == 0)
                    //{
                    //    AlertAlreadyExist("This Railway Code Is Already Exist!!!");
                    //    return View("Manage", model);
                    //}
                }
                else 
                {
                    model.Createdby = UserId; 
                    int i = railwayRepository.RailwayInsertUpdate(model);
                    if (i == 1)
                    {
                        AlertAddSuccess("Record Add Successfully.");
                    }
                    else if (i == 0)
                    {
                        AlertAlreadyExist("This Railway Code Is Already Exist!!!");
                        return View("Manage", model);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClientRailway", "RailwaySave", 1, GetIPAddress());
            }
            return View("Manage", model);
        }
    }
}
