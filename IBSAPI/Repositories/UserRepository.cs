using IBSAPI.DataAccess;
using IBSAPI.Helper;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

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
            //userModel = (from u in context.T02Users
            //             where u.UserId.Trim() == model.UserName.Trim()
            //             && u.Password.Trim() == model.Password.Trim()
            //             join ur in context.Userroles on u.UserId equals ur.UserId into userRoles
            //             from ur in userRoles.DefaultIfEmpty()
            //             join r in context.Roles on ur.RoleId equals r.RoleId into roles
            //             from r in roles.DefaultIfEmpty()
            //             join t106 in context.T106LoOrgns on model.UserName.Trim() equals t106.Mobile into orgns
            //             from orgn in orgns.DefaultIfEmpty()
            //             join t32 in context.T32ClientLogins on model.UserName.Trim() equals t32.Mobile into clientLogins
            //             from clientLogin in clientLogins.DefaultIfEmpty()
            //             join t09 in context.T09Ies on model.UserName equals t09.IeEmpNo into ies
            //             from ie in ies.DefaultIfEmpty()
            //             select new UserModel
            //             {
            //                 userName = Convert.ToString(u.UserName),
            //                 userId = Convert.ToString(u.UserId.Trim()),
            //                 Region = Convert.ToString(GetRegion(u.Region)),
            //                 userType = r != null ? Convert.ToString(r.Rolename) : string.Empty,
            //                 RoleId = ur != null ? Convert.ToInt32(ur.RoleId) : 0,
            //                 RoleName = r != null ? Convert.ToString(r.Rolename) : string.Empty,
            //                 OrgnType = clientLogin != null ? Convert.ToString(clientLogin.OrgnType) : string.Empty,
            //                 Organisation = clientLogin != null ? Convert.ToString(clientLogin.Organisation) : string.Empty,
            //                 IeCd = ie != null ? Convert.ToInt16(ie.IeCd) : 0,
            //                 CO_CD = u.CoCd != null ? Convert.ToInt16(u.CoCd) : 0,
            //             }).FirstOrDefault();

            //UserSessionModel userSessionModel = new UserSessionModel();

            DataSet ds = new DataSet();
            OracleParameter[] parameter = new OracleParameter[4];
            parameter[0] = new OracleParameter("P_USER_NAME", OracleDbType.Varchar2, model.UserName.Trim(), ParameterDirection.Input);
            parameter[1] = new OracleParameter("P_PASSWORD", OracleDbType.Varchar2, model.Password.Trim(), ParameterDirection.Input);
            parameter[2] = new OracleParameter("P_USERTYPE", OracleDbType.Varchar2, model.UserType.Trim(), ParameterDirection.Input);
            parameter[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("GET_LOGINDETAILSForAPI", parameter);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                userModel.userName = Convert.ToString(ds.Tables[0].Rows[0]["USER_NAME"]).Trim();
                userModel.userId = Convert.ToString(ds.Tables[0].Rows[0]["USER_ID"]).Trim();
                userModel.Region = Convert.ToString(ds.Tables[0].Rows[0]["REGION"]).Trim();
                userModel.userType = Convert.ToString(ds.Tables[0].Rows[0]["ROLE_NAME"]).Trim();
                userModel.RoleId = Convert.ToInt32(ds.Tables[0].Rows[0]["ROLE_ID"]);
                userModel.RoleName = Convert.ToString(ds.Tables[0].Rows[0]["ROLE_NAME"]).Trim();
                userModel.OrgnType = Convert.ToString(ds.Tables[0].Rows[0]["ORGN_TYPE"]).Trim();
                userModel.Organisation = Convert.ToString(ds.Tables[0].Rows[0]["ORGN_CHASED"]).Trim();
                userModel.IeCd = Convert.ToInt32(ds.Tables[0].Rows[0]["IECD"]);
                userModel.CO_CD = Convert.ToInt32(ds.Tables[0].Rows[0]["COCD"]);
            }
            else
            {
                userModel = null;
            }
            return userModel;
        }

        public UserSessionModel FindByUsernameOrEmail(string UserName, string UserType)
        {
            //UserModel userModel = (from u in context.T02Users
            //                       where u.UserId.Trim() == UserName.Trim()
            //                       select new UserModel
            //                       {
            //                           userName = Convert.ToString(u.UserName),
            //                           userId = Convert.ToString(u.UserId.Trim()),
            //                       }).FirstOrDefault();
            //return userModel;

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
