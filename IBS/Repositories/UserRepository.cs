using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using static IBS.Helper.Enums;

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

        public DTResult<UserModel> GetUserList(DTParameters dtParameters)
        {

            DTResult<UserModel> dTResult = new() { draw = 0 };
            IQueryable<UserModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "UserName";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "UserName";
                orderAscendingDirection = true;
            }
            query = from l in context.T02Users
                    select new UserModel
                    {
                        UserId = l.UserId,
                        UserName = l.UserName,
                        EmpNo = l.EmpNo,
                        Region = l.Region,
                        Status = l.Status,
                        AllowPo = l.AllowPo,
                        AllowDnChksht = l.AllowDnChksht,
                        CallMarking = l.CallMarking,
                        CallRemarking = l.CallRemarking,
                        UserType = l.UserType
                        //Createddate = l.Createddate,
                        //Createdby = l.Createdby,
                        //Updateddate = l.Updateddate,
                        //Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.UserName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int UserID)
        {
            var User = context.T02Users.Find(UserID);
            if (User == null) { return false; }

            //User.Isdeleted = Convert.ToByte(true);
            //User.Updatedby = Convert.ToString(UserID);
            //User.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int UserDetailsInsertUpdate(UserModel model)
        {
            int UserId = 0;
            var User = context.T02Users.Find(model.UserId);
            #region User save
            if (User == null)
            {
                T02User obj = new T02User();
                //obj.UserId = model.UserId;
                obj.UserName = model.UserName;
                obj.EmpNo = model.EmpNo;
                obj.Region = model.Region;
                obj.Status = model.Status;
                obj.UserType = model.UserType;
                //obj.Isdeleted = Convert.ToByte(false);
                //obj.Createdby = model.Createdby;
                //obj.Createddate = DateTime.Now;
                context.T02Users.Add(obj);
                context.SaveChanges();
                UserId = Convert.ToInt32(obj.UserId);
            }
            else
            {
                User.UserId = model.UserId;
                User.UserName = model.UserName;
                User.EmpNo = model.EmpNo;
                User.Region = model.Region;
                User.Status = model.Status;
                User.UserType = model.UserType;
                //User.Isdeleted = Convert.ToByte(false);
                //User.Createdby = model.Createdby;
                //User.Createddate = DateTime.Now;
                context.SaveChanges();
                UserId = Convert.ToInt32(User.UserId);
            }
            #endregion
            return UserId;
        }
    }

}
