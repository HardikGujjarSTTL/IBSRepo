using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IBS.Controllers
{
    public class LabTDSEntryController : BaseController
    {
        #region Variables
        private readonly ILaboratoryMstRepository LaboratoryMstRepository;
        #endregion
        public LabTDSEntryController(ILaboratoryMstRepository _LaboratoryMstRepository)
        {
            LaboratoryMstRepository = _LaboratoryMstRepository;
        }

        #region Lab TDS Entry
        public IActionResult LabTDSEntry()
        {
            return View();
        }
        
        #endregion


    }
}
