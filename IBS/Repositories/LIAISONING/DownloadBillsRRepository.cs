using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using static IBS.Helper.Enums;

namespace IBS.Repositories
{
    public class DownloadBillsRRepository : IDownloadBillsRepository
    {
        private readonly ModelContext context;

        public DownloadBillsRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<DownloadBillsModel> GetReturnedBills(DTParameters dtParameters, string OrgType, string Org, IWebHostEnvironment webHostEnvironment)
        {

            
            DTResult<DownloadBillsModel> dTResult = new() { draw = 0 };
            IQueryable<DownloadBillsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            //if (dtParameters.Order != null)
            //{
            //    // in this example we just default sort on the 1st column
            //    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

            //    if (orderCriteria == "")
            //    {
            //        orderCriteria = "BILL_NO";
            //    }
            //    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            //}
            //else
            //{
            //    // if we have an empty search then just order the results by Id ascending
            //    orderCriteria = "BILL_NO";
            //    orderAscendingDirection = true;
            //}
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "BillNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "BillNo";
                orderAscendingDirection = true;
            }
            string month = dtParameters.AdditionalValues?.GetValueOrDefault("Month");
            string year = dtParameters.AdditionalValues?.GetValueOrDefault("Year");
            string wYrMth;
            
            if (dtParameters.AdditionalValues?.GetValueOrDefault("Rb1") == "true")                
            {
                 wYrMth = year + month;
            }
            else
            {
                 wYrMth = null;
            }
           
            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_LO_ORGN_TYPE", OracleDbType.NVarchar2, OrgType, ParameterDirection.Input);
            par[1] = new OracleParameter("p_LO_ORGN", OracleDbType.NVarchar2, Org, ParameterDirection.Input);
            par[2] = new OracleParameter("p_wYrMth", OracleDbType.NVarchar2, wYrMth, ParameterDirection.Input);
            par[3] = new OracleParameter("p_fromDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("FromDate"), ParameterDirection.Input);
            par[4] = new OracleParameter("p_toDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("ToDate"), ParameterDirection.Input);
            par[5] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_BILL_DATA", par, 5);
            
            List<DownloadBillsModel> modelList = new List<DownloadBillsModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    
                    string IC = Path.GetFullPath("BILL_IC/" + row["IC_PHOTO"].ToString() + ".PDF");
                    string CaseNo = Path.GetFullPath("CASE_NO/" + row["CASE_NO"].ToString() + ".PDF");
                    string ICPath = IC;
                    string CaseNoPath = CaseNo;
                    if (File.Exists(IC) == false)
                    {
                        IC = null;
                    }
                    else
                    {
                        IC = row["IC_PHOTO"].ToString();
                    }
                    if (File.Exists(CaseNo) == false)
                    {
                        CaseNo = null;
                    }
                    else
                    {
                        CaseNo = row["CASE_NO"].ToString();
                    }

                    DownloadBillsModel model = new DownloadBillsModel
                    {                        
                        BpoRly = Convert.ToString(row["BPO_RLY"]),
                        PoNo = Convert.ToString(row["PO_NO"]),
                        PoDt = Convert.ToString(row["PO_DT"]),
                        CaseNo = Convert.ToString(row["CASE_NO"]),
                        PoOrLetter = Convert.ToString(row["PO_OR_LETTER"]),
                        BpoName = Convert.ToString(row["BPO_NAME"]),
                        BillNo = Convert.ToString(row["BILL_NO"]),
                        BillDt = Convert.ToString(row["BILL_DT"]),
                        InvoiceNo = Convert.ToString(row["INVOICE_NO"]),
                        AuDesc = Convert.ToString(row["AU_DESC"]),
                        IcNo = Convert.ToString(row["IC_NO"]),
                        IcDt = Convert.ToString(row["IC_DT"]),
                        BkNo = Convert.ToString(row["BK_NO"]),
                        SetNo = Convert.ToString(row["SET_NO"]),
                        BillAmount = Convert.ToString(row["BILL_AMOUNT"]),
                        AmountOutstanding = Convert.ToString(row["AMOUNT_OUTSTANDING"]),
                        DigBillGenDate = Convert.ToString(row["DIG_BILL_GEN_DATE"]),
                        OnlineInvoice = Convert.ToString(row["ONLINE_INVOICE"]),
                        IcPhoto = Convert.ToString(row["IC_PHOTO"]),
                        ImmsRlyCd = Convert.ToString(row["IMMS_RLY_CD"]),
                        TotalTds = Convert.ToString(row["TOTAL_TDS"]),
                        RetentionMoney = Convert.ToString(row["RETENTION_MONEY"]),
                        WriteOffAmt = Convert.ToString(row["WRITE_OFF_AMT"]),
                        AmountPosted = Convert.ToString(row["AMOUNT_POSTED"]),
                        AmountRealised = Convert.ToString(row["AMOUNT_REALISED"]),
                        Consignee = Convert.ToString(row["CONSIGNEE"]),
                        Bpo = Convert.ToString(row["BPO"]),
                        Vendor = Convert.ToString(row["VENDOR"]),
                        IeName = Convert.ToString(row["IE_NAME"]),
                        RecipientGstinNo = Convert.ToString(row["RECIPIENT_GSTIN_NO"]),
                        PoYr = Convert.ToString(row["PO_YR"]),
                        PoSource = Convert.ToString(row["PO_SOURCE"]),
                        ICExists = IC,
                        CaseNoExists = CaseNo,
                        ICPATH = ICPath,
                        CASENOPATH = CaseNoPath,

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
                query = query.Where(w => Convert.ToString(w.BillNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.BillNo).ToLower().Contains(searchBy.ToLower())
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
                
                var query = "SELECT TO_CHAR(INVOICE_DT, 'yyyymm') AS INVOICE_DT, REGION_CODE FROM T55_LAB_INVOICE WHERE INVOICE_NO ='"+ InvoiceNo + "'";

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
