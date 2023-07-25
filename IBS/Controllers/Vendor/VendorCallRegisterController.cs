using IBS.Interfaces;
using IBS.Interfaces.Vendor;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers.Vendor
{
    public class VendorCallRegisterController : BaseController
    {
        #region Variables
        private readonly IVendorCallRegisterRepository venderRepository;
        #endregion
        public VendorCallRegisterController(IVendorCallRegisterRepository _venderRepository)
        {
            venderRepository = _venderRepository;
        }

        public IActionResult VendorCallRegister()
        {
            VenderCallRegisterModel model = new();
            //model = venderRepository.FindByID(UserId);
            model.CDATE = DateTime.Now.ToString("dd-MM-yyyy");
            model.CDAY = DateTime.Now.DayOfWeek.ToString("D");
            model.CTYM = DateTime.Now.ToString("HH24MI");

            if (model.CDAY == "1")
            {
                model.CDATE = DateTime.Now.AddDays(1).ToString("dd-MM-yyyy");
            }
            if (model.CaseNo == null || model.CallRecvDt == null)
            {
                if (model.CDATE == "27-01-2023")
                {
                    model.CallRecvDt = Convert.ToDateTime("27-01-2023");
                }
                else if (model.CDATE == "15-08-2023")
                {
                    model.CallRecvDt = Convert.ToDateTime("16-08-2023");
                }
                else if (model.CDATE == "02-10-2023")
                {
                    model.CallRecvDt = Convert.ToDateTime("03-10-2023");
                }
                else
                {
                    model.CallRecvDt = Convert.ToDateTime(model.CDATE);
                }
            }
            else
            {

            }

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<VenderCallRegisterModel> dTResult = venderRepository.GetUserList(dtParameters, UserName);
            return Json(dTResult);
        }

        public IActionResult VendorCallRegisterDetail(string Action, string CaseNo, string CallRecvDt, int CallSno)
        {
            VenderCallRegisterModel model = new();

            if (CaseNo != null && CallRecvDt != null && CallSno != null)
            {
                model = venderRepository.FindByID(CaseNo, CallRecvDt, CallSno);
            }

            return View(model);
        }
    }
}
