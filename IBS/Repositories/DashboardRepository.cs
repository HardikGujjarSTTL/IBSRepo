using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public DashboardModel GetDashBoardLabCount(int userid, string Regin)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("P_USER_ID", OracleDbType.Varchar2, userid, ParameterDirection.Input);
            par[1] = new OracleParameter("region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            par[3] = new OracleParameter("P_RESULT_CURSOR1", OracleDbType.RefCursor, ParameterDirection.Output);
            par[4] = new OracleParameter("P_RESULT_CURSOR2", OracleDbType.RefCursor, ParameterDirection.Output);
            par[5] = new OracleParameter("P_RESULT_CURSOR3", OracleDbType.RefCursor, ParameterDirection.Output);
            par[6] = new OracleParameter("P_RESULT_CURSOR4", OracleDbType.RefCursor, ParameterDirection.Output);
            par[7] = new OracleParameter("P_RESULT_CURSOR5", OracleDbType.RefCursor, ParameterDirection.Output);
            par[8] = new OracleParameter("P_RESULT_CURSOR6", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_LAB_DASHBOARD", par, 8);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.TOTAL_INVOICE = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_INVOICE"]);
                    //model.FINALIZED_INVOICE = Convert.ToInt32(ds.Tables[0].Rows[0]["FINALIZED_INVOICE"]);
                    object finalizedInvoiceValue = ds.Tables[0].Rows[0]["FINALIZED_INVOICE"];
                    model.FINALIZED_INVOICE = (finalizedInvoiceValue != DBNull.Value) ? Convert.ToInt32(finalizedInvoiceValue) : 0;
                    object finalizedFinalizedValue = ds.Tables[0].Rows[0]["PENDING_FINALIZED_INVOICE"];
                    model.PENDING_FINALIZED_INVOICE = (finalizedFinalizedValue != DBNull.Value) ? Convert.ToInt32(finalizedFinalizedValue) : 0;
                    //model.PENDING_FINALIZED_INVOICE = Convert.ToInt32(ds.Tables[0].Rows[0]["PENDING_FINALIZED_INVOICE"]);
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    model.TotalUploaded = Convert.ToInt32(ds.Tables[1].Rows[0]["REPORTS_GENERATED"]);

                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    model.TotalBillAmount = Convert.ToString(ds.Tables[2].Rows[0]["Bill_Amount"]);

                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[3];
                    List<LABREGISTERModel> lst = dt.AsEnumerable().Select(row => new LABREGISTERModel
                    {
                        CASE_NO = Convert.ToString(row["case_no"]),
                        IE = Convert.ToString(row["ie_name"]),
                        Date = Convert.ToString(row["datetime"]),
                        Vendor = Convert.ToString(row["vend_name"]),
                        SampleRegNo = Convert.ToString(row["sample_reg_no"]),
                        SampleRecDt = Convert.ToString(row["sample_recv_dt"])
                    }).ToList();
                    model.lstsampledata = lst;
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    model.Total_Number_Of_Samples = Convert.ToInt32(ds.Tables[4].Rows[0]["Total_Number_Of_Samples"]);

                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    model.Internal = Convert.ToInt32(ds.Tables[5].Rows[0]["Internal"]);

                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    model.External = Convert.ToInt32(ds.Tables[6].Rows[0]["External"]);

                }

            }


            return model;
        }
        public DashboardModel GetDashBoardCount(string Region, string RoleName)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[7]; //[7];
            par[0] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            par[2] = new OracleParameter("P_RESUT_HIGH_PAYMENT", OracleDbType.RefCursor, ParameterDirection.Output);
            par[3] = new OracleParameter("P_RESUT_HIGH_OUTSTANDING", OracleDbType.RefCursor, ParameterDirection.Output);
            par[4] = new OracleParameter("P_RESULT_PENDING_CASES", OracleDbType.RefCursor, ParameterDirection.Output);
            par[5] = new OracleParameter("P_RESULT_JI_CASES", OracleDbType.RefCursor, ParameterDirection.Output);
            par[6] = new OracleParameter("P_RESULT_REGION_CONSINEE_COMPLAINTS", OracleDbType.RefCursor, ParameterDirection.Output);


            DataSet ds = DataAccessDB.GetDataSet("GET_ADMIN_DASHBOARD_COUNT", par, 6); //6

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
                    model.NOofBill = Convert.ToInt32(ds.Tables[0].Rows[0]["NO_OF_BILL"]);
                    model.ICISSUERECEIVEOFFICENOTBILL = Convert.ToInt32(ds.Tables[0].Rows[0]["IC_ISSUE_RECEIVE_OFFICE_NOT_BILL"]);
                    model.NOOFIEPERCM = Convert.ToInt32(ds.Tables[0].Rows[0]["NO_OF_IE_PER_CM"]);
                }

                //if (ds.Tables.Count > 1)
                //{
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    List<ClientDetailListModel> lstHighPayment = dt.AsEnumerable().Select(row => new ClientDetailListModel
                    {
                        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                        NO_OF_BILL = Convert.ToInt32(row["NO_OF_BILL"]),
                        AMOUNT = Convert.ToDecimal(row["AMOUNT"])
                    }).ToList();
                    model.lstHightPayment = lstHighPayment;
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[2];
                    List<ClientDetailListModel> lstHighOutstanding = dt.AsEnumerable().Select(row => new ClientDetailListModel
                    {
                        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                        NO_OF_BILL = Convert.ToInt32(row["NO_OF_BILL"]),
                        AMOUNT = Convert.ToDecimal(row["AMOUNT"])
                    }).ToList();
                    model.lstHightOutstanding = lstHighOutstanding;
                }

                // Oldest Pending Cases
                if (ds.Tables[3].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[3];
                    List<PendingOrJICaseListModel> lstPendingCase = dt.AsEnumerable().Select(row => new PendingOrJICaseListModel
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_DATE = Convert.ToDateTime(row["CALL_DATE"]),
                        CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                        PO_NO = Convert.ToString(row["PO_NO"])
                    }).ToList();
                    model.lstPendingCase = lstPendingCase;
                }

                //// Oldest JI Cases
                if (ds.Tables[4].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[4];
                    List<PendingOrJICaseListModel> lstJiCase = dt.AsEnumerable().Select(row => new PendingOrJICaseListModel
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_DATE = Convert.ToDateTime(row["CALL_DATE"]),
                        PO_NO = Convert.ToString(row["PO_NO"])
                    }).ToList();
                    model.lstJiCase = lstJiCase;
                }

                // Region Wise Consignee complaints
                if (ds.Tables[5].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[5];
                    List<RegionConsigneeComplaintsListModel> lstRegionConsComp = dt.AsEnumerable().Select(row => new RegionConsigneeComplaintsListModel
                    {
                        REGION = Convert.ToString(row["REGION"]),
                        NO_OF_CONSINEE_COMPLAINTS = Convert.ToInt32(row["NO_OF_CONSINEE_COMPLAINTS"])
                    }).ToList();
                    model.lstRegionConsComp = lstRegionConsComp;
                }
                //}
            }
            ComplaintStatusDetails(model, Region, null, RoleName);

            return model;
        }

        public DashboardModel GetIEDDashBoardCount(int IeCd, string RegionCode, string RoleName)
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

            ComplaintStatusDetails(model, Convert.ToString(IeCd), null, RoleName);

            return model;
        }

        public DashboardModel GetVendorDashBoardCount(int Vend_Cd, string RegionCode, string RoleName)
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

            ComplaintStatusDetails(model, Convert.ToString(Vend_Cd), null, RoleName);

            return model;
        }

        public DashboardModel GetClientDashBoardCount(string OrgnType, string Organisation, string RegionCode, string RoleName)
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

            ComplaintStatusDetails(model, Organisation, OrgnType, RoleName);
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
        public DTResult<DashboardLabData> LoadTableInvoice(DTParameters dtParameters, string Regin, int userid)
        {

            DTResult<DashboardLabData> dTResult = new() { draw = 0 };
            IQueryable<DashboardLabData>? query = null;

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

            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("wFrmDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wFrmDtO"), ParameterDirection.Input);
            par[1] = new OracleParameter("wToDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[2] = new OracleParameter("region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[3] = new OracleParameter("Flag", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("Flag"), ParameterDirection.Input);
            par[4] = new OracleParameter("P_USER_ID", OracleDbType.NVarchar2, userid, ParameterDirection.Input);
            par[5] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Dashboard_TotalInvoice", par, 5);

            List<DashboardLabData> modelList = new List<DashboardLabData>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    DashboardLabData model = new DashboardLabData
                    {
                        BPO_NAME = row["BPO_NAME"].ToString(),
                        recipient_gstin_no = row["recipient_gstin_no"].ToString(),
                        St_cd = row["St_cd"].ToString(),
                        invoice_no = row["invoice_no"].ToString(),
                        invoice_dt = row["invoice_dt"].ToString(),
                        INV_TYPE = row["INV_TYPE"].ToString(),
                        HSN_CD = row["HSN_CD"].ToString(),
                        INVOICE_TYPE = row["INVOICE_TYPE"].ToString(),
                        Total_AMT = Convert.ToString(row["Total_AMT"]),
                        INV_amount = Convert.ToString(row["INV_amount"]),
                        INV_sgst = Convert.ToString(row["INV_sgst"]),
                        INV_cgst = Convert.ToString(row["INV_cgst"]),
                        INV_igst = Convert.ToString(row["INV_igst"]),
                        Total_GST = Convert.ToString(row["Total_GST"]),
                        BILL_FINALIZE = row["BILL_FINALIZE"].ToString(),
                        BILL_SENT = row["BILL_SENT"].ToString(),
                        //INC_TYPE = row["INC_TYPE"].ToString(),
                        //IRN_NO = row["IRN_NO"].ToString(),

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
        public DTResult<DashboardModel> Dashboard_Lab_ViewAll_List(DTParameters dtParameters, string Regin, int userid)
        {
            DTResult<DashboardModel> dTResult = new() { draw = 0 };
            IQueryable<DashboardModel>? query = null;

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
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CASE_NO";
                orderAscendingDirection = true;
            }


            string FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : null;
            string ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : null;
            
            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_USER_ID", OracleDbType.Varchar2, userid, ParameterDirection.Input);
            par[1] = new OracleParameter("P_REGION", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[2] = new OracleParameter("P_FROMDATE", OracleDbType.NVarchar2, FromDate, ParameterDirection.Input);
            par[3] = new OracleParameter("P_TODate", OracleDbType.NVarchar2, ToDate,ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            
            var ds = DataAccessDB.GetDataSet("GET_LAB_DASHBOARD_VIEWALL_LIST", par, 4);
            DataTable dt = ds.Tables[0];
            List<DashboardModel> list = new List<DashboardModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                
                    list = dt.AsEnumerable().Select(row => new DashboardModel
                    {
                        CASE_NO = Convert.ToString(row["case_no"]),
                        IE = Convert.ToString(row["ie_name"]),
                        Date = Convert.ToString(row["datetime"]),
                        Vendor = Convert.ToString(row["vend_name"]),
                        SampleRegNo = Convert.ToString(row["sample_reg_no"]),
                        SampleRecDt = Convert.ToString(row["sample_recv_dt"]),
                    }).ToList();
                
            }

            query = list.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.IE).ToLower().Contains(searchBy.ToLower())
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

        public DTResult<CMDFOListing> CMDFO_List(DTParameters dtParameters)
        {
            DTResult<CMDFOListing> dTResult = new() { draw = 0 };
            IQueryable<CMDFOListing>? query = null;

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

            FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : null;
            ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : null;
            ActionType = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ActionType"]) ? Convert.ToString(dtParameters.AdditionalValues["ActionType"]) : null;


            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_ACTION_TYPE", OracleDbType.Varchar2, ActionType, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CM_DFO_DASHBOARD_List", par, 1);
            DataTable dt = ds.Tables[0];
            List<CMDFOListing> list = new();

            if (dt != null && dt.Rows.Count > 0)
            {
                if (ActionType == "OSA")
                {

                }
                else if (ActionType == "SA")
                {
                    list = dt.AsEnumerable().Select(row => new CMDFOListing
                    {
                        CaseNo = Convert.IsDBNull(row["CASE_NO"]) ? string.Empty : Convert.ToString(row["CASE_NO"]),
                        CHQ_DT = Convert.IsDBNull(row["CHQ_DT"]) ? DateTime.MinValue : Convert.ToDateTime(row["CHQ_DT"]),
                        CHQ_NO = Convert.IsDBNull(row["CHQ_NO"]) ? string.Empty : Convert.ToString(row["CHQ_NO"]),
                        NARRATION = Convert.IsDBNull(row["NARRATION"]) ? string.Empty : Convert.ToString(row["NARRATION"]),
                        SUSPENSE_AMT = Convert.IsDBNull(row["SUSPENSE_AMT"]) ? 0 : Convert.ToDecimal(row["SUSPENSE_AMT"]),
                        VCHR_NO = Convert.IsDBNull(row["VCHR_NO"]) ? string.Empty : Convert.ToString(row["VCHR_NO"])
                    }).ToList();

                }
                else if (ActionType == "OB")
                {
                    list = dt.AsEnumerable().Select(row => new CMDFOListing
                    {
                        BILL_NO = Convert.ToString(row["BILL_NO"]),
                        BILL_DT = Convert.ToDateTime(row["BILL_DT"]),
                        CaseNo = Convert.ToString(row["CASE_NO"]),
                        MATERIAL_VALUE = Convert.ToDecimal(row["MATERIAL_VALUE"]),
                        BILL_AMOUNT = Convert.ToDecimal(row["BILL_AMOUNT"]),
                        BILL_STATUS = Convert.ToString(row["BILL_STATUS"]),
                        REMARKS = Convert.ToString(row["REMARKS"])
                    }).ToList();
                }
                else if (ActionType == "TOTI")
                {
                    list = dt.AsEnumerable().Select(row => new CMDFOListing
                    {
                        BILL_NO = Convert.ToString(row["BILL_NO"]),
                        BILL_DT = Convert.ToDateTime(row["BILL_DT"]),
                        CaseNo = Convert.ToString(row["CASE_NO"]),
                        MATERIAL_VALUE = Convert.ToDecimal(row["MATERIAL_VALUE"]),
                        BILL_AMOUNT = Convert.ToDecimal(row["BILL_AMOUNT"]),
                        BILL_STATUS = Convert.ToString(row["BILL_STATUS"]),
                        REMARKS = Convert.ToString(row["REMARKS"])
                    }).ToList();
                }
                else if (ActionType == "FI")
                {
                    list = dt.AsEnumerable().Select(row => new CMDFOListing
                    {
                        BILL_NO = Convert.ToString(row["BILL_NO"]),
                        BILL_DT = Convert.ToDateTime(row["BILL_DT"]),
                        CaseNo = Convert.ToString(row["CASE_NO"]),
                        MATERIAL_VALUE = Convert.ToDecimal(row["MATERIAL_VALUE"]),
                        BILL_AMOUNT = Convert.ToDecimal(row["BILL_AMOUNT"]),
                        BILL_STATUS = Convert.ToString(row["BILL_STATUS"]),
                        REMARKS = Convert.ToString(row["REMARKS"])
                    }).ToList();
                }
                else if (ActionType == "PIF")
                {
                    list = dt.AsEnumerable().Select(row => new CMDFOListing
                    {
                        BILL_NO = Convert.ToString(row["BILL_NO"]),
                        BILL_DT = Convert.ToDateTime(row["BILL_DT"]),
                        CaseNo = Convert.ToString(row["CASE_NO"]),
                        MATERIAL_VALUE = Convert.ToDecimal(row["MATERIAL_VALUE"]),
                        BILL_AMOUNT = Convert.ToDecimal(row["BILL_AMOUNT"]),
                        BILL_STATUS = Convert.ToString(row["BILL_STATUS"]),
                        REMARKS = Convert.ToString(row["REMARKS"])
                    }).ToList();
                }
            }
            
            query = list.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<AdminCountListing> Dashboard_Client_List(DTParameters dtParameters, string Region, string OrgnType, string Organisation)
        {
            DTResult<AdminCountListing> dTResult = new() { draw = 0 };
            IQueryable<AdminCountListing>? query = null;

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
                query = from t17 in context.T17CallRegisters
                        join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo
                        where t13.RlyCd == Organisation &&
                              t13.RlyNonrly == OrgnType &&
                              t17.CallRecvDt >= Convert.ToDateTime(FromDate) &&
                              t17.CallRecvDt <= Convert.ToDateTime(ToDate)
                        select new AdminCountListing
                        {
                            CaseNo = t17.CaseNo,
                            CallRecvDt = t17.CallRecvDt,
                            CallInstallNo = t17.CallInstallNo,
                            CallSno = Convert.ToInt16(t17.CallSno),
                            CallStatus = t17.CallStatus,
                            CallLetterNo = t17.CallLetterNo,
                            Remarks = t17.Remarks,
                            PoNo = t13.PoNo,
                            PoDt = t13.PoDt,
                            RegionCode = t17.RegionCode,
                        };
            }
            else if (ActionType == "M" || ActionType == "C" || ActionType == "U" || ActionType == "S" || ActionType == "T")
            {
                query = from t17 in context.T17CallRegisters
                        join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo
                        where t13.RlyCd == Organisation &&
                              t13.RlyNonrly == OrgnType &&
                              t17.CallRecvDt >= Convert.ToDateTime(FromDate) &&
                              t17.CallRecvDt <= Convert.ToDateTime(ToDate) &&
                              t17.CallStatus == ActionType
                        select new AdminCountListing
                        {
                            CaseNo = t17.CaseNo,
                            CallRecvDt = t17.CallRecvDt,
                            CallInstallNo = t17.CallInstallNo,
                            CallSno = Convert.ToInt16(t17.CallSno),
                            CallStatus = t17.CallStatus,
                            CallLetterNo = t17.CallLetterNo,
                            Remarks = t17.Remarks,
                            PoNo = t13.PoNo,
                            PoDt = t13.PoDt,
                            RegionCode = t17.RegionCode,
                        };


            }
            else if (ActionType == "A")
            {
                query = from t47 in context.T47IeWorkPlans
                        join t17 in context.T17CallRegisters
                        on new { t47.CaseNo, t47.CallRecvDt, t47.CallSno } equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                        join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo
                        where t17.CallStatus.Trim() == "A" &&
                              t13.RlyCd == Organisation &&
                              t13.RlyNonrly == OrgnType &&
                              t17.CallRecvDt >= Convert.ToDateTime(FromDate) &&
                              t17.CallRecvDt <= Convert.ToDateTime(ToDate)
                        select new AdminCountListing
                        {
                            CaseNo = t17.CaseNo,
                            CallRecvDt = t17.CallRecvDt,
                            CallInstallNo = t17.CallInstallNo,
                            CallSno = Convert.ToInt16(t17.CallSno),
                            CallStatus = t17.CallStatus,
                            CallLetterNo = t17.CallLetterNo,
                            Remarks = t17.Remarks,
                            PoNo = t13.PoNo,
                            PoDt = t13.PoDt,
                            RegionCode = t17.RegionCode,
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

        public DTResult<AdminCountListing> GetDataListTotalCallListing(DTParameters dtParameters, string Region)
        {
            DTResult<AdminCountListing> dTResult = new() { draw = 0 };
            IQueryable<AdminCountListing>? query = null;

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
                        where (l.CallRecvDt >= Convert.ToDateTime(FromDate) && l.CallRecvDt <= Convert.ToDateTime(ToDate)) && l.RegionCode == Region
                        orderby l.CaseNo, l.CallRecvDt
                        select new AdminCountListing
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
            else if (ActionType == "M" || ActionType == "A" || ActionType == "C" || ActionType == "U" || ActionType == "S" || ActionType == "T")
            {
                query = from l in context.ViewGetCallRegCancellations
                        where (l.CallRecvDt >= Convert.ToDateTime(FromDate) && l.CallRecvDt <= Convert.ToDateTime(ToDate)) && l.RegionCode == Region
                              && l.CStatus == ActionType
                        orderby l.CaseNo, l.CallRecvDt
                        select new AdminCountListing
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
            else if (ActionType == "TB")
            {
                query = from l in context.T22Bills
                        where (l.BillDt >= Convert.ToDateTime(FromDate) && l.BillDt <= Convert.ToDateTime(ToDate)) && l.CaseNo.StartsWith(Region)
                        select new AdminCountListing
                        {
                            CaseNo = l.CaseNo,
                            BILLDT = l.BillDt,
                            billamount = l.BillAmount,
                            BILLNO = l.BillNo,
                            Remarks = l.Remarks,
                        };

            }
            else if (ActionType == "ICNR")
            {
                query = from t20 in context.T20Ics
                        join t30 in context.T30IcReceiveds
                        on new { t20.BkNo, t20.SetNo } equals new { t30.BkNo, t30.SetNo } into t30Group
                        from t30 in t30Group.DefaultIfEmpty()
                        where t20.CaseNo.StartsWith(Region) &&
                                   t20.CallRecvDt >= Convert.ToDateTime(FromDate) &&
                                   t20.CallRecvDt <= Convert.ToDateTime(ToDate) //&& t30 == null
                        select new AdminCountListing
                        {
                            CaseNo = t20.CaseNo,
                            CallRecvDt = t20.CallRecvDt,
                            CallSno = t20.CallSno,
                            IC_NO = t20.IcNo,
                            IC_DT = t20.IcDt,
                            BKNO = t20.BkNo,
                            SETNO = t20.SetNo,
                        };

                query.Distinct();

            }
            else if (ActionType == "ICRNB")
            {
                query = from t20 in context.T20Ics
                        join t30 in context.T30IcReceiveds on new { t20.BkNo, t20.SetNo } equals new { t30.BkNo, t30.SetNo }
                        join t22 in context.T22Bills on t20.CaseNo equals t22.CaseNo into t22Group
                        from t22 in t22Group.DefaultIfEmpty()
                        where t30.Region == Region &&
                             (t20.CallRecvDt >= Convert.ToDateTime(FromDate) && t20.CallRecvDt <= Convert.ToDateTime(ToDate))
                        select new AdminCountListing
                        {
                            CaseNo = t20.CaseNo,
                            CallRecvDt = t20.CallRecvDt,
                            CallSno = t20.CallSno,
                            IC_NO = t20.IcNo,
                            IC_DT = t20.IcDt,
                            BKNO = t20.BkNo,
                            SETNO = t20.SetNo,
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
                        join t13 in context.T13PoMasters on T17.CaseNo equals t13.CaseNo
                        where T17.CallRecvDt >= Convert.ToDateTime(FromDate) && T17.CallRecvDt <= Convert.ToDateTime(ToDate) && T17.OnlineCall != "Y"
                        select new VenderCallRegisterModel
                        {
                            CaseNo = T17.CaseNo,
                            CallRecvDt = T17.CallRecvDt,
                            CallLetterDt = T17.CallLetterDt,
                            CallLetterNo = T17.CallLetterNo,
                            CallSno = Convert.ToInt16(T17.CallSno),
                            Remarks = T17.Remarks,
                            CallStatus = T17.CallStatus == "M" ? "Pending" : T17.CallStatus == "A" ? "Accepted" : T17.CallStatus == "R" ? "Rejection" : T17.CallStatus == "C" ? "Cancelled" : T17.CallStatus == "U" ? "Under Lab Testing" : T17.CallStatus == "S" ? "Still Under Inspection" : T17.CallStatus == "G" ? "Stage Inspection Accepted" : T17.CallStatus == "B" ? "Accepted and Billed" : T17.CallStatus == "T" ? "Stage Rejection" : "Withheld",
                        };
            }
            else if (ActionType == "TC")
            {
                query = from T17 in context.T17CallRegisters
                        where (T17.CallRecvDt >= Convert.ToDateTime(FromDate) && T17.CallRecvDt <= Convert.ToDateTime(ToDate))
                        select new VenderCallRegisterModel
                        {
                            CaseNo = T17.CaseNo,
                            CallRecvDt = T17.CallRecvDt,
                            CallLetterDt = T17.CallLetterDt,
                            CallLetterNo = T17.CallLetterNo,
                            CallSno = Convert.ToInt16(T17.CallSno),
                            Remarks = T17.Remarks,
                            CallStatus = T17.CallStatus == "M" ? "Pending" : T17.CallStatus == "A" ? "Accepted" : T17.CallStatus == "R" ? "Rejection" : T17.CallStatus == "C" ? "Cancelled" : T17.CallStatus == "U" ? "Under Lab Testing" : T17.CallStatus == "S" ? "Still Under Inspection" : T17.CallStatus == "G" ? "Stage Inspection Accepted" : T17.CallStatus == "B" ? "Accepted and Billed" : T17.CallStatus == "T" ? "Stage Rejection" : "Withheld",
                        };
            }
            else if (ActionType == "MCM")
            {
                query = from T17 in context.T17CallRegisters
                        where (T17.CallRecvDt >= Convert.ToDateTime(FromDate) && T17.CallRecvDt <= Convert.ToDateTime(ToDate))
                        && T17.OnlineCall == "Y"
                        select new VenderCallRegisterModel
                        {
                            CaseNo = T17.CaseNo,
                            CallRecvDt = T17.CallRecvDt,
                            CallLetterDt = T17.CallLetterDt,
                            CallLetterNo = T17.CallLetterNo,
                            CallSno = Convert.ToInt16(T17.CallSno),
                            Remarks = T17.Remarks,
                            CallStatus = T17.CallStatus == "M" ? "Pending" : T17.CallStatus == "A" ? "Accepted" : T17.CallStatus == "R" ? "Rejection" : T17.CallStatus == "C" ? "Cancelled" : T17.CallStatus == "U" ? "Under Lab Testing" : T17.CallStatus == "S" ? "Still Under Inspection" : T17.CallStatus == "G" ? "Stage Inspection Accepted" : T17.CallStatus == "B" ? "Accepted and Billed" : T17.CallStatus == "T" ? "Stage Rejection" : "Withheld",
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
                            CallLetterDt = T17.CallLetterDt,
                            CallLetterNo = T17.CallLetterNo,
                            CallSno = Convert.ToInt16(T17.CallSno),
                            Remarks = T17.Remarks,
                            CallStatus = T17.CallStatus == "M" ? "Pending" : T17.CallStatus == "A" ? "Accepted" : T17.CallStatus == "R" ? "Rejection" : T17.CallStatus == "C" ? "Cancelled" : T17.CallStatus == "U" ? "Under Lab Testing" : T17.CallStatus == "S" ? "Still Under Inspection" : T17.CallStatus == "G" ? "Stage Inspection Accepted" : T17.CallStatus == "B" ? "Accepted and Billed" : T17.CallStatus == "T" ? "Stage Rejection" : "Withheld",
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

        public DataSet ComplaintStatusSummary(string Value1, string Value2, string RoleName)
        {
            string Region = null, Rly_CD = null, Rly_NonRly = null;
            int Ie_Cd = 0, Vend_Cd = 0;
            if (RoleName == "admin") { Region = Value1; }
            else if (RoleName == "inspection engineer (ie)") { Ie_Cd = Convert.ToInt32(Value1); }
            else if (RoleName == "vendor") { Vend_Cd = Convert.ToInt32(Value1); }
            else if (RoleName == "client") { Rly_CD = Value1; Rly_NonRly = Value2; }
            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("P_ROLE_NAME", OracleDbType.Varchar2, RoleName, ParameterDirection.Input);
            par[1] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[2] = new OracleParameter("P_IE_CD", OracleDbType.Int32, Ie_Cd, ParameterDirection.Input);
            par[3] = new OracleParameter("P_VEND_CD", OracleDbType.Int32, Vend_Cd, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Rly_CD, ParameterDirection.Input);
            par[5] = new OracleParameter("P_RLY_NONRLY", OracleDbType.Varchar2, Rly_NonRly, ParameterDirection.Input);
            par[6] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

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

            List<PO_MasterModel> model = new();
            OracleParameter[] par = new OracleParameter[1];
            par[0] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("GET_CM_DASHBOARD_POMASTERLIST", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model = JsonConvert.DeserializeObject<List<PO_MasterModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            if (!string.IsNullOrEmpty(searchBy))
            {
                model = model
                    .Where(w =>
                        w.VendorName != null && w.VendorName.ToLower().Contains(searchBy.ToLower()) ||
                        w.ConsigneeSName != null && w.ConsigneeSName.ToLower().Contains(searchBy.ToLower()) ||
                        w.Remarks != null && w.Remarks.ToLower().Contains(searchBy.ToLower())
                    )
                    .ToList();
            }

            query = model.AsQueryable();
            dTResult.recordsTotal = query.Count();
            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public void ComplaintStatusDetails(DashboardModel model, string Value1, string Value2, string RoleName)
        {
            DataSet ds2 = ComplaintStatusSummary(Value1, Value2, RoleName);

            model.complaintStatusSummaryModel = new();

            if (ds2 != null && ds2.Tables.Count > 0)
            {
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    //model.complaintStatusSummaryModel.REGION = Convert.ToString(ds2.Tables[0].Rows[0]["REGION"]);
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

        public DTResult<VendorDetailListModel> GetDataVendorListing(DTParameters dtParameters, string Vend_Cd)
        {
            DTResult<VendorDetailListModel> dTResult = new() { draw = 0 };
            IQueryable<VendorDetailListModel>? query = null;

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
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CASE_NO";
                orderAscendingDirection = true;
            }


            string FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : null;
            string ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : null;
            //string VEND_CD = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Vend_CD"]) ? Convert.ToString(dtParameters.AdditionalValues["Vend_CD"]) : null;
            string Status = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Status"]) ? Convert.ToString(dtParameters.AdditionalValues["Status"]) : null;

            Status = Status == "TC" ? null : Status;

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_VENDCD", OracleDbType.Varchar2, Vend_Cd.Substring(0, 8), ParameterDirection.Input);
            par[3] = new OracleParameter("P_STATUS", OracleDbType.Varchar2, Status, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_VENDOR_DASHBOARD_COUNT_LISTING", par, 1);
            DataTable dt = ds.Tables[0];
            List<VendorDetailListModel> list = new List<VendorDetailListModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                list = dt.AsEnumerable().Select(row => new VendorDetailListModel
                {
                    CASE_NO = Convert.ToString(row["CASE_NO"]),
                    CALL_RECV_DT = Convert.ToDateTime(row["CALL_RECV_DT"]),
                    CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                    IE_NAME = Convert.ToString(row["IE_NAME"]),
                    CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                    PO_NO = Convert.ToString(row["PO_NO"])
                }).ToList();
            }

            query = list.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CASE_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.IE_NAME).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CLIENT_NAME).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<AdminViewAllList> Dashboard_Admin_ViewAll_List(DTParameters dtParameters, string RegionCode)
        {
            DTResult<AdminViewAllList> dTResult = new() { draw = 0 };
            IQueryable<AdminViewAllList>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            string FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : null;
            string ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : null;
            string Status = !string.IsNullOrEmpty(dtParameters.AdditionalValues["TypeOfList"]) ? Convert.ToString(dtParameters.AdditionalValues["TypeOfList"]) : null;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
            }

            if (Status == "CHP" || Status == "CHO")
            {
                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "Value";
                }
                else
                {
                    // if we have an empty search then just order the results by Id ascending
                    orderCriteria = "Value";
                    orderAscendingDirection = true;
                }
            }else if (Status == "OPC" || Status == "OJC")
            {
                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "CallDate";
                }
                else
                {
                    // if we have an empty search then just order the results by Id ascending
                    orderCriteria = "CallDate";
                    orderAscendingDirection = true;
                }
            }

                

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_REGION", OracleDbType.Varchar2, RegionCode, ParameterDirection.Input);
            par[1] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_TODate", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[3] = new OracleParameter("P_ACTION_TYPE", OracleDbType.Varchar2, Status, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_ADMIN_DASHBOARD_VIEWALL_LIST", par, 1);
            DataTable dt = ds.Tables[0];
            List<AdminViewAllList> list = new List<AdminViewAllList>();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Status == "CHP" || Status == "CHO")
                {
                    list = dt.AsEnumerable().Select(row => new AdminViewAllList
                    {
                        ClientName = Convert.ToString(row["CLIENT_NAME"]),
                        NoofBills = Convert.ToInt32(row["NO_OF_BILL"]),
                        Value = Convert.ToDecimal(row["AMOUNT"]),
                    }).ToList();
                }
                else if (Status == "OPC" || Status == "OJC")
                {
                    list = dt.AsEnumerable().Select(row => new AdminViewAllList
                    {
                        PONO = Convert.ToString(row["PO_NO"]),
                        CaseNo = Convert.ToString(row["CASE_NO"]),
                        CallDate = Convert.ToDateTime(row["CALL_DATE"])
                    }).ToList();
                }
            }

            query = list.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.ClientName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<VendorViewAllList> Dashboard_Vendor_ViewAll_List(DTParameters dtParameters, string RegionCode, int Vend_Cd)
        {
            DTResult<VendorViewAllList> dTResult = new() { draw = 0 };
            IQueryable<VendorViewAllList>? query = null;

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
                orderCriteria = "CaseNo";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }


            string FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : null;
            string ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : null;
            string Status = !string.IsNullOrEmpty(dtParameters.AdditionalValues["TypeOfList"]) ? Convert.ToString(dtParameters.AdditionalValues["TypeOfList"]) : null;



            OracleParameter[] par1 = new OracleParameter[5];

            par1[0] = new OracleParameter("P_VEND_CD", OracleDbType.NVarchar2, Vend_Cd, ParameterDirection.Input);
            par1[1] = new OracleParameter("P_FROMDATE", OracleDbType.NVarchar2, FromDate, ParameterDirection.Input);
            par1[2] = new OracleParameter("P_TODATE", OracleDbType.NVarchar2, ToDate, ParameterDirection.Input);
            par1[3] = new OracleParameter("P_ACTION_TYPE", OracleDbType.NVarchar2, Status, ParameterDirection.Input);
            par1[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds1 = DataAccessDB.GetDataSet("GET_VENDOR_DASHBOARD_VIEWALL_LIST", par1);

            List<VendorViewAllList> listVend = new();
            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (Status == "RCS")
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds1.Tables[0];
                        listVend = dt.AsEnumerable().Select(row => new VendorViewAllList
                        {
                            CaseNo = Convert.ToString(row["CASE_NO"]),
                            CallDate = Convert.ToDateTime(row["CALL_RECV_DT"]),
                            CallSno = Convert.ToString(row["CALL_SNO"]),
                            Details = Convert.ToString(row["DETAILS"]),
                            Client = Convert.ToString(row["CLIENT_NAME"]),
                            IE = Convert.ToString(row["IE_NAME"]),
                            IEContactNo = Convert.ToString(row["IE_PHONE_NO"]),
                            CM = Convert.ToString(row["CO_NAME"]),
                            CmContactNo = Convert.ToString(row["CO_PHONE_NO"])
                        }).ToList();
                    }
                }
                else if (Status == "RPO")
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds1.Tables[0];
                        listVend = dt.AsEnumerable().Select(row => new VendorViewAllList
                        {
                            CaseNo = Convert.ToString(row["CASE_NO"]),
                            CallDate = Convert.ToDateTime(row["CALL_RECV_DT"]),
                            Details = Convert.ToString(row["DETAILS"]),
                            Client = Convert.ToString(row["CLIENT_NAME"]),
                            PONO = Convert.ToString(row["PO_NO"]),
                            PurchaseOrder = Convert.ToString(row["PURCHASE_ORDER"]),
                            Status = Convert.ToString(row["CALL_STATUS"])
                        }).ToList();
                    }
                }
            }

            query = listVend.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public DTResult<IEViewAllList> Dashboard_IE_ViewAll_List(DTParameters dtParameters, int IE_CD, string RegionCode)
        {
            DTResult<IEViewAllList> dTResult = new() { draw = 0 };
            IQueryable<IEViewAllList>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
            }

            if (orderCriteria == "" || orderCriteria == null)
            {
                orderCriteria = "CaseNo";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }


            string FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : null;
            string ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : null;
            string Status = !string.IsNullOrEmpty(dtParameters.AdditionalValues["TypeOfList"]) ? Convert.ToString(dtParameters.AdditionalValues["TypeOfList"]) : null;



            OracleParameter[] par1 = new OracleParameter[4];

            par1[0] = new OracleParameter("P_IE_CD", OracleDbType.Int32, IE_CD, ParameterDirection.Input);
            par1[1] = new OracleParameter("P_FROMDATE", OracleDbType.NVarchar2, FromDate, ParameterDirection.Input);
            par1[2] = new OracleParameter("P_TODATE", OracleDbType.NVarchar2, ToDate, ParameterDirection.Input);
            par1[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds1 = DataAccessDB.GetDataSet("GET_IE_DASHBOARD_VIEWALL_LIST", par1);

            List<IEViewAllList> listIE = new();
            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (Status == "IFI")
                {

                    var query1 = from l in context.T72IeMessages
                                 where l.RegionCode == RegionCode && (l.Isdeleted == 0 || l.Isdeleted == null)
                                 select new IEViewAllList
                                 {
                                     MessageID = l.MessageId,
                                     LetterNo = l.LetterNo,
                                     LetterDt = l.LetterDt,
                                     Message = l.Message,
                                     MessageDt = l.MessageDt,
                                 };
                    listIE = query1.ToList();
                }
                else if (Status == "PC")
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds1.Tables[0];
                        listIE = dt.AsEnumerable().Select(row => new IEViewAllList
                        {
                            CaseNo = Convert.ToString(row["CASE_NO"]),
                            CallDate = Convert.ToDateTime(row["CALL_RECV_DT"]),
                            CallSno = Convert.ToString(row["CALL_SNO"]),
                            InspDate = Convert.ToDateTime(row["INSP_DESIRE_DT"]),
                            Client = Convert.ToString(row["CLIENT_NAME"]),
                            Vendor = Convert.ToString(row["VEND_NAME"]),
                            ContactPerson = Convert.ToString(row["CONTACT_PER"]),
                            ContactNo = Convert.ToString(row["CONTACT_NO"])
                        }).ToList();
                    }

                }
            }

            query = listIE.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

       
        public DashboardModel GetLODashBoardCount(string UserName)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_mobile", OracleDbType.Varchar2, UserName, ParameterDirection.Input);
            par[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_LO_DASHBOARD", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.TotalBillCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_BILL"]);
                    model.TotalOutstandingCount = Convert.ToInt32(ds.Tables[0].Rows[0]["OUTST_COUNT"]);
                    model.TotalPassedCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PASSED_COUNT"]);
                    model.TotalBillRupees = Convert.ToDecimal(ds.Tables[0].Rows[0]["AMOUNT_RECEIVED"]);
                }
            }
            return model;
        }

        public DTResult<CLientViewAllList> Dashboard_Client_ViewAll_List(DTParameters dtParameters, string RegionCode, string OrgnType, string Organisation)
        {
            DTResult<CLientViewAllList> dTResult = new() { draw = 0 };
            IQueryable<CLientViewAllList>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

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

            if (ActionType == "VWP")
            {
                if (dtParameters.Order != null)
                {
                    // in this example we just default sort on the 1st column
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                    if (orderCriteria == "" || orderCriteria == null)
                    {
                        orderCriteria = "Vendor";
                    }
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                }
                else
                {
                    orderCriteria = "Vendor";
                    orderAscendingDirection = true;
                }
            }
            else if (ActionType == "RRS" || ActionType == "RPO" || ActionType == "RCC")
            {
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
            }


            if (ActionType == "VWP")
            {
                query = (from t17 in context.T17CallRegisters
                         join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo
                         join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                         where t13.RlyCd == Organisation
                            && t13.RlyNonrly == OrgnType
                            && (t17.CallRecvDt >= Convert.ToDateTime(FromDate) && t17.CallRecvDt <= Convert.ToDateTime(ToDate))
                         group new { t17, t13, t05 } by new { RLY_CD = OrgnType, RLY_NONRLY = Organisation, t05.VendName } into grouped
                         orderby grouped.Count() descending
                         select new CLientViewAllList
                         {
                             Vendor = grouped.Key.VendName,
                             TotalCalls = grouped.Count(),
                             CallRejected = grouped.Sum(x => x.t17.CallStatus == "T" || x.t17.CallStatus == "R" ? 1 : 0),
                             CallCancelled = grouped.Sum(x => x.t17.CallStatus == "C" ? 1 : 0)
                         });

            }
            else if (ActionType == "RRS")
            {
                query = (from t17 in context.T17CallRegisters
                         join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo
                         join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                         join t09 in context.T09Ies on t17.IeCd equals t09.IeCd
                         join t21 in context.T21CallStatusCodes on t17.CallStatus.Trim() equals t21.CallStatusCd.Trim()
                         where t13.RlyCd == Organisation
                            && t13.RlyNonrly == OrgnType
                            && (t17.CallRecvDt >= Convert.ToDateTime(FromDate) &&
                                t17.CallRecvDt <= Convert.ToDateTime(ToDate))
                         orderby t17.CallRecvDt descending
                         select new CLientViewAllList
                         {
                             CaseNo = t17.CaseNo,
                             CallDate = t17.CallRecvDt,
                             CallSno = t17.CallSno,
                             Vendor = t05.VendName,
                             IEName = t09.IeName,
                             Status = t21.CallStatusDesc
                         });


            }
            else if (ActionType == "RPO")
            {
                query = (from t13 in context.T13PoMasters
                         join t15 in context.T15PoDetails on t13.CaseNo equals t15.CaseNo
                         join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                         where t13.RlyCd == Organisation
                            && t13.RlyNonrly == OrgnType
                            && (t13.PoDt >= Convert.ToDateTime(FromDate) &&
                                t13.PoDt <= Convert.ToDateTime(ToDate))
                         group t15 by new
                         {
                             t13.CaseNo,
                             t13.PoNo,
                             t13.PoDt,
                             t05.VendName
                         } into grouped
                         orderby grouped.Key.PoDt descending
                         select new CLientViewAllList
                         {
                             CaseNo = grouped.Key.CaseNo,
                             Qty = grouped.Sum(x => x.Value),
                             PONO = grouped.Key.PoNo,
                             PODT = grouped.Key.PoDt,
                             Vendor = grouped.Key.VendName
                         });
            }
            else if (ActionType == "RCC")
            {
                query = (from t13 in context.T13PoMasters
                         join t20 in context.T20Ics on t13.CaseNo equals t20.CaseNo
                         join c in context.T40ConsigneeComplaints on t20.CaseNo equals c.CaseNo
                         join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
                         where t13.RlyCd == Organisation
                            && t13.RlyNonrly == OrgnType
                            && (t13.PoDt >= Convert.ToDateTime(FromDate) &&
                                t13.PoDt <= Convert.ToDateTime(ToDate))
                         group c by new
                         {
                             t13.CaseNo,
                             t05.VendName
                         } into grouped
                         orderby grouped.Count() descending
                         select new CLientViewAllList
                         {
                             Vendor = grouped.Key.VendName,
                             CaseNo = grouped.Key.CaseNo,
                             NoOfComplaints = grouped.Count()
                         });
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

        public DTResult<LoListingModel> GetLoCallListingDetails(DTParameters dtParameters, string UserName)
        {
            DTResult<LoListingModel> dTResult = new() { draw = 0 };
            IQueryable<LoListingModel>? query = null;

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

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_mobile", OracleDbType.Varchar2, UserName, ParameterDirection.Input);
            par[1] = new OracleParameter("p_start_date", OracleDbType.Date, Convert.ToDateTime(FromDate), ParameterDirection.Input);
            par[2] = new OracleParameter("p_end_date", OracleDbType.Date, Convert.ToDateTime(ToDate), ParameterDirection.Input);
            par[3] = new OracleParameter("p_flag", OracleDbType.Varchar2, ActionType, ParameterDirection.Input);
            par[4] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_LO_DASHBOARD_LIST", par);

            List<LoListingModel> modelList = new List<LoListingModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LoListingModel model = new LoListingModel
                    {
                        BillNo = row["BILL_NO"].ToString(),
                        CaseNo = row["CASE_NO"].ToString(),
                        BillAmount = Convert.ToString(row["BILL_AMOUNT"]),
                        AmountReceived = Convert.ToString(row["AMOUNT_RECEIVED"]),
                        PassedOutst = Convert.ToString(row["PASSED_OUTST"]),
                    };
                    modelList.Add(model);
                }
            }
            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DashboardModel GetCMGeneralDashBoard(int CO_CD)
        {
            DashboardModel model = new DashboardModel();
            OracleParameter[] par = new OracleParameter[2];

            par[0] = new OracleParameter("P_COCD", OracleDbType.Int32, CO_CD, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds1 = DataAccessDB.GetDataSet("GET_CM_DASHBOARD_IE_WISE_PERFOMANCE", par);
            List<DashboardModel> lstIEPer = new();
            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds1.Tables[0];
                    lstIEPer = dt.AsEnumerable().Select(row => new DashboardModel
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
            model.IEWisePerformance = lstIEPer;

            return model;
        }

        public DashboardModel GetCMJIDDashBoard(int CO_CD)
        {
            DashboardModel model = new DashboardModel();
            OracleParameter[] par = new OracleParameter[2];

            par[0] = new OracleParameter("P_COCD", OracleDbType.Int32, CO_CD, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds1 = DataAccessDB.GetDataSet("GET_CM_DASHBOARD_IE_WISE_PERFOMANCE", par);
            List<DashboardModel> lstIEPer = new();
            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds1.Tables[0];
                    lstIEPer = dt.AsEnumerable().Select(row => new DashboardModel
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
            model.IEWisePerformance = lstIEPer;


            OracleParameter[] par2 = new OracleParameter[7];
            par2[0] = new OracleParameter("P_RESULT_PENDING_JI_CASES", OracleDbType.RefCursor, ParameterDirection.Output);
            par2[1] = new OracleParameter("P_RESULT_IE_WISE_CONG_COMP", OracleDbType.RefCursor, ParameterDirection.Output);
            par2[2] = new OracleParameter("P_RESULT_VENDOR_WISE_CONG_COMP", OracleDbType.RefCursor, ParameterDirection.Output);
            par2[3] = new OracleParameter("P_RESULT_CLIENT_WISE_CONG_COMP", OracleDbType.RefCursor, ParameterDirection.Output);
            par2[4] = new OracleParameter("P_RESULT_INTER_REGION_JI_COMP", OracleDbType.RefCursor, ParameterDirection.Output);
            par2[5] = new OracleParameter("P_RESULT_DEFECT_CODE_WISE_JI_COMP", OracleDbType.RefCursor, ParameterDirection.Output);
            par2[6] = new OracleParameter("P_RESULT_NO_OF_JI", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds2 = DataAccessDB.GetDataSet("GET_CM_JI_DASHBOARD", par2);

            if (ds2 != null && ds2.Tables.Count > 0)
            {
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    DataTable dt1 = ds2.Tables[0];
                    List<CM_Odlest_Pending_JI_Cases_Model> oldestPendingJICases = dt1.AsEnumerable().Select(row => new CM_Odlest_Pending_JI_Cases_Model
                    {
                        CONSIGNEE = Convert.ToString(row["CONSIGNEE"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        JI_REGION = Convert.ToString(row["JI_REGION"]),
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DT = string.IsNullOrEmpty(Convert.ToString(row["CALL_RECV_DT"])) ? null : Convert.ToDateTime(row["CALL_RECV_DT"]),
                        CALL_SNO = Convert.ToInt32(row["CALL_SNO"]),
                        JI_SNO = Convert.ToString(row["JI_SNO"]),
                        JI_DT = string.IsNullOrEmpty(Convert.ToString(row["JI_DT"])) ? null : Convert.ToDateTime(row["JI_DT"])
                    }).ToList();
                    model.oldestPendingJICases = oldestPendingJICases;
                }

                if (ds2.Tables[1].Rows.Count > 0)
                {
                    DataTable dt2 = ds2.Tables[1];
                    List<CM_NO_OF_Cons_Comp_Model> ieNoOfComp = dt2.AsEnumerable().Select(row => new CM_NO_OF_Cons_Comp_Model
                    {
                        NAME = Convert.ToString(row["NAME"]),
                        NO_OF_CONSINEE_COMPLAINTS = Convert.ToInt32(row["NO_OF_CONSINEE_COMPLAINTS"])
                    }).ToList();
                    model.ieNoOfComp = ieNoOfComp;
                }

                if (ds2.Tables[2].Rows.Count > 0)
                {
                    DataTable dt3 = ds2.Tables[2];
                    List<CM_NO_OF_Cons_Comp_Model> vendorNoOfComp = dt3.AsEnumerable().Select(row => new CM_NO_OF_Cons_Comp_Model
                    {
                        NAME = Convert.ToString(row["NAME"]),
                        NO_OF_CONSINEE_COMPLAINTS = Convert.ToInt32(row["NO_OF_CONSINEE_COMPLAINTS"])
                    }).ToList();
                    model.vendorNoOfComp = vendorNoOfComp;
                }

                if (ds2.Tables[3].Rows.Count > 0)
                {
                    DataTable dt4 = ds2.Tables[3];
                    List<CM_NO_OF_Cons_Comp_Model> clientNoOfComp = dt4.AsEnumerable().Select(row => new CM_NO_OF_Cons_Comp_Model
                    {
                        NAME = Convert.ToString(row["NAME"]),
                        NO_OF_CONSINEE_COMPLAINTS = Convert.ToInt32(row["NO_OF_CONSINEE_COMPLAINTS"])
                    }).ToList();
                    model.clientNoOfComp = clientNoOfComp;
                }

                if (ds2.Tables[4].Rows.Count > 0)
                {
                    DataTable dt5 = ds2.Tables[4];

                    Inter_Region_JI_Cons_Comp_Model interRegionJIComp = dt5.AsEnumerable().Select(row => new Inter_Region_JI_Cons_Comp_Model
                    {
                        EE = Convert.ToInt32(row["EE"]),
                        EN = Convert.ToInt32(row["EN"]),
                        ES = Convert.ToInt32(row["ES"]),
                        EW = Convert.ToInt32(row["EW"]),
                        NE = Convert.ToInt32(row["NE"]),
                        NN = Convert.ToInt32(row["NN"]),
                        NS = Convert.ToInt32(row["NS"]),
                        NW = Convert.ToInt32(row["NW"]),
                        SE = Convert.ToInt32(row["SE"]),
                        SN = Convert.ToInt32(row["SN"]),
                        SS = Convert.ToInt32(row["SS"]),
                        SW = Convert.ToInt32(row["SW"]),
                        WE = Convert.ToInt32(row["WE"]),
                        WN = Convert.ToInt32(row["WN"]),
                        WS = Convert.ToInt32(row["WS"]),
                        WW = Convert.ToInt32(row["WW"])
                    }).FirstOrDefault();
                    model.interRegionJIComp = interRegionJIComp;
                }

                if (ds2.Tables[5].Rows.Count > 0)
                {
                    DataTable dt6 = ds2.Tables[5];

                    CM_Defect_Code_JI_Comp_Model defectCodeJIComp = dt6.AsEnumerable().Select(row => new CM_Defect_Code_JI_Comp_Model
                    {
                        VISUAL = Convert.ToInt32(row["VISUAL"]),
                        DIAMENSIONAL = Convert.ToInt32(row["DIAMENSIONAL"]),
                        CHEMICAL_COMPOSITION = Convert.ToInt32(row["CHEMICAL_COMPOSITION"]),
                        PHYSICAL = Convert.ToInt32(row["PHYSICAL"]),
                        SURFACE = Convert.ToInt32(row["SURFACE"]),
                        LOAD_PERFORMANCE = Convert.ToInt32(row["LOAD_PERFORMANCE"]),
                        NDT = Convert.ToInt32(row["NDT"]),
                        MACRO_MICRO = Convert.ToInt32(row["MACRO_MICRO"]),
                        ELECTRICAL = Convert.ToInt32(row["ELECTRICAL"]),
                        WELDING = Convert.ToInt32(row["WELDING"]),
                        OTHER = Convert.ToInt32(row["OTHER"]),
                        //TOTAL = VISUAL + DIAMENSIONAL + CHEMICAL_COMPOSITION + PHYSICAL + SURFACE + LOAD_PERFORMANCE + NDT + MACRO_MICRO + ELECTRICAL + WELDING + OTHER
                    }).FirstOrDefault();
                    model.defectCodeJIComp = defectCodeJIComp;
                }

                model.DefectCodeJISummary = "[";
                model.DefectCodeJISummary += "['V'," + model.defectCodeJIComp.VISUAL + "],";
                model.DefectCodeJISummary += "['D'," + model.defectCodeJIComp.DIAMENSIONAL + "],";
                model.DefectCodeJISummary += "['C'," + model.defectCodeJIComp.CHEMICAL_COMPOSITION + "],";
                model.DefectCodeJISummary += "['P'," + model.defectCodeJIComp.PHYSICAL + "],";
                model.DefectCodeJISummary += "['S'," + model.defectCodeJIComp.SURFACE + "],";
                model.DefectCodeJISummary += "['L'," + model.defectCodeJIComp.LOAD_PERFORMANCE + "],";
                model.DefectCodeJISummary += "['N'," + model.defectCodeJIComp.NDT + "],";
                model.DefectCodeJISummary += "['M'," + model.defectCodeJIComp.MACRO_MICRO + "],";
                model.DefectCodeJISummary += "['E'," + model.defectCodeJIComp.ELECTRICAL + "],";
                model.DefectCodeJISummary += "['W'," + model.defectCodeJIComp.WELDING + "],";
                model.DefectCodeJISummary += "['O'," + model.defectCodeJIComp.OTHER + "]";
                model.DefectCodeJISummary += "]";

                if (ds2.Tables[6].Rows.Count > 0)
                {
                    DataTable dt = ds2.Tables[6];
                    No_Of_JI_Model noOfJI = dt.AsEnumerable().Select(row => new No_Of_JI_Model
                    {
                        NR = Convert.ToInt32(row["NR"]),
                        WR = Convert.ToInt32(row["WR"]),
                        SR = Convert.ToInt32(row["SR"]),
                        ER = Convert.ToInt32(row["ER"])
                    }).FirstOrDefault();
                    model.noOfJI = noOfJI;
                }

                model.NoOfJISummary = "[";
                model.NoOfJISummary += "['NR'," + model.noOfJI.NR + "],";
                model.NoOfJISummary += "['WR'," + model.noOfJI.WR + "],";
                model.NoOfJISummary += "['SR'," + model.noOfJI.SR + "],";
                model.NoOfJISummary += "['ER'," + model.noOfJI.ER + "]";
                model.NoOfJISummary += "]";
            }
            return model;
        }

        public DashboardModel GetCMDARDashBoard(int CO_CD)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[1]; //[7];
            par[0] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_CM_DAR_DASHBOARD", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.ConsigneeCompaintCount = Convert.ToInt32(ds.Tables[0].Rows[0]["CONS_COMP_CLOSE_RITES"]);
                }
            }

            OracleParameter[] par2 = new OracleParameter[2];

            par2[0] = new OracleParameter("P_COCD", OracleDbType.Int32, CO_CD, ParameterDirection.Input);
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

        public DashboardModel GetCMDFODashBoard(int CO_CD)
        {
            DashboardModel model = new DashboardModel();

            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("P_CO_CD", OracleDbType.Varchar2, Convert.ToString(CO_CD), ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            par[2] = new OracleParameter("P_RESULT_BILLING_CLIENT", OracleDbType.RefCursor, ParameterDirection.Output);
            par[3] = new OracleParameter("P_RESULT_OUTSTANDING_CLIENT", OracleDbType.RefCursor, ParameterDirection.Output);
            par[4] = new OracleParameter("P_RESULT_BULLING_COMPARISON", OracleDbType.RefCursor, ParameterDirection.Output);
            par[5] = new OracleParameter("P_RESULT_SECTOR_BILLING", OracleDbType.RefCursor, ParameterDirection.Output);
            par[6] = new OracleParameter("P_RESULT_SECTOR_BILLING_CURRENT_YEAR_WISE", OracleDbType.RefCursor, ParameterDirection.Output);

            //par[7] = new OracleParameter("P_RESULT_SECTOR_BILLING_PREV_YEAR_WISE", OracleDbType.RefCursor, ParameterDirection.Output);
            //par[8] = new OracleParameter("P_RESULT_SECTOR_BILLING_LAST_PREV_YEAR_WISE", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_CM_DFO_DASHBOARD", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    model = dt.AsEnumerable().Select(row => new DashboardModel
                    {
                        TotalOutstandingAmount = Convert.ToDecimal(row["OUTSTANDING_AMOUNT"]),
                        SuspenseAmount = Convert.ToDecimal(row["SUSPENSE_AMT"]),
                        NoOfOutstandingBillCount = Convert.ToInt32(row["OUTSTANDING_BILL"]),
                        TotalInvoiceCount = Convert.ToInt32(row["TOTAL_INVOICE"]),
                        FinalizedInvoiceCount = Convert.ToInt32(row["FINALIZED_INVOICE"]),
                        PendingInvoiceFinalizeCount = Convert.ToInt32(row["PENDING_FINALIZED_INVOICE"]),
                    }).FirstOrDefault();
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    List<ClientDetailListModel> lstBillingClient = dt.AsEnumerable().Select(row => new ClientDetailListModel
                    {
                        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                        AMOUNT = Convert.ToDecimal(row["AMOUNT"])
                    }).ToList();
                    model.lstBillingClient = lstBillingClient;
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[2];
                    List<ClientDetailListModel> lstOutstandingClient = dt.AsEnumerable().Select(row => new ClientDetailListModel
                    {
                        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                        AMOUNT = Convert.ToDecimal(row["AMOUNT"])
                    }).ToList();
                    model.lstOutstandingClient = lstOutstandingClient;
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[3];
                    List<Billing_Comparison_Model> lstBillingCompare = dt.AsEnumerable().Select(row => new Billing_Comparison_Model
                    {
                        FINALCIAL_YEAR = Convert.ToString(row["FINALCIAL_YEAR"]),
                        AMOUNT = Convert.ToDecimal(row["AMOUNT"])
                    }).ToList();
                    model.lstBillingCompare = lstBillingCompare;
                }

                if (ds.Tables[4].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[4];
                    List<Sector_Billing_Model> lstCurrYearSectorBilling = dt.AsEnumerable().Select(row => new Sector_Billing_Model
                    {
                        SECTOR = Convert.ToString(row["SECTOR"]),
                        AMOUNT = Convert.ToDecimal(row["AMOUNT"])
                    }).ToList();
                    model.lstCurrYearSectorBilling = lstCurrYearSectorBilling;
                }

                model.CurrYearSectorBillingSummary = "[";
                model.CurrYearSectorBillingSummary += "['PSU'," + model.lstCurrYearSectorBilling.Where(x => x.SECTOR == "PSU").Select(x => x.AMOUNT).FirstOrDefault() + "],";
                model.CurrYearSectorBillingSummary += "['Railway'," + model.lstCurrYearSectorBilling.Where(x => x.SECTOR == "RAILWAY").Select(x => x.AMOUNT).FirstOrDefault() + "],";
                model.CurrYearSectorBillingSummary += "['Non-Railway'," + model.lstCurrYearSectorBilling.Where(x => x.SECTOR == "NONRAILWAY").Select(x => x.AMOUNT).FirstOrDefault() + "],";
                model.CurrYearSectorBillingSummary += "['Private'," + model.lstCurrYearSectorBilling.Where(x => x.SECTOR == "PRIVATE").Select(x => x.AMOUNT).FirstOrDefault() + "],";
                model.CurrYearSectorBillingSummary += "['Lab'," + model.lstCurrYearSectorBilling.Where(x => x.SECTOR == "LAB").Select(x => x.AMOUNT).FirstOrDefault() + "],";
                model.CurrYearSectorBillingSummary += "]";

                if (ds.Tables[5].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[5];
                    List<Sector_Billing_Model> lstLastThreeYearSectorBilling = dt.AsEnumerable().Select(row => new Sector_Billing_Model
                    {
                        YEAR = Convert.ToString(row["YRS"]),
                        SECTOR = Convert.ToString(row["SECTOR"]),
                        AMOUNT = Convert.ToDecimal(row["AMOUNT"])
                    }).ToList();
                    model.lstLastThreeYearSectorBilling = lstLastThreeYearSectorBilling;
                }
                var Years = model.lstLastThreeYearSectorBilling.Select(x => x.YEAR).Distinct().ToList();

                model.LastYearSectorBillingSummary1 = "[";
                model.LastYearSectorBillingSummary1 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "PSU" && x.YEAR == Years[0]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary1 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "RAILWAY" && x.YEAR == Years[0]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary1 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "NONRAILWAY" && x.YEAR == Years[0]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary1 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "PRIVATE" && x.YEAR == Years[0]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary1 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "LAB" && x.YEAR == Years[0]).Select(x => x.AMOUNT).FirstOrDefault();
                model.LastYearSectorBillingSummary1 += "]";

                model.LastYearSectorBillingSummary2 = "[";
                model.LastYearSectorBillingSummary2 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "PSU" && x.YEAR == Years[1]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary2 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "RAILWAY" && x.YEAR == Years[1]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary2 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "NONRAILWAY" && x.YEAR == Years[1]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary2 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "PRIVATE" && x.YEAR == Years[1]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary2 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "LAB" && x.YEAR == Years[1]).Select(x => x.AMOUNT).FirstOrDefault();
                model.LastYearSectorBillingSummary2 += "]";

                model.LastYearSectorBillingSummary3 = "[";
                model.LastYearSectorBillingSummary3 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "PSU" && x.YEAR == Years[2]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary3 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "RAILWAY" && x.YEAR == Years[2]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary3 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "NONRAILWAY" && x.YEAR == Years[2]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary3 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "PRIVATE" && x.YEAR == Years[2]).Select(x => x.AMOUNT).FirstOrDefault() + ",";
                model.LastYearSectorBillingSummary3 += model.lstLastThreeYearSectorBilling.Where(x => x.SECTOR == "LAB" && x.YEAR == Years[2]).Select(x => x.AMOUNT).FirstOrDefault();
                model.LastYearSectorBillingSummary3 += "]";
            }
            return model;
        }

        public DTResult<CMDARListing> CMDARListing(DTParameters dtParameters)
        {
            DTResult<CMDARListing> dTResult = new() { draw = 0 };

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Case_No";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "Case_No";
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

            IQueryable<CMDARListing>? query = null;

            query = context.T40ConsigneeComplaints
                .Where(t40 => t40.JiStatusCd == 1 || t40.JiStatusCd == 2)
                .Where(t40 => t40.JiDt >= Convert.ToDateTime(FromDate) && t40.JiDt <= Convert.ToDateTime(ToDate))
                .Select(t40 => new CMDARListing
                {
                    Complaint_ID = t40.ComplaintId,
                    Complaint_DT = t40.ComplaintDt,
                    Case_No = t40.CaseNo,
                    Rej_Memo_No = t40.RejMemoNo,
                    Rej_Memo_Dt = t40.RejMemoDt,
                    RATE = t40.Rate,
                });


            dTResult.recordsTotal = query.Count();
            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Case_No).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
