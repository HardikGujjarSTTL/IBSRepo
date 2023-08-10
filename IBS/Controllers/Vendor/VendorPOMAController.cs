using IBS.Helper;
using IBS.Interfaces;
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
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;

        public VendorPOMAController(IVendorPOMARepository _vendorRepository, IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            vendorRepository = _vendorRepository;
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
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

        public IActionResult Manage(string id)
        {
            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.VendorMADoc, id);
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_VendorMADoc.VendorMADoc).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.VendorMADoc = FileUploaderCOI;

            return View();
        }
    }
}
