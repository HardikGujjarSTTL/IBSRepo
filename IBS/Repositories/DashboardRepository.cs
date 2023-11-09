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


            int financialYearStartMonth = 4;  
            int financialYearStartDay = 1;   

            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;

            if (currentMonth < financialYearStartMonth)
            {
                currentYear--;
            }

            DateTime financialYearStartDate = new DateTime(currentYear, financialYearStartMonth, financialYearStartDay);

            string FromDate = financialYearStartDate.ToString("dd/MM/yyyy");
            string ToDate = DateTime.Now.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[4];

            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.NVarchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.NVarchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_IECD", OracleDbType.NVarchar2, IeCd, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_IE_DASHBOARD_COUNT", par, 1);

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

            return model;
        }
        
        public DashboardModel GetVendorDashBoardCount(int Vend_Cd)
        {
            DashboardModel model = new();

            model.TotalCallsCount = 35;
            model.PendingCallsCount = 13;
            model.AcceptedCallsCount = 15;
            model.CancelledCallsCount = 2;
            model.UnderLabTestingCount = 40;
            model.StillUnderInspectionCount = 20;
            model.StageRejectionCount = 17;

            return model;
        }
    }

}

