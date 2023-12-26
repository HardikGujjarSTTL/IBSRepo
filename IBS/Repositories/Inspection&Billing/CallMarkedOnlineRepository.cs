using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Inspection_Billing;
using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories.Inspection_Billing
{
    public class CallMarkedOnlineRepository : ICallMarkedOnlineRepository
    {
        private readonly ModelContext context;
        private readonly IConfiguration configuration;
        private readonly ISendMailRepository pSendMailRepository;
        public CallMarkedOnlineRepository(ModelContext context, IConfiguration configuration, ISendMailRepository pSendMailRepository)
        {
            this.context = context;
            this.configuration = configuration;
            this.pSendMailRepository = pSendMailRepository;
        }
        public DTResult<CallMarkedOnlineModel> Get_Call_Marked_Online(DTParameters dtParameters, string Region)
        {
            DTResult<CallMarkedOnlineModel> dTResult = new() { draw = 0 };
            IQueryable<CallMarkedOnlineModel> query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
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

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CASE_NO";
                    if (RDB2 == 1)
                    {
                        orderCriteria = "CALL_RECV_DT";
                    }
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CASE_NO";
                orderAscendingDirection = true;
            }

            Date = Date.ToString() == "" ? string.Empty : Date.ToString();

            DateTime p_date = Convert.ToDateTime(Date);

            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("P_DATE", OracleDbType.Date, p_date, ParameterDirection.Input);
            par[1] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RDB1", OracleDbType.Int16, RDB1, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RDB2", OracleDbType.Int16, RDB2, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RDB3", OracleDbType.Int16, RDB3, ParameterDirection.Input);
            par[5] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_CALL_MARKED_ONLINE_NEW", par, 1);
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

        public List<SelectListItem> Get_Cluster_IE(string Region, string DeptName)
        {
            List<SelectListItem> IE = (from t99 in context.T99ClusterMasters
                                       join t101 in context.T101IeClusters on t99.ClusterCode equals t101.ClusterCode
                                       join t09 in context.T09Ies on t101.IeCode equals t09.IeCd
                                       where t99.RegionCode == Region && t09.IeStatus == null && t99.DepartmentName == DeptName
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
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, obj.CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_DATE", OracleDbType.Varchar2, obj.Date, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CALL_SNO", OracleDbType.Varchar2, obj.CALL_SNO, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            par[4] = new OracleParameter("P_PREV_CALL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_CALL_MARKED_ONLINE_DETAIL", par, 2);
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

            if (ds.Tables[1].Rows.Count > 0)
            {
                model.PREV_CALL_1 = (Convert.ToString(ds.Tables[1].Rows[0]["CALL"]) != null && Convert.ToString(ds.Tables[1].Rows[0]["CALL"]) != "") ? Convert.ToString(ds.Tables[1].Rows[0]["CALL"]) : null;
                model.PREV_CALL_2 = (Convert.ToString(ds.Tables[1].Rows[1]["CALL"]) != null && Convert.ToString(ds.Tables[1].Rows[1]["CALL"]) != "") ? Convert.ToString(ds.Tables[1].Rows[1]["CALL"]) : null;
            }

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

        #region This method use for  Send Mail for Reject Call functionality. Note: But Now using Linq Query
        //public VendorDetailModel Get_Vendor_For_Send_Mail(CallMarkedOnlineFilter obj)
        //{
        //    var model = new VendorDetailModel();
        //    OracleParameter[] par = new OracleParameter[5];
        //    par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, obj.CASE_NO, ParameterDirection.Input);
        //    par[1] = new OracleParameter("P_DATE", OracleDbType.Varchar2, obj.Date, ParameterDirection.Input);
        //    par[2] = new OracleParameter("P_CALL_SNO", OracleDbType.Varchar2, obj.CALL_SNO, ParameterDirection.Input);
        //    par[3] = new OracleParameter("P_VENDOR_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
        //    par[4] = new OracleParameter("P_VENDOR_MFG_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
        //    var ds = DataAccessDB.GetDataSet("SP_GET_VENDOR_FOR_SEND_MAIL", par, 2);
        //    DataTable dt = ds.Tables[0];
        //    DataTable dt1 = ds.Tables[1];

        //    var vendorData = dt.AsEnumerable().Select(row => new VendorDetailModel
        //    {
        //        VEND_CD = Convert.ToString(row["VEND_CD"]),
        //        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
        //        VEND_ADDRESS = Convert.ToString(row["VEND_ADDRESS"]),
        //        VEND_EMAIL = Convert.ToString(row["VEND_EMAIL"]),
        //    }).FirstOrDefault();

        //    var mfgData = dt1.AsEnumerable().Select(row => new VendorDetailModel
        //    {
        //        MANU_MAIL = Convert.ToString(row["VEND_EMAIL"]),
        //        MFG_CD = Convert.ToString(row["MFG_CD"])
        //    }).FirstOrDefault();

        //    model.VEND_CD = vendorData.VEND_CD;
        //    model.VEND_NAME = vendorData.VEND_NAME;
        //    model.VEND_ADDRESS = vendorData.VEND_ADDRESS;
        //    model.VEND_EMAIL = vendorData.VEND_EMAIL;

        //    model.MANU_MAIL = mfgData.MANU_MAIL;
        //    model.MFG_CD = mfgData.MFG_CD;

        //    return model;
        //}
        #endregion

        public bool Call_Rejected(CallMarkedOnlineFilter obj, UserSessionModel model)
        {
            var flag = false;
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    //string dt = Convert.ToDateTime(obj.Date).ToString("dd-MM-yy");
                    var callRecvDt = DateTime.ParseExact(obj.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var _data = context.T18CallDetails
                                .Where(x => x.CaseNo == obj.CASE_NO && x.CallRecvDt == callRecvDt && x.CallSno == Convert.ToInt32(obj.CALL_SNO))
                                .Select(x => x).FirstOrDefault();

                    if (_data != null)
                    {
                        _data.Isdeleted = 1;
                        _data.Updatedby = Convert.ToString(model.UserID);
                        _data.Updateddate = DateTime.Now;
                        context.SaveChanges();
                        flag = true;
                    }

                    var _callReg = context.T17CallRegisters
                                    .Where(x => x.CaseNo == obj.CASE_NO && x.CallRecvDt == callRecvDt && x.CallSno == Convert.ToInt32(obj.CALL_SNO))
                                    .Select(x => x).FirstOrDefault();
                    if (_callReg != null)
                    {
                        _callReg.Isdeleted = Convert.ToByte(1);
                        _callReg.Updatedby = Convert.ToString(model.UserID);
                        _callReg.Updateddate = DateTime.Now;
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
                    #region Comment
                    //var cl_exist = (from a in context.T100VenderClusters
                    //                join b in context.T99ClusterMasters on Convert.ToByte(a.ClusterCode) equals Convert.ToByte(b.ClusterCode)
                    //                where a.VendorCode == 46040 && a.DepartmentName == "C" && b.RegionCode == "N"
                    //                select b.ClusterCode).Distinct().Count();

                    //var IE = (from ieCluster in context.T101IeClusters
                    //          where ieCluster.ClusterCode == Convert.ToByte(677) && ieCluster.DepartmentCode == "C"
                    //          select ieCluster.IeCode).FirstOrDefault();

                    #endregion

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
                    var IE = Convert.ToInt32(ds.Tables[1].Rows.Count) == 0 ? 0 : Convert.ToInt32(ds.Tables[1].Rows[0]["IE_CODE"]);
                    model.IE_CD = IE;
                    var Co = (from ie in context.T09Ies
                              where ie.IeCd == IE
                              select ie.IeCoCd).FirstOrDefault();

                    if (Co == null) { return flag; }

                    var callRecvDt = DateTime.ParseExact(model.CALL_RECV_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var _data = context.T17CallRegisters
                                .Where(x => x.CaseNo == model.CASE_NO && x.CallRecvDt == callRecvDt && x.CallSno == Convert.ToInt32(model.CALL_SNO))
                                .Select(x => x).FirstOrDefault();

                    if (_data != null)
                    {
                        _data.Remarks = model.REMARKS;
                        _data.IeCd = IE;
                        _data.ClusterCode = Convert.ToInt32(model.IE_NAME);
                        _data.CoCd = Convert.ToInt32(Co.Value);
                        _data.UserId = uModel.UserName.Substring(0, 8);
                        _data.DepartmentCode = model.DEPT_DROPDOWN;
                        _data.Datetime = DateTime.Now;
                        _data.Updatedby = Convert.ToString(uModel.UserID);
                        _data.Updateddate = DateTime.Now;
                        context.SaveChanges();
                    }

                    if (cl_exist == 0)
                    {
                        var T100 = context.T100VenderClusters.Where(x => x.VendorCode == Convert.ToInt32(model.MFG_CD)).FirstOrDefault();
                        if (T100 == null)
                        {
                            T100VenderCluster insObj = new T100VenderCluster();
                            insObj.VendorCode = Convert.ToInt32(model.MFG_CD);
                            insObj.DepartmentName = model.DEPT_DROPDOWN;
                            insObj.ClusterCode = Convert.ToInt32(model.IE_NAME);
                            insObj.UserId = uModel.UserName.Substring(0, 8);
                            insObj.Datetime = DateTime.Now;
                            insObj.Createdby = uModel.UserID;
                            insObj.Createddate = DateTime.Now;
                            context.T100VenderClusters.Add(insObj);
                            context.SaveChanges();
                        }
                        else
                        {
                            T100.DepartmentName = model.DEPT_DROPDOWN;
                            T100.ClusterCode = Convert.ToInt32(model.IE_NAME);
                            T100.UserId = uModel.UserName.Substring(0, 8);
                            T100.Datetime = DateTime.Now;
                            T100.Updatedby = uModel.UserID;
                            T100.Updateddate = DateTime.Now;
                            context.SaveChanges();
                        }

                    }
                    var ietopoicount = (from mapping in context.T60IePoiMappings
                                        where mapping.IeCd == model.IE_CD && mapping.PoiCd == Convert.ToInt32(model.MFG_CD)
                                        select mapping).Count();

                    if (ietopoicount == 0)
                    {
                        T60IePoiMapping insObj = new T60IePoiMapping();
                        insObj.IeCd = IE;
                        insObj.PoiCd = Convert.ToInt32(model.MFG_CD);
                        insObj.Createdby = uModel.UserID;
                        insObj.Createddate = DateTime.Now;
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
                    where t13.CaseNo == Case_No && t13.RegionCode == Region
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

        public List<CaseHistoryItemModel> Get_Case_History_Item(DTParameters dtParameters, string Case_NO, string Region)
        {
            DTResult<CaseHistoryItemModel> dTResult = new() { draw = 0 };
            //var Case_NO = "";
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CASE_NO"]))
            //{
            //    Case_NO = Convert.ToString(dtParameters.AdditionalValues["CASE_NO"]);
            //}
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
            return list;
            //query = list.AsQueryable();

            //dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            //dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            //dTResult.data = DbContextHelper.OrderByDynamic(query, "ITEM_SRNO", true).Select(p => p).ToList();
            //dTResult.draw = dtParameters.Draw;
            //return dTResult;
        }

        public List<CaseHistoryPoIREPSModel> Get_Case_History_PO_IREPS(DTParameters dtParameters, string PO_NO, string PO_DT)//, string PO_DT
        {
            //string PO_NO = "", PO_DT = "";
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;

            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PO_NO"]))
            //{
            //    PO_NO = Convert.ToString(dtParameters.AdditionalValues["PO_NO"]);
            //}
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PO_DT"]))
            //{
            //    PO_DT = Convert.ToString(dtParameters.AdditionalValues["PO_DT"]);
            //}

            DTResult<CaseHistoryPoIREPSModel> dTResult = new() { draw = 0 };
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
            return list;
            //query = list.AsQueryable();

            //dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            //dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            //dTResult.data = DbContextHelper.OrderByDynamic(query, "MA_NO", true).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.draw = dtParameters.Draw;
            //return dTResult;
        }

        public List<CaseHistoryPoVendorModel> Get_Case_History_PO_Vendor(DTParameters dtParameters, string CASE_NO)//, string PO_DT
        {
            DTResult<CaseHistoryPoVendorModel> dTResult = new() { draw = 0 };

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            //string CASE_NO = "";
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CASE_NO"]))
            //{
            //    CASE_NO = Convert.ToString(dtParameters.AdditionalValues["CASE_NO"]);
            //}

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
            return list;
            //query = list.AsQueryable();

            //dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            //dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.draw = dtParameters.Draw;
            //return dTResult;
        }

        public List<CaseHistoryPreviousCallModel> Get_Case_History_Previous_Call(DTParameters dtParameters, string CASE_NO)
        {
            DTResult<CaseHistoryPreviousCallModel> dTResult = new() { draw = 0 };

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            //string CASE_NO = "";
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CASE_NO"]))
            //{
            //    CASE_NO = Convert.ToString(dtParameters.AdditionalValues["CASE_NO"]);
            //}

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
            return list;
            //query = list.AsQueryable();

            //dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            //dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.draw = dtParameters.Draw;
            //return dTResult;
        }

        public List<CaseHistoryConsigneeComplaintModel> Get_Case_History_Consignee_Complaints(DTParameters dtParameters, string VEND_CD)
        {
            DTResult<CaseHistoryConsigneeComplaintModel> dTResult = new() { draw = 0 };

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            //string VEND_CD = "";
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["VEND_CD"]))
            //{
            //    VEND_CD = Convert.ToString(dtParameters.AdditionalValues["VEND_CD"]);
            //}

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
            return list;
            //query = list.AsQueryable();

            //dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            //dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.draw = dtParameters.Draw;
            //return dTResult;
        }

        public List<CaseHistoryRejectionVendorPlaceModel> Get_Case_History_Rejection_Vendor_Place(DTParameters dtParameters, string CASE_NO, string VEND_CD, string region)
        {
            DTResult<CaseHistoryRejectionVendorPlaceModel> dTResult = new() { draw = 0 };

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            //string CASE_NO = "", VEND_CD = "";
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CASE_NO"]))
            //{
            //    CASE_NO = Convert.ToString(dtParameters.AdditionalValues["CASE_NO"]);
            //}
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["VEND_CD"]))
            //{
            //    VEND_CD = Convert.ToString(dtParameters.AdditionalValues["VEND_CD"]);
            //}

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

            return list;
            //query = list.AsQueryable();

            //dTResult.recordsTotal = ds.Tables[0].Rows.Count; //query.ToList().Count(); //ds.Tables[0].Rows.Count;
            //dTResult.recordsFiltered = ds.Tables[0].Rows.Count; //query.ToList().Count();  //ds.Tables[0].Rows.Count;
            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.draw = dtParameters.Draw;
            //return dTResult;
        }

        public string Send_Vendor_Mail_For_Rejected_Call(CallMarkedOnlineModel obj, string Region)
        {
            string email = "";
            string wRegion = "";
            string sender = "";
            wRegion = GetRegionInfo(Region);
            sender = GetSenderMail(Region);
            var query = (from t13 in context.T13PoMasters
                         join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                         join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                         where t13.CaseNo == obj.CASE_NO
                         select new
                         {
                             VEND_CD = t13.VendCd,
                             VEND_NAME = t05.VendName,
                             VEND_ADDRESS = t05.VendAdd2 != null
                                             ? t05.VendAdd1 + "/" + t05.VendAdd2
                                             : t05.VendAdd1 + "/" + t03.City,
                             VEND_EMAIL = t05.VendEmail
                         }).FirstOrDefault();

            string vend_cd = "", vend_add = "", vend_email = "";
            if (query != null)
            {
                vend_cd = Convert.ToString(query.VEND_CD);
                vend_add = query.VEND_ADDRESS;
                vend_email = query.VEND_EMAIL;
            }

            var caseNo = obj.CASE_NO; //txtCaseNo.Text.Trim();
            var callSno = obj.CALL_SNO; // Assuming "123" is an integer value
            var callRecvDt = DateTime.ParseExact(obj.CALL_RECV_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var query1 = (from t05 in context.T05Vendors
                          join t17 in context.T17CallRegisters on t05.VendCd equals t17.MfgCd
                          where t17.CaseNo == caseNo
                             && t17.CallRecvDt == callRecvDt
                             && t17.CallSno == Convert.ToInt32(callSno)
                          select new
                          {
                              VEND_EMAIL = t05.VendEmail,
                              MFG_CD = t17.MfgCd
                          }).FirstOrDefault();

            string manu_mail = "", mfg_cd = "";
            if (query1 != null)
            {
                manu_mail = query1.VEND_EMAIL;
                mfg_cd = Convert.ToString(query1.MFG_CD);
            }

            //var model = new CallMarkedOnlineFilter();
            //model.CASE_NO = obj.CASE_NO;
            //model.Date = obj.CALL_RECV_DT;
            //model.CALL_SNO = obj.CALL_SNO;
            //var data = Get_Vendor_For_Send_Mail(model);
            //if (data != null)
            //{
            //    vend_cd = data.VEND_CD;
            //    vend_add = data.VEND_ADDRESS;
            //    vend_email = data.VEND_EMAIL;
            //}
            //if (!string.IsNullOrEmpty(data.MANU_MAIL) || !string.IsNullOrEmpty(data.MFG_CD))
            //{
            //    manu_mail = data.MANU_MAIL;
            //    mfg_cd = data.MFG_CD;
            //}

            string call_letter_dt = "";
            if (obj.LETTER_DT == "")
            {
                call_letter_dt = "NIL";
            }
            else
            {
                call_letter_dt = obj.LETTER_DT;
            }

            string mail_body = "Dear Sir/Madam,\n\n Call Letter dated:  " + obj.LETTER_DT + " for inspection of material against PO No. - " + obj.PO_NO + " dated - " + obj.PO_DT + ", Case No -  " + obj.CASE_NO + ", on date: " + obj.CALL_RECV_DT + ", at SNo. " + obj.CALL_SNO + ". The Call is rejected due to following Reason:- " + obj.REJECT_REASON + ", so not marked and deleted. Please Resubmit the call after making necessary corrections. \n\n Thanks for using RITES Inspection Services. \n\n" + wRegion + ".";
            mail_body = mail_body + "\n\n THIS IS AN AUTO GENERATED EMAIL. PLEASE DO NOT REPLY. USE EMAIL GIVEN IN THE REGION ADDRESS";

            string BCC = "nrinspn@gmail.com";
            //// sender for local mail testing
            //sender = "hardiksilvertouch007@outlook.com";
            //vend_email = "naimish.rana@silvertouch.com";
            //manu_mail = "neha.gehlot@silvertouch.com";
            if (vend_cd == mfg_cd && manu_mail != "")
            {
                SendMailModel sendMailModel = new SendMailModel();
                sendMailModel.From = sender;
                sendMailModel.To = manu_mail;
                sendMailModel.Bcc = BCC;
                sendMailModel.Subject = "Your Call for Inspection By RITES";
                sendMailModel.Message = mail_body;
                bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
                email = isSend == true ? "Success" : "Error";
            }
            else if (vend_cd != mfg_cd)
            {
                if (vend_email == "")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    sendMailModel.From = sender;
                    sendMailModel.To = manu_mail;
                    sendMailModel.Bcc = BCC;
                    sendMailModel.Subject = "Your Call for Inspection By RITES";
                    sendMailModel.Message = mail_body;
                    bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
                    email = isSend == true ? "Success" : "Error";
                }
                else if (manu_mail == "")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    sendMailModel.From = sender;
                    sendMailModel.To = vend_email;
                    sendMailModel.Bcc = BCC;
                    sendMailModel.Subject = "Test"; //"Your Call for Inspection By RITES"
                    sendMailModel.Message = mail_body;
                    bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
                    email = isSend == true ? "Success" : "Error";
                }
                else
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    sendMailModel.From = sender;
                    sendMailModel.To = vend_email + "," + manu_mail;
                    sendMailModel.Bcc = BCC;
                    sendMailModel.Subject = "Your Call for Inspection By RITES";
                    sendMailModel.Message = mail_body;
                    bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
                    email = isSend == true ? "Success" : "Error";
                }
            }
            return email;
        }

        public string Send_Vendor_Email_For_Incomplete_Call_Details(CallMarkedOnlineModel obj, string Region)
        {
            string email = "";
            string wRegion = "";
            string sender = "";
            wRegion = GetRegionInfo(Region);
            sender = GetSenderMail(Region);
            var query = (from t13 in context.T13PoMasters
                         join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                         join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                         where t13.CaseNo == obj.CASE_NO
                         select new
                         {
                             VEND_CD = t13.VendCd,
                             VEND_NAME = t05.VendName,
                             VEND_ADDRESS = t05.VendAdd2 != null ? t05.VendAdd1 + "/" + t05.VendAdd2 : t05.VendAdd1 + "/" + t03.City,
                             VEND_EMAIL = t05.VendEmail
                         }).FirstOrDefault();

            string vend_cd = "", vend_add = "", vend_email = "";
            if (query != null)
            {
                vend_cd = Convert.ToString(query.VEND_CD);
                vend_add = query.VEND_ADDRESS;
                vend_email = query.VEND_EMAIL;
            }


            var caseNo = obj.CASE_NO;
            var callSno = obj.CALL_SNO;
            var callRecvDt = DateTime.ParseExact(obj.CALL_RECV_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var query1 = (from t05 in context.T05Vendors
                          join t17 in context.T17CallRegisters on t05.VendCd equals t17.MfgCd
                          where t17.CaseNo == caseNo &&
                                t17.CallRecvDt == callRecvDt &&
                                t17.CallSno == Convert.ToInt32(callSno)
                          select new
                          {
                              VEND_EMAIL = t05.VendEmail,
                              MFG_CD = t17.MfgCd
                          }).FirstOrDefault();
            string manu_mail = "", mfg_cd = "";
            if (query1 != null)
            {
                manu_mail = query1.VEND_EMAIL;
                mfg_cd = Convert.ToString(query1.MFG_CD);
            }

            string call_letter_dt = "";
            if (obj.LETTER_DT == "")
            {
                call_letter_dt = "NIL";
            }
            else
            {
                call_letter_dt = obj.LETTER_DT;
            }

            string mail_body = "Dear Sir/Madam,\n\n Call Letter dated:  " + call_letter_dt + " for inspection of material against PO No. - " + obj.PO_NO + " dated - " + obj.PO_DT + ", Case No -  " + obj.CASE_NO + ", on date: " + obj.CALL_RECV_DT + ", at SNo. " + obj.CALL_SNO + ". The Call submitted with incomplete details, so not marked and deleted.\n\nPlease re-submit with complete details.\n\n Thanks for using RITES Inspection Services. \n\n" + wRegion + ".";

            string BCC = "nrinspn@gmail.com";
            //// sender for local mail testing
            //sender = "hardiksilvertouch007@outlook.com";
            //vend_email = "naimish.rana@silvertouch.com";
            //manu_mail = "neha.gehlot@silvertouch.com";
            if (vend_cd == mfg_cd && manu_mail != "")
            {
                SendMailModel sendMailModel = new SendMailModel();
                sendMailModel.From = sender;
                sendMailModel.To = manu_mail;
                sendMailModel.Bcc = BCC;
                sendMailModel.Subject = "Your Call for Inspection By RITES";
                sendMailModel.Message = mail_body;
                bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
                email = isSend == true ? "Success" : "Error";
            }
            else if (vend_cd != mfg_cd)
            {
                if (vend_email != "")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    sendMailModel.From = sender;
                    sendMailModel.To = vend_email;
                    sendMailModel.Bcc = BCC;
                    sendMailModel.Subject = "Your Call for Inspection By RITES";
                    sendMailModel.Message = mail_body;
                    bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
                    email = isSend == true ? "Success" : "Error";
                }
                else if (manu_mail != "")
                {
                    SendMailModel sendMailModel = new SendMailModel();
                    sendMailModel.From = sender;
                    sendMailModel.To = manu_mail;
                    sendMailModel.Bcc = BCC;
                    sendMailModel.Subject = "Your Call for Inspection By RITES";
                    sendMailModel.Message = mail_body;
                    bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
                    email = isSend == true ? "Success" : "Error";
                }
            }
            return email;
        }

        public int Delete_Incomplete_Call(CallMarkedOnlineFilter obj, UserSessionModel model)
        {
            //string msg = "Error";
            int res = 0;
            var caseNo = obj.CASE_NO.Trim();
            var callRecvDate = DateTime.ParseExact(obj.Date.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var callSno = 123;

            var detailstatus = context.T18CallDetails
                        .Where(t => t.CaseNo == caseNo && t.CallRecvDt == callRecvDate && t.CallSno == callSno)
                        .Count();

            // Handle the case where there might be no matching records by using the null coalescing operator.
            var result = detailstatus != null ? detailstatus : 0;

            var callmarkstatus = context.T17CallRegisters
                        .Where(call =>
                            (call.IeCd == 0 || call.IeCd == null) &&
                            (call.CallStatus == "M" || call.CallStatus == null) &&
                            call.CaseNo == caseNo && call.CallRecvDt == callRecvDate && call.CallSno == callSno
                        ).Count();

            if (detailstatus == 0 && callmarkstatus == 1)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //string dt = Convert.ToDateTime(obj.Date).ToString("dd-MM-yy");
                        var callRecvDt = DateTime.ParseExact(obj.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var _data = context.T18CallDetails
                                    .Where(x => x.CaseNo == obj.CASE_NO && x.CallRecvDt == callRecvDt && x.CallSno == Convert.ToInt32(obj.CALL_SNO))
                                    .Select(x => x).FirstOrDefault();

                        if (_data != null)
                        {
                            _data.Isdeleted = 1;
                            _data.Updatedby = Convert.ToString(model.UserID);
                            _data.Updateddate = DateTime.Now;
                            context.SaveChanges();
                        }

                        var _callReg = context.T17CallRegisters
                                        .Where(x => x.CaseNo == obj.CASE_NO && x.CallRecvDt == callRecvDt && x.CallSno == Convert.ToInt32(obj.CALL_SNO))
                                        .Select(x => x).FirstOrDefault();
                        if (_callReg != null)
                        {
                            _callReg.Isdeleted = Convert.ToByte(1);
                            _callReg.Updatedby = Convert.ToString(model.UserID);
                            _callReg.Updateddate = DateTime.Now;
                            context.SaveChanges();
                        }
                        res = 1;
                        //msg = "This Call Has Been Deleted!!!";
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        res = 0;
                        //msg = "Error";
                        transaction.Rollback();
                    }
                }
            }
            else
            {
                res = 2;
                //msg = "This Call cannot be Deleted,Whether Item is their in the Call or The Call is Marked to IE or The Call Status is Other then Pending!!!";
            }
            return res;
        }

        public string Send_Vendor_Email(CallMarkedOnlineModel obj, string Region)
        {
            //using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            string email = "";
            string wRegion = "";
            string sender = "";
            wRegion = GetRegionInfo(Region);
            sender = GetSenderMail(Region);

            var query = (from t13 in context.T13PoMasters
                         join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                         join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                         where t13.CaseNo == obj.CASE_NO
                         select new
                         {
                             VEND_CD = t13.VendCd,
                             VEND_NAME = t05.VendName,
                             VEND_ADDRESS = !string.IsNullOrEmpty(t05.VendAdd2) ? t05.VendAdd1 + "/" + t05.VendAdd2 : t05.VendAdd1 + "/" + t03.City,
                             VEND_EMAIL = t05.VendEmail
                         }).FirstOrDefault();

            string vend_cd = "", vend_add = "", vend_email = "";
            if (query != null)
            {
                vend_cd = Convert.ToString(query.VEND_CD);
                vend_add = query.VEND_ADDRESS;
                vend_email = query.VEND_EMAIL;
            }

            var caseNo = obj.CASE_NO;
            var callSno = obj.CALL_SNO;
            var callRecvDt = DateTime.ParseExact(obj.CALL_RECV_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var query1 = (from t05 in context.T05Vendors
                          join t17 in context.T17CallRegisters on t05.VendCd equals t17.MfgCd
                          where t17.CaseNo == caseNo
                             && t17.CallRecvDt == callRecvDt
                             && t17.CallSno == Convert.ToInt32(callSno)
                          select new
                          {
                              VEND_EMAIL = t05.VendEmail,
                              MFG_CD = t17.MfgCd,
                              DESIRE_DT = Convert.ToDateTime(t17.DtInspDesire).ToString("dd/MM/yyyy")
                          }).FirstOrDefault();
            string manu_mail = "", mfg_cd = "", desire_dt = "";
            if (query1 != null)
            {
                manu_mail = query1.VEND_EMAIL;
                mfg_cd = Convert.ToString(query1.MFG_CD);
                desire_dt = query1.DESIRE_DT;
            }

            var query2 = (from t09 in context.T09Ies
                          join t08 in context.T08IeControllOfficers on t09.IeCoCd equals t08.CoCd
                          where t09.IeCd == obj.IE_CD
                          select new
                          {
                              IE_PHONE_NO = t09.IePhoneNo,
                              CO_NAME = t08.CoName,
                              CO_PHONE_NO = t08.CoPhoneNo,
                              IE_NAME = t09.IeName,
                              IE_EMAIL = t09.IeEmail
                          }).FirstOrDefault();
            string ie_phone = "", co_name = "", co_mobile = "", ie_name = "", ie_email = "";
            if (query2 != null)
            {
                ie_phone = query2.IE_PHONE_NO;
                co_name = query2.CO_NAME;
                co_mobile = query2.CO_PHONE_NO;
                ie_name = query2.IE_NAME;
                ie_email = query2.IE_EMAIL;
            }

            //var caseNo = txtCaseNo.Text.Trim();
            //var dtOfReceipt = DateTime.ParseExact(txtDtOfReceipt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //var ieCd = lblIE_CD.Text;
            string dateto_attend = "";
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == System.Data.ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "SELECT TO_CHAR(DT_INSP_DESIRE + (SELECT ROUND(COUNT(*) / 1.5) DAYS FROM T17_CALL_REGISTER WHERE(CALL_RECV_DT > '01-APR-2017') AND CALL_STATUS IN('M', 'S') AND IE_CD = " + obj.IE_CD + "),'DD/MM/YYYY') INSP_DATE FROM T17_CALL_REGISTER WHERE CASE_NO = '" + obj.CASE_NO.Trim() + "' and CALL_RECV_DT = to_date('" + obj.CALL_RECV_DT + "', 'dd/mm/yyyy') and CALL_SNO =" + obj.CALL_SNO;
                    dateto_attend = Convert.ToString(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }

            var callRecvDt1 = DateTime.ParseExact(obj.CALL_RECV_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var _data = (from t17 in context.T17CallRegisters
                         where t17.CaseNo == caseNo
                             && t17.CallRecvDt == callRecvDt
                             && t17.CallSno == Convert.ToInt32(callSno)
                         select t17).FirstOrDefault();
            //Where(x => x.CaseNo != caseNo && x.CallRecvDt == callRecvDt1 && x.CallSno == Convert.ToInt32(callSno))
            //Select(x => x).FirstOrDefault();

            _data.ExpInspDt = Convert.ToDateTime(dateto_attend);
            context.SaveChanges();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, caseNo, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_CALL_DETAIL_DAYSIC_ITEMCD", par, 1);
            DataTable dt = ds.Tables[0];

            int days_to_ic = 0;
            string item_cd = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                days_to_ic = Convert.ToInt32(ds.Tables[0].Rows[0]["DAYS_TO_IC"]);
                item_cd = Convert.ToString(ds.Tables[0].Rows[0]["ITEM_CD"]);
            }

            string call_letter_dt = "";
            if (obj.LETTER_DT == "")
            {
                call_letter_dt = "NIL";
            }
            else
            {
                call_letter_dt = obj.LETTER_DT;
            }


            string mail_body = "Dear Sir/Madam,<br><br> In Reference to your Call Letter dated:  " + call_letter_dt + " for inspection of material against PO No. - " + obj.PO_NO + " & date - " + obj.PO_DT + ", Call has been registered vide Case No -  " + obj.CASE_NO + ", on date: " + obj.CALL_RECV_DT + ", at SNo. " + obj.CALL_SNO + ".<br> ";
            if (obj.CALL_RECV_DT.Trim() != desire_dt.Trim())
            {
                mail_body = mail_body + "The Desired Inspection Date of this call shall be on or after: " + desire_dt.Trim() + ".<br>";
            }

            if (days_to_ic == 0)
            {
                mail_body = mail_body + "The inspection call has been assigned to Inspecting Engineer Sh. " + ie_name + ", Contact No. " + ie_phone + ", Email ID: " + ie_email + ". Based on the current workload with the IE, Inspection is likely to be attended on or before " + dateto_attend + " or next working day (In case the above date happens to be a holiday). Dates are subject to last minute changes due to  exigencies of work and overriding Client priorities. <br> Name of Controlling Manager of concerned IE Sh.: " + co_name + ", Contact No." + co_mobile + ". <br>Offered Material as per registration should be readily available on the indicated date along with all related documents and internal test reports.<br><a href='http://rites.ritesinsp.com/RBS/Guidelines for Vendors.pdf'>Guidelines for Vendors</a>.<br>For Inspection related information please visit : http://ritesinsp.com. <br> For any correspondence in future, please quote Case No. only. <br><br>Thanks for using RITES Inspection Services. <br><br>" + wRegion + ".";
            }
            else if (days_to_ic > 0)
            {
                System.DateTime w_dt1 = new System.DateTime(Convert.ToInt32(dateto_attend.Substring(6, 4)), Convert.ToInt32(dateto_attend.Substring(3, 2)), Convert.ToInt32(dateto_attend.Substring(0, 2)));
                System.DateTime w_dt2 = w_dt1.AddDays(days_to_ic);
                string date_to_ic = w_dt2.ToString("dd/MM/yyyy");
                mail_body = mail_body + "The inspection call has been assigned to Inspecting Engineer Sh. " + ie_name + ", Contact No. " + ie_phone + ", Email ID: " + ie_email + ". Based on the current workload with the IE, Inspection is likely to be attended on or before " + dateto_attend + " or next working day (In case the above date happens to be a holiday) and Inspection certificate is likely to issued by " + date_to_ic + ". Dates are subject to last minute changes due to  exigencies of work and overriding Client priorities. <br> Name of Controlling Manager of concerned IE Sh.: " + co_name + ", Contact No." + co_mobile + ". <br>Offered Material as per registration should be readily available on the indicated date along with all related documents and internal test reports. Inspection is proposed to be conducted as per inspection plan: <a href='http://rites.ritesinsp.com/RBS/MASTER_ITEMS_CHECKSHEETS/" + item_cd + ".RAR'>Inspection Plan</a>.<br><a href='http://rites.ritesinsp.com/RBS/Guidelines for Vendors.pdf'>Guidelines for Vendors</a>.<br>For Inspection related information please visit : http://ritesinsp.com. <br> For any correspondence in future, please quote Case No. only. <br><br> Thanks for using RITES Inspection Services. <br><br>" + wRegion + ".";
            }
            mail_body = mail_body + "<br><br> THIS IS AN AUTO GENERATED EMAIL. PLEASE DO NOT REPLY. USE EMAIL GIVEN IN THE REGION ADDRESS.<BR>NATIONAL INSPECTION HELP LINE NUMBER : 1800 425 7000 (TOLL FREE)";


            //sender = "hardiksilvertouch007@outlook.com";
            //vend_email = "naimish.rana@silvertouch.com";
            //manu_mail = "neha.gehlot@silvertouch.com";
            if (vend_cd == mfg_cd && manu_mail != "")
            {
                SendMailModel sendMailModel = new SendMailModel();
                sendMailModel.From = sender;
                sendMailModel.To = manu_mail;
                sendMailModel.Bcc = "nrinspn@gmail.com";
                sendMailModel.Subject = "Your Call for Inspection By RITES";
                sendMailModel.Message = mail_body;
                bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
                email = isSend == true ? "Success" : "Error";
            }
            else if (vend_cd != mfg_cd && vend_email != "" && manu_mail != "")
            {
                SendMailModel sendMailModel = new SendMailModel();
                sendMailModel.From = "nrinspn@gmail.com";
                sendMailModel.To = vend_email + "," + manu_mail;
                sendMailModel.Bcc = "nrinspn@gmail.com";
                sendMailModel.Subject = "Your Call for Inspection By RITES";
                sendMailModel.Message = mail_body;
                bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
                email = isSend == true ? "Success" : "Error";
            }
            else if (vend_cd != mfg_cd && (vend_email == "" || manu_mail == ""))
            {
                SendMailModel sendMailModel = new SendMailModel();
                sendMailModel.From = "nrinspn@gmail.com";
                if (vend_email == "")
                {
                    sendMailModel.To = manu_mail;
                }
                else if (manu_mail == "")
                {
                    sendMailModel.To = vend_email;
                }
                sendMailModel.Bcc = "nrinspn@gmail.com";
                sendMailModel.Subject = "Your Call for Inspection By RITES";
                sendMailModel.Message = mail_body;
                bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
                email = isSend == true ? "Success" : "Error";
            }

            var controlling_email = (from t08 in context.T08IeControllOfficers
                                     join t09 in context.T09Ies on t08.CoCd equals t09.IeCoCd
                                     where t09.IeCd == obj.IE_CD
                                     select t08.CoEmail).FirstOrDefault();

            //var caseNo = obj.CASE_NO.Trim();
            //var dtOfReceipt = DateTime.ParseExact(obj.CALL_RECV_DT.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var manu_detail = (from t17 in context.T17CallRegisters
                               join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                               join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                               where t17.CaseNo == caseNo
                                  && t17.CallRecvDt == callRecvDt
                                  && t17.CallSno == Convert.ToInt32(callSno)
                               select new
                               {
                                   VEND_NAME = t05.VendName,
                                   VEND_ADDRESS = t03.City
                               }).FirstOrDefault();
            string manu_name = "", manu_add = "";
            if (manu_detail != null)
            {
                manu_name = manu_detail.VEND_NAME;
                manu_add = manu_detail.VEND_ADDRESS;
            }

            if (!string.IsNullOrEmpty(controlling_email))
            {
                SendMailModel sendMailModel = new SendMailModel();
                sendMailModel.From = "nrinspn@gmail.com";
                sendMailModel.To = controlling_email;
                if (ie_email != "")
                {
                    sendMailModel.CC = ie_email;
                }
                //sendMailModel.Bcc = "nrinspn@gmail.com";
                sendMailModel.Subject = "Your Call (" + manu_name + " - " + manu_add + ") for Inspection By RITES";
                sendMailModel.Message = mail_body;
                bool isSend = pSendMailRepository.SendMail(sendMailModel, null);
                email = isSend == true ? "Success" : "Error";
            }
            return email;
        }

        public async Task<string> send_IE_smsAsync(CallMarkedOnlineModel model)
        {
            string sms = "";
            string sender = "";
            string wIEMobile = "", wIEName = "", wVendor = "", wCOMobile = "", wVendMobile = "", wIEMobile_for_SMS = "";
            if (model.CASE_NO.ToString().Substring(0, 1) == "N") { sender = "NR"; }
            else if (model.CASE_NO.ToString().Substring(0, 1) == "W") { sender = "WR"; }
            else if (model.CASE_NO.ToString().Substring(0, 1) == "E") { sender = "ER"; }
            else if (model.CASE_NO.ToString().Substring(0, 1) == "S") { sender = "SR"; }
            else if (model.CASE_NO.ToString().Substring(0, 1) == "C") { sender = "CR"; }
            else { sender = "RITES"; }

            var query = from t09 in context.T09Ies
                        join t08 in context.T08IeControllOfficers
                        on t09.IeCoCd equals t08.CoCd into t08Group
                        from t08 in t08Group.DefaultIfEmpty()
                        where t09.IeCd == model.IE_CD
                        select new
                        {
                            IE_NAME = t09.IeName.Trim().Substring(0, Math.Min(t09.IeName.Trim().Length, 20)),
                            IE_PHONE_NO = t09.IePhoneNo.Trim().Substring(0, Math.Min(t09.IePhoneNo.Trim().Length, 10)),
                            CO_PHONE_NO = t08 != null ? t08.CoPhoneNo.Trim().Substring(0, Math.Min(t08.CoPhoneNo.Trim().Length, 10)) : ""
                        };

            var result = query.FirstOrDefault();

            if (result != null)
            {
                wIEName = result.IE_NAME;
                wIEMobile = result.IE_PHONE_NO;
                wIEMobile_for_SMS = result.IE_PHONE_NO;
                wCOMobile = result.CO_PHONE_NO;
            }

            var queryNew = from v in context.T05Vendors
                           join c in context.T03Cities
                           on v.VendCityCd equals c.CityCd
                           where v.VendCd == Convert.ToInt32(model.MFG_CD)
                           select new
                           {
                               VEND_NAME = v.VendName.Substring(0, Math.Min(v.VendName.Length, 30)).Replace("&", "AND"),
                               VEND_TEL = v.VendContactTel1.Trim().Substring(0, Math.Min(v.VendContactTel1.Trim().Length, 10))
                           };

            var resultNew = queryNew.FirstOrDefault();

            if (resultNew != null)
            {
                wVendor = resultNew.VEND_NAME;
                wVendMobile = resultNew.VEND_TEL;
            }

            if (wCOMobile != "") { wIEMobile = wIEMobile + "," + wCOMobile; }
            if (wVendMobile != "") { wIEMobile = wIEMobile + "," + wVendMobile; }
            string message = "RITES LTD - QA Call Marked, IE-" + wIEName + ",Contact No.:" + wIEMobile_for_SMS + ",RLY-" + model.RLY + ",PO-" + model.PO_NO + ",DT- " + model.PO_DT + ", Firm Name-" + wVendor + ", Call Sno - " + model.CALL_SNO + ",DT- " + model.CALL_RECV_DT + "- RITES/" + sender;

            using (HttpClient client = new HttpClient())
            {
                string baseurl = $"http://apin.onex-aura.com/api/sms?key=QtPr681q&to={wIEMobile}&from=RITESI&body={message}&entityid=1501628520000011823&templateid=1707161588918541674";

                HttpResponseMessage response = await client.GetAsync(baseurl);
                response.EnsureSuccessStatusCode(); // Ensure a successful response

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                sms = "success";
            }


            return sms;
        }

        public string GetRegionInfo(string Region)
        {
            string wRegion = "";
            if (string.IsNullOrEmpty(Region))
            {
                return wRegion;
            }
            switch (Region)
            {
                case "N":
                    wRegion = "NORTHERN REGION \n 12th FLOOR,CORE-II,SCOPE MINAR,LAXMI NAGAR, DELHI - 110092 \n Phone : +918800018691-95 \n Fax : 011-22024665";
                    break;
                case "S":
                    wRegion = "SOUTHERN REGION \n CTS BUILDING - 2ND FLOOR, BSNL COMPLEX, NO. 16, GREAMS ROAD,  CHENNAI - 600 006 \n Phone : 044-28292807/044- 28292817 \n Fax : 044-28290359";
                    break;
                case "E":
                    wRegion = "EASTERN REGION \n CENTRAL STATION BUILDING(METRO), 56, C.R. AVENUE,3rd FLOOR,KOLKATA-700 012  \n Fax : 033-22348704";
                    break;
                case "W":
                    wRegion = "WESTERN REGION \n 5TH FLOOR, REGENT CHAMBER, ABOVE STATUS RESTAURANT,NARIMAN POINT,MUMBAI-400021 \n Phone : 022-68943400/68943445 <BR>";
                    break;
                case "C":
                    wRegion = "Central Region";
                    break;
                case "Q":
                    wRegion = "CO QA Division";
                    break;
                default:
                    break;
            }
            return wRegion;
        }

        public string GetSenderMail(string Region)
        {
            string sender = "";
            if (string.IsNullOrEmpty(Region))
            {
                return sender;
            }
            switch (Region)
            {
                case "N":
                    sender = "nrinspn@rites.com";
                    break;
                case "S":
                    sender = "srinspn@rites.com";
                    break;
                case "E":
                    sender = "erinspn@rites.com";
                    break;
                case "W":
                    sender = "wrinspn@rites.com";
                    break;
                default:
                    break;
            }
            return sender;
        }
    }
}
