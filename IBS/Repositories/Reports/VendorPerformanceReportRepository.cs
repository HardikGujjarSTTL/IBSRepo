using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
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
    public class VendorPerformanceReportRepository : IVendorPerformanceReportRepository
    {
        private readonly ModelContext context;

        public VendorPerformanceReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public VendorPerformanceReportModel GetVendorperformanceReport(string FromDate, string ToDate, string formonth, string forperiod, string month, string year, string vendcd, string Region)
        {
            VendorPerformanceReportModel model = new();
            List<VendorPerformance> lstVendorPerformance = new();

            DataSet ds = null;
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

            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_lstvendor", OracleDbType.Varchar2, vendcd, ParameterDirection.Input);
            par[2] = new OracleParameter("p_rdbForMonth", OracleDbType.Varchar2, formonth, ParameterDirection.Input);
            par[3] = new OracleParameter("p_monthyear", OracleDbType.Varchar2, year+month, ParameterDirection.Input);
            par[4] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
            par[5] = new OracleParameter("p_todt", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
            par[6] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetVendorPerformanceReports", par, 1);


            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<VendorPerformance> listcong = dt.AsEnumerable().Select(row => new VendorPerformance
                {
                    ITEM_DESC = Convert.ToString(row["ITEM_DESC"]),
                    PO_NO = Convert.ToString(row["PO_NO"]),
                    PO_DATE = Convert.ToString(row["PO_DATE"]),
                    CONSIGNEE = Convert.ToString(row["CONSIGNEE"]),
                    QTY_TO_INSP = Convert.ToString(row["QTY_TO_INSP"]),
                    UOM = Convert.ToString(row["UOM"]),
                    QTY_PASSED = Convert.ToString(row["QTY_PASSED"]),
                    QTY_REJECTED = Convert.ToString(row["QTY_REJECTED"]),
                    REASON_REJECT = Convert.ToString(row["REASON_REJECT"]),
                    CALL_DATE = Convert.ToString(row["CALL_DATE"]),
                    FIRST_INSP_DATE = Convert.ToString(row["FIRST_INSP_DATE"]),
                    LAST_INSP_DATE = Convert.ToString(row["LAST_INSP_DATE"]),
                    CALL_STATUS_DESC = Convert.ToString(row["CALL_STATUS_DESC"]),
                    IC_NO = Convert.ToString(row["IC_NO"]),
                    IC_DATE = Convert.ToString(row["IC_DATE"]),

                }).ToList();
                model.lstVendorPerformance = listcong;
            }

            return model;
        }
    }
}
