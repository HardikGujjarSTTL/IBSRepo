﻿using IBS.DataAccess;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IBS.Controllers
{
    public class BaseController : Controller
    {
        public T02User SetUserInfo
        {
            set
            {
                HttpContext.Session.SetString("UserInfo", JsonSerializer.Serialize(value));
            }
        }

        public T02User GetUserInfo
        {
            get
            {
                string userInfoString = HttpContext.Session.GetString("UserInfo");
                if (!string.IsNullOrWhiteSpace(userInfoString))
                {
                    T02User appUser = JsonSerializer.Deserialize<T02User>(userInfoString);

                    if (appUser != null)
                        return appUser;
                }
                return null;
            }
        }

        public int UserId
        {
            get
            {
                return Convert.ToInt32(GetUserInfo.Id);
            }
        }

        public void AlertAddSuccess(string message = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Record Added Successfully.";
            }
            AddAlert(AlertStyles.Success, message);
        }

        public void AlertUpdateSuccess(string message = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Record Updated Successfully.";
            }
            AddAlert(AlertStyles.Success, message);
        }

        public void AlertDanger(string message = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Looks Like Something Went Wrong. Some Error Occurs...";
            }

            AddAlert(AlertStyles.Danger, message);
        }

        public void AlertDangerfileupload(string message = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Please Upload Valid File.";
            }

            AddAlert(AlertStyles.Danger, message);
        }

        public void AlertAlreadyExist(string message = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Record Already Exists.";
            }

            AddAlert(AlertStyles.Warning, message);
        }

        public void AlertDeletedSuccess(string message = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Record Deleted Successfully.";
            }

            AddAlert(AlertStyles.Success, message);
        }

        private void AddAlert(string alertStyle, string message)
        {
            var alerts = TempData.ContainsKey("TempDataAlerts")
                ? TempData.Get<Alert>("TempDataAlerts")
                : new Alert();

            Alert objAlert = new()
            {
                AlertStyle = alertStyle,
                Message = message
            };

            TempData.Put("TempDataAlerts", objAlert);
        }

        public string GetIPAddress()
        {
            string IPAddress = this.HttpContext.Connection.RemoteIpAddress.ToString();
            return IPAddress;
        }
    }
}
