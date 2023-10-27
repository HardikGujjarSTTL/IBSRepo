using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IUserRepository
    {
        public UserModel FindByLoginDetail(LoginModel model);
        public UserModel FindByUsernameOrEmail(string UserName);
    }
}
