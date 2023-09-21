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

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.Item).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.Item).ToLower().Contains(searchBy.ToLower())
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
        public LabInvoiceModel Getdtreg(string RegNo)
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

                    // Process the retrieved data as needed
                };
            }
            return model;
        }

    }
}
