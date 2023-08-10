using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Repositories.Reports;
using IBS.Repositories.Vendor;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace IBS.Controllers.Vendor
{
    public class VendorPOMAController : BaseController
    {
        private readonly IVendorPOMARepository vendorRepository;

        public VendorPOMAController(IVendorPOMARepository _vendorRepository)
        {
            vendorRepository = _vendorRepository;
        }
        public IActionResult Index(string CaseNo, string MaNo, string MaDt, string MaStatus, string MaSNo)
        {
            VendorPOMAModel model = new();
            if (CaseNo != null)
            {
                model = vendorRepository.FindByID(CaseNo, MaNo, MaDt, MaStatus, MaSNo, UserName);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorPOMAModel> dTResult = vendorRepository.GetDataList(dtParameters, UserName);
            return Json(dTResult);
        }

        public IActionResult GetMatch(string CaseNo)
        {
            DTResult<VendorPOMAModel> dTResult = vendorRepository.FindMatchDetail(CaseNo,UserName);
            return Json(dTResult);
        }

        public IActionResult Manage()
        {
            return View();
        }
    }
}
