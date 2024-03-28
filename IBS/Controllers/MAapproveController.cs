using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class MAapproveController : BaseController
    {
        #region Variables
        private readonly IMAapproveRepository maapproveRepository;
        #endregion

        public MAapproveController(IMAapproveRepository _maapproveRepository)
        {
            maapproveRepository = _maapproveRepository;
        }

        [Authorization("MAapprove", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<MAapproveModel> dTResult = maapproveRepository.GetDataList(dtParameters, GetRegionCode);
            return Json(dTResult);
        }

        [Authorization("MAapprove", "Index", "view")]
        public IActionResult Manage(string CaseNo, string MaNo, string MaDtc, byte MaSno)
        {
            MAapproveModel model = new();
            MaNo = Common.DecryptQueryString(MaNo);
            if (CaseNo != null)
            {
                model = maapproveRepository.FindByID(CaseNo, MaNo, MaDtc, MaSno);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("MAapprove", "Index", "edit")]
        public IActionResult DetailsSave(MAapproveModel model)
        {
            try
            {
                string msg = "Inserted Successfully.";

                if (model.CaseNo != null && model.MaNo1 != null && model.MaDt != null && model.MaSno > 0)
                {
                    msg = "Updated Successfully.";
                    model.ApprovedBy = Convert.ToString(UserId);
                }

                int i = maapproveRepository.DetailsUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MA APPROVE FORM ", "DetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
    }
}
