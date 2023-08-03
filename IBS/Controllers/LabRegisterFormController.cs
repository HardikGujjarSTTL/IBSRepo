using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IBS.Controllers
{
    public class LabRegisterFormController : BaseController
    {
        #region Variables
        private readonly ILaboratoryMstRepository LaboratoryMstRepository;
        #endregion
        public LabRegisterFormController(ILaboratoryMstRepository _LaboratoryMstRepository)
        {
            LaboratoryMstRepository = _LaboratoryMstRepository;
        }

        #region Lab_Register_Form
        public IActionResult LabRegisterForm()
        {
            return View();
        }
        
        #endregion


    }
}
