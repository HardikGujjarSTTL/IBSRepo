//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Controllers
{
    public class IETrainingDetailsController : BaseController
    {
        #region Variables
        private readonly IETrainingDetailsRepository ETrainingDetailsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public IETrainingDetailsController(IETrainingDetailsRepository _ETrainingDetailsRepository, IWebHostEnvironment webHostEnvironment)
        {
            ETrainingDetailsRepository = _ETrainingDetailsRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorization("IETrainingDetails", "Index", "view")]
        public IActionResult Index()
        {

            return View();
        }
        [Authorization("IETrainingDetails", "Index", "view")]
        public IActionResult IETrainingDetails(IETrainingDetailsModel iETrainingDetailsModel)
        {
            //IETrainingDetailsModel iETrainingDetailsModel = new IETrainingDetailsModel();
            iETrainingDetailsModel.Regin = GetRegionCode;
            iETrainingDetailsModel.Outside = true;
            return View(iETrainingDetailsModel);
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<IETrainingDetailsModel> dTResult = ETrainingDetailsRepository.GetBills(dtParameters, Regin);
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult IEFetchData(string Name)
        {
            IETrainingDetailsModel dTResult = ETrainingDetailsRepository.IEFetchData(Name);
            return Json(dTResult);
        }
        [HttpPost]
        public IActionResult BindCourseName(string TrainingType, string TrainingArea)
        {
            string Regin = GetRegionCode;
            List<SelectListItem> CourseName = Common.CourseName(Regin, TrainingType, TrainingArea);
            return Json(new { status = true, list = CourseName });
        }
        [HttpPost]
        public IActionResult TrainingDFetchData(string Course)
        {
            IETrainingDetailsModel dTResult = ETrainingDetailsRepository.TrainingDFetchData(Course);
            return Json(dTResult);
        }
        [HttpPost]
        [Authorization("IETrainingDetails", "Index", "edit")]
        public IActionResult Save(IETrainingDetailsModel iETrainingDetailsModel)
        {
            iETrainingDetailsModel.Regin = GetRegionCode;
            //bool dTResult = ETrainingDetailsRepository.Save(iETrainingDetailsModel);
            //if(dTResult == true)
            //{
            //    AlertAddSuccess();
            //}
            //return RedirectToAction("IETrainingDetails","Index");
            try
            {
                string msg = "Data Inserted Successfully.";


                bool dTResult = ETrainingDetailsRepository.Save(iETrainingDetailsModel);
                if (dTResult == true)
                {
                    return Json(new { status = true, responseText = msg, Id = dTResult });
                }
                else
                {
                    return Json(new { status = false, responseText = iETrainingDetailsModel.MSG, Id = dTResult });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "IETrainingDetails", "IETrainingDetails", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
