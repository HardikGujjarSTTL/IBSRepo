//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Drawing;
using System.Text.Json;
using System.Xml.Linq;

namespace IBS.Controllers
{
    public class SuperSurpirseFormController : BaseController
    {
        #region Variables
        private readonly ISuperSurpirseFormRepository SuperSurpirseFormRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion
        public SuperSurpirseFormController(ISuperSurpirseFormRepository _SuperSurpirseFormRepository, IWebHostEnvironment webHostEnvironment)
        {
            SuperSurpirseFormRepository = _SuperSurpirseFormRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorization("SuperSurpirseForm", "Index", "view")]
        public IActionResult Index()
        {

            return View();
        }
        [Authorization("SuperSurpirseForm", "Index", "view")]
        public IActionResult SuperSurpirseManage(string CaseNo,string CallDt,string CallSNo)
        {
            SuperSurpirseFormModel SuperSurpirseFormModel = new SuperSurpirseFormModel();
            SuperSurpirseFormModel.Regin = GetRegionCode;
            SuperSurpirseFormModel = SuperSurpirseFormRepository.LoadSuperData(SuperSurpirseFormModel, CaseNo, CallDt, CallSNo);
            SuperSurpirseFormModel.Regin = GetRegionCode;
            return View(SuperSurpirseFormModel);
        }
        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            string Regin = GetRegionCode;
            DTResult<SuperSurpirseFormModel> dTResult = SuperSurpirseFormRepository.GetSuperFormData(dtParameters, Regin);
            return Json(dTResult);
        }       
        [HttpPost]
        [Authorization("SuperSurpirseForm", "Index", "edit")]
        public IActionResult Save(SuperSurpirseFormModel SuperSurpirseFormModel)
        {
            SuperSurpirseFormModel.Regin = GetRegionCode;
            SuperSurpirseFormModel.UserName = Convert.ToString(UserId);
            //bool dTResult = ETrainingDetailsRepository.Save(iETrainingDetailsModel);
            //if(dTResult == true)
            //{
            //    AlertAddSuccess();
            //}
            //return RedirectToAction("IETrainingDetails","Index");
            try
            {
                string msg = "Data Inserted Successfully.";

                string msg1 = "Registration Details not available";
                bool dTResult = SuperSurpirseFormRepository.Save(SuperSurpirseFormModel);
                if (dTResult == true)
                {
                    return Json(new { status = true, responseText = msg, Id = dTResult });
                }
                else
                {
                    return Json(new { status = false, responseText = msg1, Id = dTResult });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "SuperSurpirseManage", "SuperSurpirseForm", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
