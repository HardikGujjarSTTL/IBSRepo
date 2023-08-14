using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class VendorClusterController : BaseController
    {
        #region Variables
        private readonly IVendorCluster vendorCluster;
        #endregion
        public VendorClusterController(IVendorCluster _vendorCluster)
        {
            vendorCluster = _vendorCluster;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            VendorClusterModel model = new();
            if (id > 0)
            {
                model = vendorCluster.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<VendorClusterModel> dTResult = vendorCluster.GetVendorClusterList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (vendorCluster.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorCluster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VendorClusterDetailsSave(VendorClusterModel model)
        {
            try
            {
                string msg = "Vendor Cluster Inserted Successfully.";

                if (model.VendorCode > 0)
                {
                    msg = "Vendor luster Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = vendorCluster.VendorClusterDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorCluster", "VendorClusterDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}

