using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace IBS.Repositories
{
    public class VendorProfileRepository : IVendorProfileRepository
    {
        private readonly ModelContext context;

        public VendorProfileRepository(ModelContext context)
        {
            this.context = context;
        }

        public VendorModel FindByID(int VendCd)
        {
            VendorModel model = (from m in context.T05Vendors
                                 where m.VendCd == VendCd
                                 select new VendorModel
                                 {
                                     VendCd= m.VendCd,
                                     VendName = m.VendName,
                                     VendAdd1 =  m.VendAdd1,
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

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "VEND_NAME";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
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
                query = query.Where(w => Convert.ToString(w.VEND_NAME).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.VEND_CITY_CD).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.VEND_CITY_CD).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.VEND_ADD).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(int VEND_CD, int UserID)
        {
            var roles = context.T05Vendors.Find(VEND_CD);
            if (roles == null) { return false; }
            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = UserID;
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int VendorDetailsInsertUpdate(VendorModel model, bool isSameVendor)
        {
            int VendCd = 0;
            var t05Vendors = context.T05Vendors.Find(model.VendCd);
            #region t05Vendors save
            if (t05Vendors == null)
            {
                //int generatedVendCd = (from v in context.T05Vendors select VendCd).Max() + 1;
                int maxVendCd = context.T05Vendors.Max(v => (v.VendCd) + 1);

                T05Vendor obj = new T05Vendor();
                obj.VendCd = maxVendCd;
                obj.VendName = model.VendName;
                obj.VendAdd1 = model.VendAdd1;
                obj.VendAdd2 = model.VendAdd2;
                obj.VendCityCd = model.VendCityCd;
                obj.VendContactPer1 = model.VendContactPer1;
                obj.VendContactTel1 = model.VendContactTel1;
                obj.VendContactPer2 = model.VendContactPer2;
                obj.VendContactTel2 = model.VendContactTel2;
                obj.VendApproval = model.VendApproval;
                obj.VendApprovalFr = model.VendApprovalFr;
                obj.VendApprovalTo = model.VendApprovalTo;
                obj.VendRemarks = model.VendRemarks;
                obj.Datetime = DateTime.Now;
                obj.VendEmail = model.VendEmail;
                obj.VendPwd = model.VendPwd;

                obj.VendStatus = model.VendStatus;
                obj.VendInspStopped = model.VendInspStopped;
                obj.OnlineCallStatus = model.OnlineCallStatus;

                context.T05Vendors.Add(obj);
                context.SaveChanges();
                VendCd = Convert.ToInt32(obj.VendCd);
            }
            else
            {
                if(isSameVendor)
                {
                    t05Vendors.VendAdd1 = model.VendAdd1;
                    t05Vendors.VendAdd2 = model.VendAdd2;
                    t05Vendors.VendCityCd = model.VendCityCd;
                    t05Vendors.VendContactPer1 = model.VendContactPer1;
                    t05Vendors.VendContactTel1 = model.VendContactTel1;
                    t05Vendors.VendContactPer2 = model.VendContactPer2;
                    t05Vendors.VendContactTel2 = model.VendContactTel2;
                    t05Vendors.VendRemarks = model.VendRemarks;
                    t05Vendors.Datetime = DateTime.Now;
                    t05Vendors.VendEmail = model.VendEmail;
                }
                else
                {
                    t05Vendors.VendName = model.VendName;
                    t05Vendors.VendAdd1 = model.VendAdd1;
                    t05Vendors.VendAdd2 = model.VendAdd2;
                    t05Vendors.VendCityCd = model.VendCityCd;
                    t05Vendors.VendContactPer1 = model.VendContactPer1;
                    t05Vendors.VendContactTel1 = model.VendContactTel1;
                    t05Vendors.VendContactPer2 = model.VendContactPer2;
                    t05Vendors.VendContactTel2 = model.VendContactTel2;
                    t05Vendors.VendRemarks = model.VendRemarks;
                    t05Vendors.Datetime = DateTime.Now;
                    t05Vendors.VendEmail = model.VendEmail;
                    t05Vendors.VendApproval = model.VendApproval;
                    t05Vendors.VendApprovalFr = model.VendApprovalFr;
                    t05Vendors.VendApprovalTo = model.VendApprovalTo;
                    t05Vendors.VendStatus = model.VendStatus;
                    t05Vendors.VendInspStopped = model.VendInspStopped;
                    t05Vendors.OnlineCallStatus = model.OnlineCallStatus;
                }

                //t05Vendors.UserId = model.UserId;
                context.SaveChanges();
                VendCd = Convert.ToInt32(t05Vendors.VendCd);
            }
            #endregion
            return VendCd;
        }
    }

}

