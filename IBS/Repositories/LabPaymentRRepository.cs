using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using static IBS.Helper.Enums;

namespace IBS.Repositories
{
    public class LabPaymentRRepository : ILabPaymentFormRepository
    {
        private readonly ModelContext context;

        public LabPaymentRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<LabPaymentFormModel> GetLabPayments(DTParameters dtParameters,string Regin)
        {

            //using (var dbContext = context.Database.GetDbConnection())
            //{
            //    OracleParameter[] par = new OracleParameter[5];
            //    par[0] = new OracleParameter("p_Region", OracleDbType.NVarchar2, paymentFormModel.Regin, ParameterDirection.Input);
            //    par[1] = new OracleParameter("p_PaymentID", OracleDbType.NVarchar2, paymentFormModel.PaymentID, ParameterDirection.Input);
            //    par[2] = new OracleParameter("p_PaymentDT", OracleDbType.Date, paymentFormModel.PaymentDt, ParameterDirection.Input);
            //    par[3] = new OracleParameter("p_LabID", OracleDbType.NVarchar2, paymentFormModel.Lab, ParameterDirection.Input);
            //    par[4] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            //    var ds = DataAccessDB.GetDataSet("GetLabPayments", par, 4);

            //    List<LabPaymentFormModel> modelList = new List<LabPaymentFormModel>();
            //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            LabPaymentFormModel model = new LabPaymentFormModel
            //            {
            //                PaymentID = row["PAYMENT_ID"] as string,
            //                PaymentDt = Convert.ToString(row["PAYMENT_DATE"]),
            //                Lab = Convert.ToString(row["LAB"]),
            //                Amount = Convert.ToString(row["AMOUNT"]),
            //            };

            //            modelList.Add(model);
            //        }
            //    }

            //    return modelList;
            //}

            DTResult<LabPaymentFormModel> dTResult = new() { draw = 0 };
            IQueryable<LabPaymentFormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "PAYMENT_ID";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "PAYMENT_ID";
                orderAscendingDirection = true;
            }
            LabPaymentFormModel labPaymentFormModel = new LabPaymentFormModel();
            if(dtParameters.AdditionalValues?.GetValueOrDefault("PaymentDt") == "")
            {
                labPaymentFormModel.PaymentDt = null;
            }
            else
            {
                labPaymentFormModel.PaymentDt = dtParameters.AdditionalValues?.GetValueOrDefault("PaymentID");
               
            }

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_Region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("p_PaymentID", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("PaymentID"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_PaymentDT", OracleDbType.Date, labPaymentFormModel.PaymentDt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_LabID", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("Lab"), ParameterDirection.Input);
            par[4] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetLabPayments", par, 4);

            List<LabPaymentFormModel> modelList = new List<LabPaymentFormModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LabPaymentFormModel model = new LabPaymentFormModel
                    {
                        PaymentID = row["PAYMENT_ID"] as string,
                        PaymentDt = Convert.ToString(row["PAYMENT_DATE"]),
                        Lab = Convert.ToString(row["LAB"]),
                        Amount = Convert.ToString(row["AMOUNT"]),
                    };

                    modelList.Add(model);
                }
            }
            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.PaymentID).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.PaymentID).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public List<LabPaymentFormModel> GetPayments(LabPaymentFormModel paymentFormModel)
        {



            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_LabID", OracleDbType.NVarchar2, paymentFormModel.Lab, ParameterDirection.Input);
                par[1] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("LabPayment", par, 1);

                List<LabPaymentFormModel> modelList = new List<LabPaymentFormModel>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        LabPaymentFormModel model = new LabPaymentFormModel
                        {
                            CHQ_NO = row["CHQ_NO"] as string,
                            CHQ_DT = Convert.ToString(row["CHQ_DT"]),
                            SAMPLE_REG_NO = Convert.ToString(row["SAMPLE_REG_NO"]),
                            CASE_NO = Convert.ToString(row["CASE_NO"]),
                            SNO = Convert.ToString(row["SNO"]),
                            TOT_CHARGES = Convert.ToString(row["TOT_CHARGES"]),
                            TDS_AMT = Convert.ToString(row["TDS_AMT"]),
                            TESTING_FEE = Convert.ToString(row["TESTING_FEE"]),
                            AMT_CLEARED = Convert.ToString(row["AMT_CLEARED"]),
                            IAMOUNT = Convert.ToString(row["AMOUNT"]),
                        };

                        modelList.Add(model);
                    }
                }

                return modelList;
            }

            //return dTResult;
        }
        public DTResult<LabPaymentFormModel> GetPaymentsEdit(DTParameters dtParameters)
        {

            DTResult<LabPaymentFormModel> dTResult = new() { draw = 0 };
            IQueryable<LabPaymentFormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "SAMPLE_REG_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "SAMPLE_REG_NO";
                orderAscendingDirection = true;
            }
            LabPaymentFormModel labPaymentFormModel = new LabPaymentFormModel();
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_payment_id", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("PaymentID"), ParameterDirection.Input);
            par[1] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);


            var ds = DataAccessDB.GetDataSet("LabPaymentEdit", par, 1);

            List<LabPaymentFormModel> modelList = new List<LabPaymentFormModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LabPaymentFormModel model = new LabPaymentFormModel
                    {
                        CHQ_NO = row["CHQ_NO"] as string,
                        CHQ_DT = Convert.ToString(row["CHQ_DT"]),
                        SAMPLE_REG_NO = Convert.ToString(row["SAMPLE_REG_NO"]),
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        SNO = Convert.ToString(row["SNO"]),
                        TOT_CHARGES = Convert.ToString(row["TOT_CHARGES"]),
                        //TDS_AMT = Convert.ToString(row["TDS_AMT"]),
                        TESTING_FEE = Convert.ToString(row["TESTING_FEE"]),
                        AMT_CLEARED = Convert.ToString(row["AMT_CLEARED"]),
                        IAMOUNT = Convert.ToString(row["AMOUNT"]),
                    };

                    modelList.Add(model);
                }
            }
            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.SAMPLE_REG_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.SAMPLE_REG_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

            //using (var dbContext = context.Database.GetDbConnection())
            //{
            //    OracleParameter[] par = new OracleParameter[2];
            //    par[0] = new OracleParameter("p_payment_id", OracleDbType.NVarchar2, PaymentID, ParameterDirection.Input);
            //    par[1] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            //    var ds = DataAccessDB.GetDataSet("LabPaymentEdit", par, 1);

            //    List<LabPaymentFormModel> modelList = new List<LabPaymentFormModel>();
            //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            LabPaymentFormModel model = new LabPaymentFormModel
            //            {
            //                CHQ_NO = row["CHQ_NO"] as string,
            //                CHQ_DT = Convert.ToString(row["CHQ_DT"]),
            //                SAMPLE_REG_NO = Convert.ToString(row["SAMPLE_REG_NO"]),
            //                CASE_NO = Convert.ToString(row["CASE_NO"]),
            //                SNO = Convert.ToString(row["SNO"]),
            //                TOT_CHARGES = Convert.ToString(row["TOT_CHARGES"]),
            //                //TDS_AMT = Convert.ToString(row["TDS_AMT"]),
            //                TESTING_FEE = Convert.ToString(row["TESTING_FEE"]),
            //                AMT_CLEARED = Convert.ToString(row["AMT_CLEARED"]),
            //                IAMOUNT = Convert.ToString(row["AMOUNT"]),
            //            };

            //            modelList.Add(model);
            //        }
            //    }

            //    return modelList;
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
        public string Generate(LabPaymentFormModel LabPaymentFormModel)
        {
            string ss = LabPaymentFormModel.Regin + LabPaymentFormModel.PaymentDt.Substring(8, 2) + LabPaymentFormModel.PaymentDt.Substring(3, 2);
            string query = "Select lpad(nvl(max(to_number(nvl(substr(PAYMENT_ID,6,8),0))),0)+1,3,'0') from T56_LAB_PAYMENTS where substr(PAYMENT_ID,1,5)='" + ss + "'";
            string ds = GetDateString(query);
            string da = ss + ds;
            //OracleParameter[] par = new OracleParameter[1];
            //par[0] = new OracleParameter("SAMPLE_REG_NO", OracleDbType.Varchar2, RegNo, ParameterDirection.Input);
            //var ds = DataAccessDB.GetDataSet(query, par, 1);
            return da.ToString();


        }
        public bool SavePayment(LabPaymentFormModel LabPaymentFormModel)
        {
            string PaymentNo = Generate(LabPaymentFormModel);
            LabPaymentFormModel.PaymentID = PaymentNo;
            string ss;
            string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";

            ss = GetDateString(sqlQuery);
            try
            {

                OracleParameter[] par = new OracleParameter[10];
                par[0] = new OracleParameter("p_payment_id", OracleDbType.Varchar2, PaymentNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_payment_dt", OracleDbType.Date, LabPaymentFormModel.PaymentDt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_bank_cd", OracleDbType.Varchar2, LabPaymentFormModel.Bank, ParameterDirection.Input);
                par[3] = new OracleParameter("p_chq_no", OracleDbType.Varchar2, LabPaymentFormModel.CHQ_NO, ParameterDirection.Input);
                par[4] = new OracleParameter("p_chq_dt", OracleDbType.Date, LabPaymentFormModel.CHQ_DT, ParameterDirection.Input);
                par[5] = new OracleParameter("p_amount", OracleDbType.Varchar2, LabPaymentFormModel.IAMOUNT, ParameterDirection.Input);
                par[6] = new OracleParameter("p_lab_id", OracleDbType.Varchar2, LabPaymentFormModel.LabCd, ParameterDirection.Input);
                par[7] = new OracleParameter("p_remarks", OracleDbType.Varchar2, LabPaymentFormModel.Remarks, ParameterDirection.Input);
                par[8] = new OracleParameter("p_user_id", OracleDbType.Varchar2, LabPaymentFormModel.UserId, ParameterDirection.Input);
                par[9] = new OracleParameter("p_datetime", OracleDbType.Date, ss, ParameterDirection.Input);

                var ds = DataAccessDB.ExecuteNonQuery("InsertLabPayment", par, 1);
                Update(LabPaymentFormModel, PaymentNo);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool UpdatePayment(LabPaymentFormModel LabPaymentFormModel)
        {
            
            string ss;
            string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";

            ss = GetDateString(sqlQuery);
            try
            {

                OracleParameter[] par = new OracleParameter[6];
                par[0] = new OracleParameter("p_chq_no", OracleDbType.Varchar2, LabPaymentFormModel.CHQ_NO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_chq_dt", OracleDbType.Date, LabPaymentFormModel.CHQ_DT, ParameterDirection.Input);
                par[2] = new OracleParameter("p_remarks", OracleDbType.Varchar2, LabPaymentFormModel.Remarks, ParameterDirection.Input);
                par[3] = new OracleParameter("p_user_id", OracleDbType.Varchar2, LabPaymentFormModel.UserId, ParameterDirection.Input);
                par[4] = new OracleParameter("p_datetime", OracleDbType.Date, ss, ParameterDirection.Input);
                par[5] = new OracleParameter("p_payment_id", OracleDbType.Varchar2, LabPaymentFormModel.PaymentID, ParameterDirection.Input);

                var ds = DataAccessDB.ExecuteNonQuery("UPDATE_PAYMENT_DETAILS", par, 1);
                
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool Update(LabPaymentFormModel LabPaymentFormModel, string PaymentNo)
        {
            string[] srno = LabPaymentFormModel.SNO.Split(',');
            string[] regno = LabPaymentFormModel.SAMPLE_REG_NO.Split(',');
            for (int i = 0; i < regno.Length; i++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(regno[i].ToString()))
                    {
                        OracleParameter[] par = new OracleParameter[4];
                        par[0] = new OracleParameter("p_payment_id", OracleDbType.Varchar2, PaymentNo, ParameterDirection.Input);
                        par[1] = new OracleParameter("p_lab_id", OracleDbType.Varchar2, LabPaymentFormModel.LabCd, ParameterDirection.Input);
                        par[2] = new OracleParameter("p_sample_reg_no", OracleDbType.Varchar2, regno[i], ParameterDirection.Input);
                        par[3] = new OracleParameter("p_sno", OracleDbType.Varchar2, srno[i], ParameterDirection.Input);

                        var ds = DataAccessDB.ExecuteNonQuery("U_LabRegDetailPayment", par, 1);
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        public LabPaymentFormModel PrintLabPayment(LabPaymentFormModel paymentFormModel)
        {



            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("VNO", OracleDbType.NVarchar2, paymentFormModel.PaymentID, ParameterDirection.Input);
                par[1] = new OracleParameter("REGION", OracleDbType.NVarchar2, paymentFormModel.Regin, ParameterDirection.Input);
                par[2] = new OracleParameter("CUR_OUT_PAYMENT", OracleDbType.RefCursor, ParameterDirection.Output);
                par[3] = new OracleParameter("CUR_OUT_SAMPLES", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("GET_PAYMENT_Print", par, 2);

                LabPaymentFormModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    model = new LabPaymentFormModel
                    {
                        PaymentID = row["PAYMENT_ID"] as string,
                        PaymentDt = Convert.ToString(row["PAYMENT_DATE"]),
                        CHQ_NO = Convert.ToString(row["CHQ_NO"]),
                        CHQ_DT = Convert.ToString(row["CHQ_DATE"]),
                        Bank = Convert.ToString(row["BANK_NAME"]),
                        Amount = Convert.ToString(row["AMOUNT"]),
                        LabCd = Convert.ToString(row["LAB_ID"]),
                        Lab = Convert.ToString(row["LAB_NAME"]),

                    };
                    
                    
                }
                LabPaymentFormModel sampleDetail = new();
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                   
                        DataRow row = ds.Tables[1].Rows[0];
                     sampleDetail = new LabPaymentFormModel
                        {
                            SAMPLE_REG_NO = Convert.ToString(row["SAMPLE_REG_NO"]),
                            IAMOUNT = Convert.ToString(row["AMT"])
                        };

                    
                }
                model.SAMPLE_REG_NO = sampleDetail.SAMPLE_REG_NO;
                model.IAMOUNT = sampleDetail.IAMOUNT;
                return model;
            }

            //return dTResult;
        }
        public LabPaymentFormModel Edit(string PaymentID)
        {



            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_payment_id", OracleDbType.NVarchar2, PaymentID, ParameterDirection.Input);
                par[1] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("GET_PAYMENT_DETAILS_Edit", par, 1);

                LabPaymentFormModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    model = new LabPaymentFormModel
                    {
                        PaymentID = row["PAYMENT_ID"] as string,
                        PaymentDt = Convert.ToString(row["PAYMENT_DATE"]),
                        CHQ_NO = Convert.ToString(row["CHQ_NO"]),
                        CHQ_DT = Convert.ToString(row["CHQ_DATE"]),
                        BankCD = Convert.ToString(row["BANK_CD"]),
                        Remarks = Convert.ToString(row["REMARKS"]),
                        //Bank = Convert.ToString(row["BANK_NAME"]),
                        Amount = Convert.ToString(row["AMOUNT"]),
                        LabCd = Convert.ToString(row["LAB_ID"]),
                        //Lab = Convert.ToString(row["LAB_NAME"]),

                    };


                }
                return model;
            }

            //return dTResult;
        }
    }
}
