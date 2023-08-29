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
using System.Drawing;
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
        public DTResult<CallMarkedOnlineModel> Get_Call_Marked_Online(DTParameters dtParameters, string Region)
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
            int RDB1 = 0, RDB2 = 0, RDB3 = 0;
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Date"]))
            {
                Date = Convert.ToString(dtParameters.AdditionalValues["Date"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Rdb1"]))
            {
                RDB1 = Convert.ToString(dtParameters.AdditionalValues["Rdb1"]) == "0" ? 0 : 1;

            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Rdb2"]))
            {
                RDB2 = Convert.ToString(dtParameters.AdditionalValues["Rdb2"]) == "0" ? 0 : 1;

            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Rdb3"]))
            {
                RDB3 = Convert.ToString(dtParameters.AdditionalValues["Rdb3"]) == "0" ? 0 : 1;
            }

            Date = Date.ToString() == "" ? string.Empty : Date.ToString();

            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("P_DATE", OracleDbType.Varchar2, Date, ParameterDirection.Input);
            par[1] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RDB1", OracleDbType.Int16, RDB1, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RDB2", OracleDbType.Int16, RDB2, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RDB3", OracleDbType.Int16, RDB3, ParameterDirection.Input);
            par[5] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_CALL_MARKED_ONLINE", par, 1);
            DataTable dt = ds.Tables[0];
            List<CallMarkedOnlineModel> list = dt.AsEnumerable().Select(row => new CallMarkedOnlineModel
            {
                CASE_NO = Convert.ToString(row["CASE_NO"]),
                CALL_RECV_DT = Convert.ToString(row["CALL_RECV_DT"]),
                CALL_INSTALL_NO = Convert.ToString(row["CALL_INSTALL_NO"]),
                CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                DATE_TIME = Convert.ToString(row["DATE_TIME"]),
                CALL_STATUS = Convert.ToString(row["CALL_STATUS"]),
                CALL_LETTER_NO = Convert.ToString(row["CALL_LETTER_NO"]),
                REMARKS = Convert.ToString(row["REMARKS"]),
                PO_NO = Convert.ToString(row["PO_NO"]),
                PO_DT = Convert.ToString(row["PO_DT"]),
                VENDOR = Convert.ToString(row["VENDOR"]),
                IE_NAME = Convert.ToString(row["IE_NAME"])
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public List<SelectListItem> Get_Cluster_IE(string Region)
        {
            List<SelectListItem> IE = (from t99 in context.T99ClusterMasters
                                       join t101 in context.T101IeClusters on t99.ClusterCode equals t101.ClusterCode
                                       join t09 in context.T09Ies on t101.IeCode equals t09.IeCd
                                       where t99.RegionCode == Region && t09.IeStatus == null && t99.DepartmentName == "C"
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
                    //var cl_exist = (from a in context.T100VenderClusters
                    //                join b in context.T99ClusterMasters on Convert.ToByte(a.ClusterCode) equals Convert.ToByte(b.ClusterCode)
                    //                where a.VendorCode == 46040 && a.DepartmentName == "C" && b.RegionCode == "N"
                    //                select b.ClusterCode).Distinct().Count();

                    //var IE = (from ieCluster in context.T101IeClusters
                    //          where ieCluster.ClusterCode == Convert.ToByte(677) && ieCluster.DepartmentCode == "C"
                    //          select ieCluster.IeCode).FirstOrDefault();

                    OracleParameter[] par = new OracleParameter[6];
                    par[0] = new OracleParameter("P_VENDOR_CODE", OracleDbType.Varchar2, model.MFG_CD, ParameterDirection.Input);
                    par[1] = new OracleParameter("P_DEPT_NAME", OracleDbType.Varchar2, model.DEPT_DROPDOWN, ParameterDirection.Input);
                    par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, model.CASE_NO.Remove(1), ParameterDirection.Input);
                    par[3] = new OracleParameter("P_CLUSTER_CODE", OracleDbType.Varchar2, model.IE_NAME, ParameterDirection.Input);
                    par[4] = new OracleParameter("P_RESULT_COUNT", OracleDbType.RefCursor, ParameterDirection.Output);
                    par[5] = new OracleParameter("P_RESULT_IECODE", OracleDbType.RefCursor, ParameterDirection.Output);
                    var ds = DataAccessDB.GetDataSet("SP_GET_CALL_MARKED_ONLINE_SCALAR_VALUE", par, 2);
                    DataTable dt = ds.Tables[0];

                    int cl_exist = 0;

                    cl_exist = Convert.ToInt32(ds.Tables[0].Rows[0]["VENDOR_COUNT"]);
                    var IE = Convert.ToInt32(ds.Tables[1].Rows[0]["IE_CODE"]) == null ? 0 : Convert.ToInt32(ds.Tables[1].Rows[0]["IE_CODE"]);
                    var Co = (from ie in context.T09Ies
                              where ie.IeCd == 0
                              select ie.IeCoCd).FirstOrDefault();

                    var _data = context.T17CallRegisters
                                    .Where(x => x.CaseNo == model.CASE_NO && x.CallRecvDt == Convert.ToDateTime(model.CALL_RECV_DT) && x.CallSno == Convert.ToInt32(model.CALL_SNO))
                                    .Select(x => x);
                    foreach (var item in _data)
                    {
                        item.Remarks = model.REMARKS;
                        item.IeCd = IE;
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

                    if (ietopoicount == 0)
                    {
                        T60IePoiMapping insObj = new T60IePoiMapping();
                        insObj.IeCd = IE;
                        insObj.PoiCd = Convert.ToInt32(model.MFG_CD);
                        context.T60IePoiMappings.Add(insObj);
                        context.SaveChanges();
                    }
                    flag = true;
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    flag = false;
                    trans.Rollback();
                }
            }
            return flag;
        }

        public CaseHistoryModel Get_Vendor_Detail_By_CaseNo(string Case_No, string Region)
        {
            var data = new CaseHistoryModel();
            data = (from t13 in context.T13PoMasters
                    join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                    join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                    join t91 in context.T91Railways on t13.RlyCd equals t91.RlyCd into railways
                    from t91 in railways.DefaultIfEmpty()
                    where t13.CaseNo == "N19101278" && t13.RegionCode == "N"
                    select new CaseHistoryModel
                    {
                        CASE_NO = t13.CaseNo,
                        VEND_CD = Convert.ToString(t05.VendCd),
                        VENDOR = t05.VendName + "," + t03.City,
                        VEND_REMARKS = Convert.ToString(t05.VendRemarks),
                        PO_NO = t13.PoNo,
                        PO_DT = Convert.ToDateTime(t13.PoDt).ToString("dd/MM/yyyy"),
                        PO_SOURCE = t13.PoSource,
                        PO_YR = Convert.ToDateTime(t13.PoDt).Year.ToString(),
                        IMMS_RLY_CD = t91.ImmsRlyCd,
                        RLY_CD = t13.RlyCd,
                        REMARKS = t13.Remarks
                    }).FirstOrDefault();
            return data;
        }

        public DTResult<CaseHistoryItemModel> Get_Case_History_Item(DTParameters dtParameters, string Region)
        {
            DTResult<CaseHistoryItemModel> dTResult = new() { draw = 0 };
            IQueryable<CaseHistoryItemModel> query = null;
            var Case_NO = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CASE_NO"]))
            {
                Case_NO = Convert.ToString(dtParameters.AdditionalValues["CASE_NO"]);
            }
            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, Case_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_CASE_HISTORY_ITEM", par, 1);
            DataTable dt = ds.Tables[0];

            List<CaseHistoryItemModel> list = dt.AsEnumerable().Select(row => new CaseHistoryItemModel
            {
                CASE_NO = Convert.ToString(row["CASE_NO"]),
                ITEM_SRNO = Convert.ToString(row["ITEM_SRNO"]),
                ITEM_DESC = Convert.ToString(row["ITEM_DESC"]),
                QTY = Convert.ToInt32(row["QTY"]),
                DELV_DATE = Convert.ToString(row["DELV_DATE"]),
                PASSED = Convert.ToInt32(row["PASSED"]),
                BALANCE_QTY = (Convert.ToInt32(row["QTY"]) - Convert.ToInt32(row["PASSED"])),
                REJECTED = Convert.ToInt32(row["REJECTED"])
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, "ITEM_SRNO", true).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<CaseHistoryPoIREPSModel> Get_Case_History_PO_IREPS(DTParameters dtParameters)//, string PO_DT
        {
            string PO_NO = "", PO_DT = "";
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PO_NO"]))
            {
                PO_NO = Convert.ToString(dtParameters.AdditionalValues["PO_NO"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PO_DT"]))
            {
                PO_DT = Convert.ToString(dtParameters.AdditionalValues["PO_DT"]);
            }

            DTResult<CaseHistoryPoIREPSModel> dTResult = new() { draw = 0 };
            IQueryable<CaseHistoryPoIREPSModel> query = null;
            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_PO_NO", OracleDbType.Varchar2, PO_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_PO_DT", OracleDbType.Varchar2, PO_DT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_PO_IREPS", par, 1);
            DataTable dt = ds.Tables[0];

            List<CaseHistoryPoIREPSModel> list = dt.AsEnumerable().Select(row => new CaseHistoryPoIREPSModel
            {
                MAKEY = Convert.ToString(row["MAKEY"]),
                SLNO = Convert.ToString(row["SLNO"]),
                MAKEY_DT = Convert.ToString(row["MAKEY_DT"]),
                MA_FLD_DESCR = Convert.ToString(row["MA_FLD_DESCR"]),
                OLD_VALUE = Convert.ToString(row["OLD_VALUE"]),
                NEW_VALUE = Convert.ToString(row["NEW_VALUE"]),
                RITES_CASE_NO = Convert.ToString(row["RITES_CASE_NO"]),
                IMMS_RLY_CD = Convert.ToString(row["IMMS_RLY_CD"]),
                IMMS_POKEY = Convert.ToString(row["IMMS_POKEY"]),
                MA_NO = Convert.ToString(row["MA_NO"]),
                MA_DT = Convert.ToString(row["MA_DT"]),
                MA_STATUS = Convert.ToString(row["MA_STATUS"])
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, "MA_NO", true).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<CaseHistoryPoVendorModel> Get_Case_History_PO_Vendor(DTParameters dtParameters)//, string PO_DT
        {
            DTResult<CaseHistoryPoVendorModel> dTResult = new() { draw = 0 };
            IQueryable<CaseHistoryPoVendorModel> query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            string CASE_NO = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CASE_NO"]))
            {
                CASE_NO = Convert.ToString(dtParameters.AdditionalValues["CASE_NO"]);
            }

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

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, CASE_NO, ParameterDirection.Input);            
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_PO_VENDOR", par, 1);
            DataTable dt = ds.Tables[0];

            List<CaseHistoryPoVendorModel> list = dt.AsEnumerable().Select(row => new CaseHistoryPoVendorModel
            {
                CASE_NO = Convert.ToString(row["CASE_NO"]),
                MA_NO = Convert.ToString(row["MA_NO"]),
                MA_DT = Convert.ToString(row["MA_DT"]),
                MA_SNO = Convert.ToString(row["MA_SNO"]),
                MA_FIELD = Convert.ToString(row["MA_FIELD"]),
                MA_DESC = Convert.ToString(row["MA_DESC"]),
                OLD_PO_VALUE = Convert.ToString(row["OLD_PO_VALUE"]),
                NEW_PO_VALUE = Convert.ToString(row["NEW_PO_VALUE"]),
                MA_STATUS = Convert.ToString(row["MA_STATUS"])
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<CaseHistoryPreviousCallModel> Get_Case_History_Previous_Call(DTParameters dtParameters)
        {
            DTResult<CaseHistoryPreviousCallModel> dTResult = new() { draw = 0 };
            IQueryable<CaseHistoryPreviousCallModel> query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            string CASE_NO = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CASE_NO"]))
            {
                CASE_NO = Convert.ToString(dtParameters.AdditionalValues["CASE_NO"]);
            }

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CALL_DATE";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CALL_DATE";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_PREVIOUS_CALL", par, 1);
            DataTable dt = ds.Tables[0];

            List<CaseHistoryPreviousCallModel> list = dt.AsEnumerable().Select(row => new CaseHistoryPreviousCallModel
            {
                CALL_DATE = Convert.ToString(row["CALL_DATE"]),
                LETTER_DATE = Convert.ToString(row["LETTER_DATE"]),
                CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                CALL_INSTALL_NO = Convert.ToString(row["CALL_INSTALL_NO"]),
                IE_NAME = Convert.ToString(row["IE_NAME"]),
                CALL_STATUS = Convert.ToString(row["CALL_STATUS"]),
                REASON_REJECT = Convert.ToString(row["REASON_REJECT"]),
                REASON = Convert.ToString(row["REASON"])                
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<CaseHistoryConsigneeComplaintModel> Get_Case_History_Consignee_Complaints(DTParameters dtParameters)
        {
            DTResult<CaseHistoryConsigneeComplaintModel> dTResult = new() { draw = 0 };
            IQueryable<CaseHistoryConsigneeComplaintModel> query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            string VEND_CD = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["VEND_CD"]))
            {
                VEND_CD = Convert.ToString(dtParameters.AdditionalValues["VEND_CD"]);
            }

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "REJ_MEMO_DATE";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "REJ_MEMO_DATE";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_VEND_CD", OracleDbType.Varchar2, VEND_CD, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_CASE_HISTORY_CONSIGNEE_COMPLAINTS", par, 1);
            DataTable dt = ds.Tables[0];

            List<CaseHistoryConsigneeComplaintModel> list = dt.AsEnumerable().Select(row => new CaseHistoryConsigneeComplaintModel
            {
                ITEM_DESC = Convert.ToString(row["ITEM_DESC"]),
                REJ_MEMO_DATE = Convert.ToString(row["REJ_MEMO_DATE"]),
                REJECTION_REASON = Convert.ToString(row["REJECTION_REASON"]),
                BK_NO = Convert.ToString(row["BK_NO"]),
                SET_NO = Convert.ToString(row["SET_NO"]),
                CONSIGNEE = Convert.ToString(row["CONSIGNEE"]),
                JI_STATUS_DESC = Convert.ToString(row["JI_STATUS_DESC"])                
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<CaseHistoryRejectionVendorPlaceModel> Get_Case_History_Rejection_Vendor_Place(DTParameters dtParameters, string region)
        {
            DTResult<CaseHistoryRejectionVendorPlaceModel> dTResult = new() { draw = 0 };
            IQueryable<CaseHistoryRejectionVendorPlaceModel> query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            string CASE_NO= "",VEND_CD = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CASE_NO"]))
            {
                CASE_NO = Convert.ToString(dtParameters.AdditionalValues["CASE_NO"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["VEND_CD"]))
            {
                VEND_CD = Convert.ToString(dtParameters.AdditionalValues["VEND_CD"]);
            }

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "BILL_NO";
                }
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "BILL_NO";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_VEND_CD", OracleDbType.Varchar2, VEND_CD, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, region, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_CASE_HISTORY_REJECTIONS_VENDOR_PLACE", par, 1);
            DataTable dt = ds.Tables[0];

            List<CaseHistoryRejectionVendorPlaceModel> list = dt.AsEnumerable().Select(row => new CaseHistoryRejectionVendorPlaceModel
            {
                BILL_NO = Convert.ToString(row["BILL_NO"]),
                IC_DATE = Convert.ToString(row["IC_DATE"]),
                BK_NO = Convert.ToString(row["BK_NO"]),
                SET_NO = Convert.ToString(row["SET_NO"]),
                REASON_REJECT = Convert.ToString(row["REASON_REJECT"]),
                IE_NAME = Convert.ToString(row["IE_NAME"]),
                VENDOR = Convert.ToString(row["VENDOR"]),
                ITEM_DESC_PO = Convert.ToString(row["ITEM_DESC_PO"])
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }
    }
}
