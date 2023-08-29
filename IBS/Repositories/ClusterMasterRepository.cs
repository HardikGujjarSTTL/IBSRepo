using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
    public class ClusterMasterRepository : IClusterMasterRepository
    {
        private readonly ModelContext context;

        public ClusterMasterRepository(ModelContext context)
        {
            this.context = context;
        }

        public ClusterMasterModel FindByID(int Id)
        {
            ClusterMasterModel model = new();
            T99ClusterMaster cluster = context.T99ClusterMasters.Find(Id);

            if (cluster == null)
                return model;
            else
            {
                model.ClusterCode = cluster.ClusterCode;
                model.ClusterName = cluster.ClusterName;
                model.GeographicalPartition = cluster.GeographicalPartition;
                model.DepartmentName = cluster.DepartmentName;
                model.RegionCode = cluster.RegionCode;
                model.HqArea = cluster.HqArea;
                model.IsNew = false;

                return model;
            }
        }

        public DTResult<ClusterMasterModel> GetClusterMasterList(DTParameters dtParameters)
        {

            DTResult<ClusterMasterModel> dTResult = new() { draw = 0 };
            IQueryable<ClusterMasterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "ClusterCode";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "ClusterCode";
                orderAscendingDirection = true;
            }

            var lstQuery = (from l in context.T99ClusterMasters
                            where l.Isdeleted != 1
                            select new ClusterMasterModel
                            {
                                ClusterCode = l.ClusterCode,
                                ClusterName = l.ClusterName,
                                GeographicalPartition = l.GeographicalPartition,
                                DepartmentName = EnumUtility<Enums.Department>.GetDescriptionByKey(l.DepartmentName),
                                RegionCode = EnumUtility<Enums.Region>.GetDescriptionByKey(l.RegionCode),
                            }).ToList();

            query = lstQuery.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => w.ClusterCode.ToString().ToLower().Contains(searchBy.ToLower())
                                                || (w.ClusterName != null && w.ClusterName.ToLower().Contains(searchBy.ToLower()))
                                                || (w.GeographicalPartition != null && w.GeographicalPartition.ToLower().Contains(searchBy.ToLower()))
                                                || (w.DepartmentName != null && w.DepartmentName.ToLower().Contains(searchBy.ToLower()))
                                                || (w.RegionCode != null && w.RegionCode.ToLower().Contains(searchBy.ToLower()))
                                           );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int SaveDetails(ClusterMasterModel model)
        {
            if (model.IsNew)
            {
                int ClusterCd = GetMaxClusterCd();
                ClusterCd += 1;

                T99ClusterMaster cluster = new()
                {
                    ClusterCode = ClusterCd,
                    ClusterName = model.ClusterName,
                    GeographicalPartition = model.GeographicalPartition,
                    DepartmentName = model.DepartmentName,
                    RegionCode = model.RegionCode,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    HqArea = model.HqArea,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.T99ClusterMasters.Add(cluster);
                context.SaveChanges();
            }
            else
            {
                T99ClusterMaster cluster = context.T99ClusterMasters.Find(model.ClusterCode);

                if (cluster != null)
                {
                    cluster.ClusterName = model.ClusterName;
                    cluster.GeographicalPartition = model.GeographicalPartition;
                    cluster.DepartmentName = model.DepartmentName;
                    cluster.RegionCode = model.RegionCode;
                    cluster.HqArea = model.HqArea;
                    cluster.Updatedby = model.Updatedby;
                    cluster.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }

            return model.ClusterCode;
        }

        public int GetMaxClusterCd()
        {
            int ClusterCd = 0;

            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "SELECT NVL(MAX(CLUSTER_CODE), 0) FROM T99_CLUSTER_MASTER";
                    ClusterCd = Convert.ToInt32(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return ClusterCd;
        }

        public bool Remove(int Id, int UserID)
        {
            T99ClusterMaster cluster = context.T99ClusterMasters.Find(Id);
            if (cluster == null) { return false; }

            cluster.Isdeleted = 1;
            cluster.Updatedby = UserID;
            cluster.Updateddate = DateTime.Now;

            context.SaveChanges();
            return true;
        }
    }
}