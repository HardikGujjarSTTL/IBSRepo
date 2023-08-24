using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;
using System.Linq;

namespace IBS.Repositories.Inspection_Billing
{
    public class CallMarkedOnlineRepository : ICallMarkedOnlineRepository
    {
        private readonly ModelContext context;
        private readonly IConfiguration configuration;
        public CallMarkedOnlineRepository(ModelContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public DTResult<CallMarkedOnlineModel> Get_Call_Marked_Online(DTParameters dtParameters)
        {
            DTResult<CallMarkedOnlineModel> dTResult = new() { draw = 0 };
            IQueryable<CallMarkedOnlineModel> query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;


            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CASE_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CASE_NO";
                orderAscendingDirection = true;
            }

            string Date = "";
            bool RDB1 = false, RDB2 = false, RDB3 = false;
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Date"]))
            {
                Date = Convert.ToString(dtParameters.AdditionalValues["Date"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Rdb1"]))
            {
                RDB1 = Convert.ToString(dtParameters.AdditionalValues["Rdb1"]) == "0" ? false : true;

            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Rdb2"]))
            {
                RDB2 = Convert.ToString(dtParameters.AdditionalValues["Rdb2"]) == "0" ? false : true;

            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Rdb3"]))
            {
                RDB3 = Convert.ToString(dtParameters.AdditionalValues["Rdb3"]) == "0" ? false : true;
            }

            Date = Date.ToString() == "" ? string.Empty : Date.ToString();

            //OracleParameter[] par = new OracleParameter[4];
            //par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            //par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            //par[2] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IE_CD, ParameterDirection.Input);
            //par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            //var ds = DataAccessDB.GetDataSet("SP_GET_PENDING_JI_CASES", par, 1);
            //DataTable dt = ds.Tables[0];
            //List<CallMarkedOnlineModel> list = dt.AsEnumerable().Select(row => new CallMarkedOnlineModel
            //{                
            //    CASE_NO = Convert.ToString(row["CASE_NO"]),
            //    CALL_RECV_DT = Convert.ToString(row["CALL_RECV_DT"]),
            //    CALL_INSTALL_NO = Convert.ToString(row["CALL_INSTALL_NO"]),
            //    CALL_SNO = Convert.ToString(row["CALL_SNO"]),
            //    DATE_TIME = Convert.ToString(row["DATE_TIME"]),
            //    CALL_STATUS = Convert.ToString(row["CALL_STATUS"]),
            //    CALL_LETTER_NO = Convert.ToString(row["CALL_LETTER_NO"]),
            //    REMARKS = Convert.ToString(row["REMARKS"]),                
            //    PO_NO = Convert.ToString(row["PO_NO"]),
            //    PO_DT = Convert.ToString(row["PO_DATE"]),
            //    VENDOR = Convert.ToString(row["VENDOR"]),
            //    IE_NAME = Convert.ToString(row["IE_NAME"])
            //}).ToList();
            DateTime CHQ_DATE = Convert.ToDateTime(DateTime.ParseExact(Date, "dd/MM/yyyy", null).ToString("dd/MM/yyyy"));
            query = (from c in context.T17CallRegisters
                     join p in context.T13PoMasters on c.CaseNo equals p.CaseNo into cpJoin
                     from cp in cpJoin.DefaultIfEmpty()
                     join i in context.T09Ies on c.IeCd equals i.IeCd into ciJoin
                     from ci in ciJoin.DefaultIfEmpty()
                     join v in context.T05Vendors on cp.VendCd equals v.VendCd into cvJoin
                     from cv in cvJoin.DefaultIfEmpty()
                     join t in context.T03Cities on cv.VendCityCd equals t.CityCd into ctJoin
                     from ct in ctJoin.DefaultIfEmpty()
                     join s in context.T21CallStatusCodes on c.CallStatus equals s.CallStatusCd into csJoin
                     from cs in csJoin.DefaultIfEmpty()
                     where cp.RegionCode == "N" && c.OnlineCall == "Y"
                           && ((RDB3 && c.CallRecvDt == CHQ_DATE)
                               || (RDB1 && c.CallRecvDt == CHQ_DATE && (ci.IeCd != 0))
                               || (RDB3 || RDB2) && ci.IeCd == null
                               || (RDB2 && c.CallRecvDt <= CHQ_DATE))
                     select new CallMarkedOnlineModel
                     {
                         CASE_NO = cp.CaseNo,
                         CALL_RECV_DT = Convert.ToString(c.CallRecvDt),
                         CALL_INSTALL_NO = Convert.ToString(c.CallInstallNo),
                         CALL_SNO = Convert.ToString(c.CallSno),
                         //DATE_TIME = Convert.ToDateTime(c.Datetime).ToString("dd/MM/yyyy-HH:mm:ss"),
                         CALL_STATUS = (cs.CallStatusDesc + (c.CallCancelStatus == "N" ? " (Non Chargeable)" : c.CallCancelStatus == "C" ? " (Chargeable)" : "")) ?? cs.CallStatusDesc,
                         CALL_LETTER_NO = c.CallLetterNo,
                         REMARKS = c.Remarks,
                         PO_NO = cp.PoNo,
                         //PO_DT = Convert.ToDateTime(cp.PoDt).ToString("dd/MM/yyyy"),
                         VENDOR = cv.VendName,// + "(" + (ct.LOCATION ?? (ct.LOCATION + " : " + ct.CITY)) + ")",
                         IE_NAME = ci.IeName
                     }).AsQueryable();

            if (RDB1 || RDB3)
            {
                query = query.OrderBy(item => item.CASE_NO)
                             .ThenBy(item => item.CALL_RECV_DT)
                             .ThenBy(item => item.CALL_SNO);
            }
            else
            {
                query = query.OrderByDescending(item => item.CALL_RECV_DT)
                             .ThenBy(item => item.CALL_SNO);
            }

            //list = query.ToList();


            //query = list.AsQueryable();

            dTResult.recordsTotal = query.ToList().Count(); //ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = query.ToList().Count();  //ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public List<SelectListItem> Get_Cluster_IE(string Region)
        {
            List<SelectListItem> IE = (from t99 in context.T99ClusterMasters
                                       join t101 in context.T101IeClusters on t99.ClusterCode equals t101.ClusterCode
                                       join t09 in context.T09Ies on t101.IeCode equals t09.IeCd
                                       where t99.RegionCode == "N" && t09.IeStatus == null && t99.DepartmentName == "C"
                                       orderby t99.ClusterName, t09.IeName
                                       select new SelectListItem
                                       {
                                           Text = t99.ClusterName + " (" + t09.IeName + ")",
                                           Value = Convert.ToString(t99.ClusterCode)
                                       }).ToList();
            return IE;
        }

        public CallMarkedOnlineModel Get_Call_Marked_Online_Detail(CallMarkedOnlineFilter obj)
        {
            var model = new CallMarkedOnlineModel();
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, obj.CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_DATE", OracleDbType.Varchar2, obj.Date, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CALL_SNO", OracleDbType.Varchar2, obj.CALL_SNO, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_CALL_MARKED_ONLINE_DETAIL", par, 1);
            DataTable dt = ds.Tables[0];

            model = dt.AsEnumerable().Select(row => new CallMarkedOnlineModel
            {
                PO_NO = Convert.ToString(row["PO_NO"]),
                PO_DT = Convert.ToString(row["PO_DT"]),
                RLY = Convert.ToString(row["RLY"]),
                L5NO_PO = Convert.ToString(row["L5NO_PO"]),
                VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                VEND_REMARKS = Convert.ToString(row["VEND_REMARKS"]),
                CASE_NO = Convert.ToString(row["CASE_NO"]),
                CALL_RECV_DT = Convert.ToString(row["CALL_RECV_DATE"]),
                INSP_DESIRE_DATE = Convert.ToString(row["INSP_DESIRE_DATE"]),
                CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                LETTER_DT = Convert.ToString(row["LETTER_DT"]),
                REMARKS = Convert.ToString(row["REMARKS"]),
                MFG = Convert.ToString(row["MFG"]),
                MFG_REMARKS = Convert.ToString(row["MFG_REMARKS"]),
                ITEM_SRNO_PO = Convert.ToString(row["ITEM_SRNO_PO"]),
                ITEM_DESC_PO = Convert.ToString(row["ITEM_DESC_PO"]),
                QTY_ORDERED = Convert.ToString(row["QTY_ORDERED"]),
                QTY_TO_INSP = Convert.ToString(row["QTY_TO_INSP"]),
                MFG_CD = Convert.ToString(row["MFG_CD"]),
                STAGG_DP = Convert.ToString(row["STAGG_DP"]),
                LOT_DP_1 = Convert.ToString(row["LOT_DP_1"]),
                LOT_DP_2 = Convert.ToString(row["LOT_DP_2"]),
                REG_TIME = Convert.ToString(row["REG_TIME"]),
                DEPARTMENT = Convert.ToString(row["DEPARTMENT"]),
                DEPARTMENT_CODE = Convert.ToString(row["DEPARTMENT_CODE"]),
                FINAL_OR_STAGE = Convert.ToString(row["FINAL_OR_STAGE"])
            }).FirstOrDefault();

            return model;
        }
        public List<CallMaterialValueModel> Get_Call_Material_Value(CallMarkedOnlineFilter obj)
        {
            var model = new List<CallMaterialValueModel>();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, obj.CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_DATE", OracleDbType.Varchar2, obj.Date, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CALL_SNO", OracleDbType.Varchar2, obj.CALL_SNO, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_CALL_MATERIAL_VALUE", par, 1);
            DataTable dt = ds.Tables[0];

            model = dt.AsEnumerable().Select(row => new CallMaterialValueModel
            {
                QTY = Convert.ToString(row["QTY"]),
                VALUE = Convert.ToString(row["VALUE"]),
                QTY_TO_INSP = Convert.ToString(row["QTY_TO_INSP"]),
            }).ToList();
            return model;
        }

        public VendorDetailModel Get_Vendor_For_Send_Mail(CallMarkedOnlineFilter obj)
        {
            var model = new VendorDetailModel();
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, obj.CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_DATE", OracleDbType.Varchar2, obj.Date, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CALL_SNO", OracleDbType.Varchar2, obj.CALL_SNO, ParameterDirection.Input);
            par[3] = new OracleParameter("P_VENDOR_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            par[4] = new OracleParameter("P_VENDOR_MFG_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_VENDOR_FOR_SEND_MAIL", par, 2);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];

            var vendorData = dt.AsEnumerable().Select(row => new VendorDetailModel
            {
                VEND_CD = Convert.ToString(row["VEND_CD"]),
                VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                VEND_ADDRESS = Convert.ToString(row["VEND_ADDRESS"]),
                VEND_EMAIL = Convert.ToString(row["VEND_EMAIL"]),
            }).FirstOrDefault();

            var mfgData = dt.AsEnumerable().Select(row => new VendorDetailModel
            {
                MANU_MAIL = Convert.ToString(row["VEND_NAME"]),
                MFG_CD = Convert.ToString(row["MFG_CD"])
            }).FirstOrDefault();

            model.VEND_CD = vendorData.VEND_CD;
            model.VEND_NAME = vendorData.VEND_NAME;
            model.VEND_ADDRESS = vendorData.VEND_ADDRESS;
            model.VEND_EMAIL = vendorData.VEND_EMAIL;

            model.MANU_MAIL = mfgData.MANU_MAIL;
            model.MFG_CD = mfgData.MFG_CD;

            return model;
        }

        public bool Call_Rejected(CallMarkedOnlineFilter obj)
        {
            var flag = false;
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    string dt = Convert.ToDateTime(obj.Date).ToString("dd-MM-yy");
                    DateTime callRecvDt = DateTime.ParseExact(dt, "dd-MM-yy", CultureInfo.InvariantCulture);
                    var _data = context.T18CallDetails
                                .Where(x => x.CaseNo == obj.CASE_NO && x.CallRecvDt == callRecvDt && x.CallSno == Convert.ToInt32(obj.CALL_SNO))
                                .Select(x => x).FirstOrDefault();

                    if (_data != null)
                    {
                        _data.Isdeleted = 1;
                        context.SaveChanges();
                        flag = true;
                    }

                    var _callReg = context.T17CallRegisters
                                    .Where(x => x.CaseNo == obj.CASE_NO && x.CallRecvDt == callRecvDt && x.CallSno == Convert.ToInt32(obj.CALL_SNO))
                                    .Select(x => x).FirstOrDefault();
                    if (_callReg != null)
                    {
                        _callReg.Isdeleted = Convert.ToByte(1);
                        context.SaveChanges();
                        flag = true;

                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    flag = false;
                    transaction.Rollback();
                }
            }
            return flag;
        }

        public bool Call_Marked_Online_Save(CallMarkedOnlineModel model, UserSessionModel uModel)
        {
            var flag = false;

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    var cl_exist = (from a in context.T100VenderClusters
                                    join b in context.T99ClusterMasters on Convert.ToInt32(a.ClusterCode) equals b.ClusterCode
                                    where a.VendorCode == 46040 && a.DepartmentName == "C" && b.RegionCode == "N"
                                    select b.ClusterCode).Distinct().Count();

                    var IE = (from ieCluster in context.T101IeClusters
                              where ieCluster.ClusterCode == Convert.ToByte(677) && ieCluster.DepartmentCode == "C"
                              select ieCluster.IeCode).FirstOrDefault();

                    var Co = (from ie in context.T09Ies
                              where ie.IeCd == 0
                              select ie.IeCoCd).FirstOrDefault();

                    var _data = context.T17CallRegisters
                                    .Where(x => x.CaseNo == model.CASE_NO && x.CallRecvDt == Convert.ToDateTime(model.CALL_RECV_DT) && x.CallSno == Convert.ToInt32(model.CALL_SNO))
                                    .Select(x => x);
                    foreach (var item in _data)
                    {
                        item.Remarks = model.REMARKS;
                        item.IeCd = IE.Value;
                        item.ClusterCode = Convert.ToByte(model.IE_NAME);
                        item.CoCd = Convert.ToByte(Co.Value);
                        item.UserId = uModel.UserName;
                        if (model.DEPARTMENT_CODE == model.DEPT_DROPDOWN)
                        {
                            item.DepartmentCode = model.DEPT_DROPDOWN;
                        }
                        item.Datetime = DateTime.Now;
                        item.Updatedby = Convert.ToString(uModel.UserID);
                        item.Updateddate = DateTime.Now;
                    }
                    context.SaveChanges();
                    if (cl_exist == 0)
                    {
                        T100VenderCluster insObj = new T100VenderCluster();
                        insObj.VendorCode = Convert.ToInt32(model.MFG_CD);
                        insObj.DepartmentName = model.DEPT_DROPDOWN;
                        insObj.ClusterCode = Convert.ToByte(model.IE_NAME);
                        insObj.UserId = uModel.UserName;
                        insObj.Datetime = DateTime.Now;
                        insObj.Createdby = uModel.UserID;
                        insObj.Createddate = DateTime.Now;
                        context.T100VenderClusters.Add(insObj);
                        context.SaveChanges();
                    }
                    var ietopoicount = (from mapping in context.T60IePoiMappings
                                        where mapping.IeCd == 0 && mapping.PoiCd == Convert.ToInt32("46040")
                                        select mapping).Count();

                    if(ietopoicount == 0)
                    {
                        T60IePoiMapping insObj = new T60IePoiMapping();
                        insObj.IeCd = IE.Value;
                        insObj.PoiCd = Convert.ToInt32(model.MFG_CD);
                        context.T60IePoiMappings.Add(insObj);
                        context.SaveChanges();
                    }
                    flag = true;
                    trans.Commit();
                }
                catch (Exception)
                {
                    flag = false;
                    trans.Rollback();
                }
            }
            return flag;
        }
    }
}
