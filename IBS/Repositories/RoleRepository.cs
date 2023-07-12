using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            Role tenant = context.Roles.Find(RoleId);

            if (tenant == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.RoleId = tenant.RoleId;
                model.Rolename = tenant.Rolename;
                model.Roledescription = tenant.Roledescription;
                model.Issysadmin = tenant.Issysadmin;
                model.Isactive = tenant.Isactive;
                model.Isdeleted = tenant.Isdeleted;
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
                    select new RoleModel
                    {
                        RoleId = l.RoleId,
                        Rolename = l.Rolename,
                        Roledescription = l.Roledescription,
                        Issysadmin = l.Issysadmin,
                        Isactive = l.Isactive,
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
            var roles = context.Roles.Find(RoleId);
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToString(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int RoleDetailsInsertUpdate(RoleModel model)
        {
            int RoleId = 0;
            var role = context.Roles.Find(model.RoleId);
            #region Role save
            if (role == null)
            {
                Role obj = new Role();
                obj.Rolename = model.Rolename;
                obj.Roledescription = model.Roledescription;
                obj.Issysadmin = model.Issysadmin;
                obj.Isactive = model.Isactive;
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
                role.Issysadmin = model.Issysadmin;
                role.Isactive = model.Isactive;
                role.Isdeleted = Convert.ToByte(false);
                role.Createdby = model.Createdby;
                role.Createddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(role.RoleId);
            }
            #endregion
            return RoleId;
        }
    }

}
