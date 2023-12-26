using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class LabInvoiceReportRRepository : ILabInvoiceReportRepository
    {
        private readonly ModelContext context;

        public LabInvoiceReportRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<LabInvoiceReportModel> LabInvoiceReport(DTParameters dtParameters, string Regin)
        {

            DTResult<LabInvoiceReportModel> dTResult = new() { draw = 0 };
            IQueryable<LabInvoiceReportModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "invoice_no";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "invoice_no";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("wFrmDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wFrmDtO"), ParameterDirection.Input);
            par[1] = new OracleParameter("wToDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("wToDt"), ParameterDirection.Input);
            par[2] = new OracleParameter("region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[3] = new OracleParameter("cur", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetLabInvoiceReport", par, 3);

            List<LabInvoiceReportModel> modelList = new List<LabInvoiceReportModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LabInvoiceReportModel model = new LabInvoiceReportModel
                    {
                        BPO_NAME = row["BPO_NAME"].ToString(),
                        recipient_gstin_no = row["recipient_gstin_no"].ToString(),
                        St_cd = row["St_cd"].ToString(),
                        invoice_no = row["invoice_no"].ToString(),
                        invoice_dt = row["invoice_dt"].ToString(),
                        Total_AMT = Convert.ToString(row["Total_AMT"]),
                        INV_TYPE = row["INV_TYPE"].ToString(),
                        HSN_CD = row["HSN_CD"].ToString(),
                        INV_amount = Convert.ToString(row["INV_amount"]),
                        INV_sgst = Convert.ToString(row["INV_sgst"]),
                        INV_cgst = Convert.ToString(row["INV_cgst"]),
                        INV_igst = Convert.ToString(row["INV_igst"]),
                        INVOICE_TYPE = row["INVOICE_TYPE"].ToString(),
                        INC_TYPE = row["INC_TYPE"].ToString(),
                        Total_GST = Convert.ToString(row["Total_GST"]),
                        IRN_NO = row["IRN_NO"].ToString(),
                        BILL_FINALIZE = row["BILL_FINALIZE"].ToString(),
                        BILL_SENT = row["BILL_SENT"].ToString(),
                    };

                    modelList.Add(model);
                }
            }



            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.BPO_NAME).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.BPO_NAME).ToLower().Contains(searchBy.ToLower())
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
