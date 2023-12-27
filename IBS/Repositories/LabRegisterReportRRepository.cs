using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class LabRegisterReportRRepository : ILabRegisterReportRepository
    {
        private readonly ModelContext context;

        public LabRegisterReportRRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<LabRegisterReport> labRegisterReport(DTParameters dtParameters, string Regin)
        {

            DTResult<LabRegisterReport> dTResult = new() { draw = 0 };
            IQueryable<LabRegisterReport>? query = null;

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

            OracleParameter[] par = new OracleParameter[18];
            par[0] = new OracleParameter("p_region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("p_wFrmDtO", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("wFrmDtO"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_wToDt", OracleDbType.Date, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[3] = new OracleParameter("p_rdbPending", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rdbPending"), ParameterDirection.Input);
            par[4] = new OracleParameter("p_rdbPaid", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rdbPaid"), ParameterDirection.Input);
            par[5] = new OracleParameter("p_rdbDue", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rdbDue"), ParameterDirection.Input);
            par[6] = new OracleParameter("p_rdbPartlyPaid", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rdbPartlyPaid"), ParameterDirection.Input);
            par[7] = new OracleParameter("p_lstTStatus", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("lstTStatus"), ParameterDirection.Input);
            par[8] = new OracleParameter("p_rdbIEWise", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rdbIEWise"), ParameterDirection.Input);
            par[9] = new OracleParameter("p_rdbPIE", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rdbPIE"), ParameterDirection.Input);
            par[10] = new OracleParameter("p_lstIE", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("lstIE"), ParameterDirection.Input);
            par[11] = new OracleParameter("p_rdbVendWise", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rdbVendWise"), ParameterDirection.Input);
            par[12] = new OracleParameter("p_rdbPVend", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rdbPVend"), ParameterDirection.Input);
            par[13] = new OracleParameter("p_ddlVender", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("ddlVender"), ParameterDirection.Input);
            par[14] = new OracleParameter("p_rdbLabWise", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rdbLabWise"), ParameterDirection.Input);
            par[15] = new OracleParameter("p_rdbPLab", OracleDbType.Boolean, dtParameters.AdditionalValues?.GetValueOrDefault("rdbPLab"), ParameterDirection.Input);
            par[16] = new OracleParameter("p_lstLab", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("lstLab"), ParameterDirection.Input);
            par[17] = new OracleParameter("p_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("LabRegisterReport", par, 17);

            List<LabRegisterReport> modelList = new List<LabRegisterReport>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LabRegisterReport model = new LabRegisterReport
                    {
                        sampleRegNo = Convert.ToString(row["SAMPLE_REG_NO"]),
                        sampleRegDate = Convert.ToString(row["SAMPLE_REG_DATE"]),
                        caseNo = Convert.ToString(row["CASE_NO"]),
                        callRecvDate = Convert.ToString(row["CALL_RECV_DATE"]),
                        callSno = Convert.ToString(row["CALL_SNO"]),
                        TestingType = Convert.ToString(row["T_TYPE"]),
                        CodeNo = Convert.ToString(row["CODE_NO"]),
                        CodeDt = Convert.ToString(row["CODE_DATE"]),
                        vendor = Convert.ToString(row["VENDOR"]),
                        ieName = Convert.ToString(row["IE_NAME"]),
                        Lab = Convert.ToString(row["LAB"]),
                        TestRptDt = Convert.ToString(row["TEST_REPORT_REC_DATE"]),
                        TestStatus = Convert.ToString(row["TEST_STATUS"]),
                        TestingFee = Convert.ToString(row["TESTING_FEE"]),
                        ServiceTax = Convert.ToString(row["SERVICE_TAX"]),
                        HandlingCharge = Convert.ToString(row["HANDLING_CHARGES"]),
                        amountReceived = Convert.ToString(row["AMOUNT_RECIEVED"]),
                        tdsAmt = Convert.ToString(row["TDS_AMT"]),
                        tdsDate = Convert.ToString(row["TDS_DATE"]),
                        amtDue = Convert.ToString(row["AMT_DUE"]),
                        Test = Convert.ToString(row["TEST"]),
                        ItemDesc = Convert.ToString(row["ITEM_DESC"]),
                        Remarks = Convert.ToString(row["REMARKS"]),
                        SampleDispatchDt = Convert.ToString(row["SAMPLE_DISPATCH_DATE"]),
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
