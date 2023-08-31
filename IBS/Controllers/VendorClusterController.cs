using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class VendorClusterController : BaseController
    {
        private readonly IVendorClusterRepository vendorClusterRepository;

        public VendorClusterController(IVendorClusterRepository _vendorClusterRepository)
        {
            vendorClusterRepository = _vendorClusterRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage()
        {
            return View(new VendorClusterModel());
        }

        public IActionResult ManageVendorCluster(int VendorCode, string DepartmentCode)
        {
            VendorClusterModel model = new();
            if (VendorCode > 0 && !string.IsNullOrEmpty(DepartmentCode))
            {
                model = vendorClusterRepository.FindByID(VendorCode, DepartmentCode);
            }
            return View("Manage", model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            dtParameters.AdditionalValues.Add("Region", Region);
            DTResult<VendorClusterModel> dTResult = vendorClusterRepository.GetVendorClusterList(dtParameters);
            return Json(dTResult);
        }

        public IActionResult GetVendorDetails(string VendorCodeName)
        {
            try
            {
                var VendorDetails = vendorClusterRepository.GetVendorDetails(VendorCodeName);
                return Json(new { status = true, VendorDetails });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorCluster", "GetVendorDetails", 1, GetIPAddress());
            }

            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult GetClustersName(string RegionCode, string DepartmentName)
        {
            return Json(Common.GetClustersName(RegionCode, DepartmentName).ToList());
        }

        public IActionResult Delete(string id)
        {
            try
            {
                string[] data = id.Split('-');
                if (vendorClusterRepository.Remove(Convert.ToInt32(data[0]), data[1]))
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
        public IActionResult Manage(VendorClusterModel model, IFormCollection formCollection)
        {
            try
            {
                if (model.IsNew)
                {
                    if (formCollection.Keys.Contains("_VendorCode"))
                    {
                        model.VendorCode = Convert.ToInt32(formCollection["_VendorCode"]);
                    }

                    if (!vendorClusterRepository.IsDuplicate(model))
                    {
                        model.Createdby = UserId;
                        model.UserId = USER_ID.Substring(0, 8);
                        vendorClusterRepository.SaveDetails(model);
                        AlertAddSuccess("Record Added Successfully.");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AlertAlreadyExist("Cluster for this vender and department is already entered!!!");
                    }
                }
                else
                {
                    model.Updatedby = UserId;
                    model.UserId = USER_ID.Substring(0, 8);
                    vendorClusterRepository.SaveDetails(model);
                    AlertAddSuccess("Record Updated Successfully.");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "VendorCluster", "Manage", 1, GetIPAddress());
            }
            return View(model);
        }

    }
}

