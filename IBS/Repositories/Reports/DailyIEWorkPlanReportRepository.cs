using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Transaction;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using static IBS.Helper.Enums;

namespace IBS.Repositories.Reports
{
    public class DailyIEWorkPlanReportRepository : IDailyIEWorkPlanReportRepository
    {
        private readonly ModelContext context;

        public DailyIEWorkPlanReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public DailyIECMWorkPlanReportModel GetDailyWorkData(string FromDate, string ToDate, string lstIE, string lstCM, string AllIEs, string ParticularIEs, string AllCM, string ParticularCMs, string ReportType,string IEWise,string CMWise,string Region, string SortedIE, string visitdate)
        {
            DailyIECMWorkPlanReportModel model = new();
            List<DailyIECMWorkPlanReporttbl1> lstDailyIECMWorkPlanReporttbl1 = new();
            List<DailyIECMWorkPlanReporttbl2> lstDailyIECMWorkPlanReporttbl2 = new();
            List<DailyIEWorklanExcepReport> lstDailyIEWorklanExcepReport = new();

            DataSet ds = null;
            DataSet ds1 = null;
            DataTable dt = new DataTable();

           
            string formattedFromDate = "";
            string formattedToDate = "";

            if (FromDate != null && ToDate != null)
            {
                DateTime parsedFromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                formattedFromDate = parsedFromDate.ToString("yyyy/MM/dd");
                formattedToDate = parsedToDate.ToString("yyyy/MM/dd");
            }

            if (ReportType == "U")
            {

                OracleParameter[] par = new OracleParameter[10];
                par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[1] = new OracleParameter("rdbIEWise", OracleDbType.Varchar2, IEWise, ParameterDirection.Input);
                par[2] = new OracleParameter("rdbCOWise", OracleDbType.Varchar2, CMWise, ParameterDirection.Input);
                par[3] = new OracleParameter("FrmDt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
                par[4] = new OracleParameter("ToDt", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
                par[5] = new OracleParameter("lstIE", OracleDbType.Varchar2, lstIE, ParameterDirection.Input);
                par[6] = new OracleParameter("lstCO", OracleDbType.Varchar2, lstCM, ParameterDirection.Input);
                par[7] = new OracleParameter("rdopartIE", OracleDbType.Varchar2, ParticularIEs, ParameterDirection.Input);
                par[8] = new OracleParameter("rdopartCM", OracleDbType.Varchar2, ParticularCMs, ParameterDirection.Input);
                par[9] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("GetDailyIEWorkPlanReport", par, 1);

                OracleParameter[] par1 = new OracleParameter[10];
                par1[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par1[1] = new OracleParameter("rdbIEWise", OracleDbType.Varchar2, IEWise, ParameterDirection.Input);
                par1[2] = new OracleParameter("rdbCOWise", OracleDbType.Varchar2, CMWise, ParameterDirection.Input);
                par1[3] = new OracleParameter("FrmDt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
                par1[4] = new OracleParameter("ToDt", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
                par1[5] = new OracleParameter("lstIE", OracleDbType.Varchar2, lstIE, ParameterDirection.Input);
                par1[6] = new OracleParameter("lstCO", OracleDbType.Varchar2, lstCM, ParameterDirection.Input);
                par1[7] = new OracleParameter("rdopartIE", OracleDbType.Varchar2, ParticularIEs, ParameterDirection.Input);
                par1[8] = new OracleParameter("rdopartCM", OracleDbType.Varchar2, ParticularCMs, ParameterDirection.Input);
                par1[9] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

                ds1 = DataAccessDB.GetDataSet("GetDailyIEWorkPlanReporttbl2", par1, 1);


                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    List<DailyIECMWorkPlanReporttbl1> listcong = dt.AsEnumerable().Select(row => new DailyIECMWorkPlanReporttbl1
                    {
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        VISIT_DATE = Convert.ToString(row["VISIT_DATE"]),
                        LOGIN_TIME = Convert.ToString(row["LOGIN_TIME"]),
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DATE = Convert.ToString(row["CALL_RECV_DATE"]),
                        DESIRE_DT = Convert.ToString(row["DESIRE_DT"]),
                        CO_NAME = Convert.ToString(row["CO_NAME"]),
                        CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                        CHK_COUNT = Convert.ToString(row["CHK_COUNT"]),
                        MFG_NAME = Convert.ToString(row["MFG_NAME"]),
                        MFG_PLACE = Convert.ToString(row["MFG_PLACE"]),
                        MFG_CITY = Convert.ToString(row["MFG_CITY"]),
                        ITEM_DESC_PO = Convert.ToString(row["ITEM_DESC_PO"]),
                        VALUE = Convert.ToString(row["VALUE"]),
                        CALL_STATUS = Convert.ToString(row["CALL_STATUS"]),
                    }).ToList();
                    model.lstDailyIECMWorkPlanReporttbl1 = listcong;
                }

                if (ds1 != null && ds1.Tables.Count > 0)
                {
                    dt = ds1.Tables[0];
                    List<DailyIECMWorkPlanReporttbl2> listcong = dt.AsEnumerable().Select(row => new DailyIECMWorkPlanReporttbl2
                    {
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        CO_NAME = Convert.ToString(row["CO_NAME"]),
                        WORK_DATE = Convert.ToString(row["WORK_DATE"]),
                        LOGIN_TIME = Convert.ToString(row["LOGIN_TIME"]),
                        NI_WORK_PLAN_CD = Convert.ToString(row["NI_WORK_PLAN_CD"]),
                    }).ToList();
                    model.lstDailyIECMWorkPlanReporttbl2 = listcong;
                }
            }

            if(ReportType == "E")
            {

                OracleParameter[] par = new OracleParameter[11];
                par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[1] = new OracleParameter("rdbIEWise", OracleDbType.Varchar2, IEWise, ParameterDirection.Input);
                par[2] = new OracleParameter("rdbCOWise", OracleDbType.Varchar2, CMWise, ParameterDirection.Input);
                par[3] = new OracleParameter("rdbPartIE", OracleDbType.Varchar2, ParticularIEs, ParameterDirection.Input);
                par[4] = new OracleParameter("rdbPartCo", OracleDbType.Varchar2, ParticularCMs, ParameterDirection.Input);
                par[5] = new OracleParameter("rdbIESort", OracleDbType.Varchar2, SortedIE, ParameterDirection.Input);
                par[6] = new OracleParameter("frmdt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
                par[7] = new OracleParameter("todt", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
                par[8] = new OracleParameter("lstIE", OracleDbType.Varchar2, lstIE, ParameterDirection.Input);
                par[9] = new OracleParameter("lstCo", OracleDbType.Varchar2, lstCM, ParameterDirection.Input);
                par[10] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("GetDailyIEWisePlanEXCEPTIONReport", par, 1);


                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    List<DailyIEWorklanExcepReport> listcong = dt.AsEnumerable().Select(row => new DailyIEWorklanExcepReport
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DATE = Convert.ToString(row["CALL_RECV_DATE"]),
                        CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        BK_NO = Convert.ToString(row["BK_NO"]),
                        SET_NO = Convert.ToString(row["SET_NO"]),
                        NO_OF_INSP = Convert.ToString(row["NO_OF_INSP"]),
                        IC_DATE = Convert.ToString(row["IC_DATE"]),
                        FIRST_INSP_DATE = Convert.ToString(row["FIRST_INSP_DATE"]),
                        LAST_INSP_DATE = Convert.ToString(row["LAST_INSP_DATE"]),
                        VISIT_DATE = Convert.ToString(row["VISIT_DATE"]),
                    }).ToList();
                    model.lstDailyIEWorklanExcepReport = listcong;
                }

            }

            return model;
        }
    }
}
