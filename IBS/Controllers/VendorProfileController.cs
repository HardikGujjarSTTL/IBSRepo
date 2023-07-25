using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class VendorProfileController : BaseController
    {
        #region Variables
        private readonly IVendorProfileRepository vendorProfileRepository;
        #endregion
        public VendorProfileController(IVendorProfileRepository _vendorProfileRepository)
        {
            vendorProfileRepository = _vendorProfileRepository;
        }

        public IActionResult Manage()
        {
            int VendCd = Convert.ToInt32(IBS.Helper.SessionHelper.UserModelDTO.UserName);
            VendorModel model = new();
            if (VendCd > 0)
            {
                model = vendorProfileRepository.FindByID(VendCd);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VendorProfileDetailsSave(VendorModel model)
        {
            try
            {
                string msg = "";
                if (model.VendCd > 0)
                {
                    msg = "Vendor Updated Successfully.";
                }
                int i = vendorProfileRepository.VendorDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorProfile", "VendorProfileDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
