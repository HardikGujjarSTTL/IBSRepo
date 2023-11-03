//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing;
using System.Text.Json;

namespace IBS.Controllers
{
    public class LabInvoiceRptController : BaseController
    {
        #region Variables
        private readonly ILabInvoiceRptRepository LabInvoiceRptRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration config;
        #endregion
        public LabInvoiceRptController(ILabInvoiceRptRepository _LabInvoiceRptRepository, IWebHostEnvironment webHostEnvironment, IConfiguration _config)
        {
            LabInvoiceRptRepository = _LabInvoiceRptRepository;
            _webHostEnvironment = webHostEnvironment;
            config = _config;
        }

        public IActionResult LabInvoiceRpt()
        {
            return View();
        }
        public IActionResult ManageLabInvoiceRpt(string RegNo)
        {
            LabInvoiceModel dTResult = new LabInvoiceModel();
            try
            {
                ViewBag.ReportUrl = config.GetSection("AppSettings")["ReportUrl"];
                ViewBag.RegNo = RegNo;
                var Region = GetRegionCode;
                var User = UserId.ToString();
                dTResult = LabInvoiceRptRepository.Getdtreg(RegNo, GetRegionCode, User);
                
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabInvoiceRpt", "ManageLabInvoiceRpt", 1, GetIPAddress());
                AlertDanger(ex.Message);
                //return RedirectToAction("LabInvoiceRpt");
            }
            return View(dTResult);
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<LabInvoiceModel> dTResult = new DTResult<LabInvoiceModel>();
            try
            {
                string Regin = GetRegionCode;
                dTResult = LabInvoiceRptRepository.GetLapInvoice(dtParameters, Regin);
                if (dTResult.data != null && dTResult.data.Any())
                {
                    ViewBag.InvoiceNo = dTResult.data.ToList()[0].InvoiceNo.ToString();
                    ViewBag.InvoiceDt = dTResult.data.ToList()[0].InvoiceDt.ToString();

                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabInvoiceRpt", "LoadTable", 1, GetIPAddress());
                AlertDanger(ex.Message);
                //return RedirectToAction("LabInvoiceRpt");
            }

            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult Save(LabInvoiceModel model)
        {
            model.Region = GetRegionCode;
            model.UserId = UserId.ToString();
            string Msg1 = "Add Successfully.";
            try
            {
                string msg = "Generate Lab Invoice Successfully.";

                //model.UserId = Convert.ToString(UserId);
                string i = LabInvoiceRptRepository.Save(model);
                if (i != "0")
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
                else
                {
                    return Json(new { status = true, responseText = Msg1, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabInvoiceRpt", "Save", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
