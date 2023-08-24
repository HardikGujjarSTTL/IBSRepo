using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IBS.Helpers;

namespace IBS.Controllers
{
    [Authorization]
    public class RoleController : BaseController
    {
        #region Variables
        private readonly IRoleRepository roleRepository;
        #endregion
        public RoleController(IRoleRepository _roleRepository)
        {
            roleRepository = _roleRepository;
        }

        [Authorization("Role", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("Role", "Index", "view")]
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
        [Authorization("Role", "Index", "delete")]
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
        //[Authorization("Role", "Index", "add")]
        public IActionResult RoleDetailsSave(RoleModel model)
        {
            try
            {
                string msg = "Role Inserted Successfully.";

                if (model.RoleId > 0)
                {
                    msg = "Role Updated Successfully.";
                    model.Updatedby = UserId;
                    if (!RoleWisePermissionHelper.ActionIsAccesibleOrNot("Role", "Index", "edit"))
                    {
                        return Json(new { status = false, responseText = Common.AccessDeniedMessage });
                    }
                }
                else
                {
                    if (!RoleWisePermissionHelper.ActionIsAccesibleOrNot("Role", "Index", "add"))
                    {
                        return Json(new { status = false, responseText = Common.AccessDeniedMessage });
                    }
                }
                model.Createdby =UserId;
                int i = roleRepository.RoleDetailsInsertUpdate(model);
                if (i > 0)
                {
                    return Json(new { status = true, responseText = msg });
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
