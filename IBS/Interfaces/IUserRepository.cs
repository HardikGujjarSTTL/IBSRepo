using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IUserRepository
    {
        public void Add(UserModel model);

        public UserModel FindByID(string UserId);

        public void Update(UserModel model);

        public UserSessionModel LoginByUserName(LoginModel model);
        public UserSessionModel LoginByUserPass(LoginModel model);
        public UserSessionModel FindByLoginDetail(LoginModel model);

        public void ChangePassword(int UserId, String NewPassword);

        public T02User FindByUsernameOrEmail(string UserName);

        public void ChangePassword(ResetPasswordModel resetPassword);

        DTResult<UserModel> GetUserList(DTParameters dtParameters);

        bool Remove(string UserId);

        int UserDetailsInsertUpdate(UserModel model);

        public VendorModel FindVendorLoginDetail(LoginModel model);

        public IELoginModel FindIELoginDetail(LoginModel model);

        public List<MenuMasterModel> GenerateMenuListByRoleId(int RoleID);

        public bool SaveOTPDetails(LoginModel model);
        public bool VerifyOTP(LoginModel model);

    }
}
