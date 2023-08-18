using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;

namespace IBS.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ModelContext context;

        public RoleRepository(ModelContext context)
        {
            this.context = context;
        }
        public RoleModel FindByID(int RoleId)
        {
            RoleModel model = new();
            Role role = context.Roles.Find(Convert.ToInt32(RoleId));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.RoleId = role.RoleId;
                model.Rolename = role.Rolename;
                model.Roledescription = role.Roledescription;
                model.Issysadmin =Convert.ToBoolean(role.Issysadmin);
                model.Isactive = Convert.ToBoolean(role.Isactive);
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<RoleModel> GetRoleList(DTParameters dtParameters)
        {

            DTResult<RoleModel> dTResult = new() { draw = 0 };
            IQueryable<RoleModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Rolename";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Rolename";
                orderAscendingDirection = true;
            }
            query = from l in context.Roles
                    where l.Isdeleted == 0 
                    select new RoleModel
                    {
                        RoleId = l.RoleId,
                        Rolename = l.Rolename,
                        Roledescription = l.Roledescription,
                        Issysadmin = Convert.ToBoolean(l.Issysadmin),
                        Isactive = Convert.ToBoolean(l.Isactive),
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Rolename).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Roledescription).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int RoleId, int UserID)
        {
            var roles = context.Roles.Find(Convert.ToInt32(RoleId));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToDecimal(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int RoleDetailsInsertUpdate(RoleModel model)
        {
            int RoleId = 0;
            var role = (from r in context.Roles where r.RoleId == Convert.ToInt32(model.RoleId) select r).FirstOrDefault();
            #region Role save
            if (role == null)
            {
                Role obj = new Role();
                obj.Rolename = model.Rolename;
                obj.Roledescription = model.Roledescription;
                obj.Issysadmin = Convert.ToByte(model.Issysadmin);
                obj.Isactive = Convert.ToByte(model.Isactive);
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.Createdby;
                obj.Createddate = DateTime.Now;
                context.Roles.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.RoleId);
            }
            else
            {
                role.Rolename = model.Rolename;
                role.Roledescription = model.Roledescription;
                role.Issysadmin = Convert.ToByte(model.Issysadmin);
                role.Isactive = Convert.ToByte(model.Isactive);
                role.Isdeleted = Convert.ToByte(false);
                role.Updatedby = model.Updatedby;
                role.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(role.RoleId);
            }
            #endregion
            return RoleId;
        }


        #region UserRole
        public RoleModel FindUserRoleByID(int ID)
        {
            RoleModel model = new();
            Userrole role = context.Userroles.Find(ID);

            if (role == null)
                throw new Exception("User Role Not found");
            else
            {
                model.RoleId = role.RoleId;
                model.User_ID = role.UserId;
                return model;
            }
        }

        public DTResult<RoleModel> GetUserRoleList(DTParameters dtParameters)
        {

            DTResult<RoleModel> dTResult = new() { draw = 0 };
            IQueryable<RoleModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Rolename";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Rolename";
                orderAscendingDirection = true;
            }
            query = from l in context.Userroles
                    join u in context.T02Users on l.UserId equals u.Id.ToString()
                    join r in context.Roles on l.RoleId equals r.RoleId
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new RoleModel
                    {
                        RoleId = l.RoleId,
                        Rolename = r.Rolename,
                        User_ID = l.UserId,
                        UserName = u.UserName,
                        Id = l.Id
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Rolename).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.UserName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool RemoveUserRole(int ID, int UserID)
        {
            var roles = context.Userroles.Find(ID);
            if (roles == null) { return false; }
            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int UserRoleInsertUpdate(RoleModel model)
        {
            int ID = 0;
            var role = context.Userroles.Find(model.Id);
            #region Role save
            if (role == null)
            {
                int maxID = context.Userroles.Max(x => x.Id) + 1;
                Userrole obj = new Userrole();
                obj.Id = maxID;
                obj.UserId = model.User_ID;
                obj.RoleId = Convert.ToInt32(model.RoleId);
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = Convert.ToInt32(model.Createdby);
                obj.Createddate = DateTime.Now;
                context.Userroles.Add(obj);
                context.SaveChanges();
                ID = obj.Id;
            }
            else
            {
                role.RoleId = Convert.ToInt32(model.RoleId);
                role.Updatedby = Convert.ToInt32(model.Updatedby);
                role.Updateddate = DateTime.Now;
                context.SaveChanges();
                ID = role.Id;
            }
            #endregion
            return ID;
        }
        #endregion

        #region UserRole
        public MenuroleMappingModel FindMenuRoleMappingByID(int ID)
        {
            MenuroleMappingModel model = new();
            Menurolemapping role = context.Menurolemappings.Where(x=>x.Roleid == ID).FirstOrDefault();

            if (role == null)
                throw new Exception("Menu Role Mapping Not found");
            else
            {
                model.Roleid = role.Roleid;
                model.Menuid = role.Menuid;
                return model;
            }
        }

        public DTResult<MenuListModel> GetMenuList(DTParameters dtParameters)
        {

            DTResult<MenuListModel> dTResult = new() { draw = 0 };
            IQueryable<MenuListModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "RootParentID";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "RootParentID";
                orderAscendingDirection = true;
            }
            string Roleid = dtParameters.AdditionalValues.ToArray().Where(x => x.Key == "Roleid").FirstOrDefault().Value;
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_ROLEID", OracleDbType.Int32, Roleid == "" ? 0 : Convert.ToInt32(Roleid), ParameterDirection.Input);
            par[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_Get_Menu_Hierarchy_By_Using_RoleID", par, 1);
            List<MenuListModel> model = new List<MenuListModel>();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model = JsonConvert.DeserializeObject<List<MenuListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            query = model.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.ParentTitle).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<MenuroleMappingListModel> GetMenuRoleMappingList(DTParameters dtParameters)
        {

            DTResult<MenuroleMappingListModel> dTResult = new() { draw = 0 };
            IQueryable<MenuroleMappingListModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "rolename";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "rolename";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[1];
            par[0] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GetMenuMapping", par, 1);
            List<MenuroleMappingListModel> model = new List<MenuroleMappingListModel>();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model = JsonConvert.DeserializeObject<List<MenuroleMappingListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            query = model.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.rolename).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int MenuRoleMappingInsertUpdate(MenuroleMappingModel model)
        {
            int ID = 0;
            var menu = context.Menurolemappings.Where(x => x.Roleid == ID && x.Id == model.Id).FirstOrDefault();
            #region Menu Role Mappings save
            if (menu == null)
            {
                int maxID = context.Menurolemappings.Max(x => x.Id) + 1;
                Menurolemapping obj = new Menurolemapping();
                obj.Id = maxID;
                obj.Roleid = model.Roleid;
                obj.Menuid = model.Menuid; 
                obj.Isactive = model.Isactive;
                obj.Createdby = Convert.ToInt32(model.Createdby);
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.Menurolemappings.Add(obj);
                context.SaveChanges();
                ID = obj.Id;
            }
            #endregion
            return ID;
        }

        public bool RemoveenuRoleMapping(int RoleId, int UserID)
        {
            var roles = context.Menurolemappings.Where(x => x.Roleid == RoleId).ToList();
            if (roles == null) { return false; }

            foreach (var role in roles)
            {
                role.Isdeleted = Convert.ToByte(true);
                role.Isactive = false;
                role.Updatedby = Convert.ToInt32(UserID);
                role.Updateddate = DateTime.Now;
                context.SaveChanges();
            }
            return true;
        }
        #endregion
    }

}
