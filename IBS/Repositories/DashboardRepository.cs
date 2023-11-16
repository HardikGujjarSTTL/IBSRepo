using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ModelContext context;

        public DashboardRepository(ModelContext context)
        {
            this.context = context;
        }

        public DashboardModel GetIEDDashBoardCount(int IeCd)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[2];

            par[0] = new OracleParameter("P_IECD", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_IE_DASHBOARD_COUNT", par);

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
                
            }

            OracleParameter[] par1 = new OracleParameter[2];

            par1[0] = new OracleParameter("P_IECD", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            par1[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds1 = DataAccessDB.GetDataSet("GET_IE_DASHBOARD_PENDING_CALL", par1);

            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds1.Tables[0];
                    List<IEList> listIE = dt.AsEnumerable().Select(row => new IEList
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
                    model.lstIE = listIE;
                }
            }

            return model;
        }

        public DashboardModel GetVendorDashBoardCount(int Vend_Cd)
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

            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds1.Tables[0];
                    List<POCallStatusList> listVend = dt.AsEnumerable().Select(row => new POCallStatusList
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
                    model.lstPOCallStatus = listVend;
                }
            }

            OracleParameter[] par2 = new OracleParameter[2];

            par2[0] = new OracleParameter("P_VENDCD", OracleDbType.Int32, Vend_Cd, ParameterDirection.Input);
            par2[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds2 = DataAccessDB.GetDataSet("GET_VENDOR_DASHBOARD_RECENT_PO", par2);

            if (ds2 != null && ds2.Tables.Count > 0)
            {
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds2.Tables[0];
                    List<RecentPOList> listVend = dt.AsEnumerable().Select(row => new RecentPOList
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DT = Convert.ToDateTime(row["CALL_RECV_DT"]),
                        DETAILS = Convert.ToString(row["DETAILS"]),
                        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                        PO_NO = Convert.ToString(row["PO_NO"]),
                        PURCHASE_ORDER = Convert.ToString(row["PURCHASE_ORDER"]),
                        CALL_STATUS = Convert.ToString(row["CALL_STATUS"])
                    }).ToList();
                    model.lstRecentPO = listVend;
                }
            }
            return model;
        }

        public DashboardModel GetClientDashBoardCount(string OrgnType, string Organisation)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[3];

            par[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Organisation, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RLY_NONRLY", OracleDbType.Varchar2, OrgnType, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_CLIENT_DASHBOARD_COUNT", par);

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

            OracleParameter[] par1 = new OracleParameter[3];

            par1[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Organisation, ParameterDirection.Input);
            par1[1] = new OracleParameter("P_RLY_NONRLY", OracleDbType.Varchar2, OrgnType, ParameterDirection.Input);
            par1[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds1 = DataAccessDB.GetDataSet("GET_CLIENT_DASHBOARD_VENDOR_WISE_PERFORMANCE", par1);

            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds1.Tables[0];
                    List<ClientVENDPOList> lstClientVEND = dt.AsEnumerable().Select(row => new ClientVENDPOList
                    {
                        RLY_CD = Convert.ToString(row["RLY_CD"]),
                        RLY_NONRLY = Convert.ToString(row["RLY_NONRLY"]),
                        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                        TOTAL_CALL = Convert.ToInt32(row["TOTAL_CALL"]),
                        REJECTED_CALL = Convert.ToInt32(row["REJECTED_CALL"]),
                        CANCELLED_CALL = Convert.ToInt32(row["CANCELLED_CALL"]),
                    }).ToList();
                    model.lstClientVEND = lstClientVEND;
                }
            }

            return model;
        }
    }

}

