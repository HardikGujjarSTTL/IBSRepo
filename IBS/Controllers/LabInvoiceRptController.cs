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
            LabInvoiceModel dTResult = LabInvoiceRptRepository.Getdtreg(RegNo);
            return View(dTResult);
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabInvoiceModel> dTResult = LabInvoiceRptRepository.GetLapInvoice(dtParameters, Regin);
           ViewBag.InvoiceNo = dTResult.data.ToList()[0].InvoiceNo.ToString();
            ViewBag.InvoiceDt = dTResult.data.ToList()[0].InvoiceDt.ToString();
            return Json(dTResult);
        }
        
    }
}
