using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Linq;

namespace IBS.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ModelContext context;
        private readonly IConfiguration config;
        private readonly ISendMailRepository pSendMailRepository;

        public UserRepository(ModelContext context, IConfiguration _config, ISendMailRepository _pSendMailRepository)
        {
            this.context = context;
            config = _config;
            pSendMailRepository = _pSendMailRepository;
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

        public UserModel FindByUserID(string UserId, string UserType)
        {
            UserModel model = new();
            if (UserType == "USERS")
            {
                model = (from u in context.T02Users
                         where u.UserId.Trim() == UserId.Trim()
                         select new UserModel
                         {
                             FPUserID = u.UserId,
                             Password = u.Password
                         }).FirstOrDefault();
            }
            else if (UserType == "VENDOR")
            {
                model = (from u in context.T05Vendors
                         where u.VendCd == Convert.ToInt32(UserId.Trim())
                         select new UserModel
                         {
                             FPUserID = Convert.ToString(u.VendCd),
                             Password = u.VendPwd
                         }).FirstOrDefault();
            }
            else if (UserType == "IE")
            {
                model = (from u in context.T09Ies
                         where u.IeEmpNo.Trim() == UserId.Trim()
                         select new UserModel
                         {
                             FPUserID = u.IeEmpNo,
                             Password = u.IePwd
                         }).FirstOrDefault();
            }
            else if (UserType == "CLIENT_LOGIN")
            {
                model = (from u in context.T32ClientLogins
                         where u.Mobile.Trim() == UserId.Trim()
                         select new UserModel
                         {
                             FPUserID = u.Mobile,
                             Password = u.Pwd
                         }).FirstOrDefault();
            }
            else if (UserType == "LO_LOGIN")
            {
                model = (from u in context.T105LoLogins
                         where u.Mobile.Trim() == UserId.Trim()
                         select new UserModel
                         {
                             FPUserID = u.Mobile,
                             Password = u.Pwd
                         }).FirstOrDefault();
            }
            return model;
        }

        public List<UserModel> FindByUserType(string UserType)
        {
            List<UserModel> model = new();
            if (UserType == "USERS")
            {
                model = (from u in context.T02Users
                         where u.Password != null
                         select new UserModel
                         {
                             FPUserID = u.UserId,
                             Password = u.Password
                         }).ToList();
            }
            else if (UserType == "VENDOR")
            {
                model = (from u in context.T05Vendors
                         where u.VendPwd != null
                         select new UserModel
                         {
                             FPUserID = Convert.ToString(u.VendCd),
                             Password = u.VendPwd
                         }).ToList();
            }
            else if (UserType == "IE")
            {
                model = (from u in context.T09Ies
                         where u.IePwd != null
                         select new UserModel
                         {
                             FPUserID = u.IeEmpNo,
                             Password = u.IePwd
                         }).ToList();
            }
            else if (UserType == "CLIENT_LOGIN")
            {
                model = (from u in context.T32ClientLogins
                         where u.Pwd != null
                         select new UserModel
                         {
                             FPUserID = u.Mobile,
                             Password = u.Pwd
                         }).ToList();
            }
            else if (UserType == "LO_LOGIN")
            {
                model = (from u in context.T105LoLogins
                         where u.Pwd != null
                         select new UserModel
                         {
                             FPUserID = u.Mobile,
                             Password = u.Pwd
                         }).ToList();
            }
            return model;
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
                                        MOBILE = u.Mobile,
                                        Email = u.UserEmail
                                    }).FirstOrDefault();
            }
            else if (model.UserType == "VENDOR")
            {
                userSessionModel = (from u in context.T05Vendors
                                    where u.VendCd == Convert.ToInt32(model.UserName.Trim())
                                    select new UserSessionModel
                                    {
                                        MOBILE = u.VendContactTel1,
                                        Email = u.VendEmail
                                    }).FirstOrDefault();
            }
            else if (model.UserType == "IE")
            {
                userSessionModel = (from u in context.T09Ies
                                    where u.IeEmpNo.Trim() == model.UserName.Trim()
                                    select new UserSessionModel
                                    {
                                        MOBILE = u.IePhoneNo,
                                        Email  = u.IeEmail
                                    }).FirstOrDefault();
            }
            else if (model.UserType == "CLIENT_LOGIN")
            {
                userSessionModel = (from u in context.T32ClientLogins
                                    where u.Mobile.Trim() == model.UserName.Trim()
                                    select new UserSessionModel
                                    {
                                        MOBILE = u.Mobile,
                                        Email = u.Email
                                    }).FirstOrDefault();
            }
            else if (model.UserType == "LO_LOGIN")
            {
                userSessionModel = (from u in context.T105LoLogins
                                    where u.Mobile.Trim() == model.UserName.Trim()
                                    select new UserSessionModel
                                    {
                                        MOBILE = u.Mobile,
                                        Email = u.Email
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
                         where u.UserId.Trim() == UserId.Trim()
                         select new UserModel
                         {
                             FPUserID = u.UserId,
                             Password = u.Password
                         }).FirstOrDefault();
            }
            else if (UserType == "VENDOR")
            {
                model = (from u in context.T05Vendors
                         where u.VendCd == Convert.ToInt32(UserId.Trim())
                         select new UserModel
                         {
                             FPUserID = Convert.ToString(u.VendCd),
                             Password = u.VendPwd
                         }).FirstOrDefault();
            }
            else if (UserType == "IE")
            {
                model = (from u in context.T09Ies
                         where u.IeEmpNo.Trim() == UserId.Trim()
                         select new UserModel
                         {
                             FPUserID = u.IeEmpNo,
                             Password = u.IePwd
                         }).FirstOrDefault();
            }
            else if (UserType == "CLIENT_LOGIN")
            {
                model = (from u in context.T32ClientLogins
                         where u.Mobile.Trim() == UserId.Trim()
                         select new UserModel
                         {
                             FPUserID = u.Mobile,
                             Password = u.Pwd
                         }).FirstOrDefault();
            }
            else if (UserType == "LO_LOGIN")
            {
                model = (from u in context.T105LoLogins
                         where u.Mobile.Trim() == UserId.Trim()
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
                userSessionModel.Email = Convert.ToString(ds.Tables[0].Rows[0]["EMAILID"]).Trim();
                userSessionModel.CoCd = Convert.ToInt32(ds.Tables[0].Rows[0]["CO_CD"]);
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
                userSessionModel.MasterID = Convert.ToInt32(ds.Tables[0].Rows[0]["MASTER_ID"]);
                userSessionModel.Email = Convert.ToString(ds.Tables[0].Rows[0]["EMAIL"]);
                userSessionModel.LoginType = model.UserType.Trim();
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

        public void ChangePassword(string UserId, String NewPassword, string UserType)
        {
            if (UserType == "USERS")
            {
                var user = context.T02Users.Where(x => x.UserId == UserId).FirstOrDefault();
                if (user != null)
                {
                    user.Password = NewPassword;
                    context.SaveChanges();
                }
            }
            else if (UserType == "VENDOR")
            {
                var user = context.T05Vendors.Where(x => x.VendCd == Convert.ToInt32(UserId)).FirstOrDefault();
                if (user != null)
                {
                    user.VendPwd = NewPassword;
                    context.SaveChanges();
                }
            }
            else if (UserType == "IE")
            {
                var user = context.T09Ies.Where(x => x.IeEmpNo == UserId).FirstOrDefault();
                if (user != null)
                {
                    user.IePwd = NewPassword;
                    context.SaveChanges();
                }
            }
            else if (UserType == "CLIENT_LOGIN")
            {
                var user = context.T32ClientLogins.Where(x => x.Mobile == UserId).FirstOrDefault();
                if (user != null)
                {
                    user.Pwd = NewPassword;
                    context.SaveChanges();
                }
            }
            else if (UserType == "LO_LOGIN")
            {
                var user = context.T105LoLogins.Where(x => x.Mobile == UserId).FirstOrDefault();
                if (user != null)
                {
                    user.Pwd = NewPassword;
                    context.SaveChanges();
                }
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

        public UserModel CheckPasswordIsBlank(string UserName, string UserType)
        {
            //return context.UserMasters.FirstOrDefault(p => p.UserName.Trim() == UserName.Trim() || p.Email.Trim() == UserName.Trim());
            //return context.T02Users.FirstOrDefault(p => p.UserId.Trim() == UserName.Trim());

            UserModel userSessionModel = new UserModel();
            if (UserType == "USERS")
            {
                userSessionModel = (from u in context.T02Users
                                    where u.UserId.Trim() == UserName.Trim()
                                    select new UserModel
                                    {
                                        Password = u.Password
                                    }).FirstOrDefault();
            }
            else if (UserType == "VENDOR")
            {
                userSessionModel = (from u in context.T05Vendors
                                    where Convert.ToString(u.VendCd) == UserName.Trim()
                                    select new UserModel
                                    {
                                        Password = u.VendPwd
                                    }).FirstOrDefault();
            }
            else if (UserType == "IE")
            {
                userSessionModel = (from u in context.T09Ies
                                    where u.IeEmpNo.Trim() == UserName.Trim()
                                    select new UserModel
                                    {
                                        Password = u.IePwd
                                    }).FirstOrDefault();
            }
            else if (UserType == "CLIENT_LOGIN")
            {
                userSessionModel = (from u in context.T32ClientLogins
                                    where u.Mobile.Trim() == UserName.Trim()
                                    select new UserModel
                                    {
                                        Password = u.Pwd
                                    }).FirstOrDefault();
            }
            else if (UserType == "LO_LOGIN")
            {
                userSessionModel = (from u in context.T105LoLogins
                                    where u.Mobile.Trim() == UserName.Trim()
                                    select new UserModel
                                    {
                                        Password = u.Pwd
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
                var user = context.T05Vendors.Where(x => x.VendCd == Convert.ToInt32(resetPassword.UserId)).FirstOrDefault();
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
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "UserName";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "UserName";
                orderAscendingDirection = true;
            }

            string UserName = "", UserId = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["UserName"]))
            {
                UserName = Convert.ToString(dtParameters.AdditionalValues["UserName"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["UserId"]))
            {
                UserId = Convert.ToString(dtParameters.AdditionalValues["UserId"]);
            }

            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_UserName", OracleDbType.Varchar2, UserName.ToString() == "" ? DBNull.Value : UserName.ToString(), ParameterDirection.Input);
            par[1] = new OracleParameter("p_UserId", OracleDbType.Varchar2, UserId.ToString() == "" ? DBNull.Value : UserId.ToString(), ParameterDirection.Input);
            par[2] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
            par[3] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
            par[4] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            par[5] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Get_SP_AdministratorList", par, 2);
            List<UserModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<UserModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            int recordsTotal = 0;
            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {
                recordsTotal = Convert.ToInt32(ds.Tables[1].Rows[0]["total_records"]);
            }
            query = list.AsQueryable();
            dTResult.recordsTotal = recordsTotal;
            dTResult.recordsFiltered = recordsTotal;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Select(p => p).ToList();
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
                        AddAccess = row.Field<Int32>("IsAdd") == 1 ? true : false,
                        EditAccess = row.Field<Int32>("IsEdit") == 1 ? true : false,
                        DeleteAccess = row.Field<Int32>("PIsDelete") == 1 ? true : false,
                        ViewAccess = row.Field<Int32>("IsView") == 1 ? true : false,
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


        public string send_Vendor_Email(LoginModel model, string EmailID)
        {
            string MailID = Convert.ToString(config.GetSection("AppSettings")["MailID"]);
            string MailPass = Convert.ToString(config.GetSection("AppSettings")["MailPass"]);
            string MailSmtpClient = Convert.ToString(config.GetSection("AppSettings")["MailSmtpClient"]);

            string email = "";
            string mail_body = "Dear Sir/Madam,<br><br>";

            mail_body = mail_body + "OTP for Client Login is: " + model.OTP + ". This OTP is valid for 10 Mins only. <br><br>";
            mail_body = mail_body + "Thanks for using RITES Inspection Services. <br><br>";
            mail_body = mail_body + "<b>RITES LTD.</b>";
            string sender = "";
            sender = "nrinspn@rites.com";
            bool isSend = false;

            if (Convert.ToString(config.GetSection("MailConfig")["SendMail"]) == "1")
            {
                SendMailModel sendMailModel = new SendMailModel();
                sendMailModel.From = sender;
                sendMailModel.To = EmailID;
                sendMailModel.Subject = "Your OTP For Client Login By RITES";
                sendMailModel.Message = mail_body;
                isSend = pSendMailRepository.SendMail(sendMailModel, null);
            }

            return email;
        }        

        public CertificateDetails GetDSC_Exp_DT(int IeCd)
        {
            CertificateDetails model = new();
            var result = (from item in context.T09Ies
                         where item.IeCd == IeCd
                         select new
                         {
                             item.DscExpiryDt,
                             item.IeEmail,
                             item.IePhoneNo
                         }).FirstOrDefault();

            model.DSC_Exp_DT = (result.DscExpiryDt.HasValue && result.DscExpiryDt.Value != DateTime.MinValue) ? result.DscExpiryDt.Value : (DateTime?)null;
            model.IE_Email = result.IeEmail;
            model.IE_Phone_No = result.IePhoneNo;

            return model;
        }

        public string UpdateDSCDate(int IeCd,DateTime DSC_Exp_DT)
        {
            string msg = "";
            var recordToUpdate = context.T09Ies.SingleOrDefault(t => t.IeCd == IeCd);

            if (recordToUpdate != null)
            {
                recordToUpdate.DscExpiryDt = DSC_Exp_DT;
                context.SaveChanges();
            }
            return msg;
        }
    }
}
