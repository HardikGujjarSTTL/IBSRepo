using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
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
    public class VendorFeedbackReportRepository : IVendorFeedbackReportRepository
    {
        private readonly ModelContext context;

        public VendorFeedbackReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public VendorFeedbackReportModel GetVendorFeedbackReport(string Region)
        {
            VendorFeedbackReportModel model = new();
            List<VendorFeedbackReport> lstVendorFeedbackReport = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetVendorfeedbackReport", par, 1);


            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<VendorFeedbackReport> listcong = dt.AsEnumerable().Select(row => new VendorFeedbackReport
                {
                    Vendor = Convert.ToString(row["VENDOR"]),
                    Region = Convert.ToString(row["REGION"]),
                    FIELD_1 = Convert.IsDBNull(row["FIELD_1"]) ? 0 : Convert.ToInt32(row["FIELD_1"]),
                    FIELD_2 = Convert.IsDBNull(row["FIELD_2"]) ? 0 : Convert.ToInt32(row["FIELD_2"]),
                    FIELD_3 = Convert.IsDBNull(row["FIELD_3"]) ? 0 : Convert.ToInt32(row["FIELD_3"]),
                    FIELD_4 = Convert.IsDBNull(row["FIELD_4"]) ? 0 : Convert.ToInt32(row["FIELD_4"]),
                    FIELD_5 = Convert.IsDBNull(row["FIELD_5"]) ? 0 : Convert.ToInt32(row["FIELD_5"]),
                    FIELD_6 = Convert.IsDBNull(row["FIELD_5"]) ? 0 : Convert.ToInt32(row["FIELD_6"]),
                    FIELD_7 = Convert.IsDBNull(row["FIELD_5"]) ? 0 : Convert.ToInt32(row["FIELD_7"]),
                    FIELD_8 = Convert.IsDBNull(row["FIELD_5"]) ? 0 : Convert.ToInt32(row["FIELD_8"]),
                    FIELD_9 = Convert.IsDBNull(row["FIELD_9"]) ? string.Empty : Convert.ToString(row["FIELD_9"]),
                    FIELD_10 = Convert.IsDBNull(row["FIELD_10"]) ? string.Empty : Convert.ToString(row["FIELD_10"]),
                    MTHYR_PERIOD = Convert.IsDBNull(row["MTHYR_PERIOD"]) ? 0 : Convert.ToInt32(row["MTHYR_PERIOD"]),
                }).ToList();

                model.lstVendorFeedbackReport = listcong;
            }

            return model;
        }
    }
}
