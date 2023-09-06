using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Signing;
using System;
using System.Data;
using System.Linq.Expressions;
using static IBS.Helper.Enums;

namespace IBS.Filters
{
    public class Authorization : ActionFilterAttribute
    {
        private readonly string ControllerName;
        private readonly string ActionName;
        private readonly string PermissionName;

        public Authorization(string controller = "", string action = "",string permission = "")
        {
            ControllerName = controller;
            ActionName = action;
            PermissionName = permission;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MenuMasterModel currentpermi = new MenuMasterModel();

            var user = SessionHelper.UserModelDTO ?? null;
            var userPermissionList = SessionHelper.MenuModelDTO ?? null;
            if (user == null || userPermissionList == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }
            else if (user != null && userPermissionList != null && !string.IsNullOrEmpty(ControllerName) && !string.IsNullOrEmpty(ActionName) && !string.IsNullOrEmpty(PermissionName))
            {

                currentpermi = SessionHelper.MenuModelDTO.Where(p => p.ControllerName == ControllerName && p.ActionName.Contains(ActionName)).FirstOrDefault();

                SessionHelper.CurrentAccess = currentpermi;

                var validpermission = false;
                switch (PermissionName.ToLower().Trim())
                {
                    case "view":
                        validpermission = SessionHelper.MenuModelDTO.Where(p => p.ControllerName == ControllerName && p.ActionName.Contains(ActionName) &&
                         p.ViewAccess != false && p.ViewAccess != false).Any();
                        break;
                    case "edit":
                        validpermission = SessionHelper.MenuModelDTO.Where(p => p.ControllerName == ControllerName && p.ActionName.Contains(ActionName) &&
                         p.EditAccess != false && p.EditAccess != false).Any();
                        break;
                    case "add":
                        validpermission = SessionHelper.MenuModelDTO.Where(p => p.ControllerName == ControllerName && p.ActionName.Contains(ActionName) &&
                         p.AddAccess != false && p.AddAccess != false).Any();
                        break;
                    case "delete":
                        validpermission = SessionHelper.MenuModelDTO.Where(p => p.ControllerName == ControllerName && p.ActionName.Contains(ActionName) &&
                         p.DeleteAccess != false && p.DeleteAccess != false).Any();
                        break;
                    default:
                        break;
                }
                if (!validpermission)
                {
                    if (!filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Home",
                            action = "UserAccessDenied",
                            error = "You don't have permission to do this action"
                        }));
                    }
                    else if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        var result = new JsonResult(new { status = false, responseText = "You don't have permission to do this action." });
                        filterContext.Result = result;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }


}
