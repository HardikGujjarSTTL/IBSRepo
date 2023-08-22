using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Reflection.Emit;
using static IBS.Helper.Enums;

namespace IBS.Repositories
{
    public class LabBillFinalRepository : ILabBillFinalisationRepository
    {
        private readonly ModelContext context;

        public LabBillFinalRepository(ModelContext context)
        {
            this.context = context;
        }
        public List<LabBillFinalisationModel> GetBill(string FromDate, string ToDate, string Regin)
        {

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_wDtFr", OracleDbType.Date, FromDate, ParameterDirection.Input);
                par[1] = new OracleParameter("p_wDtTo", OracleDbType.Date, ToDate, ParameterDirection.Input);
                par[2] = new OracleParameter("p_Region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
                par[3] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("GetLabInvoiceFinalisation", par, 3);

                List<LabBillFinalisationModel> modelList = new List<LabBillFinalisationModel>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        LabBillFinalisationModel model = new LabBillFinalisationModel
                        {
                            InvoiceNo = row["invoice_no"] as string,
                            InvoiceDate = Convert.ToString(row["invoice_dt"]),
                            TotalAmount = Convert.ToString(row["Total_AMT"]),
                            InvoiceSGST = Convert.ToString(row["INV_sgst"]),
                            InvoiceCGST = Convert.ToString(row["INV_cgst"]),
                            InvoiceIGST = Convert.ToString(row["INV_igst"]),
                            InvoiceAmount = Convert.ToString(row["INV_amount"]),
                            BPO_NAME = Convert.ToString(row["BPO_NAME"]),
                            RecipientGSTINNo = Convert.ToString(row["recipient_gstin_no"]),
                        };

                        modelList.Add(model);
                    }
                }

                return modelList;
            }

            //return dTResult;
        }
        public static string GetDateString(string sqlQuery)
        {
            ModelContext context = new ModelContext(DbContextHelper.GetDbContextOptions());
            string dateResult = null;
            try
            {
                var conn = (OracleConnection)context.Database.GetDbConnection();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlQuery;

                    context.Database.OpenConnection();

                    // Execute the SQL query and fetch the date result
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        dateResult = result.ToString();
                    }

                    context.Database.CloseConnection();
                }
            }
            catch (Exception)
            {
                context.Database.CloseConnection();
            }

            return dateResult;
        }
        public bool UpdateBill(LabBillFinalisationModel LabBillFinalisationModel)
        {
            string[] BillNo = LabBillFinalisationModel.InvoiceNo.Split(',');
            string ss;
            string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";

            ss = GetDateString(sqlQuery);
            for (int i = 0; i < BillNo.Length; i++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(BillNo[i].ToString()))
                    {
                        OracleParameter[] par = new OracleParameter[2];
                        par[0] = new OracleParameter("p_invoice_no", OracleDbType.Varchar2, BillNo[i], ParameterDirection.Input);
                        par[1] = new OracleParameter("p_new_invoice_dt", OracleDbType.Date, ss, ParameterDirection.Input);
                        
                        var ds = DataAccessDB.ExecuteNonQuery("UpdateInvoiceDateAndFinalize", par, 1);
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
