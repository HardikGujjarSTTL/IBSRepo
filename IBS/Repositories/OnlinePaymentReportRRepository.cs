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
    public class OnlinePaymentReportRRepository : IOnlinePaymentReportRepository
    {
        private readonly ModelContext context;

        public OnlinePaymentReportRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<OnlinePaymentReportModel> OnlinePaymentReport(DTParameters dtParameters, string Regin)
        {

            DTResult<OnlinePaymentReportModel> dTResult = new() { draw = 0 };
            IQueryable<OnlinePaymentReportModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "DATETIME";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "DATETIME";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("wDtFr", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wFrmDtO"), ParameterDirection.Input);
            par[1] = new OracleParameter("wDtTo", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[2] = new OracleParameter("region", OracleDbType.NVarchar2,Regin , ParameterDirection.Input);
            par[3] = new OracleParameter("cur", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetOnlinePaymentReport", par, 3);

            List<OnlinePaymentReportModel> modelList = new List<OnlinePaymentReportModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    OnlinePaymentReportModel model = new OnlinePaymentReportModel
                    {
                        MER_TXN_REF = Convert.ToString(row["MER_TXN_REF"]),
                        ORDER_INFO = Convert.ToString(row["ORDER_INFO"]),
                        TRANSACTION_NO = Convert.ToString(row["TRANSACTION_NO"]),
                        RRN_NO = Convert.ToString(row["RRN_NO"]),
                        AUTH_CD = Convert.ToString(row["AUTH_CD"]),
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_DT = Convert.ToString(row["CALL_DT"]),
                        CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                        VENDOR = Convert.ToString(row["VENDOR"]),
                        AMOUNT = Convert.ToString(row["AMOUNT"]),
                        TYPE = Convert.ToString(row["TYPE"]),
                        STATUS = Convert.ToString(row["STATUS"]),
                        DATETIME = Convert.ToString(row["DATETIME"]),
                    };

                    modelList.Add(model);
                }
            }



            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.VENDOR).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.VENDOR).ToLower().Contains(searchBy.ToLower())
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
