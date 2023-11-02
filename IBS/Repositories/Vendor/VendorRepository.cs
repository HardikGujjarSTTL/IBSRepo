using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static IBS.Helper.Enums;

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
            VendorMasterModel model = new();

            T05Vendor vendor = context.T05Vendors.Find(Id);

            if (vendor == null)
                return model;
            else
            {
                model.VendCd = vendor.VendCd;
                model.VendName = vendor.VendName;
                model.VendAdd1 = vendor.VendAdd1;
                model.VendAdd2 = vendor.VendAdd2;
                model.VendCityCd = vendor.VendCityCd;
                model.VendContactPer1 = vendor.VendContactPer1;
                model.VendContactTel1 = vendor.VendContactTel1;
                model.VendContactPer2 = vendor.VendContactPer2;
                model.VendContactTel2 = vendor.VendContactTel2;
                model.VendApproval = vendor.VendApproval;
                model.VendApprovalFr = vendor.VendApprovalFr;
                model.VendApprovalTo = vendor.VendApprovalTo;
                model.VendStatus = vendor.VendStatus;
                model.VendStatusDtFr = vendor.VendStatusDtFr;
                model.VendStatusDtTo = vendor.VendStatusDtTo;
                model.VendRemarks = vendor.VendRemarks;
                model.VendEmail = vendor.VendEmail;
                model.VendInspStopped = vendor.VendInspStopped;
                model.OnlineCallStatus = vendor.OnlineCallStatus;
                model.OfflineCallStatus = vendor.OfflineCallStatus;
                model.GSTNO = vendor.VendGstno;
                model.TANNO = vendor.VendTanno;
                model.PANNO = vendor.VendPanno;
            }

            return model;
        }

        public DTResult<VendorlistModel> GetVendorList(DTParameters dtParameters)
        {

            DTResult<VendorlistModel> dTResult = new() { draw = 0 };
            IQueryable<VendorlistModel>? query = null;

            //var searchBy = dtParameters.Search?.Value;
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

            string VendorCode = !string.IsNullOrEmpty(dtParameters.AdditionalValues["VendorCode"]) ? Convert.ToString(dtParameters.AdditionalValues["VendorCode"]) : "";
            string VendorName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["VendorName"]) ? Convert.ToString(dtParameters.AdditionalValues["VendorName"]) : "";
            string VendorAddress = !string.IsNullOrEmpty(dtParameters.AdditionalValues["VendorAddress"]) ? Convert.ToString(dtParameters.AdditionalValues["VendorAddress"]) : "";
            string VendorCity = !string.IsNullOrEmpty(dtParameters.AdditionalValues["VendorCity"]) ? Convert.ToString(dtParameters.AdditionalValues["VendorCity"]) : "";

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

            if (!string.IsNullOrEmpty(VendorCode)) query = query.Where(w => w.VEND_CD.ToString() == VendorCode);
            if (!string.IsNullOrEmpty(VendorName)) query = query.Where(w => w.VEND_NAME != null && w.VEND_NAME.ToString().ToLower().Contains(VendorName.ToString().ToLower()));
            if (!string.IsNullOrEmpty(VendorAddress)) query = query.Where(w => w.VEND_ADD != null && w.VEND_ADD.ToString().ToLower().Contains(VendorAddress.ToString().ToLower()));
            if (!string.IsNullOrEmpty(VendorCity)) query = query.Where(w => w.VEND_CITY_CD != null && w.VEND_CITY_CD.ToString().ToLower().Contains(VendorCity.ToString().ToLower()));

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
                model.VendCd = VendCd + 1;

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
                    VendEmail = model.VendEmail,
                    VendInspStopped = model.VendInspStopped,
                    OnlineCallStatus = model.OnlineCallStatus,
                    OfflineCallStatus = model.OfflineCallStatus,
                    VendGstno = model.GSTNO,
                    VendTanno = model.TANNO,
                    VendPanno = model.PANNO,
                    VendPwd = model.VendCd.ToString(),
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                    Createdby = model.Createdby,
                    Createddate = DateTime.Now,
                    Isdeleted = model.Isdeleted,
                };
                context.T05Vendors.Add(vendor);
                context.SaveChanges();
            }
            else
            {
                T05Vendor vendor = context.T05Vendors.Find(model.VendCd);

                if (vendor != null)
                {
                    vendor.VendName = model.VendName;
                    vendor.VendAdd1 = model.VendAdd1;
                    vendor.VendAdd2 = model.VendAdd2;
                    vendor.VendCityCd = model.VendCityCd;
                    vendor.VendContactPer1 = model.VendContactPer1;
                    vendor.VendContactTel1 = model.VendContactTel1;
                    vendor.VendContactPer2 = model.VendContactPer2;
                    vendor.VendContactTel2 = model.VendContactTel2;
                    vendor.VendApproval = model.VendApproval;
                    vendor.VendApprovalFr = model.VendApprovalFr;
                    vendor.VendApprovalTo = model.VendApprovalTo;
                    vendor.VendStatus = model.VendStatus;
                    vendor.VendStatusDtFr = model.VendStatusDtFr;
                    vendor.VendStatusDtTo = model.VendStatusDtTo;
                    vendor.VendEmail = model.VendEmail;
                    vendor.VendInspStopped = model.VendInspStopped;
                    vendor.OnlineCallStatus = model.OnlineCallStatus;
                    vendor.OfflineCallStatus = model.OfflineCallStatus;
                    vendor.VendRemarks = model.VendRemarks;
                    vendor.VendGstno = model.GSTNO;
                    vendor.VendTanno = model.TANNO;
                    vendor.VendPanno = model.PANNO;
                    vendor.UserId = model.UserId;
                    vendor.Datetime = DateTime.Now.Date;
                    vendor.Updatedby = model.Updatedby;
                    vendor.Updateddate = DateTime.Now;

                    context.SaveChanges();
                }

            }
            UserUpdate(model);
            return model.VendCd;
        }

        public string GetUserID(string VendCd)
        {
            string ID = "";
            var UserId = context.T02Users.Where(x => x.UserId == VendCd).FirstOrDefault();
            if (UserId != null)
            {
                ID = "1";
            }
            else
            {
                ID = "0";
            }
            return ID;
        }

        public void UserUpdate(VendorMasterModel model)
        {
            var UserDetails = context.T02Users.Where(x => x.UserId == Convert.ToString(model.VendCd)).FirstOrDefault();
            if (UserDetails == null)
            {
                T02User User = new();
                User.UserId = Convert.ToString(model.VendCd);
                User.UserName = model.VendName;
                User.RitesEmp = "Y";
                User.EmpNo = Convert.ToString(model.VendCd);
                User.Region = model.Region;
                User.Password = Convert.ToString(model.VendCd);
                User.Createdby = model.UserId;
                User.Createddate = DateTime.Now.Date;
                User.Isdeleted = 0;
                User.Migtype = "V";
                User.Mobile = model.VendContactTel1;

                context.T02Users.Add(User);
                context.SaveChanges();
            }
            else
            {
                UserDetails.UserName = model.VendName;
                UserDetails.RitesEmp = "Y";
                UserDetails.EmpNo = Convert.ToString(model.VendCd);
                UserDetails.Region = model.Region;
                UserDetails.Password = Convert.ToString(model.VendCd);
                UserDetails.Updatedby = model.UserId;
                UserDetails.Updateddate = DateTime.Now.Date;
                UserDetails.Isdeleted = 0;
                UserDetails.Migtype = "V";
                UserDetails.Mobile = model.VendContactTel1;

                context.SaveChanges();
            }
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

        public bool Remove(int id)
        {
            if (context.T05Vendors.Any(x => x.VendCd == id))
            {
                context.T05Vendors.RemoveRange(context.T05Vendors.Where(x => x.VendCd == id).ToList());
                context.SaveChanges();
            }
            return true;
        }

    }
}
