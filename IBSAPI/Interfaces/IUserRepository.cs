using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IUserRepository
    {
        public UserModel FindByLoginDetail(LoginModel model);
        public UserSessionModel FindByUsernameOrEmail(string UserName,string UserType);        
    }
}
