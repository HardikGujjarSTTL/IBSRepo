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
    public class LabSamplePaymentRptRRepository : ILabSamplePaymentRptRepository
    {
        private readonly ModelContext context;

        public LabSamplePaymentRptRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<LabSamplePaymentRptModel> GetPaymentReport(DTParameters dtParameters, string Regin)
        {

            DTResult<LabSamplePaymentRptModel> dTResult = new() { draw = 0 };
            IQueryable<LabSamplePaymentRptModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_lstStatus", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("ReportStatus"), ParameterDirection.Input);
            par[1] = new OracleParameter("p_rdbrecvdt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("RadioButton"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_frmDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("FromDate"), ParameterDirection.Input);
            par[3] = new OracleParameter("p_toDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("ToDate"), ParameterDirection.Input);
            par[4] = new OracleParameter("p_REGION", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[5] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_PaymentReport", par, 5);

            List<LabSamplePaymentRptModel> modelList = new List<LabSamplePaymentRptModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LabSamplePaymentRptModel model = new LabSamplePaymentRptModel
                    {
                        SampleRecvDate = Convert.ToString(row["call_recv_dt"]),
                        SampleRegNo = Convert.ToString(row["sample_reg_no"]),
                        CaseNo = Convert.ToString(row["case_no"]),
                        IEName = Convert.ToString(row["ie_name"]),
                        VendorName = Convert.ToString(row["vend_name"]),
                        ManufacturerName = Convert.ToString(row["mfg_name"]),
                        LikelyDateReport = Convert.ToString(row["likely_dt_report"]),
                        LabStatus = Convert.ToString(row["LAB_STATUS"]),
                        TestingChargesByLab = Convert.ToString(row["testing_charges_by_lab"]),
                        TestingChargesByVendor = Convert.ToString(row["testing_charges_by_vendor"]),
                        TdsChargesByVendor = Convert.ToString(row["tds_charges_by_vendor"]),
                        VendorInitDate = Convert.ToString(row["Vend_INIT_DT"]),
                        UTRNo = Convert.ToString(row["UTR_NO"]),
                        UTRDate = Convert.ToString(row["UTR_DATE"]),
                        DocStatusFin = Convert.ToString(row["doc_status_fin"]),
                        FinInitDate = Convert.ToString(row["FIN_INIT_DT"]),
                        Remarks = Convert.ToString(row["REMARKS"]),
                        DocRejRemark = Convert.ToString(row["DOC_REJ_REMARK"]),
                    };

                    modelList.Add(model);
                }
            }

            
            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CallSno).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

            
        }

    }
}
