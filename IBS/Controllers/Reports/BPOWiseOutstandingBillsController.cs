using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using IBS.Repositories;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using System;
using System.Drawing;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using IBS.Interfaces.Reports;
using System.Data;

namespace IBS.Controllers.Reports
{
    public class BPOWiseOutstandingBillsController : BaseController
    {
        #region Variables
        private readonly IBPOWiseOutstandingBillsRepository BPOWiseOutstandingBillsRepository;
        #endregion
        public BPOWiseOutstandingBillsController(IBPOWiseOutstandingBillsRepository _BPOWiseOutstandingBillsRepository)
        {
            BPOWiseOutstandingBillsRepository = _BPOWiseOutstandingBillsRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BPOWiseOutBills()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetBPO(string BPO)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetBillPayingOfficerUsingSBPO(BPO);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BPOWiseOutstandingBills", "GetBPO", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpGet]
        public IActionResult GetRlyCode(string BpoType)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetBPORLY(BpoType);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BPOWiseOutstandingBills", "GetRlyCode", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpPost]
        public IActionResult GenerateReport([FromBody] BPOWiseOutstandingBillsModel BPOWiseOutstandingBillsModel)
        {
            BPOWiseOutstandingBillsModel.Region = GetRegionCode;
            DataSet dTResult = BPOWiseOutstandingBillsRepository.GenerateReport(BPOWiseOutstandingBillsModel);
            return Json(dTResult);
        }
    }
}
