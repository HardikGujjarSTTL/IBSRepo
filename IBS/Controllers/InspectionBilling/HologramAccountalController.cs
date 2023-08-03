using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.InspectionBilling
{
    public class HologramAccountalController : BaseController
    {

        #region Variables
        private readonly IHologramAccountalRepository hologramaccountRepository;
        #endregion
        public HologramAccountalController(IHologramAccountalRepository _hologramaccountalRepository)
        {
            hologramaccountRepository = _hologramaccountalRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetHologramAccountal(HologramAccountalSearchModel hologramAccountalSeachModel)
        //public IActionResult Index([FromBody] DTParameters dtParameters)//(HologramAccountalSearchModel hologramAccountalSeachModel)
        {
            try
            {
                //DTResult<HologramAccountalModel> dTResult = hologramaccountRepository.GetHologramAcountList(dtParameters);
                return Json(null);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramAccountal", "GetHologramAccountal", 1, GetIPAddress());
            }
            return Json(null);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            var dTResult = new DTResult<HologramAccountalModel>();
            try
            {
                //DTResult<HologramAccountalModel> dTResult = hologramaccountRepository.GetHologramAcountList(dtParameters);
                dTResult = hologramaccountRepository.GetHologramAcountList(dtParameters);                
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "HologramAccountal", "GetHologramAccountal", 1, GetIPAddress());
            }
            //DTResult<RoleModel> dTResult = null;//_hologramaccountalRepository.GetRoleList(dtParameters);
            return Json(dTResult);
        }

    }
}
