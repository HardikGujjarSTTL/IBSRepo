using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    public class RoleController : BaseController
    {
        #region Variables
        private readonly IRoleRepository roleRepository;
        #endregion
        public RoleController(IRoleRepository _roleRepository)
        {
            roleRepository = _roleRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Manage(int id)
        {
            RoleModel model = new();
            if (id > 0)
            {
                model = roleRepository.FindByID(id);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<RoleModel> dTResult = roleRepository.GetRoleList(dtParameters);
            return Json(dTResult);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (roleRepository.Remove(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Role", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RoleDetailsSave(RoleModel model)
        {
            try
            {
                string msg = "Role Inserted Successfully.";

                if (model.RoleId > 0)
                {
                    msg = "Role Updated Successfully.";
                }

                int i = roleRepository.RoleDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg, TenantId = i });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "Role", "RoleDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

    }
}
