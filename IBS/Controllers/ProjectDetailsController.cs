using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class ProjectDetailsController : BaseController
    {
        private readonly IProjectDetailsRepository projectDetailsRepository;
        SessionHelper objSessionHelper = new SessionHelper();

        public ProjectDetailsController(IProjectDetailsRepository _projectDetailsRepository)
        {
            projectDetailsRepository = _projectDetailsRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProjectDetailsSave(ProjectDetails model)
        {
            try
            {
                List<ProjectDetails> lstProjectDetails = new List<ProjectDetails>();
                if (objSessionHelper.lstProjectDetails != null)
                {
                    lstProjectDetails = objSessionHelper.lstProjectDetails;
                }
                //model = projectDetailsRepository.SaveProductDetailsList(model);
                return Json(new { status = true, responseText = "Product Details Added Successfully." });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails ", "SaveDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult SaveDetails(ProjectDetails model)
        {
            try
            {
                List<ProjectDetails> lstProjectDetails = objSessionHelper.lstProjectDetails == null ? new List<ProjectDetails>() : objSessionHelper.lstProjectDetails;
                lstProjectDetails.RemoveAll(x => x.In_ID == Convert.ToInt32(model.In_ID));
                if (model.In_ID > 0)
                {
                    model.In_ID = model.In_ID;
                }
                else
                {
                    model.In_ID = lstProjectDetails.Count > 0 ? (lstProjectDetails.OrderByDescending(a => a.In_ID).FirstOrDefault().In_ID) + 1 : 1;
                }
                lstProjectDetails.Add(model);
                objSessionHelper.lstProjectDetails = lstProjectDetails;
                return Json(new { status = true, responseText = "Product Details Added Successfully." });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails ", "SaveDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            List<ProjectDetails> lstProjectDetails = new List<ProjectDetails>();
            if (objSessionHelper.lstProjectDetails != null)
            {
                lstProjectDetails = objSessionHelper.lstProjectDetails;
            }

            DTResult<ProjectDetails> dTResult = projectDetailsRepository.GetProductDetailsList(dtParameters, lstProjectDetails);
            return Json(dTResult);
        }

        [HttpGet]
        public IActionResult EditProject(string id)
        {
            try
            {
                ProjectDetails Clster = objSessionHelper.lstProjectDetails.Where(x => x.In_ID == Convert.ToInt32(id)).FirstOrDefault();
                return Json(new { status = true, list = Clster });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails", "EditProject", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        [HttpPost]
        public IActionResult DeleteprojectDetail(string id)
        {
            try
            {
                List<ProjectDetails> lstProjectDetails = objSessionHelper.lstProjectDetails == null ? new List<ProjectDetails>() : objSessionHelper.lstProjectDetails;
                lstProjectDetails.RemoveAll(x => x.In_ID == Convert.ToInt32(id));
                objSessionHelper.lstProjectDetails = lstProjectDetails;
                return Json(new { status = true, responseText = "Project Detail Deleted Successfully" });
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "ProjectDetails", "DeleteprojectDetail", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
