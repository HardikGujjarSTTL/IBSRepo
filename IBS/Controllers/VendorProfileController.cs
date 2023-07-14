using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class VendorProfileController : Controller
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
    }
}
