using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports.RealisationPayment;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Reports.RealisationPayment
{
    public class RealisationPaymentRepository : IRealisationPaymentRepository
    {
        private readonly ModelContext context;

        public RealisationPaymentRepository(ModelContext context)
        {
            this.context = context;
        }

        public SummaryOnlinePaymentModel GetSummaryOnlinePayment(DateTime FromDate, DateTime ToDate, string Region)
        {
            SummaryOnlinePaymentModel model = new();
            List<SummaryOnlinePaymentListModel> lstOnlinePayment = new();
            var startDate = Common.DateConcate(Convert.ToString(FromDate));
            var endDate = Common.DateConcate(Convert.ToString(ToDate));

            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);
            model.FromDate = FromDate;
            model.ToDate = ToDate;

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, startDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, endDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_SUMMARY_ONLINE_PAYMENT", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstOnlinePayment = JsonConvert.DeserializeObject<List<SummaryOnlinePaymentListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            model.lstOnlinePayment = lstOnlinePayment;
            return model;
        }

        public SummaryCrisRlyPaymentModel GetSummaryCrisRlyPaymentDetailed(DateTime FromDate, DateTime ToDate, string IsRly, string Rly, string IsAU, string AU, string IsAllRegion, string Status, string Region)
        {
            SummaryCrisRlyPaymentModel model = new();
            List<SummaryCrisRlyPaymentDetailedModel> lstDetailed = new();
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);
            model.FromDate = FromDate;
            model.ToDate = ToDate;

            IsRly = IsRly == "true" ? "true" : null;
            Rly = Rly != null ? Rly : null;
            IsAU = IsAU == "true" ? "true" : null;

            OracleParameter[] par = new OracleParameter[10];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, model.Display_FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, model.Display_ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_ISRLY", OracleDbType.Varchar2, IsRly, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RLY", OracleDbType.Varchar2, Rly, ParameterDirection.Input);
            par[4] = new OracleParameter("P_ISAU", OracleDbType.Varchar2, IsAU, ParameterDirection.Input);
            par[5] = new OracleParameter("P_AU", OracleDbType.Varchar2, AU, ParameterDirection.Input);
            par[6] = new OracleParameter("P_ALLREGION", OracleDbType.Int16, Convert.ToInt16(IsAllRegion), ParameterDirection.Input);
            par[7] = new OracleParameter("P_STATUS", OracleDbType.Varchar2, Status, ParameterDirection.Input);
            par[8] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[9] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_SUMMARY_CRIS_RLY_PAYMENT_DETAILED", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstDetailed = JsonConvert.DeserializeObject<List<SummaryCrisRlyPaymentDetailedModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            model.lstCrisRlyDetailed = lstDetailed;

            return model;
        }

        public SummaryCrisRlyPaymentModel GetSummaryCrisRlyPaymentSummary(DateTime FromDate, DateTime ToDate, string IsRlyWise, string Status, string Region)
        {
            SummaryCrisRlyPaymentModel model = new();
            List<SummaryCrisRlyPaymentSummaryListModel> lstSummary = new();
            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.IsRlyWise = IsRlyWise;
            model.Status = Status;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, model.Display_FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, model.Display_ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_STATUS", OracleDbType.Varchar2, Status, ParameterDirection.Input);
            par[3] = new OracleParameter("P_ISRLYWISE", OracleDbType.Varchar2, IsRlyWise, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_SUMMARY_CRIS_RLY_PAYMENT_SUMMARY", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstSummary = JsonConvert.DeserializeObject<List<SummaryCrisRlyPaymentSummaryListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            model.lstCrisRlySummary = lstSummary;

            return model;
        }
    }
}
