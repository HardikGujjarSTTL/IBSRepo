using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
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
            using (var connection = context.Database.GetDbConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SP_GetRoleByID"; // Replace with the name of your stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Add any parameters to the command if required
                    // For example:
                     command.Parameters.Add(new OracleParameter("@role_id", "1"));

                    DataTable dataTable = new DataTable();
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);

                        //reader.NextResult();
                        //DataTable dataTable1 = new DataTable();
                        //dataTable1.Load(reader);
                    }

                }
            }

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
    }

}
