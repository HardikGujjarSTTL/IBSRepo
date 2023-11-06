using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;

namespace IBSAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ModelContext context;
        public UserRepository(ModelContext context)
        {
            this.context = context;
        }
        public UserModel FindByLoginDetail(LoginModel model)
        {
            UserModel userModel = new UserModel();
            userModel = (from u in context.T02Users
                         where u.UserId.Trim() == model.UserName.Trim()
                         && u.Password.Trim() == model.Password.Trim()
                         join ur in context.Userroles on u.UserId equals ur.UserId into userRoles
                         from ur in userRoles.DefaultIfEmpty()
                         join r in context.Roles on ur.RoleId equals r.RoleId into roles
                         from r in roles.DefaultIfEmpty()
                         join t106 in context.T106LoOrgns on model.UserName.Trim() equals t106.Mobile into orgns
                         from orgn in orgns.DefaultIfEmpty()
                         join t32 in context.T32ClientLogins on model.UserName.Trim() equals t32.Mobile into clientLogins
                         from clientLogin in clientLogins.DefaultIfEmpty()
                         join t09 in context.T09Ies on model.UserName equals t09.IeEmpNo into ies
                         from ie in ies.DefaultIfEmpty()
                         select new UserModel
                         {
                             userName = Convert.ToString(u.UserName),
                             userId = Convert.ToString(u.UserId.Trim()),
                             Region = Convert.ToString(GetRegion(u.Region)),
                             userType = r != null ? Convert.ToString(r.Rolename) : string.Empty,
                             RoleId = ur != null ? Convert.ToInt32(ur.RoleId) : 0,
                             OrgnType = clientLogin != null ? Convert.ToString(clientLogin.OrgnType) : string.Empty,
                             Organisation = clientLogin != null ? Convert.ToString(clientLogin.Organisation) : string.Empty,
                             IeCd = ie != null ? Convert.ToInt16(ie.IeCd) : 0
                         }).FirstOrDefault();

            return userModel;

        }

        public UserModel FindByUsernameOrEmail(string UserName)
        {
            UserModel userModel = (from u in context.T02Users
                                   where u.UserId.Trim() == UserName.Trim()
                                   select new UserModel
                                   {
                                       userName = Convert.ToString(u.UserName),
                                       userId = Convert.ToString(u.UserId.Trim()),
                                   }).FirstOrDefault();
            return userModel;
        }

        public static string GetRegion(string Region)
        {
            string RetRegion = "";
            if (Region != "" && Region != null)
            {
                if (Region == "N")
                {
                    RetRegion = "Northern";
                }
                else if (Region == "S")
                {
                    RetRegion = "Southern";
                }
                else if (Region == "E")
                {
                    RetRegion = "Eastern";
                }
                else if (Region == "W")
                {
                    RetRegion = "Westrern";
                }
                else if (Region == "C")
                {
                    RetRegion = "Central";
                }
                else if (Region == "Q")
                {
                    RetRegion = "QA";
                }
            }
            return RetRegion;
        }       
    }
}
