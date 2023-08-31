using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
namespace IBS.Repositories
{
    public class VendorClusterRepository : IVendorClusterRepository
    {
        private readonly ModelContext context;

        public VendorClusterRepository(ModelContext context)
        {
            this.context = context;
        }

        public VendorClusterModel FindByID(int VendorCode, string DepartmentCode)
        {
            VendorClusterModel model = new();

            T100VenderCluster venderCluster = context.T100VenderClusters.Where(x => x.VendorCode == VendorCode && x.DepartmentName == DepartmentCode).FirstOrDefault();

            if (venderCluster == null)
                return model;
            else
            {
                model.VendorCode = venderCluster.VendorCode;
                model.DepartmentName = venderCluster.DepartmentName;
                model.ClusterCode = venderCluster.ClusterCode;

                VendorDetailsModel data = (from v in context.T05Vendors
                                           join c in context.T03Cities on v.VendCityCd equals c.CityCd
                                           where v.VendCd == VendorCode
                                           select new VendorDetailsModel
                                           {
                                               VendCd = v.VendCd,
                                               VendName = v.VendName ?? "",
                                               VendAdd1 = v.VendAdd1 ?? "",
                                               Location = c.Location ?? "",
                                               City = c.City ?? ""
                                           }).FirstOrDefault();

                if(data != null)
                {
                    model.VendFullName = data.VendName + "/" + data.VendAdd1 + "/" + data.Location + " / " + data.City;
                    model.VendAdd1 = data.VendAdd1;
                }

                model.IsNew = false;

                return model;
            }
        }

        public DTResult<VendorClusterModel> GetVendorClusterList(DTParameters dtParameters)
        {

            DTResult<VendorClusterModel> dTResult = new() { draw = 0 };
            IQueryable<VendorClusterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "VendorName";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "VendorName";
                orderAscendingDirection = true;
            }

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";

            var lstQuery = (from vc in context.T100VenderClusters
                            join cm in context.T99ClusterMasters on vc.ClusterCode equals cm.ClusterCode
                            join v in context.T05Vendors on vc.VendorCode equals v.VendCd
                            where cm.RegionCode == Region
                            select new VendorClusterModel
                            {
                                VendorCode = vc.VendorCode,
                                VendorName = v.VendName,
                                ClusterName = cm.ClusterName,
                                GeographicalPartition = cm.GeographicalPartition,
                                DepartmentCode = vc.DepartmentName ?? "",
                                DepartmentName = EnumUtility<Enums.Department>.GetDescriptionByKey(vc.DepartmentName),
                            }).ToList();

            query = lstQuery.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => (w.VendorName != null && w.VendorName.ToLower().Contains(searchBy.ToLower()))
                                                || (w.ClusterName != null && w.ClusterName.ToLower().Contains(searchBy.ToLower()))
                                                || (w.GeographicalPartition != null && w.GeographicalPartition.ToLower().Contains(searchBy.ToLower()))
                                                || (w.DepartmentName != null && w.DepartmentName.ToLower().Contains(searchBy.ToLower()))
                                           );

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public VendorDetailsModel GetVendorDetails(string VendorCodeName)
        {
            VendorDetailsModel data = new();

            if (int.TryParse(VendorCodeName, out int n))
            {
                data = (from v in context.T05Vendors
                        join c in context.T03Cities on v.VendCityCd equals c.CityCd
                        where v.VendCd == Convert.ToInt32(VendorCodeName)
                        select new VendorDetailsModel
                        {
                            VendCd = v.VendCd,
                            VendName = v.VendName ?? "",
                            VendAdd1 = v.VendAdd1 ?? "",
                            VendStatus = v.VendStatus ?? "",
                            VendStatusDtFr = v.VendStatusDtFr,
                            VendStatusDtTo = v.VendStatusDtTo,
                            Location = c.Location ?? "",
                            City = c.City ?? ""
                        }).FirstOrDefault();
            }
            else
            {
                data = (from v in context.T05Vendors
                        join c in context.T03Cities on v.VendCityCd equals c.CityCd
                        where v.VendName != null && v.VendName.ToString().ToUpper().StartsWith(VendorCodeName.ToString().ToUpper())
                        select new VendorDetailsModel
                        {
                            VendCd = v.VendCd,
                            VendName = v.VendName ?? "",
                            VendAdd1 = v.VendAdd1 ?? "",
                            VendStatus = v.VendStatus ?? "",
                            VendStatusDtFr = v.VendStatusDtFr,
                            VendStatusDtTo = v.VendStatusDtTo,
                            Location = c.Location ?? "",
                            City = c.City ?? ""
                        }).FirstOrDefault();
            }

            return data;
        }

        public bool IsDuplicate(VendorClusterModel model)
        {
            if (model.IsNew)
            {
                return context.T100VenderClusters.Any(x => x.VendorCode == model.VendorCode && x.DepartmentName == model.DepartmentName);
            }
            else
            {
                return false;
            }
        }

        public int SaveDetails(VendorClusterModel model)
        {
            if (model.IsNew)
            {
                T100VenderCluster venderCluster = new()
                {
                    VendorCode = model.VendorCode,
                    DepartmentName = model.DepartmentName,
                    ClusterCode = model.ClusterCode,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                };

                context.T100VenderClusters.Add(venderCluster);
                context.SaveChanges();
            }
            else
            {
                T100VenderCluster venderCluster = context.T100VenderClusters.Where(x => x.VendorCode == model.VendorCode && x.DepartmentName == model.DepartmentName).FirstOrDefault();

                if (venderCluster != null)
                {
                    venderCluster.ClusterCode = model.ClusterCode;
                    venderCluster.UserId = model.UserId;
                    venderCluster.Datetime = DateTime.Now.Date;
                    venderCluster.Updatedby = model.Updatedby;
                    venderCluster.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }

            return model.VendorCode;
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

    }
}
