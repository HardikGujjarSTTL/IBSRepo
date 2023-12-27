﻿using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class LabInvoiceDownloadRRepository : ILabInvoiceDownloadRepository
    {
        private readonly ModelContext context;

        public LabInvoiceDownloadRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<LabInvoiceDownloadModel> GetLapInvoice(DTParameters dtParameters, string Regin)
        {

            DTResult<LabInvoiceDownloadModel> dTResult = new() { draw = 0 };
            IQueryable<LabInvoiceDownloadModel>? query = null;

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

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_case_no", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CaseNo"), ParameterDirection.Input);
            par[1] = new OracleParameter("p_sample_reg_no", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("RegNo"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_transaction_no", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("TranNo"), ParameterDirection.Input);
            par[3] = new OracleParameter("p_invoice_no", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("InNo"), ParameterDirection.Input);
            par[4] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_INVOICE_DETAILS", par, 4);

            List<LabInvoiceDownloadModel> modelList = new List<LabInvoiceDownloadModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LabInvoiceDownloadModel model = new LabInvoiceDownloadModel
                    {
                        CASE_NO = Convert.ToString(row["case_no"]),
                        SAMPLE_REG_NO = Convert.ToString(row["sample_reg_no"]),
                        TRANSACTION_NO = Convert.ToString(row["transaction_no"]),
                        INVOICE_NO = Convert.ToString(row["invoice_no"]),
                        INVOICE_DT = Convert.ToString(row["invoice_dt"]),
                        INV_AMOUNT = Convert.ToString(row["INV_amount"]),
                        INV_SGST = Convert.ToString(row["INV_sgst"]),
                        INV_CGST = Convert.ToString(row["INV_cgst"]),
                        INV_IGST = Convert.ToString(row["INV_igst"]),


                    };

                    modelList.Add(model);
                }
            }

            //query = from l in context.Roles
            //        where l.Isdeleted == 0
            //        select new RoleModel
            //        {
            //            RoleId = l.RoleId,
            //            Rolename = l.Rolename,
            //            Roledescription = l.Roledescription,
            //            Issysadmin = Convert.ToBoolean(l.Issysadmin),
            //            Isactive = Convert.ToBoolean(l.Isactive),
            //            Isdeleted = l.Isdeleted,
            //            Createddate = l.Createddate,
            //            Createdby = l.Createdby,
            //            Updateddate = l.Updateddate,
            //            Updatedby = l.Updatedby
            //        };

            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.INVOICE_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.INVOICE_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

            //using (var dbContext = context.Database.GetDbConnection())
            //{

            //}

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
        public string GetSrNo(string InvoiceNo)
        {

            string query = "Select max(ITEM_SRNO) from T86_LAB_INVOICE_DETAILS where INVOICE_NO='" + InvoiceNo + "'";
            string ds = GetDateString(query);
            return ds.ToString();

        }
        public static DataSet GetDataset(string sqlQuery)
        {
            DataSet ds = new DataSet();
            ModelContext context = new ModelContext(DbContextHelper.GetDbContextOptions());

            try
            {
                using (var conn = (OracleConnection)context.Database.GetDbConnection())
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sqlQuery;

                        using (var adapter = new OracleDataAdapter(cmd))
                        {
                            // Fill the dataset with the data retrieved by the query
                            adapter.Fill(ds);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Handle exceptions if needed
            }

            return ds;
        }
        public LabInvoiceDownloadModel Getdtreg(string InvoiceNo)
        {


            OracleParameter[] par = new OracleParameter[1];

            var query = "SELECT TO_CHAR(INVOICE_DT, 'yyyymm') AS INVOICE_DT, REGION_CODE FROM T55_LAB_INVOICE WHERE INVOICE_NO ='" + InvoiceNo + "'";

            DataSet ds = GetDataset(query);

            LabInvoiceDownloadModel model = new LabInvoiceDownloadModel();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                model.INVOICE_DT = Convert.ToString(row["INVOICE_DT"]);
                model.Resign = Convert.ToString(row["REGION_CODE"]);
            }

            return model;

        }
        public DataSet Getdata(string CaseNo, string RegNo, string InvoiceNo, string TranNo)
        {
            DataSet ds = new DataSet();
            try
            {

                OracleParameter[] par = new OracleParameter[5];
                par[0] = new OracleParameter("p_InvoiceNo", OracleDbType.Varchar2, InvoiceNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CaseNo", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
                par[2] = new OracleParameter("p_SamNo", OracleDbType.Date, RegNo, ParameterDirection.Input);
                par[3] = new OracleParameter("p_TN", OracleDbType.Varchar2, TranNo, ParameterDirection.Input);
                par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Input);

                ds = DataAccessDB.GetDataSet("LabInvoiceDownload", par, 4);
            }
            catch (Exception ex)
            {
                //return false;
            }
            return ds;
        }
    }
}
