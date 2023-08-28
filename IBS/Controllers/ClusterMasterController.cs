using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ClusterMasterController : BaseController
    {
        private readonly IClusterMasterRepository clusterMasterRepository;

        public ClusterMasterController(IClusterMasterRepository _clusterMasterRepository)
        {
            clusterMasterRepository = _clusterMasterRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage(int id)
        {
            int ClusterCd = clusterMasterRepository.GetMaxClusterCd();
            ClusterCd += 1;

            ClusterMasterModel model = new() { ClusterCode = ClusterCd, RegionCode = Region };
            if (id > 0)
            {
                model = clusterMasterRepository.FindByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<ClusterMasterModel> dTResult = clusterMasterRepository.GetClusterMasterList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult Manage(ClusterMasterModel model)
        {
            try
            {
                if (model.IsNew)
                {
                    model.Createdby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    clusterMasterRepository.SaveDetails(model);
                    AlertAddSuccess("Record Added Successfully.");
                }
                else
                {
                    model.Updatedby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    clusterMasterRepository.SaveDetails(model);
                    AlertAddSuccess("Record Updated Successfully.");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ClusterMaster", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (clusterMasterRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "clusterMasterRepository", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }
    }
}

