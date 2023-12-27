using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class SummaryConsigneeWiseInspRRepository : ISummaryConsigneeWiseInspRepository
    {
        private readonly ModelContext context;

        public SummaryConsigneeWiseInspRRepository(ModelContext context)
        {
            this.context = context;
        }
        //public DTResult<SummaryConsigneeWiseInspModel> SummaryConsigneeWiseInsp(DTParameters dtParameters, string Regin)
        //{

        //    DTResult<SummaryConsigneeWiseInspModel> dTResult = new() { draw = 0 };
        //    IQueryable<SummaryConsigneeWiseInspModel>? query = null;

        //    var searchBy = dtParameters.Search?.Value;
        //    var orderCriteria = string.Empty;
        //    var orderAscendingDirection = true;
        //    if (dtParameters.Order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

        //        if (orderCriteria == "")
        //        {
        //            orderCriteria = "NO_OF_INSP";
        //        }
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }
        //    else
        //    {
        //        // if we have an empty search then just order the results by Id ascending
        //        orderCriteria = "NO_OF_INSP";
        //        orderAscendingDirection = true;
        //    }

        //    OracleParameter[] par = new OracleParameter[11];
        //    par[0] = new OracleParameter("P_REGION_CODE", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
        //    par[1] = new OracleParameter("P_ForGPer", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rbrecdt"), ParameterDirection.Input);
        //    par[2] = new OracleParameter("P_rdbICDT", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("status"), ParameterDirection.Input);
        //    par[3] = new OracleParameter("P_rdbPart", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wFrmDtO"), ParameterDirection.Input);
        //    par[4] = new OracleParameter("P_rdbMatValue", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
        //    par[5] = new OracleParameter("p_From_Date", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
        //    par[6] = new OracleParameter("p_To_Date", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
        //    par[7] = new OracleParameter("P_lstConsignee", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
        //    par[8] = new OracleParameter("p_Year", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
        //    par[9] = new OracleParameter("p_Month", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
        //    par[10] = new OracleParameter("P_CONSIGNEE_INFO", OracleDbType.RefCursor, ParameterDirection.Output);

        //    var ds = DataAccessDB.GetDataSet("GET_CONSIGNEE_INFO", par, 10);

        //    List<SummaryConsigneeWiseInspModel> modelList = new List<SummaryConsigneeWiseInspModel>();
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {

        //            SummaryConsigneeWiseInspModel model = new SummaryConsigneeWiseInspModel
        //            {
        //                CONSIGNEE = row["CONSIGNEE"].ToString(),
        //                NO_OF_INSP = row["NO_OF_INSP"].ToString(),
        //                MATERIAL_VALUE = row["MATERIAL_VALUE"].ToString(),
        //                INSP_FEE = row["INSP_FEE"].ToString(),

        //            };

        //            modelList.Add(model);
        //        }
        //    }



        //    query = modelList.AsQueryable();

        //    dTResult.recordsTotal = query.Count();

        //    if (!string.IsNullOrEmpty(searchBy))
        //        query = query.Where(w => Convert.ToString(w.CONSIGNEE).ToLower().Contains(searchBy.ToLower())
        //        || Convert.ToString(w.CONSIGNEE).ToLower().Contains(searchBy.ToLower())
        //        );

        //    dTResult.recordsFiltered = query.Count();

        //    dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
        //    //dTResult.data = query.ToList();

        //    dTResult.draw = dtParameters.Draw;

        //    return dTResult;

        //    //using (var dbContext = context.Database.GetDbConnection())
        //    //{

        //    //}

        //    //return dTResult;
        //}
        public SummaryConsigneeWiseInspModel SummaryConsigneeWiseInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin)
        {

            SummaryConsigneeWiseInspModel model = new();
            List<SummaryConsigneeWiseInspModel> lstSummary = new();


            OracleParameter[] par = new OracleParameter[11];
            par[0] = new OracleParameter("P_REGION_CODE", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("P_ForGPer", OracleDbType.Boolean, Convert.ToBoolean(ForGiven), ParameterDirection.Input);
            par[2] = new OracleParameter("P_rdbICDT", OracleDbType.Boolean, Convert.ToBoolean(ReportBasedon), ParameterDirection.Input);
            par[3] = new OracleParameter("P_rdbPart", OracleDbType.Boolean, ForParticular, ParameterDirection.Input);
            par[4] = new OracleParameter("P_rdbMatValue", OracleDbType.Boolean, MaterialValue, ParameterDirection.Input);
            par[5] = new OracleParameter("p_From_Date", OracleDbType.Date, FromDate, ParameterDirection.Input);
            par[6] = new OracleParameter("p_To_Date", OracleDbType.Date, ToDate, ParameterDirection.Input);
            par[7] = new OracleParameter("P_lstConsignee", OracleDbType.NVarchar2, lstParticular, ParameterDirection.Input);
            par[8] = new OracleParameter("p_Year", OracleDbType.NVarchar2, Year, ParameterDirection.Input);
            par[9] = new OracleParameter("p_Month", OracleDbType.NVarchar2, Month, ParameterDirection.Input);
            par[10] = new OracleParameter("P_CONSIGNEE_INFO", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CONSIGNEE_INFO", par, 10);
            int recCount = 0;
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstSummary = JsonConvert.DeserializeObject<List<SummaryConsigneeWiseInspModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                recCount = recCount + 1;
                //lstPerformance.ToList().ForEach(i => { i.C3 = decimal.Truncate(i.C3); i.C7 = decimal.Truncate(i.C7); i.CM7 = decimal.Truncate(i.CM7); i.C10 = decimal.Truncate(i.C10); i.CALLS = decimal.Truncate(i.CALLS); i.CALL_CANCEL = decimal.Truncate(i.CALL_CANCEL); i.REJECTIONS = decimal.Truncate(i.REJECTIONS); });
                model.SrNo = recCount;
                model.CONSIGNEE = Convert.ToString(ds.Tables[0].Rows[0]["CONSIGNEE"]);
                model.NO_OF_INSP = Convert.ToString(ds.Tables[0].Rows[0]["NO_OF_INSP"]);
                model.MATERIAL_VALUE = Convert.ToString(ds.Tables[0].Rows[0]["MATERIAL_VALUE"]);
                model.INSP_FEE = Convert.ToString(ds.Tables[0].Rows[0]["INSP_FEE"]);
            }
            model.lstSummaryConreport = lstSummary;
            return model;
        }
    }
}
