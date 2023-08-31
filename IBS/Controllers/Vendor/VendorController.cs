using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Vendor
{
    public class VendorController : BaseController
    {
        private readonly IVendorRepository vendorRepository;
        private readonly IDocument iDocument;

        public VendorController(IVendorRepository _vendorRepository, IDocument _iDocument)
        {
            this.vendorRepository = _vendorRepository;
            this.iDocument = _iDocument;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int Id)
        {
            VendorModel model = new();
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

        public IActionResult Delete(string id)
        {
            try
            {
                string[] data = id.Split('-');
                if (vendorRepository.Remove(Convert.ToInt32(data[0]), data[1]))
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

        [HttpPost]
        public IActionResult Manage(VendorModel model, IFormCollection formCollection)
        {
            try
            {
                //if (model.IsNew)
                //{
                //    if (formCollection.Keys.Contains("_VendorCode"))
                //    {
                //        model.VendorCode = Convert.ToInt32(formCollection["_VendorCode"]);
                //    }

                //    if (!vendorClusterRepository.IsDuplicate(model))
                //    {
                //        model.Createdby = UserId;
                //        model.UserId = USER_ID.Substring(0, 8);
                //        vendorClusterRepository.SaveDetails(model);
                //        AlertAddSuccess("Record Added Successfully.");
                //        return RedirectToAction("Index");
                //    }
                //    else
                //    {
                //        AlertAlreadyExist("Cluster for this vender and department is already entered!!!");
                //    }
                //}
                //else
                //{
                //    model.Updatedby = UserId;
                //    model.UserId = USER_ID.Substring(0, 8);
                //    vendorClusterRepository.SaveDetails(model);
                //    AlertAddSuccess("Record Updated Successfully.");
                //    return RedirectToAction("Index");
                //}
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Vendor", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

    }
}

