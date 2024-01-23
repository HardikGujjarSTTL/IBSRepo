using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class LabInvoiceController : BaseController
    {
        private readonly ILabInvoiceRepository labInvoiceRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration config;

        public LabInvoiceController(ILabInvoiceRepository _labInvoiceRepository, IWebHostEnvironment env, IConfiguration _config)
        {
            labInvoiceRepository = _labInvoiceRepository;
            this.env = env;
            config = _config;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<LabInvoiceReportModel> dTResult = labInvoiceRepository.GetLabInvoice(dtParameters);
            //GlobalDeclaration.AllGeneratedBillModel = dTResult.data.ToList();
            return Json(dTResult);
        }
    }
}
