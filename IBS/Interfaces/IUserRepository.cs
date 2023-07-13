using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IUserRepository
    {
        public void Add(UserModel model);

        public UserModel FindByID(int UserId);

        public void Update(UserModel model);

        public T02User FindByLoginDetail(LoginModel model);

        public void ChangePassword(int UserId, String NewPassword);
        public T02User FindByUsernameOrEmail(string UserName);
        public void ChangePassword(ResetPasswordModel resetPassword);

        public VendorModel FindVendorLoginDetail(LoginModel model);
    }
}
