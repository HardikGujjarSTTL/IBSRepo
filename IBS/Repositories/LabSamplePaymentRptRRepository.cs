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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories
{
    public class LabSamplePaymentRptRRepository : ILabSamplePaymentRptRepository
    {
        private readonly ModelContext context;

        public LabSamplePaymentRptRRepository(ModelContext context)
        {
            this.context = context;
        }
        //public LabSamplePaymentRptModel GetPaymentReport(string ReportType, string wFrmDtO, string wToDt, string Status, string rbsrdt, string Regin)
        //{

        //    DTResult<LabSamplePaymentRptModel> dTResult = new() { draw = 0 };
        //    IQueryable<LabSamplePaymentRptModel>? query = null;

        //    var searchBy = dtParameters.Search?.Value;
        //    var orderCriteria = string.Empty;
        //    var orderAscendingDirection = true;

        //    if (dtParameters.Order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

        //        if (orderCriteria == "")
        //        {
        //            orderCriteria = "CaseNo";
        //        }
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }
        //    else
        //    {
        //        // if we have an empty search then just order the results by Id ascending
        //        orderCriteria = "CaseNo";
        //        orderAscendingDirection = true;
        //    }

        //    OracleParameter[] par = new OracleParameter[6];
        //    par[0] = new OracleParameter("p_lstStatus", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("ReportStatus"), ParameterDirection.Input);
        //    par[1] = new OracleParameter("p_rdbrecvdt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("RadioButton"), ParameterDirection.Input);
        //    par[2] = new OracleParameter("p_frmDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("FromDate"), ParameterDirection.Input);
        //    par[3] = new OracleParameter("p_toDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("ToDate"), ParameterDirection.Input);
        //    par[4] = new OracleParameter("p_REGION", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
        //    par[5] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

        //    var ds = DataAccessDB.GetDataSet("GET_PaymentReport", par, 5);

        //    List<LabSamplePaymentRptModel> modelList = new List<LabSamplePaymentRptModel>();
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            LabSamplePaymentRptModel model = new LabSamplePaymentRptModel
        //            {
        //                SampleRecvDate = Convert.ToString(row["call_recv_dt"]),
        //                SampleRegNo = Convert.ToString(row["sample_reg_no"]),
        //                CaseNo = Convert.ToString(row["case_no"]),
        //                IEName = Convert.ToString(row["ie_name"]),
        //                VendorName = Convert.ToString(row["vend_name"]),
        //                ManufacturerName = Convert.ToString(row["mfg_name"]),
        //                LikelyDateReport = Convert.ToString(row["likely_dt_report"]),
        //                LabStatus = Convert.ToString(row["LAB_STATUS"]),
        //                TestingChargesByLab = Convert.ToString(row["testing_charges_by_lab"]),
        //                TestingChargesByVendor = Convert.ToString(row["testing_charges_by_vendor"]),
        //                TdsChargesByVendor = Convert.ToString(row["tds_charges_by_vendor"]),
        //                VendorInitDate = Convert.ToString(row["Vend_INIT_DT"]),
        //                UTRNo = Convert.ToString(row["UTR_NO"]),
        //                UTRDate = Convert.ToString(row["UTR_DATE"]),
        //                DocStatusFin = Convert.ToString(row["doc_status_fin"]),
        //                FinInitDate = Convert.ToString(row["FIN_INIT_DT"]),
        //                Remarks = Convert.ToString(row["REMARKS"]),
        //                DocRejRemark = Convert.ToString(row["DOC_REJ_REMARK"]),
        //            };

        //            modelList.Add(model);
        //        }
        //    }

            
        //    query = modelList.AsQueryable();

        //    dTResult.recordsTotal = query.Count();

        //    if (!string.IsNullOrEmpty(searchBy))
        //        query = query.Where(w => Convert.ToString(w.CallSno).ToLower().Contains(searchBy.ToLower())
        //        || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
        //        );

        //    dTResult.recordsFiltered = query.Count();

        //    dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

        //    dTResult.draw = dtParameters.Draw;

        //    return dTResult;

            
        //}
        public LabSamplePaymentRptModel GetPaymentReport(string ReportType, string wFrmDtO, string wToDt, string Status, string rbsrdt, string Regin)
        {

            LabSamplePaymentRptModel model = new();
            List<LabSamplePaymentRptModel> lstlab = new();


            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_lstStatus", OracleDbType.NVarchar2, Status, ParameterDirection.Input);
            par[1] = new OracleParameter("p_rdbrecvdt", OracleDbType.NVarchar2, rbsrdt, ParameterDirection.Input);
            par[2] = new OracleParameter("p_frmDt", OracleDbType.NVarchar2, wFrmDtO, ParameterDirection.Input);
            par[3] = new OracleParameter("p_toDt", OracleDbType.NVarchar2, wToDt, ParameterDirection.Input);
            par[4] = new OracleParameter("p_REGION", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[5] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_PaymentReport", par, 5);

            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    lstlab = JsonConvert.DeserializeObject<List<LabSamplePaymentRptModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


                    
                    model.sample_reg_no = Convert.ToString(ds.Tables[0].Rows[0]["sample_reg_no"]);
                    model.call_recv_dt = Convert.ToString(ds.Tables[0].Rows[0]["call_recv_dt"]);
                    model.case_no = Convert.ToString(ds.Tables[0].Rows[0]["case_no"]);
                    model.ie_name = Convert.ToString(ds.Tables[0].Rows[0]["ie_name"]);
                    model.vend_name = Convert.ToString(ds.Tables[0].Rows[0]["vend_name"]);
                    model.mfg_name = Convert.ToString(ds.Tables[0].Rows[0]["mfg_name"]);
                    model.likely_dt_report = Convert.ToString(ds.Tables[0].Rows[0]["likely_dt_report"]);
                    model.LAB_STATUS = Convert.ToString(ds.Tables[0].Rows[0]["LAB_STATUS"]);
                    model.testing_charges_by_lab = Convert.ToString(ds.Tables[0].Rows[0]["testing_charges_by_lab"]);
                    model.testing_charges_by_vendor = Convert.ToString(ds.Tables[0].Rows[0]["testing_charges_by_vendor"]);
                    model.tds_charges_by_vendor = Convert.ToString(ds.Tables[0].Rows[0]["tds_charges_by_vendor"]);
                    model.Vend_INIT_DT = Convert.ToString(ds.Tables[0].Rows[0]["Vend_INIT_DT"]);
                    model.UTR_NO = Convert.ToString(ds.Tables[0].Rows[0]["UTR_NO"]);
                    model.UTR_DATE = Convert.ToString(ds.Tables[0].Rows[0]["UTR_DATE"]);
                    model.doc_status_fin = Convert.ToString(ds.Tables[0].Rows[0]["doc_status_fin"]);
                    model.FIN_INIT_DT = Convert.ToString(ds.Tables[0].Rows[0]["FIN_INIT_DT"]);
                    model.REMARKS = Convert.ToString(ds.Tables[0].Rows[0]["REMARKS"]);
                    model.DOC_REJ_REMARK = Convert.ToString(ds.Tables[0].Rows[0]["DOC_REJ_REMARK"]);

                }
                model.lstLabSample = lstlab;
            }
            return model;
        }
    }
}
