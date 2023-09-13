using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace IBS.Repositories.Reports
{
    public class PurchaseOrdersofSpecificValuesRepository : IPurchaseOrdersofSpecificValuesRepository
    {
        private readonly ModelContext context;

        public PurchaseOrdersofSpecificValuesRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<PurchaseOrdersofSpecificValueModel> GetDataList(string p_wAgency, string p_frmDt, string p_toDt, string p_SelCriteria, string p_wClient, string p_wFrmAmt, string p_wToAmt, string Region)
        {
            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("p_SelCriteria", OracleDbType.Varchar2, p_SelCriteria, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[2] = new OracleParameter("p_frmDt", OracleDbType.Varchar2, p_frmDt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_toDt", OracleDbType.Varchar2, p_toDt, ParameterDirection.Input);
            par[4] = new OracleParameter("p_wAgency", OracleDbType.Varchar2, p_wAgency, ParameterDirection.Input);
            par[5] = new OracleParameter("p_wClient", OracleDbType.Varchar2, p_wClient, ParameterDirection.Input);
            par[6] = new OracleParameter("p_wFrmAmt", OracleDbType.Varchar2, p_wFrmAmt, ParameterDirection.Input);
            par[7] = new OracleParameter("p_wToAmt", OracleDbType.Varchar2, p_wToAmt, ParameterDirection.Input);
            par[8] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_DetailedReport_POofSpecificValues", par, 1);
            DataTable dt = ds.Tables[0];

            PurchaseOrdersofSpecificValueModel model = new();
            List<PurchaseOrdersofSpecificValueModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<PurchaseOrdersofSpecificValueModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

        public List<PurchaseOrdersofSummaryModel> GetSummaryDataList(string p_wAgency, string p_frmDt, string p_toDt, string p_SelCriteria, string p_wFrmAmt, string p_wToAmt, string Region)
        {
            OracleParameter[] par = new OracleParameter[8];
            par[0] = new OracleParameter("p_SelCriteria", OracleDbType.Varchar2, p_SelCriteria, ParameterDirection.Input);
            par[1] = new OracleParameter("p_wRegion", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[2] = new OracleParameter("p_frmDt", OracleDbType.Varchar2, p_frmDt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_toDt", OracleDbType.Varchar2, p_toDt, ParameterDirection.Input);
            par[4] = new OracleParameter("p_wAgency", OracleDbType.Varchar2, p_wAgency, ParameterDirection.Input);
            par[5] = new OracleParameter("p_wFrmAmt", OracleDbType.Decimal, p_wFrmAmt, ParameterDirection.Input);
            par[6] = new OracleParameter("p_wToAmt", OracleDbType.Decimal, p_wToAmt, ParameterDirection.Input);
            par[7] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_SummarizedReport_POofSpecificValues", par, 1);
            DataTable dt = ds.Tables[0];

            PurchaseOrdersofSummaryModel model = new();
            List<PurchaseOrdersofSummaryModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<PurchaseOrdersofSummaryModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return list;
        }

    }
}
