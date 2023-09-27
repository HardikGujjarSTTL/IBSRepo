using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class TDSEntryController : BaseController
    {
        private readonly ITDSEntryRepository iTDSEntryRepository;

        public TDSEntryController(ITDSEntryRepository _iTDSEntryRepository)
        {
            iTDSEntryRepository = _iTDSEntryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BillDetails(string BillNo)
        {
            TDSEntryModel model = iTDSEntryRepository.GetBillDetails(BillNo, Region);

            if (model == null)
            {
                return Json(new { status = false, responseText = "Record not found for the given Bill No.!!!" });
            }
            return Json(new { status = true, responseText = model });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Manage(TDSEntryModel model)
        {
            try
            {
                model.Createdby = UserId;
                iTDSEntryRepository.SaveDetails(model);
                AlertAddSuccess("Record Added Successfully.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "TDSEntry", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTableTDSHistory([FromBody] DTParameters dtParameters)
        {
            dtParameters.AdditionalValues.Add("Region", Region);
            DTResult<TDSEntryModel> dTResult = iTDSEntryRepository.GetTDSHistroyList(dtParameters);
            return Json(dTResult);
        }
    }
}
