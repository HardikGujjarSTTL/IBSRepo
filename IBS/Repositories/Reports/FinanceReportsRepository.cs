using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Reports
{
    public class FinanceReportsRepository : IFinanceReportsRepository
    {
        public FinanceReportModel GetFinanceReport(DateTime? FromDate, DateTime? ToDate, string AccCd, string Region)
        {
            FinanceReportModel model = new();
            List<FinanceReportList> lstFinanceReport = new();

            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_FromDate", OracleDbType.Date, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_ToDate", OracleDbType.Date, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_ACC_CD", OracleDbType.Varchar2, AccCd, ParameterDirection.Input);
            par[3] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_INTER_UNIT_TRANSFER_REPORT", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstFinanceReport = JsonConvert.DeserializeObject<List<FinanceReportList>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                if (ds.Tables.Count > 0)
                {
                    model.BANK_CD = Convert.ToString(ds.Tables[0].Rows[0]["BANK_CD"]);
                    model.Acc_Cd = Convert.ToString(ds.Tables[0].Rows[0]["ACC_CD"]);
                    model.FromDate = FromDate;
                    model.ToDate = ToDate;
                }
            }

            model.lstFinanceReport = lstFinanceReport;
            return model;
        }
    }
}
