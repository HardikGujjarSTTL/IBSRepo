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
    public class LabSamInfoReportRRepository : ILabSamInfoReportRepository
    {
        private readonly ModelContext context;

        public LabSamInfoReportRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<LabSamInfoReportModel> LabSamInfoReport(DTParameters dtParameters, string Regin)
        {

            DTResult<LabSamInfoReportModel> dTResult = new() { draw = 0 };
            IQueryable<LabSamInfoReportModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "SampleRecvDt";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "SampleRecvDt";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_lstStatus", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("status"), ParameterDirection.Input);
            par[1] = new OracleParameter("rdbrecvdt", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rbrecdt"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_Region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[3] = new OracleParameter("p_frmDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("wFrmDtO"), ParameterDirection.Input);
            par[4] = new OracleParameter("p_toDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[5] = new OracleParameter("p_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("getLabSampleInfoReport", par, 5);

            List<LabSamInfoReportModel> modelList = new List<LabSamInfoReportModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LabSamInfoReportModel model = new LabSamInfoReportModel
                    {
                        CaseNo = row["case_no"].ToString(),
                        CallRecvDt = row["call_recv_dt"].ToString(),
                        SampleRegNo = row["sample_reg_no"].ToString(),
                        CallSno = row["call_sno"].ToString(),
                        VendName = row["vend_name"].ToString(),
                        MfgName = row["mfg_name"].ToString(),
                        IeName = row["ie_name"].ToString(),
                        Remarks = row["REMARKS"].ToString(),
                        LabStatus = row["LAB_STATUS"].ToString(),
                        DocRejRemark = row["DOC_REJ_REMARK"].ToString(),
                        LikelyDtReport = row["likely_dt_report"].ToString(),
                        TestingChargesByLab = row["testing_charges_by_lab"].ToString(),
                        TestingChargesByVendor = row["testing_charges_by_vendor"].ToString(),
                        TdsChargesByVendor = row["tds_charges_by_vendor"].ToString(),
                        CallDocDt = row["call_doc_dt"].ToString(),
                        DocStatusFin = row["doc_status_fin"].ToString(),
                        VendInitDt = row["Vend_INIT_DT"].ToString(),
                        FinInitDt = row["FIN_INIT_DT"].ToString(),
                        SampleRecvDt = row["SAMPLE_RECV_DT"].ToString(),
                        UtrNo = row["UTR_NO"].ToString(),
                        UtrDate = row["UTR_DATE"].ToString(),
                    };

                    modelList.Add(model);
                }
            }



            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.VendName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.VendName).ToLower().Contains(searchBy.ToLower())
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
