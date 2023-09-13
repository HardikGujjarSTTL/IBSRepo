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
    public class LabPerformanceReportRRepository : ILabPerfomanceReportRepository
    {
        private readonly ModelContext context;

        public LabPerformanceReportRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<LabPerfomanceReport> labPerformanceReport(DTParameters dtParameters, string Regin)
        {

            DTResult<LabPerfomanceReport> dTResult = new() { draw = 0 };
            IQueryable<LabPerfomanceReport>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "NO_OF_TEST";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "NO_OF_TEST";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("wFrmDt", OracleDbType.Varchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wFrmDtO"), ParameterDirection.Input);
            par[1] = new OracleParameter("wToDt", OracleDbType.Varchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[2] = new OracleParameter("result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Lab_Performance_Report", par, 2);

            List<LabPerfomanceReport> modelList = new List<LabPerfomanceReport>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LabPerfomanceReport model = new LabPerfomanceReport
                    {
                        LAB = Convert.ToString(row["LAB"]),
                        NO_OF_TEST = Convert.ToString(row["NO_OF_TEST"]),
                        NO_OF_SAMPLES = Convert.ToString(row["NO_OF_SAMPLES"]),
                        NO_OF_FAILURE = Convert.ToString(row["NO_OF_FAILURE"]),
                        NO_OF_FAIL_SAMPLES = Convert.ToString(row["NO_OF_FAIL_SAMPLES"]),
                        NO_OFNOCOMMENTS = Convert.ToString(row["NO_OFNOCOMMENTS"]),
                        MAXM_DAYS = Convert.ToString(row["MAXM_DAYS"]),
                        MIN_DAYS = Convert.ToString(row["MIN_DAYS"]),
                        AVG_DAYS = Convert.ToString(row["AVG_DAYS"]),
                        TOTAL_FEE = Convert.ToString(row["TOTAL_FEE"]),
                       
                    };

                    modelList.Add(model);
                }
            }



            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.LAB).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.LAB).ToLower().Contains(searchBy.ToLower())
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
