using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class DetailsofPurchaserWiseInspRRepository : IDetailsofPurchaserWiseInspRepository
    {
        private readonly ModelContext context;

        public DetailsofPurchaserWiseInspRRepository(ModelContext context)
        {
            this.context = context;
        }

        public DetailsofPurchaserWiseInspModel SummaryInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin, string TextPurchaser)
        {

            DetailsofPurchaserWiseInspModel model = new();
            List<DetailsofPurchaserWiseInspModel> lstSummary = new();


            OracleParameter[] par = new OracleParameter[11];
            par[0] = new OracleParameter("P_REGION_CODE", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("P_ForGPer", OracleDbType.Boolean, Convert.ToBoolean(ForGiven), ParameterDirection.Input);
            par[2] = new OracleParameter("P_rdbICDT", OracleDbType.Boolean, Convert.ToBoolean(ReportBasedon), ParameterDirection.Input);
            par[3] = new OracleParameter("P_rdbPart", OracleDbType.Boolean, ForParticular, ParameterDirection.Input);
            par[4] = new OracleParameter("P_TextPur", OracleDbType.NVarchar2, TextPurchaser, ParameterDirection.Input);
            par[5] = new OracleParameter("p_From_Date", OracleDbType.Date, FromDate, ParameterDirection.Input);
            par[6] = new OracleParameter("p_To_Date", OracleDbType.Date, ToDate, ParameterDirection.Input);
            par[7] = new OracleParameter("P_lstPurchaser", OracleDbType.NVarchar2, lstParticular, ParameterDirection.Input);
            par[8] = new OracleParameter("p_Year", OracleDbType.NVarchar2, Year, ParameterDirection.Input);
            par[9] = new OracleParameter("p_Month", OracleDbType.NVarchar2, Month, ParameterDirection.Input);
            par[10] = new OracleParameter("P_DetailsPurchase_INFO", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_DetailsPurchase_INFO", par, 10);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstSummary = JsonConvert.DeserializeObject<List<DetailsofPurchaserWiseInspModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                model.PURCHASER = Convert.ToString(ds.Tables[0].Rows[0]["PURCHASER"]);
                model.CASE_NO = Convert.ToString(ds.Tables[0].Rows[0]["CASE_NO"]);
                model.PO_NO = Convert.ToString(ds.Tables[0].Rows[0]["PO_NO"]);
                model.PO_DT = Convert.ToString(ds.Tables[0].Rows[0]["PO_DT"]);
                model.REMARKS = Convert.ToString(ds.Tables[0].Rows[0]["REMARKS"]);
                model.IC_NO = Convert.ToString(ds.Tables[0].Rows[0]["IC_NO"]);
                model.IC_DT = Convert.ToString(ds.Tables[0].Rows[0]["IC_DT"]);
                model.BK_NO = Convert.ToString(ds.Tables[0].Rows[0]["BK_NO"]);
                model.SET_NO = Convert.ToString(ds.Tables[0].Rows[0]["SET_NO"]);
                model.BILL_NO = Convert.ToString(ds.Tables[0].Rows[0]["BILL_NO"]);
                model.BILL_DATE = Convert.ToString(ds.Tables[0].Rows[0]["BILL_DATE"]);
                model.IE_SNAME = Convert.ToString(ds.Tables[0].Rows[0]["IE_SNAME"]);
                model.BPO = Convert.ToString(ds.Tables[0].Rows[0]["BPO"]);
                model.VENDOR = Convert.ToString(ds.Tables[0].Rows[0]["VENDOR"]);
                model.INSP_FEE = Convert.ToString(ds.Tables[0].Rows[0]["INSP_FEE"]);
                model.VISITS = Convert.ToString(ds.Tables[0].Rows[0]["VISITS"]);
                model.VALUE = Convert.ToString(ds.Tables[0].Rows[0]["VALUE"]);
                model.ITEM_DESC = Convert.ToString(ds.Tables[0].Rows[0]["ITEM_DESC"]);
            }
            model.lstdetailspinsp = lstSummary;
            return model;
        }
    }
}
