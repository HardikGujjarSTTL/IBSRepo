namespace IBSAPI.Models
{
    public class UserModel
    {
        public string userName { get; set; }
        public string userImage { get; set; }
        public string userType { get; set; }
        public string token { get; set; }
        public string userId { get; set; }
        public string Region { get; set; }
        public string Email { get; set; }
        public int IeCd { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Organisation { get; set; }
        public string OrgnType { get; set; }
        public int CO_CD { get; set; }
    }

    public class UserSessionModel
    {
        public string LoginType { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }
        public string AuthLevl { get; set; }
        public int IeCd { get; set; }
        public int RoleId { get; set; }
        public string OrganisationL { get; set; }
        public string OrgnTypeL { get; set; }
        public string Organisation { get; set; }
        public string OrgnType { get; set; }
        public string RoleName { get; set; }
        public string USER_ID { get; set; }
        public string MOBILE { get; set; }
        public int CoCd { get; set; }
        public string FPUserID { get; set; }
    }
}
