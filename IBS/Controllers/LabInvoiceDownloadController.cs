using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public class LabInvoiceDownloadController : BaseController
    {
        #region Variables
        private readonly ILabInvoiceDownloadRepository LabInvoiceDownloadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public LabInvoiceDownloadController(ILabInvoiceDownloadRepository _LabInvoiceDownloadRepository, IWebHostEnvironment webHostEnvironment)
        {
            LabInvoiceDownloadRepository = _LabInvoiceDownloadRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        

        

        public IActionResult LabInvoiceDownload()
        {

            return View();
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<LabInvoiceDownloadModel> dTResult = LabInvoiceDownloadRepository.GetLapInvoice(dtParameters, Regin);
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult Download(string CaseNo,string RegNo,string InvoiceNo,string TranNo)
        {
            //string Regin = GetRegionCode;
            //DataSet dTResult = LabInvoiceDownloadRepository.Download(CaseNo, RegNo, InvoiceNo, TranNo);
            string Srno = LabInvoiceDownloadRepository.GetSrNo(InvoiceNo);
            LabInvoiceDownloadModel dtreg = LabInvoiceDownloadRepository.Getdtreg(InvoiceNo);
            ReportDocument rd = new ReportDocument();
            string reportPath = "";
            if (Convert.ToInt32(Srno) > 3)
            {
                //reportPath = Server.MapPath("~/Reports/LAB_INVOICE_GEN_NEW.rpt");
                reportPath  = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "LAB_INVOICE_GEN_NEW.rpt");

            }
            else if ((Convert.ToInt32(dtreg.INVOICE_DT) >= 202207) && (dtreg.Resign == "N"))
            {
                // reportPath = Server.MapPath("~/Reports/LAB_INVOICE_GEN_HR.rpt");
                reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "LAB_INVOICE_GEN_HR.rpt");
            }
            else
            {
                //reportPath = Server.MapPath("~/Reports/LAB_INVOICE_GEN.rpt");
                reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "LAB_INVOICE_GEN.rpt");
            }
            rd.Load(reportPath);

            // Replace with your data retrieval logic
            DataSet dsCustom = LabInvoiceDownloadRepository.Getdata( CaseNo,  RegNo,  InvoiceNo,  TranNo);
            rd.SetDataSource(dsCustom);

            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/pdf", "CustomerList.pdf");
        }

    }
}
