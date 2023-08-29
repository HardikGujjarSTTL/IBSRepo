using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
namespace IBS.Repositories
{
    public class VendorCluster : IVendorCluster
    {
        private readonly ModelContext context;

        public VendorCluster(ModelContext context)
        {
            this.context = context;
        }
        public VendorClusterModel FindByID(int VendorCode)
        {
            VendorClusterModel model = new();

            T100VenderCluster role = context.T100VenderClusters.Find(Convert.ToByte(VendorCode));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.VendorCode = role.VendorCode;
                model.DepartmentName = role.DepartmentName;
                model.ClusterCode = Convert.ToByte(role.ClusterCode);
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<VendorClusterModel>GetVendorClusterList(DTParameters dtParameters)
        {

            DTResult<VendorClusterModel> dTResult = new() { draw = 0 };
            IQueryable<VendorClusterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "DepartmentName";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "DepartmentName";
                orderAscendingDirection = true;
            }
            query = from l in context.T100VenderClusters
                    where !l.Isdeleted.HasValue
                    //where l.Isdeleted == null // l.Isdeleted == 0 ||
                    select new VendorClusterModel
                    {
                        VendorCode = l.VendorCode,
                        DepartmentName = l.DepartmentName,
                        ClusterCode = Convert.ToByte(l.ClusterCode),
                        UserId = l.UserId,
                        //Isdeleted = l.Isdeleted,
                        //Createddate = l.Createddate,
                        //Createdby = l.Createdby,
                        //Updateddate = l.Updateddate,
                        //Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.VendorCode).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.DepartmentName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int VendorCode, int UserID)
        {
            var roles = context.T100VenderClusters.Find(Convert.ToByte(VendorCode));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int VendorClusterDetailsInsertUpdate(VendorClusterModel model)
        {
            int RoleId = 0;
            var CM = context.T100VenderClusters.Where(x => x.VendorCode == model.VendorCode).FirstOrDefault();
            #region Role save
            if (CM == null || CM.VendorCode == 0)
            {
                T100VenderCluster obj = new T100VenderCluster();

                obj.VendorCode = model.VendorCode;
                obj.DepartmentName = model.DepartmentName;
                obj.ClusterCode = model.ClusterCode;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T100VenderClusters.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.VendorCode);
            }
            else
            {
                CM.DepartmentName = model.DepartmentName;
                CM.ClusterCode = model.ClusterCode;
                CM.Updatedby = model.Updatedby;
                CM.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(CM.VendorCode);
            }
            #endregion
            return RoleId;
        }
    }

}
