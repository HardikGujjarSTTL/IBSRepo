using IBS.Helper;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBS.Helpers
{
    public class RoleWisePermissionHelper
    {
        public static bool ActionIsAccesibleOrNot(string ControllerName = "", string ActionName = "", string PermissionName = "")
        {
            if (SessionHelper.MenuModelDTO != null)
            {
                var validpermission = false;
                switch (PermissionName.ToLower().Trim())
                {
                    case "view":
                        validpermission = SessionHelper.MenuModelDTO.Where(p => p.ControllerName == ControllerName && p.ActionName == ActionName &&
                         p.ViewAccess != false && p.ViewAccess != false).Any();
                        break;
                    case "edit":
                        validpermission = SessionHelper.MenuModelDTO.Where(p => p.ControllerName == ControllerName && p.ActionName == ActionName &&
                         p.EditAccess != false && p.EditAccess != false).Any();
                        break;
                    case "add":
                        validpermission = SessionHelper.MenuModelDTO.Where(p => p.ControllerName == ControllerName && p.ActionName == ActionName &&
                         p.AddAccess != false && p.AddAccess != false).Any();
                        break;
                    case "delete":
                        validpermission = SessionHelper.MenuModelDTO.Where(p => p.ControllerName == ControllerName && p.ActionName == ActionName &&
                         p.DeleteAccess != false && p.DeleteAccess != false).Any();
                        break;
                    default:
                        break;
                }
                return validpermission;
            }
            return false;
        }
    }
}