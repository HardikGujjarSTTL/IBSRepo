using Humanizer;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Reports.Billing;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;
using System.Globalization;
using static IBS.Helper.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories.Reports
{
    public class PCDOReportRepository : IPCDOReportRepository
    {
        private readonly ModelContext context;

        public PCDOReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<HighlightReportsModel> GetHighlightData(string p_wYrMth)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_wYrMth", OracleDbType.Varchar2, p_wYrMth, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_Highlights", par, 1);
            DataTable dt = ds.Tables[0];

            HighlightReportsModel model = new();
            List<HighlightReportsModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<HighlightReportsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public List<FinancialBillingModel> GetFinancialBillingData(int dmonth, string wYrMth_Past, string CumYrPast, string wYrMth, string CumYrMth, int byear)
        {
            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("dmonth", OracleDbType.Varchar2,Convert.ToString(dmonth), ParameterDirection.Input);
            par[1] = new OracleParameter("wYrMth_Past", OracleDbType.Varchar2, wYrMth_Past, ParameterDirection.Input);
            par[2] = new OracleParameter("CumYrPast", OracleDbType.Varchar2, CumYrPast, ParameterDirection.Input);
            par[3] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[4] = new OracleParameter("CumYrMth", OracleDbType.Varchar2, CumYrMth, ParameterDirection.Input);
            par[5] = new OracleParameter("byear", OracleDbType.Varchar2, Convert.ToString(byear), ParameterDirection.Input);
            par[6] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_FinancialBilling", par, 1);
            DataTable dt = ds.Tables[0];

            FinancialBillingModel model = new();
            List<FinancialBillingModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<FinancialBillingModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

    }
}
