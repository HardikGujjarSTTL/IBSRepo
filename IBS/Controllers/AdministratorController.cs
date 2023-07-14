using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IBS.Controllers
{
    public class AdministratorController : BaseController
    {
        #region Variables
        private readonly IUserRepository userRepository;
        #endregion
        public AdministratorController(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        #region User Master
        public IActionResult UserMaster()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<UserModel> dTResult = userRepository.GetUserList(dtParameters);
            return Json(dTResult);
        }

        #endregion

        public IActionResult UserManage(string UserId)
        {
            UserModel model = new();

            if (UserId != null)
            {
                model = userRepository.FindByID(UserId);
            }

            return View(model);
        }

        public IActionResult Delete(string UserId)
        {
            try
            {
                if (userRepository.Remove(UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Administrator", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("UserMaster");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserDetailsSave(UserModel model)
        {
            try
            {
                string msg = "User Inserted Successfully.";

                if (model.UserId != null)
                {
                    msg = "User Updated Successfully.";
                }

                int i = userRepository.UserDetailsInsertUpdate(model);
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

    }
}
