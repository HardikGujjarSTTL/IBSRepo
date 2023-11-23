using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using static IBS.Helper.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ModelContext context;

        public DashboardRepository(ModelContext context)
        {
            this.context = context;
        }

        public DashboardModel GetDashBoardCount(string Region)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            //par[2] = new OracleParameter("P_RESUT_HIGH_PAYMENT", OracleDbType.RefCursor, ParameterDirection.Output);
            //par[3] = new OracleParameter("P_RESUT_HIGH_OUTSTANDING", OracleDbType.RefCursor, ParameterDirection.Output);
            //par[4] = new OracleParameter("P_RESULT_PENDING_CASES", OracleDbType.RefCursor, ParameterDirection.Output);
            //par[5] = new OracleParameter("P_RESULT_JI_CASES", OracleDbType.RefCursor, ParameterDirection.Output);
            //par[6] = new OracleParameter("P_RESULT_REGION_CONSINEE_COMPLAINTS", OracleDbType.RefCursor, ParameterDirection.Output);


            DataSet ds = DataAccessDB.GetDataSet("GET_ADMIN_DASHBOARD_COUNT", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.TotalCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_CALL"]);
                    model.PendingCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PENDING_CALL"]);
                    model.AcceptedCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ACCEPTED_CALL"]);
                    model.CancelledCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["CANCELLED_CALL"]);
                    model.UnderLabTestingCount = Convert.ToInt32(ds.Tables[0].Rows[0]["UNDER_LAB_TESTING"]);
                    model.StillUnderInspectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STILL_UNDER_INSPECTION"]);
                    model.StageRejectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STAGE_REJECTION"]);
                    model.NotRecievedCount = Convert.ToInt32(ds.Tables[0].Rows[0]["IC_ISSUE_BUT_NOT_RECEIVE_OFFICE"]);
                }


                //if (ds.Tables[1].Rows.Count > 0)
                //{
                //    DataTable dt = ds.Tables[1];
                //    List<ClientDetailListModel> lstHighPayment = dt.AsEnumerable().Select(row => new ClientDetailListModel
                //    {
                //        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                //        NO_OF_BILL = Convert.ToInt32(row["NO_OF_BILL"]),
                //        AMOUNT = Convert.ToDecimal(row["AMOUNT"])
                //    }).ToList();
                //    model.lstHightPayment = lstHighPayment;
                //}

                //if (ds.Tables[2].Rows.Count > 0)
                //{
                //    DataTable dt = ds.Tables[2];
                //    List<ClientDetailListModel> lstHighOutstanding = dt.AsEnumerable().Select(row => new ClientDetailListModel
                //    {
                //        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                //        NO_OF_BILL = Convert.ToInt32(row["NO_OF_BILL"]),
                //        AMOUNT = Convert.ToDecimal(row["AMOUNT"])
                //    }).ToList();
                //    model.lstHightOutstanding = lstHighOutstanding;
                //}

                //// Oldest Pending Cases
                //if(ds.Tables[3].Rows.Count > 0)
                //{
                //    DataTable dt = ds.Tables[3];
                //    List<PendingOrJICaseListModel> lstPendingCase = dt.AsEnumerable().Select(row => new PendingOrJICaseListModel
                //    {
                //        CASE_NO = Convert.ToString(row["CASE_NO"]),
                //        DATE = Convert.ToDateTime(row["DATE"]),
                //        CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                //        PO_NO = Convert.ToString(row["PO_NO"])
                //    }).ToList();
                //    model.lstPendingCase = lstPendingCase;
                //}

                ////// Oldest JI Cases
                //if (ds.Tables[4].Rows.Count > 0)
                //{
                //    DataTable dt = ds.Tables[4];
                //    List<PendingOrJICaseListModel> lstJiCase = dt.AsEnumerable().Select(row => new PendingOrJICaseListModel
                //    {
                //        CASE_NO = Convert.ToString(row["CASE_NO"]),
                //        DATE = Convert.ToDateTime(row["DATE"]),                        
                //        PO_NO = Convert.ToString(row["PO_NO"])
                //    }).ToList();
                //    model.lstJiCase = lstJiCase;
                //}

                //// Region Wise Consignee complaints
                //if (ds.Tables[5].Rows.Count > 0) 
                //{
                //    DataTable dt = ds.Tables[5];
                //    List<RegionConsigneeComplaintsListModel> lstRegionConsComp = dt.AsEnumerable().Select(row => new RegionConsigneeComplaintsListModel
                //    {
                //        REGION = Convert.ToString(row["REGION"]),
                //        NO_OF_CONSINEE_COMPLAINTS = Convert.ToInt32(row["NO_OF_CONSINEE_COMPLAINTS"])
                //    }).ToList();
                //    model.lstRegionConsComp = lstRegionConsComp;
                //}

            }
            ComplaintStatusDetails(model, Region);

            return model;
        }

        public DashboardModel GetIEDDashBoardCount(int IeCd,string RegionCode)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[3];

            par[0] = new OracleParameter("P_IECD", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            par[2] = new OracleParameter("P_RESULT_CURSOR1", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_IE_DASHBOARD", par);
            List<IEList> listIE = new();

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.TotalCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_CALL"]);
                    model.PendingCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PENDING_CALL"]);
                    model.AcceptedCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ACCEPTED_CALL"]);
                    model.CancelledCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["CANCELLED_CALL"]);
                    model.UnderLabTestingCount = Convert.ToInt32(ds.Tables[0].Rows[0]["UNDER_LAB_TESTING"]);
                    model.StillUnderInspectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STILL_UNDER_INSPECTION"]);
                    model.StageRejectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STAGE_REJECTION"]);
                    model.DSCExpiryDateCount = 0;
                    model.NCIsuedAgainstIECount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_NO_OF_NC_ISSUE"]);
                    model.OutstandingNCCount = 0;
                    model.NotRecievedCount = Convert.ToInt32(ds.Tables[0].Rows[0]["IC_ISSUE_BUT_NOT_RECEIVE_OFFICE"]);
                    model.ConsigneeCompaintCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_CONSIGNEE_COMPLAINT"]);
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    listIE = dt.AsEnumerable().Select(row => new IEList
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DT = Convert.ToDateTime(row["CALL_RECV_DT"]),
                        CALL_SNO = Convert.ToInt32(row["CALL_SNO"]),
                        INSP_DESIRE_DT = Convert.ToDateTime(row["INSP_DESIRE_DT"]),
                        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                        CONTACT_PER = Convert.ToString(row["CONTACT_PER"]),
                        CONTACT_NO = Convert.ToString(row["CONTACT_NO"])
                    }).ToList();
                }
            }
            model.lstIE = listIE;

            var query = from l in context.T72IeMessages
                    where l.RegionCode == RegionCode && (l.Isdeleted == 0 || l.Isdeleted == null)
                    select new InstructionsIE
                    {
                        MessageId = l.MessageId,
                        LetterNo = l.LetterNo,
                        LetterDt = l.LetterDt,
                        Message = l.Message,
                        MessageDt = l.MessageDt,
                    };

            model.lstInstructionsIE = query.ToList();

            ComplaintStatusDetails(model, RegionCode);

            return model;
        }

        public DashboardModel GetVendorDashBoardCount(int Vend_Cd,string RegionCode)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[2];

            par[0] = new OracleParameter("P_VENDCD", OracleDbType.Int32, Vend_Cd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_VENDOR_DASHBOARD_COUNT", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.TotalCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_CALL"]);
                    model.PendingCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PENDING_CALL"]);
                    model.AcceptedCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ACCEPTED_CALL"]);
                    model.CancelledCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["CANCELLED_CALL"]);
                    model.UnderLabTestingCount = Convert.ToInt32(ds.Tables[0].Rows[0]["UNDER_LAB_TESTING"]);
                    model.StillUnderInspectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STILL_UNDER_INSPECTION"]);
                    model.StageRejectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STAGE_REJECTION"]);
                }
            }

            OracleParameter[] par1 = new OracleParameter[2];

            par1[0] = new OracleParameter("P_VEND_CD", OracleDbType.Int32, Vend_Cd, ParameterDirection.Input);
            par1[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds1 = DataAccessDB.GetDataSet("GET_VENDOR_DASHBOARD_RECENR_CALL_STATUS", par1);
            List<POCallStatusList> listVend = new();
            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds1.Tables[0];
                    listVend = dt.AsEnumerable().Select(row => new POCallStatusList
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DT = Convert.ToDateTime(row["CALL_RECV_DT"]),
                        CALL_SNO = Convert.ToInt32(row["CALL_SNO"]),
                        DETAILS = Convert.ToString(row["DETAILS"]),
                        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        IE_PHONE_NO = Convert.ToString(row["IE_PHONE_NO"]),
                        CO_NAME = Convert.ToString(row["CO_NAME"]),
                        CO_PHONE_NO = Convert.ToString(row["CO_PHONE_NO"])
                    }).ToList();
                }
            }
            model.lstPOCallStatus = listVend;

            OracleParameter[] par2 = new OracleParameter[2];

            par2[0] = new OracleParameter("P_VENDCD", OracleDbType.Int32, Vend_Cd, ParameterDirection.Input);
            par2[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds2 = DataAccessDB.GetDataSet("GET_VENDOR_DASHBOARD_RECENT_PO", par2);
            List<RecentPOList> lstRecentPO = new();
            if (ds2 != null && ds2.Tables.Count > 0)
            {
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds2.Tables[0];
                    lstRecentPO = dt.AsEnumerable().Select(row => new RecentPOList
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DT = Convert.ToDateTime(row["CALL_RECV_DT"]),
                        DETAILS = Convert.ToString(row["DETAILS"]),
                        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                        PO_NO = Convert.ToString(row["PO_NO"]),
                        PURCHASE_ORDER = Convert.ToString(row["PURCHASE_ORDER"]),
                        CALL_STATUS = Convert.ToString(row["CALL_STATUS"])
                    }).ToList();
                }
            }
            model.lstRecentPO = lstRecentPO;

            ComplaintStatusDetails(model, RegionCode);

            return model;
        }

        public DashboardModel GetClientDashBoardCount(string OrgnType, string Organisation,string RegionCode)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[7];

            par[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Organisation, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RLY_NONRLY", OracleDbType.Varchar2, OrgnType, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            par[3] = new OracleParameter("P_RESULT_VND_WISE_PER", OracleDbType.RefCursor, ParameterDirection.Output);
            par[4] = new OracleParameter("P_RESULT_REC_REQ", OracleDbType.RefCursor, ParameterDirection.Output);
            par[5] = new OracleParameter("P_RESULT_REC_PO", OracleDbType.RefCursor, ParameterDirection.Output);
            par[6] = new OracleParameter("P_RESULT_VND_CONS_COMP", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_CLIENT_DASHBOARD", par);
            List<ClientVENDPOList> lstClientVEND = new();
            List<ClientRecentReqList> lstClientRecentReq = new();
            List<ClientRecentPOList> lstClientRecentPO = new();
            List<ClientVendConCompList> lstClientVendConComp = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.TotalCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_CALL"]);
                    model.PendingCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PENDING_CALL"]);
                    model.AcceptedCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ACCEPTED_CALL"]);
                    model.CancelledCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["CANCELLED_CALL"]);
                    model.UnderLabTestingCount = Convert.ToInt32(ds.Tables[0].Rows[0]["UNDER_LAB_TESTING"]);
                    model.StillUnderInspectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STILL_UNDER_INSPECTION"]);
                    model.StageRejectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STAGE_REJECTION"]);
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    lstClientVEND = dt.AsEnumerable().Select(row => new ClientVENDPOList
                    {
                        RLY_CD = Convert.ToString(row["RLY_CD"]),
                        RLY_NONRLY = Convert.ToString(row["RLY_NONRLY"]),
                        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                        TOTAL_CALL = Convert.ToInt32(row["TOTAL_CALL"]),
                        REJECTED_CALL = Convert.ToInt32(row["REJECTED_CALL"]),
                        CANCELLED_CALL = Convert.ToInt32(row["CANCELLED_CALL"]),
                    }).ToList();
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[2];
                    lstClientRecentReq = dt.AsEnumerable().Select(row => new ClientRecentReqList
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DT = Convert.ToDateTime(row["CALL_RECV_DT"]),
                        CALL_SNO = Convert.ToInt32(row["CALL_SNO"]),
                        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                        DETAILS = Convert.ToString(row["DETAILS"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        CALL_STATUS = Convert.ToString(row["CALL_STATUS"]),
                    }).ToList();
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[3];
                    lstClientRecentPO = dt.AsEnumerable().Select(row => new ClientRecentPOList
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        PO_DT = Convert.ToDateTime(row["PO_DT"]),
                        VALUE = Convert.ToDecimal(row["VALUE"]),
                        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                        PO_NO = Convert.ToString(row["PO_NO"]),
                    }).ToList();
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[4];
                    lstClientVendConComp = dt.AsEnumerable().Select(row => new ClientVendConCompList
                    {
                        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                        NO_OF_COMPLAINTS = Convert.ToInt32(row["NO_OF_COMPLAINTS"]),
                    }).ToList();
                }
            }

            model.lstClientVEND = lstClientVEND;

            model.lstClientRecentReq = lstClientRecentReq;

            model.lstClientRecentPO = lstClientRecentPO;

            model.lstClientVendConComp = lstClientVendConComp;

            ComplaintStatusDetails(model,RegionCode);
            return model;
        }

        public DashboardModel GetCMDashBoardCount(int CoCd)
        {
            DashboardModel model = new();

            OracleParameter[] par1 = new OracleParameter[2];

            par1[0] = new OracleParameter("P_COCD", OracleDbType.Int32, CoCd, ParameterDirection.Input);
            par1[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_CM_DASHBOARD_COUNT", par1);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.TotalCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_CALL"]);
                    model.OnlineRegCall = Convert.ToInt32(ds.Tables[0].Rows[0]["ONLINE_REG_CALL"]);
                    model.ManualRegCall = Convert.ToInt32(ds.Tables[0].Rows[0]["MANUAL_REG_CALL"]);
                    model.POAwaitingCaseNo = Convert.ToInt32(ds.Tables[0].Rows[0]["PO_AWAITING_CASE_NO"]);
                    model.PendingCallRemarks = Convert.ToInt32(ds.Tables[0].Rows[0]["PENDING_CALL_REMARK"]);
                    model.PendingOnlineCallAwaitingMark = Convert.ToInt32(ds.Tables[0].Rows[0]["PENDING_ONLINE_CALL_AWAITING_MARK"]);
                }
            }

            OracleParameter[] par2 = new OracleParameter[2];

            par2[0] = new OracleParameter("P_COCD", OracleDbType.Int32, CoCd, ParameterDirection.Input);
            par2[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds1 = DataAccessDB.GetDataSet("GET_CM_DASHBOARD_IE_WISE_PERFOMANCE", par2);
            List<DashboardModel> listVend = new();
            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds1.Tables[0];
                    listVend = dt.AsEnumerable().Select(row => new DashboardModel
                    {
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        TotalCallsCount = Convert.ToInt32(row["TOTAL_CALL"]),
                        PendingCallsCount = Convert.ToInt32(row["PENDING_CALL"]),
                        AcceptedCallsCount = Convert.ToInt32(row["ACCEPTED_CALL"]),
                        CancelledCallsCount = Convert.ToInt32(row["CANCELLED_CALL"]),
                        UnderLabTestingCount = Convert.ToInt32(row["UNDER_LAB_CALL"]),
                        StillUnderInspectionCount = Convert.ToInt32(row["STILL_INSP_CALL"]),
                        StageRejectionCount = Convert.ToInt32(row["STAGE_REJECTION_CALL"]),
                    }).ToList();
                }
            }
            model.IEWisePerformance = listVend;
            return model;
        }
        public DTResult<LabReportsModel> LoadTableInvoice(DTParameters dtParameters, string Regin)
        {

            DTResult<LabReportsModel> dTResult = new() { draw = 0 };
            IQueryable<LabReportsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "invoice_dt";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "invoice_dt";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("wFrmDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wFrmDtO"), ParameterDirection.Input);
            par[1] = new OracleParameter("wToDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[2] = new OracleParameter("region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[3] = new OracleParameter("Flag", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("Flag"), ParameterDirection.Input);
            par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Dashboard_TotalInvoice", par, 4);

            List<LabReportsModel> modelList = new List<LabReportsModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    LabReportsModel model = new LabReportsModel
                    {
                        BPO_NAME = row["BPO_NAME"].ToString(),
                        recipient_gstin_no = row["recipient_gstin_no"].ToString(),
                        St_cd = row["St_cd"].ToString(),
                        invoice_no = row["invoice_no"].ToString(),
                        invoice_dt = row["invoice_dt"].ToString(),
                        Total_AMT = Convert.ToString(row["Total_AMT"]),
                        INV_TYPE = row["INV_TYPE"].ToString(),
                        HSN_CD = row["HSN_CD"].ToString(),
                        INV_amount = Convert.ToString(row["INV_amount"]),
                        INV_sgst = Convert.ToString(row["INV_sgst"]),
                        INV_cgst = Convert.ToString(row["INV_cgst"]),
                        INV_igst = Convert.ToString(row["INV_igst"]),
                        INVOICE_TYPE = row["INVOICE_TYPE"].ToString(),
                        INC_TYPE = row["INC_TYPE"].ToString(),
                        Total_GST = Convert.ToString(row["Total_GST"]),
                        IRN_NO = row["IRN_NO"].ToString(),
                        BILL_FINALIZE = row["BILL_FINALIZE"].ToString(),
                        BILL_SENT = row["BILL_SENT"].ToString(),
                    };

                    modelList.Add(model);
                }
            }

            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BPO_NAME).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.BPO_NAME).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;


        }

        public DTResult<LabSampleInfoModel> LoadTableReportU(DTParameters dtParameters, string Regin)
        {

            DTResult<LabSampleInfoModel> dTResult = new() { draw = 0 };
            IQueryable<LabSampleInfoModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CallRecDt";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CallRecDt";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("wFrmDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wFrmDtO"), ParameterDirection.Input);
            par[1] = new OracleParameter("wToDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[2] = new OracleParameter("region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[3] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Dashboard_TotalReportsU", par, 3);

            List<LabSampleInfoModel> modelList = new List<LabSampleInfoModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    LabSampleInfoModel model = new LabSampleInfoModel
                    {
                        CaseNo = Convert.ToString(row["case_no"]),
                        CallRecDt = Convert.ToString(row["call_recv_dt"]),
                        CallSNO = Convert.ToString(row["call_sno"]),
                        IEName = Convert.ToString(row["ie_name"]),
                    };

                    modelList.Add(model);
                }
            }

            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.IEName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.IEName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;


        }

        public LabSampleInfoModel GetNOOfRegisterCount(string Regin)
        {

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
                par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("Dashboard_NOOFRegisterCount", par, 1);

                LabSampleInfoModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new LabSampleInfoModel
                    {
                        NO_OF_Register_Per_Day = Convert.ToString(row["NO_OF_Sample_Register_Per_Day"]),

                    };
                }

                return model;
            }
        }
        public DTResult<IE_Per_CM_Model> Get_CM_Wise_IE_Detail(DTParameters dtParameters)
        {
            DTResult<IE_Per_CM_Model> dTResult = new() { draw = 0 };
            IQueryable<IE_Per_CM_Model>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;


            }

            if (orderCriteria == "" || orderCriteria == null)
            {
                orderCriteria = "CASE_NO";
            }
            //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            //orderAscendingDirection = true;


            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CASE_NO";
                orderAscendingDirection = true;
            }


            string FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : null;
            string ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : null;
            string CO_CD = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CO_CD"]) ? Convert.ToString(dtParameters.AdditionalValues["CO_CD"]) : null;

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CO_CD", OracleDbType.Varchar2, CO_CD, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CM_WISE_IE_DETAIL", par, 1);
            DataTable dt = ds.Tables[0];
            List<IE_Per_CM_Model> list = new List<IE_Per_CM_Model>();
            if (dt != null && dt.Rows.Count > 0)
            {
                list = dt.AsEnumerable().Select(row => new IE_Per_CM_Model
                {
                    CASE_NO = Convert.ToString(row["CASE_NO"]),
                    CALL_RECV_DT = Convert.ToDateTime(row["CALL_RECV_DT"]),
                    CALL_SNO = Convert.ToInt32(row["CALL_SNO"]),
                    IE_NAME = Convert.ToString(row["IE_NAME"]),
                    VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                    CALL_STATUS = Convert.ToString(row["CALL_STATUS"])
                }).ToList();
            }

            query = list.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CASE_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.IE_NAME).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.VEND_NAME).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<IEList> Get_IE_Dashboard_Details_List(DTParameters dtParameters)
        {
            DTResult<IEList> dTResult = new() { draw = 0 };
            IQueryable<IEList>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
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

            string FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : null;
            string ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : null;
            string IE_CD = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IE_CD"]) ? Convert.ToString(dtParameters.AdditionalValues["IE_CD"]) : null;
            string TypeOfList = !string.IsNullOrEmpty(dtParameters.AdditionalValues["TypeOfList"]) ? Convert.ToString(dtParameters.AdditionalValues["TypeOfList"]) : null;


            OracleParameter[] par = new OracleParameter[5];

            par[0] = new OracleParameter("p_IE_CD", OracleDbType.Varchar2, IE_CD, ParameterDirection.Input);
            par[1] = new OracleParameter("p_STATUS_TYPE", OracleDbType.Varchar2, TypeOfList, ParameterDirection.Input);
            par[2] = new OracleParameter("p_FROM_DATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[3] = new OracleParameter("p_TO_DATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("GET_IE_Dashboard_List", par, 1);

            DataTable dt = null;
            dt = ds.Tables[0];

            List<IEList> list = new List<IEList>();
            if (dt != null && dt.Rows.Count > 0)
            {
                list = dt.AsEnumerable().Select(row => new IEList
                {
                    CASE_NO = Convert.ToString(row["CASE_NO"]),
                    CALL_RECV_DT = Convert.ToDateTime(row["DATES"]),
                    CALL_SNO = Convert.ToInt32(row["CALL_SNO"]),
                    VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                    CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                    CONTACT_PER = Convert.ToString(row["CONTACT_PER"]),
                    CONTACT_NO = Convert.ToString(row["CONTACT_NO"]),
                }).ToList();
            }

            query = list.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CASE_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.VEND_NAME).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<VenderCallRegisterModel> GetDataListTotalCallListing(DTParameters dtParameters, string Region)
        {
            DTResult<VenderCallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<VenderCallRegisterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string FromDate = "", ToDate = "", ActionType = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            {
                FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            {
                ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ActionType"]))
            {
                ActionType = Convert.ToString(dtParameters.AdditionalValues["ActionType"]);
            }
            if (ActionType == "TC")
            {
                query = from l in context.ViewGetCallRegCancellations
                        where (l.CallRecvDt >= Convert.ToDateTime(FromDate) && l.CallRecvDt <= Convert.ToDateTime(ToDate))
                        orderby l.CaseNo, l.CallRecvDt
                        select new VenderCallRegisterModel
                        {
                            CaseNo = l.CaseNo,
                            CallRecvDt = l.CallRecvDt,
                            CallInstallNo = l.CallInstallNo,
                            CallSno = Convert.ToInt16(l.CallSno),
                            CallStatus = l.CallStatus,
                            CallLetterNo = l.CallLetterNo,
                            Remarks = l.Remarks,
                            PoNo = l.PoNo,
                            PoDt = l.PoDt,
                            IeSname = l.IeSname,
                            Vendor = l.Vendor,
                            RegionCode = l.RegionCode,
                        };
            }
            else
            {
                query = from l in context.ViewGetCallRegCancellations
                        where (l.CallRecvDt >= Convert.ToDateTime(FromDate) && l.CallRecvDt <= Convert.ToDateTime(ToDate))
                              && l.CStatus == ActionType
                        orderby l.CaseNo, l.CallRecvDt
                        select new VenderCallRegisterModel
                        {
                            CaseNo = l.CaseNo,
                            CallRecvDt = l.CallRecvDt,
                            CallInstallNo = l.CallInstallNo,
                            CallSno = Convert.ToInt16(l.CallSno),
                            CallStatus = l.CallStatus,
                            CallLetterNo = l.CallLetterNo,
                            Remarks = l.Remarks,
                            PoNo = l.PoNo,
                            PoDt = l.PoDt,
                            IeSname = l.IeSname,
                            Vendor = l.Vendor,
                            RegionCode = l.RegionCode,
                        };
            }



            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<VenderCallRegisterModel> GetDataCallDeskInfoListing(DTParameters dtParameters, string Region)
        {
            DTResult<VenderCallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<VenderCallRegisterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string FromDate = "", ToDate = "", ActionType = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            {
                FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            {
                ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ActionType"]))
            {
                ActionType = Convert.ToString(dtParameters.AdditionalValues["ActionType"]);
            }
            if (ActionType == "ACM")
            {
                query = from T17 in context.T17CallRegisters
                        join T09 in context.T09Ies on T17.IeCd equals T09.IeCd
                        join T05 in context.T05Vendors on T17.MfgCd equals T05.VendCd
                        where (T17.CallRecvDt >= Convert.ToDateTime(FromDate) && T17.CallRecvDt <= Convert.ToDateTime(ToDate))
                        && T17.AutomaticCall == "Y"
                        select new VenderCallRegisterModel
                        {
                            CaseNo = T17.CaseNo,
                            CallRecvDt = T17.CallRecvDt,
                            CallSno = Convert.ToInt16(T17.CallSno),
                            CallMarkDt = T17.CallMarkDt,
                            CallStatus = T17.CallStatus == "M" ? "Pending" : T17.CallStatus == "A" ? "Accepted" : T17.CallStatus == "R" ? "Rejection" : T17.CallStatus == "C" ? "Cancelled" : T17.CallStatus == "U" ? "Under Lab Testing" : T17.CallStatus == "S" ? "Still Under Inspection" : T17.CallStatus == "G" ? "Stage Inspection Accepted" : T17.CallStatus == "B" ? "Accepted and Billed" : T17.CallStatus == "T" ? "Stage Rejection" : "Withheld",
                            IE_name = T09.IeName,
                            Vendor = T05.VendName,
                            RegionCode = T17.RegionCode,
                        };
            }
            else if (ActionType == "MCM")
            {
                query = from T17 in context.T17CallRegisters
                        join T09 in context.T09Ies on T17.IeCd equals T09.IeCd
                        join T05 in context.T05Vendors on T17.MfgCd equals T05.VendCd
                        where (T17.CallRecvDt >= Convert.ToDateTime(FromDate) && T17.CallRecvDt <= Convert.ToDateTime(ToDate))
                        && T17.AutomaticCall != "Y"
                        select new VenderCallRegisterModel
                        {
                            CaseNo = T17.CaseNo,
                            CallRecvDt = T17.CallRecvDt,
                            CallSno = Convert.ToInt16(T17.CallSno),
                            CallMarkDt = T17.CallMarkDt,
                            CallStatus = T17.CallStatus == "M" ? "Pending" : T17.CallStatus == "A" ? "Accepted" : T17.CallStatus == "R" ? "Rejection" : T17.CallStatus == "C" ? "Cancelled" : T17.CallStatus == "U" ? "Under Lab Testing" : T17.CallStatus == "S" ? "Still Under Inspection" : T17.CallStatus == "G" ? "Stage Inspection Accepted" : T17.CallStatus == "B" ? "Accepted and Billed" : T17.CallStatus == "T" ? "Stage Rejection" : "Withheld",
                            IE_name = T09.IeName,
                            Vendor = T05.VendName,
                            RegionCode = T17.RegionCode,
                        };
            }
            //else if (ActionType == "POAC")
            //{
            //    query = from T80 in context.ViewPomasterlists
            //            where T80.RegionCode == Region
            //            && T80.RealCaseNo == null
            //            && T80.Isdeleted != Convert.ToByte(true)
            //            && (T80.PoDt >= Convert.ToDateTime(FromDate) && T80.PoDt <= Convert.ToDateTime(ToDate))
            //            select new VenderCallRegisterModel
            //            {
            //                CaseNo = T80.CaseNo,
            //                PoNo = T80.PoNo,
            //                PoDt = Convert.ToDateTime(T80.PoDt),
            //                Rly = T80.RlyCd,
            //                Vendor = T80.VendName,
            //                Consignee = T80.ConsigneeSName,
            //                Remarks = T80.Remarks,
            //                RegionCode = T80.RegionCode,
            //            };
            //}
            else if (ActionType == "PCR")
            {
                query = from t108 in context.T108RemarkedCalls
                        join t09From in context.T09Ies on (int)t108.FrIeCd equals t09From.IeCd
                        join t10To in context.T09Ies on (int)t108.ToIeCd equals t10To.IeCd
                        join t02 in context.T02Users on t108.RemInitBy equals t02.UserId
                        where t108.RemarkingStatus == "P" && t108.CaseNo.Substring(0, 1) == Region
                        select new VenderCallRegisterModel
                        {
                            CaseNo = t108.CaseNo,
                            CallRecvDt = t108.CallRecvDt,
                            CallSno = t108.CallSno,
                            CallRemarkStatus = t108.RemarkingStatus == "P" ? "Pending" : null,
                            FrIeName = t09From.IeName,
                            ToIeName = t10To.IeName,
                            UserName = t02.UserName,
                            RemInitDatetime = t108.RemInitDatetime
                        };
            }
            else if (ActionType == "POCAM")
            {
                query = from T17 in context.T17CallRegisters
                        join T05 in context.T05Vendors on T17.MfgCd equals T05.VendCd
                        where T17.IeCd == null
                        && (T17.CallRecvDt >= Convert.ToDateTime(FromDate) && T17.CallRecvDt <= Convert.ToDateTime(ToDate))

                        select new VenderCallRegisterModel
                        {
                            CaseNo = T17.CaseNo,
                            CallRecvDt = T17.CallRecvDt,
                            CallSno = Convert.ToInt16(T17.CallSno),
                            CallMarkDt = T17.CallMarkDt,
                            CallStatus = T17.CallStatus == "M" ? "Pending" : T17.CallStatus == "A" ? "Accepted" : T17.CallStatus == "R" ? "Rejection" : T17.CallStatus == "C" ? "Cancelled" : T17.CallStatus == "U" ? "Under Lab Testing" : T17.CallStatus == "S" ? "Still Under Inspection" : T17.CallStatus == "G" ? "Stage Inspection Accepted" : T17.CallStatus == "B" ? "Accepted and Billed" : T17.CallStatus == "T" ? "Stage Rejection" : "Withheld",
                            IE_name = null,
                            Vendor = T05.VendName,
                            RegionCode = T17.RegionCode,
                        };
            }
            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DataSet ComplaintStatusSummary(string Region)
        {
            OracleParameter[] par = new OracleParameter[2];

            par[0] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            return DataAccessDB.GetDataSet("GET_ADMIN_DASHBOARD_COMPLAINT_STATUS", par);
        }

        public DTResult<PO_MasterModel> GetPOMasterList(DTParameters dtParameters)
        {

            DTResult<PO_MasterModel> dTResult = new() { draw = 0 };
            IQueryable<PO_MasterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "pDatetime";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "pDatetime";
                orderAscendingDirection = true;
            }

            //List<PO_MasterModel> model = new();
            //OracleParameter[] par = new OracleParameter[1];
            //par[0] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            //var ds = DataAccessDB.GetDataSet("SP_CM_DASHBOARD_POMASTERLIST", par, 1);
            //if (ds != null && ds.Tables.Count > 0)
            //{
            //    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            //    model = JsonConvert.DeserializeObject<List<PO_MasterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //}
            //query = model.AsQueryable();
            //dTResult.recordsTotal = query.Count();

            query = from POMaster in context.ViewDashboardPomasterlists
                    select new PO_MasterModel
                    {
                        VendCd = POMaster.VendCd,
                        PoNo = POMaster.PoNo,
                        PoDtDate = Convert.ToDateTime(POMaster.PoDt).ToString("dd/MM/yyyy"),
                        RlyCd = POMaster.RlyCd,
                        VendorName = POMaster.VendName,
                        ConsigneeSName = POMaster.ConsigneeSName,
                        Remarks = POMaster.Remarks,
                        RlyNonrly = POMaster.RlyNonrly,
                        MainrlyCd = POMaster.MainrlyCd,
                        pDatetime=POMaster.Pdatetime
                    };
            dTResult.recordsTotal = query.Count();
            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.VendorName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.ConsigneeSName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Remarks).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public void ComplaintStatusDetails(DashboardModel model,string Region)
        {
            DataSet ds2 = ComplaintStatusSummary(Region);

            model.complaintStatusSummaryModel = new();

            if (ds2 != null && ds2.Tables.Count > 0)
            {
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    model.complaintStatusSummaryModel.REGION = Convert.ToString(ds2.Tables[0].Rows[0]["REGION"]);
                    model.complaintStatusSummaryModel.PENDING = Convert.ToInt32(ds2.Tables[0].Rows[0]["PENDING"]);
                    model.complaintStatusSummaryModel.ACCEPTED = Convert.ToInt32(ds2.Tables[0].Rows[0]["ACCEPTED"]);
                    model.complaintStatusSummaryModel.UPHELD = Convert.ToInt32(ds2.Tables[0].Rows[0]["UPHELD"]);
                    model.complaintStatusSummaryModel.SORTING = Convert.ToInt32(ds2.Tables[0].Rows[0]["SORTING"]);
                    model.complaintStatusSummaryModel.RECTIFICATION = Convert.ToInt32(ds2.Tables[0].Rows[0]["RECTIFICATION"]);
                    model.complaintStatusSummaryModel.PRICE_REDUCTION = Convert.ToInt32(ds2.Tables[0].Rows[0]["PRICE_REDUCTION"]);
                    model.complaintStatusSummaryModel.LIFTED_BEFORE_JI = Convert.ToInt32(ds2.Tables[0].Rows[0]["LIFTED_BEFORE_JI"]);
                    model.complaintStatusSummaryModel.TRANSIT_DEMANGE = Convert.ToInt32(ds2.Tables[0].Rows[0]["TRANSIT_DEMANGE"]);
                    model.complaintStatusSummaryModel.UNSTAMPED = Convert.ToInt32(ds2.Tables[0].Rows[0]["UNSTAMPED"]);
                    model.complaintStatusSummaryModel.NOT_ON_RITES = Convert.ToInt32(ds2.Tables[0].Rows[0]["NOT_ON_RITES"]);
                    model.complaintStatusSummaryModel.DELETED = Convert.ToInt32(ds2.Tables[0].Rows[0]["DELETED"]);
                }
            }

            model.ComplaintStatusSummary = "[";
            model.ComplaintStatusSummary += "['Pending'," + model.complaintStatusSummaryModel.PENDING + "],";
            model.ComplaintStatusSummary += "['Accepted'," + model.complaintStatusSummaryModel.ACCEPTED + "],";
            model.ComplaintStatusSummary += "['Upheld'," + model.complaintStatusSummaryModel.UPHELD + "],";
            model.ComplaintStatusSummary += "['Sorting'," + model.complaintStatusSummaryModel.SORTING + "],";
            model.ComplaintStatusSummary += "['Rectification'," + model.complaintStatusSummaryModel.RECTIFICATION + "],";
            model.ComplaintStatusSummary += "['Price Reduction'," + model.complaintStatusSummaryModel.PRICE_REDUCTION + "],";
            model.ComplaintStatusSummary += "['Lifted Before JI'," + model.complaintStatusSummaryModel.LIFTED_BEFORE_JI + "],";
            model.ComplaintStatusSummary += "['Transit Damange'," + model.complaintStatusSummaryModel.TRANSIT_DEMANGE + "],";
            model.ComplaintStatusSummary += "['Unstamped'," + model.complaintStatusSummaryModel.UNSTAMPED + "],";
            model.ComplaintStatusSummary += "['Not on Rites A/C'," + model.complaintStatusSummaryModel.NOT_ON_RITES + "],";
            model.ComplaintStatusSummary += "['Deleted'," + model.complaintStatusSummaryModel.DELETED + "]";
            model.ComplaintStatusSummary += "]";
        }
    }
}
