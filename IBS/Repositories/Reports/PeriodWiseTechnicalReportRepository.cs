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
    public class PeriodWiseTechnicalReportRepository : IPeriodWiseTechnicalReportRepository
    {
        private readonly ModelContext context;

        public PeriodWiseTechnicalReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public PeriodWiseTechnicalRefReportModel Getperiodwisetechrefdetails(string FromDate, string ToDate, string Region)
        {
            PeriodWiseTechnicalRefReportModel model = new();
            List<PeriodWiseTechnicalRef> lstPeriodWiseTechnicalRef = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_todt", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[3] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetPeriodWiseTechRefReport", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<PeriodWiseTechnicalRef> listcong = dt.AsEnumerable().Select(row => new PeriodWiseTechnicalRef
                {
                    cm_name = Convert.ToString(row["cm_name"]),
                    ie_name = Convert.ToString(row["ie_name"]),
                    item_des = Convert.ToString(row["item_des"]),
                    spec_drg = Convert.ToString(row["spec_drg"]),
                    letter_no = Convert.ToString(row["letter_no"]),
                    tech_date = Convert.ToString(row["tech_date"]),
                    ref_made = Convert.ToString(row["ref_made"]),
                    tech_content = Convert.ToString(row["tech_content"]),
                }).ToList();

                model.lstPeriodWiseTechnicalRef = listcong;
            }

            return model;
        }
    }
}
