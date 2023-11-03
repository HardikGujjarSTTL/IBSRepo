using IBS.DataAccess;
using IBS.Filters;
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
        [Authorization("LabTDSEntry", "LabTDSEntry", "view")]
        public IActionResult LabTDSEntry()
        {

            return View();
        }
        [HttpPost]
        public IActionResult SearchRegNo(string RegNo)
        {

            LabTDSEntryModel model = new();
            try
            {
                string Region = GetRegionCode;
                model = TDSEntryRepository.SearchRegNo(RegNo, Region);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabTDSEntry", "SearchRegNo", 1, GetIPAddress());
            }
            return Json(model);
        }
        [HttpPost]
        [Authorization("LabTDSEntry", "LabTDSEntry", "edit")]
        public JsonResult SaveTDSEntry(string RegNo, string TDSAmt, string TDSDate)
        {

            LabTDSEntryModel model = new();
            bool result = false;
            try
            {
                string Region = GetRegionCode;
                
                result = TDSEntryRepository.SaveTDSEntry(RegNo, TDSAmt, TDSDate);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabTDSEntry", "SaveTDSEntry", 1, GetIPAddress());
            }
            return Json(result);
        }

        #endregion


    }
}
