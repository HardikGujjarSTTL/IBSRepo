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

        public VendorCallsMarkedForSpecificPOController(IVendorCallsMarkedForSpecificPORepository _vendorRepository)
        {
            vendorRepository = _vendorRepository;
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
                List<SelectListItem> agencyClient = Common.GetAgencyClient(RlyNorly);
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
    }
}
