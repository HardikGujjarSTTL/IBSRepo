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
    public class PeriodWiseChecksheetReportRepository : IPeriodWiseChecksheetReportRepository
    {
        private readonly ModelContext context;

        public PeriodWiseChecksheetReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public PeriodWiseChecksheetReportModel Getperiodwisechecksheetdetails(string FromDate, string ToDate, string Region)
        {
            PeriodWiseChecksheetReportModel model = new();
            List<PeriodWiseChecksheet> lstPeriodWiseChecksheet = new();

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

           
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[3] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetPeriodWiseChecksheetReports", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<PeriodWiseChecksheet> listcong = dt.AsEnumerable().Select(row => new PeriodWiseChecksheet
                {
                    ITEM_DESC = Convert.ToString(row["ITEM_DESC"]),
                    IE = Convert.ToString(row["IE"]),
                    CO_NAME = Convert.ToString(row["CO_NAME"]),
                    CREATION_REV_DT = Convert.ToString(row["CREATION_REV_DT"]),
                }).ToList();

                model.lstPeriodWiseChecksheet = listcong;
            }

            return model;
        }
    }
}
