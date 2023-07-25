using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace IBS.Controllers
{
    public class Allow_Old_Bill_DateController : BaseController
    {
        #region Variables
        private readonly IAllow_Old_Bill_DateRepository allowoldbilldateRepository;
        #endregion
        public Allow_Old_Bill_DateController(IAllow_Old_Bill_DateRepository _allowoldbilldateRepository)
        {
            allowoldbilldateRepository = _allowoldbilldateRepository;
        }

        public IActionResult Allow_Old_Bill_DateMaster()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<Allow_Old_Bill_DateModel> dTResult = allowoldbilldateRepository.GetMessageList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsSave(Allow_Old_Bill_DateModel model)
        {
            try
            {
                string msg = "Message Inserted Successfully.";

                if (model.Region != null)
                {
                    msg = "Message Updated Successfully.";
                    model.Updatedby = Convert.ToString(UserId);
                }
                //model.Createdby = Convert.ToString(UserId);
                int i = allowoldbilldateRepository.DetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, Id = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "User", "UserDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult Allow_Old_Bill_DateManage(string region)
        {
            Allow_Old_Bill_DateModel model = new();

            if (region != null)
            {
                model = allowoldbilldateRepository.FindByID(region);
            }

            return View(model);
        }
    }
}
