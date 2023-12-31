﻿using IBS.DataAccess;
using IBS.Filters;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IBS.Controllers
{
    [Authorization]
    public class UserRoleController : BaseController
    {
        #region Variables
        private readonly IRoleRepository roleRepository;
        #endregion
        public UserRoleController(IRoleRepository _roleRepository)
        {
            roleRepository = _roleRepository;
        }
        [Authorization("UserRole", "Index", "view")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorization("UserRole", "Index", "view")]
        public IActionResult Manage(int id)
        {
            RoleModel model = new();
            if (id > 0)
            {
                model = roleRepository.FindUserRoleByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<RoleModel> dTResult = roleRepository.GetUserRoleList(dtParameters);
            return Json(dTResult);
        }
        [Authorization("UserRole", "Index", "delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (roleRepository.RemoveUserRole(id, UserId))
                    AlertDeletedSuccess();
                else
                    AlertDanger();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "UserRole", "Delete", 1, GetIPAddress());
                AlertDanger();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization("UserRole", "Index", "edit")]
        public IActionResult UserRoleSave(RoleModel model)
        {
            try
            {
                string msg = "User Role Inserted Successfully.";

                if (model.Id > 0)
                {
                    msg = "User Role Updated Successfully.";
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                int id = roleRepository.UserRoleInsertUpdate(model);
                if (id > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "UserRole", "RoleDetailsSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public ActionResult SearchUsers(string searchTerm, string userType)
        {
            var filteredUsers = Common.GetUsersgetbyName(searchTerm, userType);

            var result = filteredUsers.Select(u => new
            {
                id = u.Value,
                text = u.Text
            });
            return Json(new { data = result });
        }
    }
}
