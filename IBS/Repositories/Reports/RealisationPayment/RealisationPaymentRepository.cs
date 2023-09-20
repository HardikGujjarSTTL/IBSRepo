using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports.RealisationPayment;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
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
    }
}
