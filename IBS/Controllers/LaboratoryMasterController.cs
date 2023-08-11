using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IBS.Controllers
{
    public class LaboratoryMasterController : BaseController
    {
        #region Variables
        private readonly ILaboratoryMstRepository LaboratoryMstRepository;
        #endregion
        public LaboratoryMasterController(ILaboratoryMstRepository _LaboratoryMstRepository)
        {
            LaboratoryMstRepository = _LaboratoryMstRepository;
        }

        #region Lab Master
        public IActionResult LaboratoryMaster()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<LaboratoryMstModel> dTResult = LaboratoryMstRepository.GetLaboratoryMstList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult LaboratoryManage(int LabID)
        {
            LaboratoryMstModel model = new();

            if (LabID >0)
            {
                model = LaboratoryMstRepository.FindByID(LabID);
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LabDetailsSave(LaboratoryMstModel model)
        {
            try
            {
                string msg = "Laboratory Inserted Successfully.";
                
                if (model.LabId > 0)
                {
                    msg = "Laboratory Updated Successfully.";                   
                }
                model.UserId = Convert.ToString(UserId);
                int i = LaboratoryMstRepository.LabDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "LaboratoryMaster", "LabDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion


    }
}
