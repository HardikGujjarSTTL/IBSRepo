using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ClusterMasterController : BaseController
    {
        #region Variables
        private readonly IClusterMaster clusterMaster;
        #endregion
        public ClusterMasterController(IClusterMaster _clusterMaster)
        {
            clusterMaster = _clusterMaster;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            ClusterMasterModel model = new();
            if (id > 0)
            {
                model = clusterMaster.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ClusterMasterModel> dTResult = clusterMaster.GetClusterMasterList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (clusterMaster.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClusterMaster", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClusterMasterDetailsSave(ClusterMasterModel model)
        {
            try
            {
                string msg = "Cluster Master Inserted Successfully.";

                if (model.ClusterCode > 0)
                {
                    msg = "Cluster Master Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int i = clusterMaster.ClusterMasterDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClusterMaster", "ClusterMasterDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}

