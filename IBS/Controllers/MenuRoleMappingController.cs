using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;

namespace IBS.Controllers
{
    public class MenuRoleMappingController : BaseController
    {
        #region Variables
        private readonly IRoleRepository roleRepository;
        #endregion
        public MenuRoleMappingController(IRoleRepository _roleRepository)
        {
            roleRepository = _roleRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Manage(int id)
        {
            MenuroleMappingModel model = new();
            if (id > 0)
            {
                model = roleRepository.FindMenuRoleMappingByID(id);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LoadTable([FromBody] DTParameters dtParameters)
        {
            DTResult<MenuroleMappingListModel> dTResult = roleRepository.GetMenuRoleMappingList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        public IActionResult LoadMenuTable([FromBody] DTParameters dtParameters)
        {
            DTResult<MenuListModel> dTResult = roleRepository.GetMenuList(dtParameters);
            return Json(dTResult);
        }

        [HttpPost]
        //public IActionResult MenuRoleMappingSave(string IDs, string Id, string RoleId)
        public IActionResult MenuRoleMappingSave(string Id, string RoleId, string dataModel)
        {
            try
            {
                List<MenuroleMappingAjaxData> selectedItems = JsonConvert.DeserializeObject<List<MenuroleMappingAjaxData>>(dataModel);
                MenuroleMappingModel model = new MenuroleMappingModel();
                int Rid = 0;
                string msg = "Menu Role Mapping Inserted Successfully.";
                if (Convert.ToInt32(Id) > 0)
                {
                    roleRepository.RemoveenuRoleMapping(Convert.ToInt32(RoleId), UserId);
                    model.Updatedby = UserId;
                }
                model.Createdby = UserId;
                model.Roleid = Convert.ToInt32(RoleId);
                foreach (var item in selectedItems)
                {
                    model.Menuid =Convert.ToInt32(item.ID);
                    model.detail = item.detail;
                    model.Isadd = item.Isadd;
                    model.Isedit = item.Isedit;
                    model.PIsdelete = item.Pisdelete;
                    model.Isview = item.Isview;
                    model.Isactive = true;
                    Rid = roleRepository.MenuRoleMappingInsertUpdate(model);
                }
                if (Rid > 0)
                {
                    return Json(new { status = true, responseText = msg });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "MenuRoleMapping", "MenuRoleMappingSave", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        public IActionResult Delete(int id)
        {
            try
            {
                if (roleRepository.RemoveenuRoleMapping(id, UserId))
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
    }
}
