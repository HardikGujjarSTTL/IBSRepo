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

namespace IBS.Controllers
{
    [Authorization]
    public class BillingController : BaseController
    {
        #region Variables
        private readonly IReceiptsRemitanceRepository ReceiptsRemitanceRepository;
        private readonly IWebHostEnvironment env;
        #endregion
        public BillingController(IReceiptsRemitanceRepository _ReceiptsRemitanceRepository, IWebHostEnvironment _env)
        {
            ReceiptsRemitanceRepository = _ReceiptsRemitanceRepository;
            this.env = _env;
        }

        public IActionResult Index()
        {
            ViewBag.Region = Region;
            return View();
        }
        [HttpGet]
        public IActionResult GetBPO(string BpoType)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetBPO(BpoType);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Billing", "GetBPO", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpGet]
        public IActionResult GetBPOCode(string BpoType)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetBPORLY(BpoType);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Billing", "GetBPORLY", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
