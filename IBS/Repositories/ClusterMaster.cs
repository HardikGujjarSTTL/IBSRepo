using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class ClusterMaster : IClusterMaster
    {
        private readonly ModelContext context;

        public ClusterMaster(ModelContext context)
        {
            this.context = context;
        }
        public ClusterMasterModel FindByID(int ClusterCode)
        {
            ClusterMasterModel model = new();
            T99ClusterMaster role = context.T99ClusterMasters.Find(Convert.ToByte(ClusterCode));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.ClusterCode = role.ClusterCode;
                model.ClusterName = role.ClusterName;
                model.GeographicalPartition = role.GeographicalPartition;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<ClusterMasterModel>GetClusterMasterList(DTParameters dtParameters)
        {

            DTResult<ClusterMasterModel> dTResult = new() { draw = 0 };
            IQueryable<ClusterMasterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "ClusterCode";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "ClusterCode";
                orderAscendingDirection = true;
            }
            query = from l in context.T99ClusterMasters
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new ClusterMasterModel
                    {
                        ClusterCode = l.ClusterCode,
                        ClusterName = l.ClusterName,
                        GeographicalPartition = l.GeographicalPartition,
                        UserId = l.UserId,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.ClusterName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.ClusterCode).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int ClusterCode, int UserID)
        {
            var roles = context.T99ClusterMasters.Find(Convert.ToByte(ClusterCode));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int ClusterMasterDetailsInsertUpdate(ClusterMasterModel model)
        {
            int RoleId = 0;
            var CM = context.T99ClusterMasters.Where(x => x.ClusterCode == model.ClusterCode).FirstOrDefault();
            #region Role save
            if (CM == null || CM.ClusterCode == 0)
            {
                T99ClusterMaster obj = new T99ClusterMaster();
                obj.ClusterName = model.ClusterName;
                obj.GeographicalPartition = model.GeographicalPartition;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T99ClusterMasters.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.ClusterCode);
            }
            else
            {
                CM.ClusterName = model.ClusterName;
                CM.GeographicalPartition = model.GeographicalPartition;
                CM.Updatedby = model.Updatedby;
                CM.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(CM.ClusterCode);
            }
            #endregion
            return RoleId;
        }
    }

}