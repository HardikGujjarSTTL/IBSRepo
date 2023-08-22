using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class LabTDSEntryController : BaseController
    {
        #region Variables
        private readonly ILabTDSEntryRepository TDSEntryRepository;
        #endregion
        public LabTDSEntryController(ILabTDSEntryRepository _TDSEntryRepository)
        {
            TDSEntryRepository = _TDSEntryRepository;
        }

        #region Lab TDS Entry
        public IActionResult LabTDSEntry()
        {

            return View();
        }
        [HttpPost]
        public IActionResult SearchRegNo(string RegNo)
        {
            
            LabTDSEntryModel model = new();
            string Region = GetRegionCode;
            model = TDSEntryRepository.SearchRegNo(RegNo, Region);            
            return Json(model);
        }
        [HttpPost]
        public JsonResult SaveTDSEntry(string RegNo, string TDSAmt, string TDSDate)
        {

            LabTDSEntryModel model = new();
            string Region = GetRegionCode;
            bool result;
            result = TDSEntryRepository.SaveTDSEntry(RegNo, TDSAmt, TDSDate);
            return Json(result);
        }

        #endregion


    }
}
