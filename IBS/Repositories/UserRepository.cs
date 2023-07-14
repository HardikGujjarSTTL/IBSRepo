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

        public UserModel FindByID(string UserId)
        {
            UserModel model = new();
            T02User user = context.T02Users.Find(UserId);

            if (user == null)
                throw new Exception("User Record Not found");
            else
            {
                model.ID = Convert.ToDecimal(user.Id);
                model.UserId = user.UserId;
                model.UserName = user.UserName;
                model.Password = user.Password;
                model.EmpNo = user.EmpNo;
                model.Region = user.Region;
                model.AuthLevl = user.AuthLevl;
                model.Status = user.Status;
                model.AllowPo = user.AllowPo;
                model.AllowUpChksht = user.AllowUpChksht;
                model.AllowDnChksht = user.AllowDnChksht;
                model.CallMarking = user.CallMarking;
                model.CallRemarking = user.CallRemarking;
                model.UserType = user.UserType;

                model.Isdeleted = user.Isdeleted;
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

        public VendorModel FindVendorLoginDetail(LoginModel model)
        {
            VendorModel vendorModel = new VendorModel();
            vendorModel = (from m in context.T05Vendors 
                           where m.VendCd == Convert.ToInt32(model.UserName) && m.VendPwd.Trim() == model.Password.Trim()
                           select new VendorModel
                           {
                               VendName = m.VendName,
                               VendCd = m.VendCd
                           }).FirstOrDefault();
            return vendorModel;
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
                    where (l.Isdeleted == 0 || l.Isdeleted == null)
                    select new UserModel
                    {
                        ID = Convert.ToDecimal(l.Id),
                        UserId = l.UserId,
                        UserName = l.UserName,
                        EmpNo = l.EmpNo,
                        Region = l.Region,
                        Status = l.Status,
                        AllowPo = l.AllowPo,
                        AllowDnChksht = l.AllowDnChksht,
                        CallMarking = l.CallMarking,
                        CallRemarking = l.CallRemarking,
                        UserType = l.UserType,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
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
        public bool Remove(string UserID)
        {
            var User = context.T02Users.Find(UserID);
            if (User == null)
            {
                return false;
            }

            User.Isdeleted = Convert.ToByte(true);
            User.Updatedby = UserID;
            User.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int UserDetailsInsertUpdate(UserModel model)
        {
            int Id = 0;
            var User = context.T02Users.Find(model.UserId);
            #region User save
            if (User == null)
            {
                T02User obj = new T02User();
                obj.UserId = model.UserId;
                obj.UserName = model.UserName;
                obj.EmpNo = model.EmpNo;
                obj.Region = model.Region;
                obj.Status = model.Status;
                obj.UserType = model.UserType;
                obj.AuthLevl = model.AuthLevl;
                obj.AllowPo = model.AllowPo;
                obj.AllowDnChksht = model.AllowDnChksht;
                obj.CallMarking = model.CallMarking;
                obj.CallRemarking = model.CallRemarking;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.UserId;
                obj.Createddate = DateTime.Now;
                context.T02Users.Add(obj);
                context.SaveChanges();
                Id = Convert.ToInt32(obj.Id);
            }
            else
            {
                //User.UserId = model.UserId;
                User.UserName = model.UserName;
                User.EmpNo = model.EmpNo;
                User.Region = model.Region;
                User.Status = model.Status;
                User.UserType = model.UserType;
                User.AuthLevl = model.AuthLevl;
                User.AllowPo = model.AllowPo;
                User.AllowDnChksht = model.AllowDnChksht;
                User.CallMarking = model.CallMarking;
                User.CallRemarking = model.CallRemarking;

                User.Isdeleted = Convert.ToByte(false);
                User.Updatedby = model.UserId;
                User.Updateddate = DateTime.Now;
                context.SaveChanges();
                Id = Convert.ToInt32(User.Id);
            }
            #endregion
            return Id;
        }
    }

}
