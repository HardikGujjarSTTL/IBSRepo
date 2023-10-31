using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using Oracle.ManagedDataAccess.Client;
using PuppeteerSharp;
using System.Data;
using System.Drawing;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace IBS.Repositories.Reports
{
    public class BPOWiseOutRRepository : IBPOWiseOutstandingBillsRepository
    {
        private readonly ModelContext context;

        public BPOWiseOutRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DataSet GenerateReport(BPOWiseOutstandingBillsModel BPOWiseOutstandingBillsModel)
        {

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[10];
                par[0] = new OracleParameter("p_RegionCode", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.Region, ParameterDirection.Input);
                par[1] = new OracleParameter("p_StartDate", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.FromDt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_EndDate", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.ToDt, ParameterDirection.Input);
                par[3] = new OracleParameter("p_BPOCode", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.BpoCd, ParameterDirection.Input);
                par[4] = new OracleParameter("p_BPOType", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.BpoType, ParameterDirection.Input);
                par[5] = new OracleParameter("p_BPORly", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.BpoRly, ParameterDirection.Input);
                par[6] = new OracleParameter("p_BPORegion", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.BpoRegion, ParameterDirection.Input);
                par[7] = new OracleParameter("p_BPOTypeFilter", OracleDbType.NVarchar2, "", ParameterDirection.Input);
                par[8] = new OracleParameter("p_TypeOutstanding", OracleDbType.NVarchar2, BPOWiseOutstandingBillsModel.TypeofOutStandingBills, ParameterDirection.Input);
                par[9] = new OracleParameter("p_ResultCursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("BPO_OUTSTANDING_BILLS_REPORT", par, 9);
                
        //            var columns = new string[]
        //{
        //    "BILL_NO", "BILL_DT", "BILL_AMOUNT", "BK_NO", "SET_NO", "TOTAL_TDS",
        //    "RETENTION_MONEY", "CNOTE_AMOUNT", "WRITE_OFF_AMT", "AMOUNT_POSTED", "AMOUNT_REALISED", "AMOUNT_OUTSTADING",
        //    "CASE_NO", "PO_NO", "PO_DT", "CONSIGNEE", "BPO", "SAP_CUST_CD_BPO", "VENDOR", "IE", "INVOICE_NO", "LO_REMARKS"
        //};
                //byte[] fileContents;
                //return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Outstanding.xlsx");
                //List<LABREGISTERModel> modelList = new List<LABREGISTERModel>();
                //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    foreach (DataRow row in ds.Tables[0].Rows)
                //    {
                //        LABREGISTERModel model = new LABREGISTERModel
                //        {
                //            CHQ_NO = row["CHQ_NO"] as string,
                //            CHQ_DATE = Convert.ToString(row["CHQ_DATE"]),
                //            AMOUNT = Convert.ToString(row["AMOUNT"]),
                //            SUSPENSE_AMT = Convert.ToString(row["SUSPENSE_AMT"]),
                //            BANK_NAME = Convert.ToString(row["BANK_NAME"]),
                //            CASE_NO = Convert.ToString(row["CASE_NO"]),
                //            BANK_CD = Convert.ToString(row["BANK_CD"]),
                //            NARRATION = Convert.ToString(row["NARRATION"]),
                //        };

                //        modelList.Add(model);
                //    }
                //}

                return ds;
            }

            //return dTResult;
        }
    }
}
