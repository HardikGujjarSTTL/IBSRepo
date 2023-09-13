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
    public class LabPostingReportRRepository : ILabPostingReportRepository
    {
        private readonly ModelContext context;

        public LabPostingReportRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<LabPostingReport> labPostingReport(DTParameters dtParameters, string Regin)
        {

            DTResult<LabPostingReport> dTResult = new() { draw = 0 };
            IQueryable<LabPostingReport>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "sampleRegNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "sampleRegNo";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_Region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("p_FromDate", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("wFrmDtO"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_ToDate", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[3] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("LabRegisterReportPosting", par, 3);

            List<LabPostingReport> modelList = new List<LabPostingReport>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LabPostingReport model = new LabPostingReport
                    {
                        sampleRegNo = Convert.ToString(row["SAMPLE_REG_NO"]),
                        sampleRegDate = Convert.ToString(row["SAMPLE_REG_DATE"]),
                        caseNo = Convert.ToString(row["CASE_NO"]),
                        callRecvDate = Convert.ToString(row["CALL_RECV_DATE"]),
                        callSno = Convert.ToString(row["CALL_SNO"]),
                        vendor = Convert.ToString(row["VENDOR"]),
                        ieName = Convert.ToString(row["IE_NAME"]),
                        amountReceived = Convert.ToString(row["AMOUNT_RECIEVED"]),
                        totalLabCharges = Convert.ToString(row["TOTAL_LAB_CHARGES"]),
                        tdsAmt = Convert.ToString(row["TDS_AMT"]),
                        tdsDate = Convert.ToString(row["TDS_DATE"]),
                        amtDue = Convert.ToString(row["AMT_DUE"]),
                        chqNo = Convert.ToString(row["CHQ_NO"]),
                        chqDate = Convert.ToString(row["CHQ_DATE"]),
                        amount = Convert.ToString(row["AMOUNT"]),
                        chqCaseNo = Convert.ToString(row["CHQ_CASE_NO"]),
                        amountAdjusted = Convert.ToString(row["AMOUNT_ADJUSTED"]),
                        suspenseAmt = Convert.ToString(row["SUSPENSE_AMT"]),
                    };

                    modelList.Add(model);
                }
            }



            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.ieName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.ieName).ToLower().Contains(searchBy.ToLower())
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
