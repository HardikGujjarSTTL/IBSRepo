using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;

namespace IBS.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ModelContext context;

        public UserRepository(ModelContext context)
        {
            this.context = context;
        }

        public void Add(UserModel model)
        {
            T02User user = new()
            {
                UserName = model.UserName,
                Password = model.Password,
            };

            //context.T02Users.Add(user);
            //context.SaveChanges();
        }

        public UserModel FindByID(int UserId)
        {
            UserModel model = new();
            T02User user = context.T02Users.Find(UserId);

            if (user == null)
                throw new Exception("User Record Not found");
            else
            {
                model.UserId = user.UserId;
                model.UserName = user.UserName;
                model.Password = user.Password;
                return model;
            }
        }

        public void Update(UserModel model)
        {
            var user = context.T02Users.Find(model.UserId);

            if (user == null)
                throw new Exception("User Record Not found");
            else
            {
                user.UserName = model.UserName;
                user.Password = model.Password;

                context.SaveChanges();
            }
        }
                
        public T02User FindByLoginDetail(LoginModel model)
        {
            //return context.UserMasters.FirstOrDefault(p => p.UserName.Trim() == model.UserName.Trim() && p.Password.Trim() == model.Password.Trim() && p.IsActive.HasValue && p.IsActive.Value);
            return context.T02Users.FirstOrDefault(p => p.UserId.Trim() == model.UserName.Trim() && p.Password.Trim() == model.Password.Trim());
            //return new T02User();
        }
        
        public void ChangePassword(int UserId, String NewPassword)
        {
            var user = context.T02Users.Find(UserId);
            if (user == null)
                throw new Exception("User Record Not found");
            else
            {
                user.Password = NewPassword;
                context.SaveChanges();
            }
        }
        public T02User FindByUsernameOrEmail(string UserName)
        {
            //return context.UserMasters.FirstOrDefault(p => p.UserName.Trim() == UserName.Trim() || p.Email.Trim() == UserName.Trim());
            return context.T02Users.FirstOrDefault(p => p.UserName.Trim() == UserName.Trim());
            //return new T02User();
        }
        public void ChangePassword(ResetPasswordModel resetPassword)
        {
            var user = context.T02Users.Find(resetPassword.UserId);
            if (user == null)
                throw new Exception("User Record Not found");
            else
            {
                user.Password = resetPassword.ConfirmPassword;
                context.SaveChanges();
            }
        }
    }

}
