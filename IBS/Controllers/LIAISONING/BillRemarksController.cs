//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class BillRemarksController : BaseController
    {
        #region Variables
        private readonly IBillRemarksRepository BillRemarksRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public BillRemarksController(IBillRemarksRepository _BillRemarksRepository, IWebHostEnvironment webHostEnvironment)
        {
            BillRemarksRepository = _BillRemarksRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        

        public IActionResult BillRemarks()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string OrgType = OrgnType;
            string Org = Organisation;
            DTResult<BillRemarksModel> dTResult = BillRemarksRepository.GetBills(dtParameters, OrgType, Org, _webHostEnvironment);
            return Json(dTResult);
        }
        [HttpPost]
        [Authorization("BillRemarks", "BillRemarks", "edit")]
        public JsonResult SaveData()
        {
            BillRemarksModel BillRemarksModel = new BillRemarksModel();

            BillRemarksModel.BILL_NO = Request.Form["BILL_NO"];
            BillRemarksModel.LO_REMARKS = Request.Form["LO_REMARKS"];

            bool result;
            result = BillRemarksRepository.SaveData(BillRemarksModel);
            if (result == false)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }

        }

    }
}
