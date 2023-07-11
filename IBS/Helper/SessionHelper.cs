using IBS.Models;
using System.Text.Json;

namespace IBS.Helper
{
    public class SessionHelper
    {
        private static IHttpContextAccessor httpContextAccessor;

        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }

        public static T02User UserModelDTO
        {
            get
            {
                string userInfoString = httpContextAccessor.HttpContext.Session.GetString("UserInfo");
                if (!string.IsNullOrWhiteSpace(userInfoString))
                {
                    T02User appUser = JsonSerializer.Deserialize<T02User>(userInfoString);

                    if (appUser != null)
                        return appUser;
                }
                return null;
            }
            set
            {
                httpContextAccessor.HttpContext.Session.SetString("UserInfo", JsonSerializer.Serialize(value));
            }
        }
    }
}
