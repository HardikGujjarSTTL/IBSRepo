using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
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
                model.RoleId = (from u in context.Userroles where u.UserId == user.UserId select u.RoleId).FirstOrDefault();
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

        public UserSessionModel LoginByUserName(LoginModel model)
        {
            UserSessionModel userSessionModel = new UserSessionModel();

            if (model.UserType == "USERS")
            {
                userSessionModel = (from u in context.T02Users
                                    where u.UserId.Trim() == model.UserName.Trim()
                                    select new UserSessionModel
                                    {
                                        MOBILE = u.Mobile
                                    }).FirstOrDefault();
            }
            else if (model.UserType == "VENDOR")
            {
                userSessionModel = (from u in context.T05Vendors
                                    where u.VendCd == Convert.ToInt32(model.UserName.Trim())
                                    select new UserSessionModel
                                    {
                                        MOBILE = u.VendContactTel1
                                    }).FirstOrDefault();
            }
            else if (model.UserType == "IE")
            {
                userSessionModel = (from u in context.T09Ies
                                    where u.IeEmpNo.Trim() == model.UserName.Trim()
                                    select new UserSessionModel
                                    {
                                        MOBILE = u.IePhoneNo
                                    }).FirstOrDefault();
            }
            else if (model.UserType == "CLIENT_LOGIN")
            {
                userSessionModel = (from u in context.T32ClientLogins
                                    where u.Mobile.Trim() == model.UserName.Trim()
                                    select new UserSessionModel
                                    {
                                        MOBILE = u.Mobile
                                    }).FirstOrDefault();
            }
            else if (model.UserType == "LO_LOGIN")
            {
                userSessionModel = (from u in context.T105LoLogins
                                    where u.Mobile.Trim() == model.UserName.Trim()
                                    select new UserSessionModel
                                    {
                                        MOBILE = u.Mobile
                                    }).FirstOrDefault();
            }
            return userSessionModel;
        }

        public UserModel FindByIDForResetPass(string UserId, string UserType)
        {
            UserModel model = new();
            if (UserType == "USERS")
            {
                model = (from u in context.T02Users
                         where u.UserId.Trim() == UserType.Trim()
                         select new UserModel
                         {
                             FPUserID = u.UserId,
                             Password = u.Password
                         }).FirstOrDefault();
            }
            else if (UserType == "VENDOR")
            {
                model = (from u in context.T05Vendors
                         where u.VendCd == Convert.ToInt32(UserType.Trim())
                         select new UserModel
                         {
                             FPUserID = Convert.ToString(u.VendCd),
                             Password = u.VendPwd
                         }).FirstOrDefault();
            }
            else if (UserType == "IE")
            {
                model = (from u in context.T09Ies
                         where u.IeEmpNo.Trim() == UserType.Trim()
                         select new UserModel
                         {
                             FPUserID = u.IeEmpNo,
                             Password = u.IePwd
                         }).FirstOrDefault();
            }
            else if (UserType == "CLIENT_LOGIN")
            {
                model = (from u in context.T32ClientLogins
                         where u.Mobile.Trim() == UserType.Trim()
                         select new UserModel
                         {
                             FPUserID = u.Mobile,
                             Password = u.Pwd
                         }).FirstOrDefault();
            }
            else if (UserType == "LO_LOGIN")
            {
                model = (from u in context.T105LoLogins
                         where u.Mobile.Trim() == UserType.Trim()
                         select new UserModel
                         {
                             FPUserID = u.Mobile,
                             Password = u.Pwd
                         }).FirstOrDefault();
            }
            return model;
        }

        public UserSessionModel LoginByUserPass(LoginModel model)
        {
            UserSessionModel userSessionModel = new UserSessionModel();

            DataSet ds = new DataSet();
            OracleParameter[] parameter = new OracleParameter[4];
            parameter[0] = new OracleParameter("P_USER_NAME", OracleDbType.Varchar2, model.UserName.Trim(), ParameterDirection.Input);
            parameter[1] = new OracleParameter("P_PASSWORD", OracleDbType.Varchar2, model.Password.Trim(), ParameterDirection.Input);
            parameter[2] = new OracleParameter("P_USERTYPE", OracleDbType.Varchar2, model.UserType.Trim(), ParameterDirection.Input);
            parameter[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("GET_LOGINBYUSERPASS", parameter);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                userSessionModel.MOBILE = Convert.ToString(ds.Tables[0].Rows[0]["MOBILE"]).Trim();
                userSessionModel.UserID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                userSessionModel.Name = Convert.ToString(ds.Tables[0].Rows[0]["USER_NAME"]).Trim();
                userSessionModel.UserName = Convert.ToString(ds.Tables[0].Rows[0]["USER_ID"]).Trim();
            }
            else
            {
                userSessionModel = null;
            }

            //userSessionModel = (from u in context.T02Users
            //                    where u.UserId.Trim() == model.UserName.Trim() && u.Password.Trim() == model.Password.Trim()
            //                    select new UserSessionModel
            //                    {
            //                        MOBILE = u.Mobile,
            //                        UserID = Convert.ToInt32(u.Id),
            //                        Name = Convert.ToString(u.UserName),
            //                        UserName = Convert.ToString(u.UserId)
            //                    }).FirstOrDefault();


            return userSessionModel;

        }

        public UserSessionModel FindByLoginDetail(LoginModel model)
        {
            UserSessionModel userSessionModel = new UserSessionModel();

            DataSet ds = new DataSet();
            OracleParameter[] parameter = new OracleParameter[3];
            parameter[0] = new OracleParameter("P_USER_NAME", OracleDbType.Varchar2, model.UserName.Trim(), ParameterDirection.Input);
            parameter[1] = new OracleParameter("P_USERTYPE", OracleDbType.Varchar2, model.UserType.Trim(), ParameterDirection.Input);
            parameter[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("GET_LOGINDETAILS", parameter);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                userSessionModel.MOBILE = Convert.ToString(ds.Tables[0].Rows[0]["MOBILE"]).Trim();
                userSessionModel.UserID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                userSessionModel.Name = Convert.ToString(ds.Tables[0].Rows[0]["USER_NAME"]).Trim();
                userSessionModel.UserName = Convert.ToString(ds.Tables[0].Rows[0]["USER_ID"]).Trim();
                userSessionModel.Region = Convert.ToString(ds.Tables[0].Rows[0]["REGION"]).Trim();
                userSessionModel.AuthLevl = Convert.ToString(ds.Tables[0].Rows[0]["AUTH_LEVL"]).Trim();
                userSessionModel.RoleId = Convert.ToInt32(ds.Tables[0].Rows[0]["ROLE_ID"]);
                userSessionModel.RoleName = Convert.ToString(ds.Tables[0].Rows[0]["ROLE_NAME"]).Trim();
                userSessionModel.OrgnTypeL = Convert.ToString(ds.Tables[0].Rows[0]["ORGN_TYPE"]).Trim();
                userSessionModel.OrganisationL = Convert.ToString(ds.Tables[0].Rows[0]["ORGN_CHASED"]).Trim();
                userSessionModel.OrgnType = Convert.ToString(ds.Tables[0].Rows[0]["ORGN_TYPE"]).Trim();
                userSessionModel.Organisation = Convert.ToString(ds.Tables[0].Rows[0]["ORGANISATION"]).Trim();
                userSessionModel.IeCd = Convert.ToInt32(ds.Tables[0].Rows[0]["IECD"]);
                userSessionModel.CoCd = Convert.ToInt32(ds.Tables[0].Rows[0]["COCD"]);
            }
            else
            {
                userSessionModel = null;
            }

            if (model.UserType.Trim() == "LO_LOGIN")
            {
                T107LoLogginLog T107 = new()
                {
                    Mobile = model.UserName.Trim(),
                    Otp = Convert.ToByte(model.OTP),
                    OtpGenTime = DateTime.Now,
                    OtpExpTime = DateTime.Now,
                    //OtpGenTime = Convert.ToDateTime(DateTime.Now.Date.ToString("dd/MM/yyyy") + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second),
                    //OtpExpTime = Convert.ToDateTime(DateTime.Now.Date.ToString("dd/MM/yyyy") + " " + DateTime.Now.Hour + ":" + (Convert.ToInt32(DateTime.Now.Minute) + 10) + ":" + DateTime.Now.Second),
                    LogginTime = DateTime.Now,
                    Status = "A"
                };

                context.T107LoLogginLogs.Add(T107);
                context.SaveChanges();
            }

            //userSessionModel = (from u in context.T02Users
            //                    where u.UserId.Trim() == model.UserName.Trim()
            //                    //&& u.Password.Trim() == model.Password.Trim()
            //                    join ur in context.Userroles on u.UserId equals ur.UserId into userRoles
            //                    from ur in userRoles.DefaultIfEmpty()
            //                    join r in context.Roles on ur.RoleId equals r.RoleId into roles
            //                    from r in roles.DefaultIfEmpty()
            //                    join t106 in context.T106LoOrgns on model.UserName.Trim() equals t106.Mobile into orgns
            //                    from orgn in orgns.DefaultIfEmpty()
            //                    join t32 in context.T32ClientLogins on model.UserName.Trim() equals t32.Mobile into clientLogins
            //                    from clientLogin in clientLogins.DefaultIfEmpty()
            //                    join t09 in context.T09Ies on model.UserName equals t09.IeEmpNo into ies
            //                    from ie in ies.DefaultIfEmpty()
            //                    select new UserSessionModel
            //                    {
            //                        MOBILE = u.Mobile,
            //                        UserID = Convert.ToInt32(u.Id),
            //                        Name = Convert.ToString(u.UserName),
            //                        UserName = Convert.ToString(u.UserId),
            //                        Region = Convert.ToString(u.Region),
            //                        AuthLevl = Convert.ToString(u.AuthLevl),
            //                        RoleId = ur != null ? Convert.ToInt32(ur.RoleId) : 0,
            //                        RoleName = r != null ? Convert.ToString(r.Rolename) : string.Empty,
            //                        OrgnTypeL = orgn != null ? Convert.ToString(orgn.OrgnType) : string.Empty,
            //                        OrganisationL = orgn != null ? Convert.ToString(orgn.OrgnChased) : string.Empty,
            //                        OrgnType = clientLogin != null ? Convert.ToString(clientLogin.OrgnType) : string.Empty,
            //                        Organisation = clientLogin != null ? Convert.ToString(clientLogin.Organisation) : string.Empty,
            //                        IeCd = ie != null ? Convert.ToInt16(ie.IeCd) : 0,
            //                        CoCd = u != null ? Convert.ToInt16(u.CoCd) : 0
            //                    }).FirstOrDefault();

            return userSessionModel;

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

        public IELoginModel FindIELoginDetail(LoginModel model)
        {
            IELoginModel ieModel = new IELoginModel();
            ieModel = (from m in context.T09Ies
                       where m.IeEmpNo == model.UserName && m.IePwd.Trim() == model.Password.Trim() && m.IeStatus == null
                       select new IELoginModel
                       {
                           IeCd = m.IeCd,
                           IeEmpNo = m.IeEmpNo,
                           IePwd = m.IePwd,
                           IeName = m.IeName,
                           IeRegion = m.IeRegion,
                           IePhoneNo = m.IePhoneNo,
                           IeEmail = m.IeEmail
                       }).FirstOrDefault();
            return ieModel;
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

        public UserSessionModel FindByUsernameOrEmail(string UserName, string UserType)
        {
            //return context.UserMasters.FirstOrDefault(p => p.UserName.Trim() == UserName.Trim() || p.Email.Trim() == UserName.Trim());
            //return context.T02Users.FirstOrDefault(p => p.UserId.Trim() == UserName.Trim());

            UserSessionModel userSessionModel = new UserSessionModel();
            if (UserType == "USERS")
            {
                userSessionModel = (from u in context.T02Users
                                    where u.UserId.Trim() == UserName.Trim()
                                    select new UserSessionModel
                                    {
                                        UserName = u.UserName,
                                        FPUserID = u.UserId,
                                        Email = ""
                                    }).FirstOrDefault();
            }
            else if (UserType == "VENDOR")
            {
                userSessionModel = (from u in context.T05Vendors
                                    where u.VendCd == Convert.ToInt32(UserName.Trim())
                                    select new UserSessionModel
                                    {
                                        UserName = u.VendName,
                                        FPUserID = Convert.ToString(u.VendCd),
                                        Email = ""
                                    }).FirstOrDefault();
            }
            else if (UserType == "IE")
            {
                userSessionModel = (from u in context.T09Ies
                                    where u.IeEmpNo.Trim() == UserName.Trim()
                                    select new UserSessionModel
                                    {
                                        UserName = u.IeName,
                                        FPUserID = Convert.ToString(u.IeEmpNo),
                                        Email = ""
                                    }).FirstOrDefault();
            }
            else if (UserType == "CLIENT_LOGIN")
            {
                userSessionModel = (from u in context.T32ClientLogins
                                    where u.Mobile.Trim() == UserName.Trim()
                                    select new UserSessionModel
                                    {
                                        UserName = u.UserName,
                                        FPUserID = Convert.ToString(u.Mobile),
                                        Email = ""
                                    }).FirstOrDefault();
            }
            else if (UserType == "LO_LOGIN")
            {
                userSessionModel = (from u in context.T105LoLogins
                                    where u.Mobile.Trim() == UserName.Trim()
                                    select new UserSessionModel
                                    {
                                        UserName = u.LoName,
                                        FPUserID = Convert.ToString(u.Mobile),
                                        Email = ""
                                    }).FirstOrDefault();
            }

            return userSessionModel;
        }

        public void ChangePassword(ResetPasswordModel resetPassword)
        {
            if (resetPassword.UserType == "USERS")
            {
                var user = context.T02Users.Find(resetPassword.UserId);
                if (user != null)
                {
                    user.Password = resetPassword.ConfirmPassword;
                    context.SaveChanges();
                }
            }
            else if (resetPassword.UserType == "VENDOR")
            {
                var user = context.T05Vendors.Where(x=>x.VendCd == Convert.ToInt32(resetPassword.UserId)).FirstOrDefault();
                if (user != null)
                {
                    user.VendPwd = resetPassword.ConfirmPassword;
                    context.SaveChanges();
                }
            }
            else if (resetPassword.UserType == "IE")
            {
                var user = context.T09Ies.Where(x => x.IeEmpNo == resetPassword.UserId).FirstOrDefault();
                if (user != null)
                {
                    user.IePwd = resetPassword.ConfirmPassword;
                    context.SaveChanges();
                }
            }
            else if (resetPassword.UserType == "CLIENT_LOGIN")
            {
                var user = context.T32ClientLogins.Where(x => x.Mobile == resetPassword.UserId).FirstOrDefault();
                if (user != null)
                {
                    user.Pwd = resetPassword.ConfirmPassword;
                    context.SaveChanges();
                }
            }
            else if (resetPassword.UserType == "LO_LOGIN")
            {
                var user = context.T105LoLogins.Where(x => x.Mobile == resetPassword.UserId).FirstOrDefault();
                if (user != null)
                {
                    user.Pwd = resetPassword.ConfirmPassword;
                    context.SaveChanges();
                }
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
                        Updatedby = l.Updatedby,
                        RoleName = (from u in context.Userroles join r in context.Roles on u.RoleId equals r.RoleId where u.UserId == l.UserId select r.Rolename).FirstOrDefault(),
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

            var roles = (from ur in context.Userroles where ur.UserId == UserID select ur).FirstOrDefault();
            if (roles != null)
            {
                roles.Isdeleted = Convert.ToByte(true);
                roles.Updatedby = User.Id;
                roles.Updateddate = DateTime.Now;
                context.SaveChanges();
            }
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

                var role = (from ur in context.Userroles where ur.UserId == obj.UserId select ur).FirstOrDefault();
                if (role == null)
                {
                    int maxID = context.Userroles.Max(x => x.Id) + 1;
                    Userrole objUR = new Userrole();
                    objUR.Id = maxID;
                    objUR.UserId = Convert.ToString(obj.UserId);
                    objUR.RoleId = Convert.ToInt32(model.RoleId);
                    objUR.Isdeleted = Convert.ToByte(false);
                    objUR.Createdby = Convert.ToInt32(model.Createdby);
                    objUR.Createddate = DateTime.Now;
                    context.Userroles.Add(objUR);
                    context.SaveChanges();
                }
                else
                {
                    role.RoleId = Convert.ToInt32(model.RoleId);
                    role.Updatedby = Convert.ToInt32(model.Updatedby);
                    role.Updateddate = DateTime.Now;
                    context.SaveChanges();
                }
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

                var role = (from ur in context.Userroles where ur.UserId == User.UserId select ur).FirstOrDefault();
                if (role == null)
                {
                    int maxID = context.Userroles.Max(x => x.Id) + 1;
                    Userrole objUR = new Userrole();
                    objUR.Id = maxID;
                    objUR.UserId = Convert.ToString(User.UserId);
                    objUR.RoleId = Convert.ToInt32(model.RoleId);
                    objUR.Isdeleted = Convert.ToByte(false);
                    objUR.Createdby = Convert.ToInt32(model.Createdby);
                    objUR.Createddate = DateTime.Now;
                    context.Userroles.Add(objUR);
                    context.SaveChanges();
                }
                else
                {
                    role.RoleId = Convert.ToInt32(model.RoleId);
                    role.Updatedby = Convert.ToInt32(model.Updatedby);
                    role.Updateddate = DateTime.Now;
                    context.SaveChanges();
                }

                Id = Convert.ToInt32(User.Id);
            }
            #endregion
            return Id;
        }

        public List<MenuMasterModel> GenerateMenuListByRoleId(int RoleID)
        {

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_RoleID", OracleDbType.Int32, RoleID, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GetMenuMaster", par, 1);
            List<MenuMasterModel> menuList = new List<MenuMasterModel>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataTable table in ds.Tables)
                {
                    menuList.AddRange(table.AsEnumerable().Select(row => new MenuMasterModel
                    {
                        MenuId = row.Field<Int32>("MENUID"),
                        Title = row.Field<string>("TITLE"),
                        ParentId = row.Field<Int32?>("PARENTID") != null ? row.Field<Int32?>("PARENTID") : 0,
                        SortOrder = row.Field<Int32>("SORTORDER"),
                        ControllerName = row.Field<string>("CONTROLLERNAME"),
                        ActionName = row.Field<string>("ACTIONNAME"),
                        IconPath = row.Field<string>("ICONPATH"),
                        Role_Id = row.Field<Int32>("ROLE_ID"),
                        AddAccess = row.Field<decimal>("IsAdd") == 1 ? true : false,
                        EditAccess = row.Field<decimal>("IsEdit") == 1 ? true : false,
                        DeleteAccess = row.Field<decimal>("PIsDelete") == 1 ? true : false,
                        ViewAccess = row.Field<decimal>("IsView") == 1 ? true : false,
                    }));
                }
            }
            return menuList;
        }

        public bool SaveOTPDetails(LoginModel model)
        {
            int Id = 0;
            IbsUsersOtp obj = new IbsUsersOtp();
            obj.UserId = model.UserName;
            obj.Mobile = model.MOBILE;
            obj.Otp = model.OTP;
            obj.Createddate = DateTime.Now;
            obj.Status = 0;
            context.IbsUsersOtps.Add(obj);
            context.SaveChanges();
            Id = obj.Id;
            if (Id != 0)
            {
                return true;
            }
            return false;
        }

        public bool VerifyOTP(LoginModel model)
        {
            var objOtp = (from o in context.IbsUsersOtps
                          where o.UserId == model.UserName
                          select o).OrderByDescending(x => x.Createddate).FirstOrDefault();
            if (objOtp == null)
            {
                return false;
            }
            else
            {
                if (objOtp.Otp == model.OTP)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
