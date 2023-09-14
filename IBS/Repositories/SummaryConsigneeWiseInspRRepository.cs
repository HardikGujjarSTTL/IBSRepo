using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Reflection.Emit;
using static IBS.Helper.Enums;

namespace IBS.Repositories
{
    public class SummaryConsigneeWiseInspRRepository : ISummaryConsigneeWiseInspRepository
    {
        private readonly ModelContext context;

        public SummaryConsigneeWiseInspRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<SummaryConsigneeWiseInspModel> SummaryConsigneeWiseInsp(DTParameters dtParameters, string Regin)
        {

            DTResult<SummaryConsigneeWiseInspModel> dTResult = new() { draw = 0 };
            IQueryable<SummaryConsigneeWiseInspModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "NO_OF_INSP";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "NO_OF_INSP";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[11];
            par[0] = new OracleParameter("P_REGION_CODE", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("P_ForGPer", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rbrecdt"), ParameterDirection.Input);
            par[2] = new OracleParameter("P_rdbICDT", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("status"), ParameterDirection.Input);
            par[3] = new OracleParameter("P_rdbPart", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wFrmDtO"), ParameterDirection.Input);
            par[4] = new OracleParameter("P_rdbMatValue", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[5] = new OracleParameter("p_From_Date", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[6] = new OracleParameter("p_To_Date", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[7] = new OracleParameter("P_lstConsignee", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[8] = new OracleParameter("p_Year", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[9] = new OracleParameter("p_Month", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[10] = new OracleParameter("P_CONSIGNEE_INFO", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CONSIGNEE_INFO", par, 10);

            List<SummaryConsigneeWiseInspModel> modelList = new List<SummaryConsigneeWiseInspModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    SummaryConsigneeWiseInspModel model = new SummaryConsigneeWiseInspModel
                    {
                        CONSIGNEE = row["CONSIGNEE"].ToString(),
                        NO_OF_INSP = row["NO_OF_INSP"].ToString(),
                        MATERIAL_VALUE = row["MATERIAL_VALUE"].ToString(),
                        INSP_FEE = row["INSP_FEE"].ToString(),
                        
                    };

                    modelList.Add(model);
                }
            }



            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CONSIGNEE).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CONSIGNEE).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.data = query.ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

            //using (var dbContext = context.Database.GetDbConnection())
            //{

            //}

            //return dTResult;
        }

    }
}
