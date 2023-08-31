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

        public VendorMasterModel FindByID(int Id)
        {
            VendorMasterModel model = (from m in context.T05Vendors
                                       where m.VendCd == Id
                                       select new VendorMasterModel
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

        public int SaveDetails(VendorMasterModel model)
        {
            if (model.VendCd == 0)
            {
                int VendCd = GetMaxVend_CD();
                VendCd += 1;

                T05Vendor vendor = new()
                {
                    VendCd = model.VendCd,
                    VendName = model.VendName,
                    VendAdd1 = model.VendAdd1,
                    VendAdd2 = model.VendAdd2,
                    VendCityCd = model.VendCityCd,
                    VendContactPer1 = model.VendContactPer1,
                    VendContactTel1 = model.VendContactTel1,
                    VendContactPer2 = model.VendContactPer2,
                    VendContactTel2 = model.VendContactTel2,
                    VendApproval = model.VendApproval,
                    VendApprovalFr = model.VendApprovalFr,
                    VendApprovalTo = model.VendApprovalTo,
                    VendStatus = model.VendStatus,
                    VendStatusDtFr = model.VendStatusDtFr,
                    VendStatusDtTo = model.VendStatusDtTo,
                    VendRemarks = model.VendRemarks,
                    VendCdAlpha = model.VendCdAlpha,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    VendEmail = model.VendEmail,
                    VendInspStopped = model.VendInspStopped,
                    VendPwd = model.VendPwd,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                    Isdeleted = model.Isdeleted,
                };

                context.T05Vendors.Add(vendor);
                context.SaveChanges();
            }
            else
            {
                T05Vendor uom = context.T05Vendors.Find(model.VendCd);

                if (uom != null)
                {
                    uom.UserId = model.UserId;
                    uom.Datetime = DateTime.Now.Date;
                    uom.Updatedby = model.Updatedby;
                    uom.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }
            }

            return model.VendCd;
        }

        public int GetMaxVend_CD()
        {
            int UOM_CD = 0;

            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "SELECT NVL(MAX(VEND_CD),0) from T05_VENDOR";
                    UOM_CD = Convert.ToInt32(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return UOM_CD;
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
