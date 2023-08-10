using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Repositories.Vendor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers.Vendor
{
    public class VendorCallsMarkedForSpecificPOController : BaseController
    {
        private readonly IVendorCallsMarkedForSpecificPORepository vendorRepository;
        private readonly IWebHostEnvironment env;

        public VendorCallsMarkedForSpecificPOController(IVendorCallsMarkedForSpecificPORepository _vendorRepository, IWebHostEnvironment _environment)
        {
            vendorRepository = _vendorRepository;
            env = _environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetRlyCd(string RlyNorly)
        {
            try
            {
                List<SelectListItem> agencyClient = Common.GetRlyCd(RlyNorly);
                return Json(new { status = true, list = agencyClient });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorCallsMarkedForSpecificPO", "GetRlyCd", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorCallsMarkedForSpecificPOModel> dTResult = vendorRepository.GetDataList(dtParameters, UserName);
            return Json(dTResult);
        }

        public IActionResult ManageByReport(string L5NoPo, string PoDt, string RlyNorly, string RlyCd, string Action)
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTableReportCall([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorCallsForSpecificPOReportModel> dTResult = vendorRepository.GetDataReportCallList(dtParameters, UserName);
            return Json(dTResult);
        }

        public IActionResult ManageByReportCall(string L5NoPo, string PoDt, string RlyNorly, string RlyCd, string Action)
        {
            return View();
        }

        public IActionResult ManageByReportIC(string L5NoPo, string PoDt, string RlyNorly, string RlyCd, string Action)
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTableReportIC([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorCallsMarkedForSpecificICModel> dTResult = vendorRepository.GetDataReportICList(dtParameters, UserName);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadTableReportICSub([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorCallsMarkedForSpecificICSubModel> dTResult = vendorRepository.GetDataReportICSubList(dtParameters, UserName);
            return Json(dTResult);
        }
    }
}
