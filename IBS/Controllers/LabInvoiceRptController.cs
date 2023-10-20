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
        #endregion
        public LabInvoiceRptController(ILabInvoiceRptRepository _LabInvoiceRptRepository, IWebHostEnvironment webHostEnvironment)
        {
            LabInvoiceRptRepository = _LabInvoiceRptRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public IActionResult LabInvoiceRpt()
        {
            
            return View();
        }
        public IActionResult ManageLabInvoiceRpt(string RegNo)
        {
            ViewBag.RegNo = RegNo;
            var Region = GetRegionCode;
            var User = UserId.ToString();
            LabInvoiceModel dTResult = LabInvoiceRptRepository.Getdtreg(RegNo, GetRegionCode, User);
            return View(dTResult);
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabInvoiceModel> dTResult = LabInvoiceRptRepository.GetLapInvoice(dtParameters, Regin);
            if(dTResult.data != null && dTResult.data.Any())
            {
                ViewBag.InvoiceNo = dTResult.data.ToList()[0].InvoiceNo.ToString();
                ViewBag.InvoiceDt = dTResult.data.ToList()[0].InvoiceDt.ToString();

            }
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult Save(LabInvoiceModel model)
        {
            model.Region = GetRegionCode;
            model.UserId = UserId.ToString();
            try
            {
                string msg = "Generate Lab Invoice Successfully.";

                //model.UserId = Convert.ToString(UserId);
                string i = LabInvoiceRptRepository.Save(model);
                if (i != "0")
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LabInvoiceRptRepository", "ManageLabInvoiceRpt", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
