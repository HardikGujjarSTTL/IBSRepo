using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Reports
{
    public class PurchaseOrdersofSpecificValuesRepository : IPurchaseOrdersofSpecificValuesRepository
    {
        private readonly ModelContext context;

        public PurchaseOrdersofSpecificValuesRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<PurchaseOrdersofSpecificValueModel> GetDataList(string p_wAgency, DateTime p_frmDt, DateTime p_toDt, string p_SelCriteria, string p_wClient, string p_wFrmAmt, string p_wToAmt, string Region)
        {
            string fromdt = p_frmDt.ToString("dd/MM/yyyy");
            string todt = p_toDt.ToString("dd/MM/yyyy");
            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("p_SelCriteria", OracleDbType.Varchar2, p_SelCriteria, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[2] = new OracleParameter("p_frmDt", OracleDbType.Varchar2, fromdt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_toDt", OracleDbType.Varchar2, todt, ParameterDirection.Input);
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

        public List<PurchaseOrdersofSummaryModel> GetSummaryDataList(string p_wAgency, DateTime p_frmDt, DateTime p_toDt, string p_SelCriteria, string p_wFrmAmt, string p_wToAmt, string Region)
        {
            OracleParameter[] par = new OracleParameter[8];
            par[0] = new OracleParameter("p_SelCriteria", OracleDbType.Varchar2, p_SelCriteria, ParameterDirection.Input);
            par[1] = new OracleParameter("p_wRegion", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[2] = new OracleParameter("p_frmDt", OracleDbType.Date, p_frmDt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_toDt", OracleDbType.Date, p_toDt, ParameterDirection.Input);
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

        public List<InspectionDataModel> GetItemWiseInspectionsList(ItemWiseInspectionsParamModel model)
        {
            OracleParameter[] par = new OracleParameter[10];
            par[0] = new OracleParameter("p_RegionCode", OracleDbType.Varchar2, model.Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_ItemDesc1", OracleDbType.Varchar2, model.ItemDesc1, ParameterDirection.Input);
            par[2] = new OracleParameter("p_ItemDesc2", OracleDbType.Varchar2, model.ItemDesc2, ParameterDirection.Input);
            par[3] = new OracleParameter("p_ItemDesc3", OracleDbType.Varchar2, model.ItemDesc3, ParameterDirection.Input);
            par[4] = new OracleParameter("p_ItemDesc4", OracleDbType.Varchar2, model.ItemDesc4, ParameterDirection.Input);
            par[5] = new OracleParameter("p_ItemDesc5", OracleDbType.Varchar2, model.ItemDesc5, ParameterDirection.Input);
            par[6] = new OracleParameter("p_FrmDt", OracleDbType.Date, model.frmDt, ParameterDirection.Input);
            par[7] = new OracleParameter("p_ToDt", OracleDbType.Date, model.toDt, ParameterDirection.Input);
            par[8] = new OracleParameter("p_OneRegion", OracleDbType.Varchar2, model.OneRegion, ParameterDirection.Input);
            par[9] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_Get_ItemWiseInspections", par, 1);
            DataTable dt = ds.Tables[0];
            List<InspectionDataModel> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<InspectionDataModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            return list;
        }

        public DataTable GetItemWiseInspectionsForTenderQueriesList(ItemWiseInspectionsParamModel model)
        {
            OracleParameter[] par = new OracleParameter[12];
            par[0] = new OracleParameter("p_RegionCode", OracleDbType.Varchar2, model.Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_ItemDesc1", OracleDbType.Varchar2, model.ItemDesc1, ParameterDirection.Input);
            par[2] = new OracleParameter("p_ItemDesc2", OracleDbType.Varchar2, model.ItemDesc2, ParameterDirection.Input);
            par[3] = new OracleParameter("p_ItemDesc3", OracleDbType.Varchar2, model.ItemDesc3, ParameterDirection.Input);
            par[4] = new OracleParameter("p_ItemDesc4", OracleDbType.Varchar2, model.ItemDesc4, ParameterDirection.Input);
            par[5] = new OracleParameter("p_ItemDesc5", OracleDbType.Varchar2, model.ItemDesc5, ParameterDirection.Input);
            par[6] = new OracleParameter("p_FrmDt", OracleDbType.Date, model.frmDt, ParameterDirection.Input);
            par[7] = new OracleParameter("p_ToDt", OracleDbType.Date, model.toDt, ParameterDirection.Input);
            par[8] = new OracleParameter("p_OneRegion", OracleDbType.Varchar2, model.OneRegion, ParameterDirection.Input);
            par[9] = new OracleParameter("p_ClientType", OracleDbType.Varchar2, model.Client, ParameterDirection.Input);
            par[10] = new OracleParameter("p_RlyCd", OracleDbType.Varchar2, model.RCode, ParameterDirection.Input);
            par[11] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_Get_ClientWiseInspections", par, 1);
            DataTable dt = ds.Tables[0];
            return dt;
        }
    }
}
