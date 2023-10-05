using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Configuration;
using System.Data;
using System.Reflection.Emit;
using static IBS.Helper.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories
{
    public class LabRegiFormRepository : ILabRegFormRepository
    {
        private readonly ModelContext context;

        public LabRegiFormRepository(ModelContext context)
        {
            this.context = context;
        }

        public LABREGISTERModel LoaddataModify(string RegNo)
        {

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.NVarchar2, RegNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_LabRegLoadData", par, 1);

                LABREGISTERModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new LABREGISTERModel
                    {
                        SampleRegNo = row["SAMPLE_REG_NO"] as string, // Replace "Property1" with the actual column name in the table
                        CaseNo = Convert.ToString(row["CASE_NO"]),
                        SampleRegDate = Convert.ToString(row["SAMPLE_REG_DATE"]),
                        CallDateAndSno = Convert.ToString(row["CALL_DT"]),
                        Vendor = Convert.ToString(row["VENDOR"]),
                        VendorCode = Convert.ToString(row["VEND_CD"]),
                        IE = Convert.ToString(row["IE_NAME"]),
                        SampleDispatchDate = Convert.ToString(row["SAMPLE_DISPATCH_DATE"]),
                        SampleDrawalDate = Convert.ToString(row["SAMPLE_DRAWL_DATE"]),
                        SampleReceiptDate = Convert.ToString(row["SAMPLE_RECIEPT_DATE"]),
                        TestingType = Convert.ToString(row["TESTING_TYPE"]),
                        TotalTestingFee = Convert.ToString(row["TOTAL_TESTING_FEE"]),
                        TotalHandlingCharges = Convert.ToString(row["TOTAL_HANDLING_CHARGES"]),
                        TotalServiceTax = Convert.ToString(row["TOTAL_SERVICE_TAX"]),
                        TotalLabCharges = Convert.ToString(row["TOTAL_LAB_CHARGES"]),
                        TotalTDS = Convert.ToString(row["TDS"]),
                        AmountRecieved = Convert.ToString(row["AMOUNT_RECIEVED"]),
                        //TotalAmountCleared = Convert.ToString(row["TOTAL_CHARGES"]),
                        CodeNo = Convert.ToString(row["CODE_NO"]),
                        CodeDate = Convert.ToString(row["CODE_DATE"]),
                        CallSNO = Convert.ToString(row["CALL_SNO"]),
                        SNO = Convert.ToString(row["sno"]),
                    };
                }
                //if (ds != null && ds.Tables.Count > 0)
                //{
                //    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                //    List<LabTDSEntryModel> modelList = JsonConvert.DeserializeObject<List<LabTDSEntryModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //    model = modelList.FirstOrDefault();
                //    //model = JsonConvert.DeserializeObject<List<LabTDSEntryModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
                //}


                return model;
            }
        }
        //public List<LABREGISTERModel> GetLabRegDtl(string RegNo, string SNO)
        //{

        //    //DTResult<LABREGISTERModel> dTResult = new() { draw = 0 };
        //    //IQueryable<LABREGISTERModel>? query = null;




        //    using (var dbContext = context.Database.GetDbConnection())
        //    {
        //        OracleParameter[] par = new OracleParameter[3];
        //        par[0] = new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.NVarchar2, RegNo, ParameterDirection.Input);
        //        par[1] = new OracleParameter("p_SNO", OracleDbType.NVarchar2, SNO, ParameterDirection.Input);
        //        par[2] = new OracleParameter("p_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

        //        var ds = DataAccessDB.GetDataSet("GetLabRegisterDetails", par, 2);

        //        List<LABREGISTERModel> modelList = new List<LABREGISTERModel>();
        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in ds.Tables[0].Rows)
        //            {
        //                LABREGISTERModel model = new LABREGISTERModel
        //                {
        //                    SampleRegNo = row["SAMPLE_REG_NO"] as string,
        //                    SNO = Convert.ToString(row["SNO"]),
        //                    ItemDesc = Convert.ToString(row["ITEM_DESC"]),
        //                    Qty = Convert.ToString(row["QTY"]),
        //                    TestCategoryCode = Convert.ToString(row["TEST_CATEGORY_CD"]),
        //                    Test = Convert.ToString(row["TEST"]),
        //                    LabID = Convert.ToString(row["LAB_ID"]),
        //                    LabName = Convert.ToString(row["LAB_NAME"]),
        //                    TestingFee = Convert.ToString(row["TESTING_FEE"]),
        //                    ServiceTax = Convert.ToString(row["SERVICE_TAX"]),
        //                    HandlingCharges = Convert.ToString(row["HANDLING_CHARGES"]),
        //                    TestReportRequestDate = Convert.ToString(row["TEST_REPORT_REQ_DATE"]),
        //                    TestReportReceiveDate = Convert.ToString(row["TEST_REPORT_REC_DATE"]),
        //                    TestStatus = Convert.ToString(row["TEST_STATUS"]),
        //                    Remarks = Convert.ToString(row["REMARKS"]),
        //                    SampleDispatchLabDate = Convert.ToString(row["SAMPLE_DISPATCH_LAB_DT"]),
        //                };

        //                modelList.Add(model);
        //            }
        //        }

        //        return modelList;
        //    }

        //    //return dTResult;
        //}

        public DTResult<LABREGISTERModel> GetLabRegDtl(DTParameters dtParameters)
        {

            DTResult<LABREGISTERModel> dTResult = new() { draw = 0 };
            IQueryable<LABREGISTERModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;


            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "LabName";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "LabName";
                orderAscendingDirection = true;
            }


            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("RegNo"), ParameterDirection.Input);
            par[1] = new OracleParameter("p_SNO", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("SNO"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);


            var ds = DataAccessDB.GetDataSet("GetLabRegisterDetails", par, 2);

            List<LABREGISTERModel> modelList = new List<LABREGISTERModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    LABREGISTERModel model = new LABREGISTERModel
                    {
                        SampleRegNo = row["SAMPLE_REG_NO"] as string,
                        SNO = Convert.ToString(row["SNO"]),
                        ItemDesc = Convert.ToString(row["ITEM_DESC"]),
                        Qty = Convert.ToString(row["QTY"]),
                        TestCategoryCode = Convert.ToString(row["TEST_CATEGORY_CD"]),
                        Test = Convert.ToString(row["TEST"]),
                        LabID = Convert.ToString(row["LAB_ID"]),
                        LabName = Convert.ToString(row["LAB_NAME"]),
                        TestingFee = Convert.ToString(row["TESTING_FEE"]),
                        ServiceTax = Convert.ToString(row["SERVICE_TAX"]),
                        HandlingCharges = Convert.ToString(row["HANDLING_CHARGES"]),
                        TestReportRequestDate = Convert.ToString(row["TEST_REPORT_REQ_DATE"]),
                        TestReportReceiveDate = Convert.ToString(row["TEST_REPORT_REC_DATE"]),
                        TestStatus = Convert.ToString(row["TEST_STATUS"]),
                        Remarks = Convert.ToString(row["REMARKS"]),
                        SampleDispatchLabDate = Convert.ToString(row["SAMPLE_DISPATCH_LAB_DT"]),
                    };

                    modelList.Add(model);
                }
            }
            query = modelList.AsQueryable();
            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.SampleRegNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public LABREGISTERModel LabDtlModify(string RegNo, string SNO)
        {

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.NVarchar2, RegNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_SNO", OracleDbType.NVarchar2, SNO, ParameterDirection.Input);
                par[2] = new OracleParameter("p_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("GetLabRegisterDetails", par, 2);

                LABREGISTERModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    model = new LABREGISTERModel
                    {
                        SampleRegNo = row["SAMPLE_REG_NO"] as string,
                        SNO = Convert.ToString(row["SNO"]),
                        ItemDesc = Convert.ToString(row["ITEM_DESC"]),
                        Qty = Convert.ToString(row["QTY"]),
                        TestCategoryCode = Convert.ToString(row["TEST_CATEGORY_CD"]),
                        Test = Convert.ToString(row["TEST"]),
                        LabID = Convert.ToString(row["LAB_ID"]),
                        LabName = Convert.ToString(row["LAB_NAME"]),
                        TestingFee = Convert.ToString(row["TESTING_FEE"]),
                        ServiceTax = Convert.ToString(row["SERVICE_TAX"]),
                        HandlingCharges = Convert.ToString(row["HANDLING_CHARGES"]),
                        TestReportRequestDate = Convert.ToString(row["TEST_REPORT_REQ_DATE"]),
                        TestReportReceiveDate = Convert.ToString(row["TEST_REPORT_REC_DATE"]),
                        TestStatus = Convert.ToString(row["TEST_STATUS"]),
                        Remarks = Convert.ToString(row["REMARKS"]),
                        SampleDispatchLabDate = Convert.ToString(row["SAMPLE_DISPATCH_LAB_DT"]),
                    };

                    //modelList.Add(model);

                }

                return model;
            }

        }

        public List<LABREGISTERModel> LabPaymentModify(string CaseNo, string VCode)
        {

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("p_CASE_NO", OracleDbType.NVarchar2, CaseNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_VEND_CD", OracleDbType.NVarchar2, VCode, ParameterDirection.Input);
                par[2] = new OracleParameter("p_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_LabPayRecV", par, 2);

                List<LABREGISTERModel> modelList = new List<LABREGISTERModel>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        LABREGISTERModel model = new LABREGISTERModel
                        {
                            CHQ_NO = row["CHQ_NO"] as string,
                            CHQ_DATE = Convert.ToString(row["CHQ_DATE"]),
                            AMOUNT = Convert.ToString(row["AMOUNT"]),
                            SUSPENSE_AMT = Convert.ToString(row["SUSPENSE_AMT"]),
                            BANK_NAME = Convert.ToString(row["BANK_NAME"]),
                            CASE_NO = Convert.ToString(row["CASE_NO"]),
                            BANK_CD = Convert.ToString(row["BANK_CD"]),
                            NARRATION = Convert.ToString(row["NARRATION"]),
                        };

                        modelList.Add(model);
                    }
                }

                return modelList;
            }

            //return dTResult;
        }

        //public List<LABREGISTERModel> LapIndexData(string CaseNo, string CallRdt, string RegNo)
        //{

        //    using (var dbContext = context.Database.GetDbConnection())
        //    {
        //        OracleParameter[] par = new OracleParameter[4];
        //        par[0] = new OracleParameter("p_CaseNo", OracleDbType.NVarchar2, CaseNo, ParameterDirection.Input);
        //        par[1] = new OracleParameter("p_CallRecvDt", OracleDbType.Date, CallRdt, ParameterDirection.Input);
        //        par[2] = new OracleParameter("p_LabRegNo", OracleDbType.NVarchar2, RegNo, ParameterDirection.Input);
        //        par[3] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);

        //        var ds = DataAccessDB.GetDataSet("SP_GETCALLREGISTER", par, 3);

        //        List<LABREGISTERModel> modelList = new List<LABREGISTERModel>();
        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in ds.Tables[0].Rows)
        //            {
        //                LABREGISTERModel model = new LABREGISTERModel
        //                {
        //                    SampleRegNo = row["SAMPLE_REG_NO"] as string,
        //                    CaseNo = Convert.ToString(row["CASE_NO"]),
        //                    CallRecDt = Convert.ToString(row["CALL_RECV_DATE"]),
        //                    CallSNO = Convert.ToString(row["CALL_SNO"]),
        //                    IE = Convert.ToString(row["IE_SNAME"]),
        //                    TestStatus = Convert.ToString(row["CALL_STATUS"]),

        //                };

        //                modelList.Add(model);
        //            }
        //        }

        //        return modelList;
        //    }

        //    //return dTResult;
        //}
        public DTResult<LABREGISTERModel> LapIndexData(DTParameters dtParameters)
        {

            DTResult<LABREGISTERModel> dTResult = new() { draw = 0 };
            IQueryable<LABREGISTERModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;


            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "SampleRegNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "SampleRegNo";
                orderAscendingDirection = true;
            }


            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_CaseNo", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CaseNo"), ParameterDirection.Input);
            par[1] = new OracleParameter("p_CallRecvDt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CallRdt"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_LabRegNo", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("RegNo"), ParameterDirection.Input);
            par[3] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);

           
            var ds = DataAccessDB.GetDataSet("SP_GETCALLREGISTER", par, 3);

            List<LABREGISTERModel> modelList = new List<LABREGISTERModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    LABREGISTERModel model = new LABREGISTERModel
                    {
                        SampleRegNo = row["SAMPLE_REG_NO"] as string,
                        CaseNo = Convert.ToString(row["CASE_NO"]),
                        CallRecDt = Convert.ToString(row["CALL_RECV_DATE"]),
                        CallSNO = Convert.ToString(row["CALL_SNO"]),
                        IE = Convert.ToString(row["IE_SNAME"]),
                        TestStatus = Convert.ToString(row["CALL_STATUS"]),

                    };

                    modelList.Add(model);
                }
            }
            query = modelList.AsQueryable();
            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.IE).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public LABREGISTERModel LabRegisterFormNew(string CaseNo, string CallDt, string CallSno)
        {

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_CASE_NO", OracleDbType.NVarchar2, CaseNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CALL_RECV_DT", OracleDbType.NVarchar2, CallDt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_CALL_SNO", OracleDbType.NVarchar2, CallSno, ParameterDirection.Input);
                par[3] = new OracleParameter("p_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_GetLabRegAddNew", par, 3);

                LABREGISTERModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new LABREGISTERModel
                    {
                        IECode = Convert.ToString(row["IE_CD"]), // Replace "Property1" with the actual column name in the table
                        IE = Convert.ToString(row["IE_NAME"]),
                        Vendor = Convert.ToString(row["VENDOR"]),
                        VendorCode = Convert.ToString(row["VEND_CD"]),
                    };
                }

                return model;
            }
        }
        public bool SaveDataDetails(LABREGISTERModel LABREGISTERModel)
        {
            using (var conn1 = context.Database.GetDbConnection())
            {
                conn1.Open();
                if (LABREGISTERModel.TestToBe == "D")
                {
                    LABREGISTERModel.Test = LABREGISTERModel.TestTobeCon;
                }
                string ss;
                string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";
                using (OracleCommand cmd2 = new OracleCommand(sqlQuery, (OracleConnection)conn1))
                {
                    ss = Convert.ToString(cmd2.ExecuteScalar());
                }

                using (OracleTransaction myTrans = (OracleTransaction)conn1.BeginTransaction())
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand("SP_UPDATE_LAB_DETAILS_51", (OracleConnection)conn1);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_SAMPLE_REG_NO", OracleDbType.Varchar2).Value = LABREGISTERModel.SampleRegNo;
                        cmd.Parameters.Add("p_SNO", OracleDbType.Varchar2).Value = LABREGISTERModel.SNO;
                        cmd.Parameters.Add("p_ITEM_DESC", OracleDbType.Varchar2).Value = LABREGISTERModel.ItemDesc;
                        cmd.Parameters.Add("p_QTY", OracleDbType.Varchar2).Value = LABREGISTERModel.Qty;
                        cmd.Parameters.Add("p_TEST_CATEGORY_CD", OracleDbType.Varchar2).Value = LABREGISTERModel.TestCategoryCode;
                        cmd.Parameters.Add("p_TEST", OracleDbType.Varchar2).Value = LABREGISTERModel.Test;
                        cmd.Parameters.Add("p_LAB_ID", OracleDbType.Varchar2).Value = LABREGISTERModel.LabID;
                        cmd.Parameters.Add("p_TESTING_FEE", OracleDbType.Varchar2).Value = LABREGISTERModel.DTestingFee;
                        cmd.Parameters.Add("p_SERVICE_TAX", OracleDbType.Varchar2).Value = LABREGISTERModel.DServiceTax;
                        cmd.Parameters.Add("p_HANDLING_CHARGES", OracleDbType.Varchar2).Value = LABREGISTERModel.DHandlingCharges;
                        cmd.Parameters.Add("p_TEST_REPORT_REQ_DT", OracleDbType.Date).Value = LABREGISTERModel.TestReportRequestDate;
                        cmd.Parameters.Add("p_TEST_REPORT_REC_DT", OracleDbType.Date).Value = LABREGISTERModel.TestReportReceiveDate;
                        cmd.Parameters.Add("p_TEST_STATUS", OracleDbType.Varchar2).Value = LABREGISTERModel.TestStatus;
                        cmd.Parameters.Add("p_REMARKS", OracleDbType.Varchar2).Value = LABREGISTERModel.DRemarks;
                        cmd.Parameters.Add("p_SAMPLE_DISPATCHED_TO_LAB_DT", OracleDbType.Date).Value = LABREGISTERModel.SampleDispatchLabDate;
                        cmd.Parameters.Add("p_USER_ID", OracleDbType.Varchar2).Value = LABREGISTERModel.UName;
                        cmd.Parameters.Add("p_DATETIME", OracleDbType.Date).Value = ss;

                        cmd.ExecuteNonQuery();
                        SaveDataDetails2(LABREGISTERModel);

                        //myTrans.Commit();
                        conn1.Close();
                    }
                    catch (Exception ex)
                    {
                        myTrans.Rollback();

                    }
                }
            }

            return true;
        }
        public bool SaveDataDetails2(LABREGISTERModel LABREGISTERModel)
        {
            using (var conn1 = context.Database.GetDbConnection())
            {
                //conn1.Open();

                string ss;
                string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";
                using (OracleCommand cmd2 = new OracleCommand(sqlQuery, (OracleConnection)conn1))
                {
                    ss = Convert.ToString(cmd2.ExecuteScalar());
                }

                //using (OracleTransaction myTrans = (OracleTransaction)conn1.BeginTransaction())
                //{
                try
                {
                    OracleCommand cmd = new OracleCommand("SP_UPDATE_LAB_DETAILS_50", (OracleConnection)conn1);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_TOTAL_TESTING_FEE", OracleDbType.Varchar2).Value = LABREGISTERModel.TotalTestingFee;
                    cmd.Parameters.Add("p_TOTAL_SERVICE_TAX", OracleDbType.Varchar2).Value = LABREGISTERModel.TotalServiceTax;
                    cmd.Parameters.Add("p_TOTAL_HANDLING_CHARGES", OracleDbType.Varchar2).Value = LABREGISTERModel.TotalHandlingCharges;
                    cmd.Parameters.Add("p_TOTAL_LAB_CHARGES", OracleDbType.Varchar2).Value = LABREGISTERModel.TotalLabCharges;
                    cmd.Parameters.Add("p_SAMPLE_REG_NO", OracleDbType.Varchar2).Value = LABREGISTERModel.SampleRegNo;
                    cmd.Parameters.Add("p_USER_ID", OracleDbType.Varchar2).Value = LABREGISTERModel.UName;
                    cmd.Parameters.Add("p_DATETIME", OracleDbType.Date).Value = ss;

                    cmd.ExecuteNonQuery();


                }
                catch (Exception ex)
                {
                    //myTrans.Rollback();

                }
                //}
            }

            return true;
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
        public bool InsertLabReg(LABREGISTERModel LABREGISTERModel)
        {
            string REG_NO = GenerateSampleRegNo(LABREGISTERModel);
            string ss;
            string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";

            ss = GetDateString(sqlQuery);
            try
            {
                if (LABREGISTERModel.TestingType == "Re-Testing")
                {
                    LABREGISTERModel.TestingType = "R";
                }
                else if(LABREGISTERModel.TestingType == "Private_Case")
                {
                    LABREGISTERModel.TestingType = "P";
                }
                else if (LABREGISTERModel.TestingType == "IREPS_Case")
                {
                    LABREGISTERModel.TestingType = "I";
                }
                else
                {
                    LABREGISTERModel.TestingType = "";
                }
                OracleParameter[] par = new OracleParameter[15];
                par[0] = new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.Varchar2, REG_NO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_SAMPLE_REG_DT", OracleDbType.Date, LABREGISTERModel.SampleRegDate, ParameterDirection.Input);
                par[2] = new OracleParameter("p_SAMPLE_DRAWL_DT", OracleDbType.Date, LABREGISTERModel.SampleDrawalDate, ParameterDirection.Input);
                par[3] = new OracleParameter("p_SAMPLE_RECIEPT_DT", OracleDbType.Date, LABREGISTERModel.SampleReceiptDate, ParameterDirection.Input);
                par[4] = new OracleParameter("p_SAMPLE_DISPATCH_DT", OracleDbType.Date, LABREGISTERModel.SampleDispatchDate, ParameterDirection.Input);
                par[5] = new OracleParameter("p_IE_CD", OracleDbType.Varchar2, LABREGISTERModel.IECode, ParameterDirection.Input);
                par[6] = new OracleParameter("p_CASE_NO", OracleDbType.Varchar2, LABREGISTERModel.CaseNo, ParameterDirection.Input);
                par[7] = new OracleParameter("p_CALL_SNO", OracleDbType.Varchar2, LABREGISTERModel.CallSNO, ParameterDirection.Input);
                par[8] = new OracleParameter("p_CALL_RECV_DT", OracleDbType.Date, LABREGISTERModel.CallRecDt, ParameterDirection.Input);
                par[9] = new OracleParameter("p_VEND_CD", OracleDbType.Varchar2, LABREGISTERModel.VendorCode, ParameterDirection.Input);
                par[10] = new OracleParameter("p_USER_ID", OracleDbType.Varchar2, LABREGISTERModel.UName, ParameterDirection.Input);
                par[11] = new OracleParameter("p_DATETIME", OracleDbType.Date, ss, ParameterDirection.Input);
                par[12] = new OracleParameter("p_TESTING_TYPE", OracleDbType.Varchar2, LABREGISTERModel.TestingType, ParameterDirection.Input);
                par[13] = new OracleParameter("p_CODE_NO", OracleDbType.Varchar2, LABREGISTERModel.CodeNo, ParameterDirection.Input);
                par[14] = new OracleParameter("p_CODE_DT", OracleDbType.Date, LABREGISTERModel.CodeDate, ParameterDirection.Input);


                var ds = DataAccessDB.ExecuteNonQuery("SP_INSERT_LAB_REGISTER", par, 1);
                string Sno = GetSNo(REG_NO);
                InsertLabRegDetails(REG_NO, Sno, LABREGISTERModel);
                UpdateLabReg(REG_NO, LABREGISTERModel);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool InsertDataDetails(LABREGISTERModel LABREGISTERModel)
        {
            
            try
            {
                string REG_NO = LABREGISTERModel.SampleRegNo;
                string Sno = GetSNo(REG_NO);
                InsertLabRegDetails(REG_NO, Sno, LABREGISTERModel);
                UpdateLabReg(REG_NO, LABREGISTERModel);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public string GetSNo(string RegNo)
        {
            string query = "Select NVL(max(SNO),0)+1 from T51_LAB_REGISTER_DETAIL WHERE SAMPLE_REG_NO ='" + RegNo + "'";
            string ds = GetDateString(query);

            //OracleParameter[] par = new OracleParameter[1];
            //par[0] = new OracleParameter("SAMPLE_REG_NO", OracleDbType.Varchar2, RegNo, ParameterDirection.Input);
            //var ds = DataAccessDB.GetDataSet(query, par, 1);
            return ds.ToString();


        }

        public bool InsertLabRegDetails(string REG_NO, string Sno, LABREGISTERModel LABREGISTERModel)
        {
            try
            {
                if(LABREGISTERModel.TestToBe == "D")
                {
                    LABREGISTERModel.Test = LABREGISTERModel.TestTobeCon;
                }
                
                string ss;
                string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";
                ss = GetDateString(sqlQuery);
                OracleParameter[] par = new OracleParameter[17];
                par[0] = new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.Varchar2, REG_NO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_SNO", OracleDbType.Varchar2, Sno, ParameterDirection.Input);
                par[2] = new OracleParameter("p_ITEM_DESC", OracleDbType.Varchar2, LABREGISTERModel.ItemDesc, ParameterDirection.Input);
                par[3] = new OracleParameter("p_QTY", OracleDbType.Varchar2, LABREGISTERModel.Qty, ParameterDirection.Input);
                par[4] = new OracleParameter("p_TEST_CATEGORY_CD", OracleDbType.Varchar2, LABREGISTERModel.TestCategoryCode, ParameterDirection.Input);
                par[5] = new OracleParameter("p_TEST", OracleDbType.Varchar2, LABREGISTERModel.Test, ParameterDirection.Input);
                par[6] = new OracleParameter("p_LAB_ID", OracleDbType.Varchar2, LABREGISTERModel.LabID, ParameterDirection.Input);
                par[7] = new OracleParameter("p_TESTING_FEE", OracleDbType.Varchar2, LABREGISTERModel.DTestingFee, ParameterDirection.Input);
                par[8] = new OracleParameter("p_SERVICE_TAX", OracleDbType.Varchar2, LABREGISTERModel.DServiceTax, ParameterDirection.Input);
                par[9] = new OracleParameter("p_HANDLING_CHARGES", OracleDbType.Varchar2, LABREGISTERModel.DHandlingCharges, ParameterDirection.Input);
                par[10] = new OracleParameter("p_TEST_REPORT_REQ_DT", OracleDbType.Date, LABREGISTERModel.TestReportRequestDate, ParameterDirection.Input);
                par[11] = new OracleParameter("p_TEST_REPORT_REC_DT", OracleDbType.Date, LABREGISTERModel.TestReportReceiveDate, ParameterDirection.Input);
                par[12] = new OracleParameter("p_TEST_STATUS", OracleDbType.Varchar2, LABREGISTERModel.TestStatus, ParameterDirection.Input);
                par[13] = new OracleParameter("p_REMARKS", OracleDbType.Varchar2, LABREGISTERModel.DRemarks, ParameterDirection.Input);
                par[14] = new OracleParameter("p_SAMPLE_DISPATCHED_TO_LAB_DT", OracleDbType.Date, LABREGISTERModel.SampleDispatchLabDate, ParameterDirection.Input);
                par[15] = new OracleParameter("p_USER_ID", OracleDbType.Varchar2, LABREGISTERModel.UName, ParameterDirection.Input);
                par[16] = new OracleParameter("p_DATETIME", OracleDbType.Date, ss, ParameterDirection.Input);
                var ds = DataAccessDB.ExecuteNonQuery("SP_INSERT_LAB_REGISTER_DETAIL", par, 1);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool UpdateLabReg(string REG_NO, LABREGISTERModel LABREGISTERModel)
        {
            try
            {
                string ss;
                string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";
                ss = GetDateString(sqlQuery);
                OracleParameter[] par = new OracleParameter[7];
                par[0] = new OracleParameter("p_TOTAL_TESTING_FEE", OracleDbType.Varchar2, LABREGISTERModel.TotalTestingFee, ParameterDirection.Input);
                par[1] = new OracleParameter("p_TOTAL_SERVICE_TAX", OracleDbType.Varchar2, LABREGISTERModel.TotalServiceTax, ParameterDirection.Input);
                par[2] = new OracleParameter("p_TOTAL_HANDLING_CHARGES", OracleDbType.Varchar2, LABREGISTERModel.TotalHandlingCharges, ParameterDirection.Input);
                par[3] = new OracleParameter("p_TOTAL_LAB_CHARGES", OracleDbType.Varchar2, LABREGISTERModel.TotalLabCharges, ParameterDirection.Input);
                par[4] = new OracleParameter("p_SAMPLE_REG_NO", OracleDbType.Varchar2, REG_NO, ParameterDirection.Input);
                par[5] = new OracleParameter("p_USER_ID", OracleDbType.Varchar2, LABREGISTERModel.UName, ParameterDirection.Input);
                par[6] = new OracleParameter("p_DATETIME", OracleDbType.Date, ss, ParameterDirection.Input);
                var ds = DataAccessDB.ExecuteNonQuery("SP_UPDATE_LAB_REGISTER", par, 1);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        public string GenerateSampleRegNo(LABREGISTERModel LABREGISTERModel)
        {
            string date = LABREGISTERModel.SampleRegDate.ToString().Substring(0, 10);
            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Char, LABREGISTERModel.Region, ParameterDirection.Input);
            par[1] = new OracleParameter("IN_SAMPLE_REG_DT", OracleDbType.Varchar2, date, ParameterDirection.Input);
            par[2] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GENERATE_SAMPLE_REG_NO", par, 1);

            LABREGISTERModel model1 = new();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                DataRow row = ds.Tables[0].Rows[0];
                model1 = new LABREGISTERModel
                {
                    SampleRegNo = row["SAMPLE_REG_NO"] as string,
                };
            }
            var RegNo = model1.SampleRegNo.Trim();
            return RegNo;
        }

        public bool PrintInvoice(string REG_NO, LABREGISTERModel LABREGISTERModel)
        {
            try
            {
                string ss;

                string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";
                ss = GetDateString(sqlQuery);

                string ss1;
                int yy = 0;
                if (Convert.ToInt16(ss.Substring(3, 2)) > 3)
                {
                    yy = Convert.ToInt16(ss.Substring(8, 2));
                }
                else
                {
                    yy = Convert.ToInt16(ss.Substring(8, 2)) - 1;
                }
                string wRegion_yy = LABREGISTERModel.Region + "/" + Convert.ToString(yy);
                string invoice;
                string sqlQuery1 = "SELECT NVL(MAX(TO_NUMBER(nvl(TRIM(SUBSTR(INVOICE_NO,6,5)),'0'))),10000)+1 FROM T55_LAB_INVOICE WHERE SUBSTR(INVOICE_NO,1,4)='" + wRegion_yy + "'";
                string inv = GetDateString(sqlQuery1);
                invoice = wRegion_yy + "/" + inv;


                OracleParameter[] par = new OracleParameter[5];
                par[0] = new OracleParameter("w_inv_no", OracleDbType.Varchar2, invoice, ParameterDirection.Input);
                par[1] = new OracleParameter("ss", OracleDbType.Date, ss, ParameterDirection.Input);
                par[2] = new OracleParameter("sample_reg_no", OracleDbType.Varchar2, REG_NO, ParameterDirection.Input);
                par[3] = new OracleParameter("user_id", OracleDbType.Varchar2, LABREGISTERModel.UName, ParameterDirection.Input);
                par[4] = new OracleParameter("ss1", OracleDbType.Date, ss, ParameterDirection.Input);
                var ds = DataAccessDB.ExecuteNonQuery("INSERT_INVOICE_PROC", par, 1);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool PostAmount(LABREGISTERModel LABREGISTERModel)
        {
            string[] Bankno = LABREGISTERModel.BANK_CD.Split(',');
            string[] chqno = LABREGISTERModel.CHQ_NO.Split(',');
            string[] chqdt = LABREGISTERModel.CHQ_DATE.Split(',');
            string[] amt = LABREGISTERModel.AMOUNT.Split(',');
            string[] susamt = LABREGISTERModel.SUSPENSE_AMT.Split(',');
            for (int i = 0; i < Bankno.Length; i++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Bankno[i].ToString()))
                    {
                        string ss;

                        string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";
                        ss = GetDateString(sqlQuery);
                        int wposting_amt = 0;
                        if (Convert.ToInt32(amt[i]) >= (Convert.ToInt32(LABREGISTERModel.TotalLabCharges) - (Convert.ToInt32(LABREGISTERModel.AmountRecieved) + Convert.ToInt32(LABREGISTERModel.TotalTDS))))
                        {
                            wposting_amt = Convert.ToInt32(LABREGISTERModel.TotalLabCharges) - (Convert.ToInt32(LABREGISTERModel.AmountRecieved) + Convert.ToInt32(LABREGISTERModel.TotalTDS));
                        }
                        else
                        {
                            wposting_amt = Convert.ToInt32(amt[i]);
                        }
                        OracleParameter[] par = new OracleParameter[8];
                        par[0] = new OracleParameter("RegNo", OracleDbType.Varchar2, LABREGISTERModel.SampleRegNo, ParameterDirection.Input);
                        par[1] = new OracleParameter("BankCode", OracleDbType.Varchar2, Bankno[i], ParameterDirection.Input);
                        par[2] = new OracleParameter("ChqNo", OracleDbType.Varchar2, chqno[i], ParameterDirection.Input);
                        par[3] = new OracleParameter("ChqDt", OracleDbType.Date, chqdt[i], ParameterDirection.Input);
                        par[4] = new OracleParameter("AmtClear", OracleDbType.Varchar2, wposting_amt, ParameterDirection.Input);
                        par[5] = new OracleParameter("TotalLabC", OracleDbType.Varchar2, LABREGISTERModel.TotalAmountCleared, ParameterDirection.Input);
                        par[6] = new OracleParameter("UserId", OracleDbType.Varchar2, LABREGISTERModel.UName, ParameterDirection.Input);
                        par[7] = new OracleParameter("Datetime", OracleDbType.Date, ss, ParameterDirection.Input);
                        var ds = DataAccessDB.ExecuteNonQuery("INSERT_LAB_POSTING_PROC", par, 1);
                        string ChNO = chqno[i];
                        string ChDt = chqdt[i];
                        string Bnkc = Bankno[i];
                        AmountReceive(wposting_amt, LABREGISTERModel);
                        RvDetailsUpdate(wposting_amt, LABREGISTERModel, ChNO, ChDt, Bnkc);
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        public bool AmountReceive(int wposting_amt, LABREGISTERModel LABREGISTERModel)
        {
            try
            {
                
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_sample_reg_no", OracleDbType.Varchar2, LABREGISTERModel.SampleRegNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_wposting_amt", OracleDbType.Varchar2, wposting_amt, ParameterDirection.Input);                
                var ds = DataAccessDB.ExecuteNonQuery("UPDATE_AMOUNT_RECIEVED_PROC", par, 1);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        public bool RvDetailsUpdate(int wposting_amt, LABREGISTERModel LABREGISTERModel,string ChNO,string ChDt,string Bnkc)
        {
            try
            {

                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_chq_no", OracleDbType.Varchar2, ChNO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_chq_dt", OracleDbType.Date, ChDt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_bank_cd", OracleDbType.Varchar2, Bnkc, ParameterDirection.Input);
                par[3] = new OracleParameter("p_wposting_amt", OracleDbType.Varchar2, wposting_amt, ParameterDirection.Input);
                var ds = DataAccessDB.ExecuteNonQuery("UPDATE_RV_DETAILS_PROC", par, 1);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
    }
}
