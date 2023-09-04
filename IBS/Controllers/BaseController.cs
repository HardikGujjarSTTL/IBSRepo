using IBS.DataAccess;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IBS.Controllers
{
    public class BaseController : Controller
    {
        public UserSessionModel SetUserInfo
        {
            set
            {
                HttpContext.Session.SetString("UserInfo", JsonSerializer.Serialize(value));
            }
        }

        public UserSessionModel GetUserInfo
        {
            get
            {
                string userInfoString = HttpContext.Session.GetString("UserInfo");
                if (!string.IsNullOrWhiteSpace(userInfoString))
                {
                    UserSessionModel appUser = JsonSerializer.Deserialize<UserSessionModel>(userInfoString);

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
                return Convert.ToInt32(GetUserInfo.UserID);
            }
        }

        public string GetRegionCode
        {
            get
            {
                return (GetUserInfo.Region);
            }
        }

        public int GetIeCd
        {
            get
            {
                return Convert.ToInt32(GetUserInfo.IeCd);
            }
        }
        public string GetAuthType
        {
            get
            {
                return GetUserInfo.AuthLevl;
            }
        }

        public string UserName
        {
            get
            {
                return (GetUserInfo.UserName);
            }
        }

        public string Region
        {
            get
            {
                return (GetUserInfo.Region);
            }
        }

        public string USER_ID
        {
            get
            {
                return (GetUserInfo.USER_ID);
            }
        }
        public string OrgnType
        {
            get
            {
                return (GetUserInfo.OrgnTypeL);
            }
        }
        public string Organisation
        {
            get
            {
                return (GetUserInfo.OrganisationL);
            }
        }
        public string OrgnTypeClient
        {
            get
            {
                return (GetUserInfo.OrgnType);
            }
        }
        public string OrganisationClient
        {
            get
            {
                return (GetUserInfo.Organisation);
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
