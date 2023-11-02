using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Globalization;
using System.Reflection.Emit;
using static IBS.Helper.Enums;
using System.Globalization;
using Oracle.ManagedDataAccess.Types;

namespace IBS.Repositories
{
    public class LabInvoiceRptRRepository : ILabInvoiceRptRepository
    {
        private readonly ModelContext context;

        public LabInvoiceRptRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<LabInvoiceModel> GetLapInvoice(DTParameters dtParameters, string RegNo)
        {

            DTResult<LabInvoiceModel> dTResult = new() { draw = 0 };
            IQueryable<LabInvoiceModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "InvoiceNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "InvoiceNo";
                orderAscendingDirection = true;
            }
            //if (dtParameters.Order != null)
            //{
            //    // in this example we just default sort on the 1st column
            //    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

            //    if (orderCriteria == "")
            //    {
            //        orderCriteria = "item_srno";
            //    }
            //    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            //}
            //else
            //{
            //    // if we have an empty search then just order the results by Id ascending
            //    orderCriteria = "item_srno";
            //    orderAscendingDirection = true;
            //}

            //OracleParameter[] par = new OracleParameter[2];
            //par[0] = new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.NVarchar2, RegNo, ParameterDirection.Input);
            //par[1] = new OracleParameter("p_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            //var ds = DataAccessDB.GetDataSet("GET_LabINVOICE_GridLoad", par, 1);

            //List<LabInvoiceModel> modelList = new List<LabInvoiceModel>();
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow row in ds.Tables[0].Rows)
            //    {
            //        LabInvoiceModel model = new LabInvoiceModel
            //        {
            //            InvoiceNo = Convert.ToString(row["Invoice_No"]),
            //            InvoiceDt = Convert.ToString(row["Invoice_DT"]),
            //            RegNo = Convert.ToString(row["Sample_REG_NO"]),
            //            GSTINNO = Convert.ToString(row["recipient_gstin_no"]),
            //            SNO = Convert.ToString(row["Item_SRNO"]),
            //            Item = Convert.ToString(row["ITEM_DESC"]),
            //            Quantity = Convert.ToString(row["QTY"]),
            //            TestingCharges = Convert.ToString(row["Testing_Charges"]),
            //            CGST = Convert.ToString(row["CGST"]),
            //            SGST = Convert.ToString(row["SGST"]),
            //            IGST = Convert.ToString(row["IGST"]),


            //        };

            //        modelList.Add(model);
            //    }
            //}
            //var par = new List<OracleParameter>();
            //par.Add(new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("RegNo"), ParameterDirection.Input));
            //par.Add(new OracleParameter("p_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("RegNo"), ParameterDirection.Input);
            par[1] = new OracleParameter("p_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_LabINVOICE_GridLoad", par, 1);

            List<LabInvoiceModel> modelList = new List<LabInvoiceModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LabInvoiceModel model = new LabInvoiceModel
                    {
                        InvoiceNo = Convert.ToString(row["Invoice_No"]),
                        InvoiceDt = Convert.ToString(row["Invoice_DT"]),
                        RegNo = Convert.ToString(row["Sample_REG_NO"]),
                        GSTINNO = Convert.ToString(row["recipient_gstin_no"]),
                        SNO = Convert.ToString(row["Item_SRNO"]),
                        Item = Convert.ToString(row["ITEM_DESC"]),
                        Quantity = Convert.ToString(row["QTY"]),
                        TestingCharges = Convert.ToString(row["Testing_Charges"]),
                        CGST = Convert.ToString(row["CGST"]),
                        SGST = Convert.ToString(row["SGST"]),
                        IGST = Convert.ToString(row["IGST"]),
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
            dTResult.data = query;
            dTResult.recordsFiltered = query.Count();
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
        public LabInvoiceModel Getdtreg(string RegNo, string GetRegionCode, string UserId)
        {


            using (var dbContext = context.Database.GetDbConnection())
            {
                //OracleParameter[] par = new OracleParameter[2];
                //par[0] = new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.Varchar2, RegNo, ParameterDirection.Input);
                //par[1] = new OracleParameter("p_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Input);

                //var ds = DataAccessDB.GetDataSet("Lab_InvoiceRptDetails", par, 1);
                string CURR_DATE;
                string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";

                CURR_DATE = GetDateString(sqlQuery);

                LabInvoiceModel labInvoiceModel = Getdtreg1(RegNo);
                string BPoCD = labInvoiceModel.BPOCD;
                LabInvoiceModel model = new();
                if (CURR_DATE != labInvoiceModel.InvoiceDt)
                {
                    LabInvoiceModel mo = BPOList(BPoCD);

                    var par = new List<OracleParameter>();
                    par.Add(new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.NVarchar2, RegNo, ParameterDirection.Input));
                    par.Add(new OracleParameter("p_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));

                    var ds = DataAccessDB.GetDataSet("Lab_InvoiceRptDetails", par.ToArray(), 1);



                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        model = new LabInvoiceModel
                        {
                            CaseNo = Convert.ToString(row["case_no"]),
                            CallDt = Convert.ToString(row["call_recv_dt"]),
                            CallSNO = Convert.ToString(row["call_sno"]),
                            VendorName = Convert.ToString(row["vendor"]),
                            ManufacturerNM = Convert.ToString(row["manufacturer"]),
                            Vendor = Convert.ToString(row["vend_cd"]),
                            ManufacturerCD = Convert.ToString(row["mfg_cd"]),
                            BPOCD = mo.BPOCD,
                            BPONM = mo.BPONM,
                            State = mo.State,
                            GSTINNO = mo.GSTINNO,
                            RegNo = RegNo,
                            InvoiceNo = labInvoiceModel.InvoiceNo,
                        };
                    }
                }
                else
                {
                    LabInvoiceModel mo = BPOList(BPoCD);
                    var par = new List<OracleParameter>();
                    par.Add(new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.NVarchar2, RegNo, ParameterDirection.Input));
                    par.Add(new OracleParameter("p_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));

                    var ds = DataAccessDB.GetDataSet("Lab_InvoiceRptDetails", par.ToArray(), 1);



                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        model = new LabInvoiceModel
                        {
                            CaseNo = Convert.ToString(row["case_no"]),
                            CallDt = Convert.ToString(row["call_recv_dt"]),
                            CallSNO = Convert.ToString(row["call_sno"]),
                            VendorName = Convert.ToString(row["vendor"]),
                            ManufacturerNM = Convert.ToString(row["manufacturer"]),
                            Vendor = Convert.ToString(row["vend_cd"]),
                            ManufacturerCD = Convert.ToString(row["mfg_cd"]),
                            BPOCD = mo.BPOCD,
                            BPONM = mo.BPONM,
                        };
                    }
                }

                return model;
            }
        }
        public LabInvoiceModel Getdtreg1(string RegNo)
        {
            //string OracleQuery = "SELECT Invoice_no, TO_CHAR(Invoice_dt, 'dd/mm/yyyy') AS Invoice_dt, bpo_cd FROM t55_lab_invoice WHERE sample_reg_no = :RegNo";

            //OracleParameter[] par = new OracleParameter[1];
            //par[0] = new OracleParameter("RegNo", OracleDbType.Varchar2);
            //par[0].Value = RegNo;

            //var ds = DataAccessDB.GetDataSet(OracleQuery, par, 1);
            //LabInvoiceModel model = new();
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    DataRow row = ds.Tables[0].Rows[0];
            //    model = new LabInvoiceModel
            //    {
            //        InvoiceNo = Convert.ToString(row["Invoice_no"]),
            //        InvoiceDt = Convert.ToString(row["Invoice_dt"]),
            //        BPOCD = Convert.ToString(row["bpo_cd"]),

            //        // Process the retrieved data as needed
            //    };
            //}
            //return model;
            using (var conn1 = context.Database.GetDbConnection())
            {
                string OracleQuery = "SELECT Invoice_no, TO_CHAR(Invoice_dt, 'dd/mm/yyyy') AS Invoice_dt, bpo_cd FROM t55_lab_invoice WHERE sample_reg_no = :RegNo";

                // Create an OracleCommand
                using (OracleCommand cmd = new OracleCommand(OracleQuery, (OracleConnection)conn1))
                {
                    // Bind the parameter to the command
                    cmd.Parameters.Add(new OracleParameter("RegNo", OracleDbType.Varchar2, RegNo, ParameterDirection.Input));

                    // Execute the query
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        LabInvoiceModel model = new LabInvoiceModel();
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            model.InvoiceNo = Convert.ToString(row["Invoice_no"]);
                            model.InvoiceDt = Convert.ToString(row["Invoice_dt"]);
                            model.BPOCD = Convert.ToString(row["bpo_cd"]);

                            // Process the retrieved data as needed
                        }
                        return model;
                    }
                }
            }
        }
        public LabInvoiceModel BPOList(string BPoCD)
        {
            var par = new List<OracleParameter>();
            par.Add(new OracleParameter("p_BPO_SEARCH", OracleDbType.NVarchar2, BPoCD, ParameterDirection.Input));
            par.Add(new OracleParameter("p_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));

            var ds = DataAccessDB.GetDataSet("GET_BPO_DETAILS", par.ToArray(), 1);


            LabInvoiceModel model = new();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                model = new LabInvoiceModel
                {
                    BPONM = Convert.ToString(row["BPO_NAME"]),
                    BPOTYPE = Convert.ToString(row["BPO_TYPE"]),
                    BPOCD = Convert.ToString(row["BPO_CD"]),
                    GSTINNO = Convert.ToString(row["GSTIN_NO"]),
                    State = Convert.ToString(row["BPO_STATE"]),
                    // Process the retrieved data as needed
                };
            }
            return model;
        }
        public string Save(LabInvoiceModel model)
        {
            string Id = "0";
            if (model.InvoiceNo == null)
            {

               
                string InvoiceNo = Generate_Invoice_Bill_No(model);
                int SrNo = Getsrno(InvoiceNo);
                model.InvoiceDt = DateTime.Now.ToString("MM/dd/yyyy");
                decimal Testing_Charges = Math.Round((Convert.ToDecimal(model.TestingCharges.Trim()) / 1.18M), 2);
                #region save
                T55LabInvoice obj = new T55LabInvoice();

                obj.InvoiceNo = InvoiceNo;
                obj.InvoiceDt = DateTime.ParseExact(model.InvoiceDt, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                obj.SampleRegNo = model.RegNo;
                obj.CaseNo = model.CaseNo;
                obj.BpoCd = model.BPOCD;
                obj.RecipientGstinNo = model.GSTINNO;
                obj.BillAmount = Convert.ToDecimal(Testing_Charges);
                obj.TotalCgst = Convert.ToDecimal(model.CGST);
                obj.TotalSgst = Convert.ToDecimal(model.SGST);
                obj.TotalIgst = Convert.ToDecimal(model.IGST);
                obj.UserId = model.UserId;
                obj.Datetime = DateTime.Now;
                obj.RegionCode = model.Region;
                obj.IncType = "N";
                context.T55LabInvoices.Add(obj);
                context.SaveChanges();
                Id = obj.InvoiceNo;

                #endregion
                #region save details
                LabInvoiceDetails(model, InvoiceNo, SrNo);
                #endregion

                //return Id;

            }
            else
            {
                string Testingfee = model.TestingCharges;
                decimal totalCgst = 0;
                decimal totalSgst = 0;
                decimal totalIgst = 0;
                decimal billAmount = 0;
                string InvoiceNo = model.InvoiceNo;
                int SrNo = Getsrno(InvoiceNo);
                decimal Testing_Charges_N = Math.Round((Convert.ToDecimal(model.TestingCharges.Trim()) / 1.18M), 2);
                decimal AMT_RATE_N = Math.Round((Testing_Charges_N / Convert.ToDecimal(model.Quantity.Trim())), 2);
                model.TestingCharges = Convert.ToString(Testing_Charges_N);
                //insert lab invoice detail
                LabInvoiceDetailsNew(model, InvoiceNo, SrNo, AMT_RATE_N, Testingfee);
               
                //select lab invoice
                var result = context.T55LabInvoices
                    .Where(invoice => invoice.InvoiceNo == InvoiceNo)
                    .Select(invoice => new
                    {
                        invoice.TotalCgst,
                        invoice.TotalSgst,
                        invoice.TotalIgst,
                        invoice.BillAmount
                    })
                    .FirstOrDefault(); 

                            if (result != null)
                            {
                                 totalCgst = Convert.ToDecimal(result.TotalCgst);
                                 totalSgst = Convert.ToDecimal(result.TotalSgst);
                                 totalIgst = Convert.ToDecimal(result.TotalIgst);
                                 billAmount = Convert.ToDecimal(result.BillAmount);
                            }
                totalCgst += Convert.ToDecimal(model.CGST.Trim());
                totalSgst += Convert.ToDecimal(model.SGST.Trim());
                totalIgst += Convert.ToDecimal(model.IGST.Trim());
                billAmount += Testing_Charges_N;

                //update lab invoice
                var invoice = context.T55LabInvoices.FirstOrDefault(i => i.InvoiceNo == InvoiceNo);

                if (invoice != null)
                {
                    invoice.TotalCgst = Convert.ToDecimal(totalCgst);
                    invoice.TotalSgst = Convert.ToDecimal(totalSgst);
                    invoice.TotalIgst = Convert.ToDecimal(totalIgst);
                    invoice.BillAmount = billAmount;

                    context.SaveChanges();
                }
               
            }
            return Id;
        }
        public bool LabInvoiceDetails(LabInvoiceModel model, string InvoiceNo, int SrNo)
        {
            try
            {
                decimal AMT_RATE = Math.Round((Convert.ToDecimal(model.TestingCharges) / Convert.ToDecimal(model.Quantity.Trim())), 2);
                OracleParameter[] par = new OracleParameter[9];
                par[0] = new OracleParameter("p_InvoiceNo", OracleDbType.Varchar2, InvoiceNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_ItemSrno", OracleDbType.Varchar2, SrNo, ParameterDirection.Input);
                par[2] = new OracleParameter("p_ItemDesc", OracleDbType.Varchar2, model.Item, ParameterDirection.Input);
                par[3] = new OracleParameter("p_Qty", OracleDbType.Varchar2, model.Quantity, ParameterDirection.Input);
                par[4] = new OracleParameter("p_Rate", OracleDbType.Varchar2, AMT_RATE, ParameterDirection.Input);
                par[5] = new OracleParameter("p_TestingCharges", OracleDbType.Varchar2, model.TestingCharges, ParameterDirection.Input);
                par[6] = new OracleParameter("p_Cgst", OracleDbType.Varchar2, model.CGST, ParameterDirection.Input);
                par[7] = new OracleParameter("p_Sgst", OracleDbType.Varchar2, model.SGST, ParameterDirection.Input);
                par[8] = new OracleParameter("p_Igst", OracleDbType.Varchar2, model.IGST, ParameterDirection.Input);

                var ds = DataAccessDB.ExecuteNonQuery("SP_Insert_T86LabInvoiceDetails", par, 1);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool LabInvoiceDetailsNew(LabInvoiceModel model, string InvoiceNo, int SrNo, decimal AMT_RATE_N,string Testingfee)
        {
            try
            {
                //decimal AMT_RATE = Math.Round((Convert.ToDecimal(model.TestingCharges) / Convert.ToDecimal(model.Quantity.Trim())), 2);
                OracleParameter[] par = new OracleParameter[9];
                par[0] = new OracleParameter("p_InvoiceNo", OracleDbType.Varchar2, InvoiceNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_ItemSrno", OracleDbType.Varchar2, SrNo, ParameterDirection.Input);
                par[2] = new OracleParameter("p_ItemDesc", OracleDbType.Varchar2, model.Item, ParameterDirection.Input);
                par[3] = new OracleParameter("p_Qty", OracleDbType.Varchar2, model.Quantity, ParameterDirection.Input);
                par[4] = new OracleParameter("p_Rate", OracleDbType.Varchar2, AMT_RATE_N, ParameterDirection.Input);
                par[5] = new OracleParameter("p_TestingCharges", OracleDbType.Varchar2, Testingfee, ParameterDirection.Input);
                par[6] = new OracleParameter("p_Cgst", OracleDbType.Varchar2, model.CGST, ParameterDirection.Input);
                par[7] = new OracleParameter("p_Sgst", OracleDbType.Varchar2, model.SGST, ParameterDirection.Input);
                par[8] = new OracleParameter("p_Igst", OracleDbType.Varchar2, model.IGST, ParameterDirection.Input);

                var ds = DataAccessDB.ExecuteNonQuery("SP_Insert_T86LabInvoiceDetails", par, 1);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //public string Generate_Invoice_Bill_No(LabInvoiceModel model)
        //{

        //    string labelInvDt = DateTime.Now.ToString().Replace("/", "");
        //    string regionCd = model.Region;

        //    try
        //    {
        //        using (var conn1 = context.Database.GetDbConnection())
        //        {
        //            conn1.Open();
        //            using (OracleCommand cmd = new OracleCommand("GENERATE_LAB_INVOICE_BILL", (OracleConnection)conn1))
        //            {

        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("IN_LAB_REG_DT", OracleDbType.Char).Value = labelInvDt;
        //                cmd.Parameters.Add("IN_REGION_CD", OracleDbType.Char).Value = regionCd;
        //                cmd.Parameters.Add("OUT_LAB_INVOICE_NO", OracleDbType.Char, 14).Direction = ParameterDirection.Output;
        //                cmd.Parameters.Add("OUT_ERR_CD", OracleDbType.Int32, 1).Direction = ParameterDirection.Output;

        //                cmd.ExecuteNonQuery();

        //                LabInvoiceModel labInvoice = new LabInvoiceModel
        //                {
        //                    InvoiceNo = Convert.ToString(cmd.Parameters["OUT_LAB_INVOICE_NO"].Value),
        //                    ErrorCode = Convert.ToInt32(cmd.Parameters["OUT_ERR_CD"].Value)
        //                };

        //                if (labInvoice.ErrorCode == -1)
        //                {
        //                    return "-1";
        //                }
        //                return labInvoice.InvoiceNo;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message.Replace("\n", "");
        //        return "An error occurred: " + error;
        //    }
        //    finally
        //    {
        //        if (conn1.State == ConnectionState.Open)
        //        {
        //            conn1.Close(); // Close the connection in a finally block
        //        }
        //    }

        //}
        public string Generate_Invoice_Bill_No(LabInvoiceModel model)
        {
            string labelInvDt = DateTime.Now.ToString().Replace("/", "");
            string regionCd = model.Region;
            OracleConnection conn1 = null;

            try
            {
                conn1 = (OracleConnection)context.Database.GetDbConnection();
                conn1.Open(); // Open the connection

                using (OracleCommand cmd = new OracleCommand("GENERATE_LAB_INVOICE_BILL", conn1))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("IN_LAB_REG_DT", OracleDbType.Char).Value = labelInvDt;
                    cmd.Parameters.Add("IN_REGION_CD", OracleDbType.Char).Value = regionCd;
                    cmd.Parameters.Add("OUT_LAB_INVOICE_NO", OracleDbType.Char, 14).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("OUT_ERR_CD", OracleDbType.Int32, 1).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    LabInvoiceModel labInvoice = new LabInvoiceModel
                    {
                        InvoiceNo = Convert.ToString(cmd.Parameters["OUT_LAB_INVOICE_NO"].Value),
                        ErrorCode = (int)((OracleDecimal)cmd.Parameters["OUT_ERR_CD"].Value)
                    };

                    if (labInvoice.ErrorCode == -1)
                    {
                        return "-1";
                    }
                    return labInvoice.InvoiceNo;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message.Replace("\n", "");
                return "An error occurred: " + error;
            }
            finally
            {
                if (conn1 != null && conn1.State == ConnectionState.Open)
                {
                    conn1.Close(); // Close the connection in a finally block
                }
            }
        }
        public int Getsrno(string InvoiceNo)
        {
            int srno = 0;

            using (ModelContext context = new ModelContext(DbContextHelper.GetDbContextOptions()))
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "Select NVL(max(ITEM_SRNO),0)+1 from T86_LAB_INVOICE_DETAILS where INVOICE_NO='" + InvoiceNo + "'";
                    srno = Convert.ToInt32(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }

            return srno;
        }
    }
}
