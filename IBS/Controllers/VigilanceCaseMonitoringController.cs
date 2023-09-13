using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers
{
    public class VigilanceCaseMonitoringController : BaseController
    {
        private readonly IVigilanceCaseMonitoringRepository vigilanceCaseMonitoringRepository;

        public VigilanceCaseMonitoringController(IVigilanceCaseMonitoringRepository _vigilanceCaseMonitoringRepository)
        {
            vigilanceCaseMonitoringRepository = _vigilanceCaseMonitoringRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            VigilanceCasesMasterModel model = new() { Region = Region };
            if (id > 0)
            {
                model = vigilanceCaseMonitoringRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            dtParameters.AdditionalValues.Add("Region", Region);
            DTResult<VigilanceCasesMasterModel> dTResult = vigilanceCaseMonitoringRepository.GetVigilanceCaseList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult Manage(VigilanceCasesMasterModel model, IFormCollection formCollection)
        {
            try
            {
                ModelState.Remove("RefDt");
                ModelState.Remove("RefReplyDt");
                ModelState.Remove("ActionProposedDt");
                ModelState.Remove("FinalActionDt");

                if (formCollection.Keys.Contains("checkedCaseNos"))
                {
                    model.lstVigilanceCasesList = JsonConvert.DeserializeObject<List<VigilanceCasesListModel>>(formCollection["checkedCaseNos"]);
                }
                model.UserId = USER_ID.Substring(0, 8);

                if (model.Id == 0)
                {
                    int id = vigilanceCaseMonitoringRepository.SaveDetails(model);

                    if (id == -1)
                    {
                        AlertDanger("Registration Details not available!!!");
                    }
                    else
                    {
                        AlertAddSuccess("Record Added Successfully.");
                    }
                }
                else
                {
                    vigilanceCaseMonitoringRepository.SaveDetails(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BankMaster", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTableVigilanceList([FromBody] DTParameters dtParameters)
        {
            DTResult<VigilanceCasesListModel> dTResult = vigilanceCaseMonitoringRepository.GetVigilanceList(dtParameters);
            return Json(dTResult);
        }

    }
}
