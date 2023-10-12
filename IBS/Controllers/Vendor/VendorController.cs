using IBS.Filters;
using IBS.Helper;
using IBS.Helpers;
using IBS.Interfaces;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IBS.Controllers.Vendor
{
    public class VendorController : BaseController
    {
        private readonly IVendorRepository vendorRepository;
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;

        public VendorController(IVendorRepository _vendorRepository, IDocument _iDocument, IWebHostEnvironment _env)
        {
            this.vendorRepository = _vendorRepository;
            this.iDocument = _iDocument;
            this.env = _env;
        }

        [Authorization("Vendor", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorization("Vendor", "Index", "view")]
        public IActionResult Manage(int Id)
        {
            VendorMasterModel model = new();
            if (Id > 0)
            {
                model = vendorRepository.FindByID(Id);
            }

            List<IBS_DocumentDTO> lstDocument = iDocument.GetRecordsList((int)Enums.DocumentCategory.Vendor, Convert.ToString(Id));
            FileUploaderDTO FileUploaderCOI = new FileUploaderDTO();
            FileUploaderCOI.Mode = (int)Enums.FileUploaderMode.Add_Edit;
            FileUploaderCOI.IBS_DocumentList = lstDocument.Where(m => m.ID == (int)Enums.DocumentCategory_CANRegisrtation.Document_Vendor_manufacturer_created).ToList();
            FileUploaderCOI.OthersSection = false;
            FileUploaderCOI.MaxUploaderinOthers = 5;
            FileUploaderCOI.FilUploadMode = (int)Enums.FilUploadMode.Single;
            ViewBag.Vendor_Manufacturer = FileUploaderCOI;

            return View(model);

        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorlistModel> dTResult = vendorRepository.GetVendorList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        [Authorization("Vendor", "Index", "edit")]
        public IActionResult Manage(VendorMasterModel model, IFormCollection formCollection)
        {
            try
            {
                if (model.VendCd == 0)
                {
                    model.Createdby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    model.VendCd = vendorRepository.SaveDetails(model);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Updatedby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    vendorRepository.SaveDetails(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }

                if (model.VendCd > 0)
                {
                    if (!string.IsNullOrEmpty(formCollection["hdnUploadedDocumentList"]))
                    {
                        int[] DocumentIds = { (int)Enums.DocumentCategory_CANRegisrtation.Document_Vendor_manufacturer_created };
                        List<APPDocumentDTO> DocumentsList = JsonConvert.DeserializeObject<List<APPDocumentDTO>>(formCollection["hdnUploadedDocumentList"]);
                        DocumentHelper.SaveFiles(Convert.ToString(model.VendCd), DocumentsList, Enums.GetEnumDescription(Enums.FolderPath.Vendor), env, iDocument, "Venor", string.Empty, DocumentIds);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Vendor", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        [Authorization("Vendor", "Index", "delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (vendorRepository.Remove(id))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Vendor", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

    }
}

