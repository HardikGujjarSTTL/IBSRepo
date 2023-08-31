using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Vendor
{
    public class VendorRepository : IVendorRepository
    {
        private readonly ModelContext context;

        public VendorRepository(ModelContext context)
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

                if (data != null)
                {
                    model.VendFullName = data.VendName + "/" + data.VendAdd1 + "/" + data.Location + " / " + data.City;
                    model.VendAdd1 = data.VendAdd1;
                }

                model.IsNew = false;

                return model;
            }
        }

        public VendorModel FindByID(int Id)
        {
            VendorModel model = (from m in context.T05Vendors
                                 where m.VendCd == Id
                                 select new VendorModel
                                 {
                                     VendCd = m.VendCd,
                                     VendName = m.VendName,
                                     VendAdd1 = m.VendAdd1,
                                     VendAdd2 = m.VendAdd2,
                                     VendCityCd = m.VendCityCd,
                                     VendApproval = m.VendApproval,
                                     VendApprovalFr = m.VendApprovalFr,
                                     VendApprovalTo = m.VendApprovalTo,
                                     VendContactPer1 = m.VendContactPer1,
                                     VendContactTel1 = m.VendContactTel1,
                                     VendContactPer2 = m.VendContactPer2,
                                     VendContactTel2 = m.VendContactTel2,
                                     VendEmail = m.VendEmail,
                                     VendRemarks = m.VendRemarks,
                                     VendStatus = m.VendStatus,
                                     VendInspStopped = m.VendInspStopped,
                                     OnlineCallStatus = m.OnlineCallStatus,
                                 }).FirstOrDefault();

            return model;
        }

        public DTResult<VendorlistModel> GetVendorList(DTParameters dtParameters)
        {

            DTResult<VendorlistModel> dTResult = new() { draw = 0 };
            IQueryable<VendorlistModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "VEND_NAME";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "VEND_NAME";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[1];
            par[0] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_VENDOR_DATA", par, 1);

            List<VendorlistModel> model = new List<VendorlistModel>();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model = JsonConvert.DeserializeObject<List<VendorlistModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }

            query = model.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => (w.VEND_CD.ToString().Contains(searchBy.ToLower()))
                    || (w.VEND_NAME != null && w.VEND_NAME.ToLower().Contains(searchBy.ToLower()))
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

        public bool Remove(int VendorCode, string DepartmentCode)
        {
            if (context.T100VenderClusters.Any(x => x.VendorCode == VendorCode && x.DepartmentName == DepartmentCode))
            {
                context.T100VenderClusters.RemoveRange(context.T100VenderClusters.Where(x => x.VendorCode == VendorCode && x.DepartmentName == DepartmentCode).ToList());
                context.SaveChanges();
            }
            return true;
        }

    }
}
