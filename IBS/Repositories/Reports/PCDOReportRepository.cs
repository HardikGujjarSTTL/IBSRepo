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
using System.Collections.Generic;
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

        public FinancialExpenditureRealizationMainModel GetFinancialExpenditureRealizationData(string wYrMth_Past, string CumYrPast, string wYrMth, string CumYrMth, int byear)
        {
            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("wYrMth_Past", OracleDbType.Varchar2, wYrMth_Past, ParameterDirection.Input);
            par[1] = new OracleParameter("CumYrPast", OracleDbType.Varchar2, CumYrPast, ParameterDirection.Input);
            par[2] = new OracleParameter("wYrMth", OracleDbType.Varchar2, wYrMth, ParameterDirection.Input);
            par[3] = new OracleParameter("CumYrMth", OracleDbType.Varchar2, CumYrMth, ParameterDirection.Input);
            par[4] = new OracleParameter("byear", OracleDbType.Varchar2, Convert.ToString(byear), ParameterDirection.Input);
            par[5] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            par[6] = new OracleParameter("p_Result1", OracleDbType.RefCursor, ParameterDirection.Output);
            par[7] = new OracleParameter("p_Result2", OracleDbType.RefCursor, ParameterDirection.Output);
            par[8] = new OracleParameter("p_Result3", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("sp_PCDOReport_FinancialExpenditureRealization", par, 4);
            DataTable dt = ds.Tables[0];

            FinancialExpenditureRealizationMainModel model = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model.financialExpenditureRealizationModels = JsonConvert.DeserializeObject<List<FinancialExpenditureRealizationModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
                model.realisationModel = JsonConvert.DeserializeObject<List<RealisationModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt2 = JsonConvert.SerializeObject(ds.Tables[2], Formatting.Indented);
                model.realisation1Model = JsonConvert.DeserializeObject<List<Realisation1Model>>(serializeddt2, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt3 = JsonConvert.SerializeObject(ds.Tables[3], Formatting.Indented);
                model.realisation2Model = JsonConvert.DeserializeObject<List<Realisation2Model>>(serializeddt3, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            }
            return model;
        }

    }
}
