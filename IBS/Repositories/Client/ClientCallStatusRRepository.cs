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
    public class ClientCallStatusRRepository : IClientCallStatusRepository
    {
        private readonly ModelContext context;

        public ClientCallStatusRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<ClientCallRptModel> GetCallStatus(DTParameters dtParameters)
        {

            DTResult<ClientCallRptModel> dTResult = new() { draw = 0 };
            IQueryable<ClientCallRptModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "PO_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "PO_NO";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_CLT", OracleDbType.NVarchar2, "R", ParameterDirection.Input);
            par[1] = new OracleParameter("p_RLYCD", OracleDbType.NVarchar2, "SR", ParameterDirection.Input);
            par[2] = new OracleParameter("p_wFrmDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("FromDate"), ParameterDirection.Input);
            par[3] = new OracleParameter("p_wToDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("ToDate"), ParameterDirection.Input);           
            par[4] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_PaymentReport", par, 4);

            List<ClientCallRptModel> modelList = new List<ClientCallRptModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ClientCallRptModel model = new ClientCallRptModel
                    {
                        VendorName = Convert.ToString(row["Vendor"]),
                        PO_NO = Convert.ToString(row["PO_NO"]),
                        PO_DT = Convert.ToString(row["PO_DT"]),
                        ITEM_DESC = Convert.ToString(row["ITEM_DESC"]),                       
                        QTY_TO_INSP = Convert.ToString(row["QTY_TO_INSP"]),
                        QTY_REJECTED = Convert.ToString(row["QTY_REJECTED"]),
                        REASON_REJECT = Convert.ToString(row["REASON_REJECT"]),
                        IC_DT = Convert.ToString(row["IC_DT"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        BK_NO = Convert.ToString(row["BK_NO"]),
                        SET_NO = Convert.ToString(row["SET_NO"]),
                        
                        
                    };

                    modelList.Add(model);
                }
            }

            
            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.PO_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.PO_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

            
        }

    }
}
