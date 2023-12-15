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
using System.Dynamic;
using System.Reflection.Emit;
using static IBS.Helper.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace IBS.Repositories
{
    public class LabSampleRepository : ILabSampleInfoRepository
    {
        private readonly ModelContext context;

        public LabSampleRepository(ModelContext context)
        {
            this.context = context;
        }
        //public List<LabSampleInfoModel> LapSampleIndex(string CaseNo, string CallRdt,string CallSno, string Regin)
        //{

        //    using (var dbContext = context.Database.GetDbConnection())
        //    {
        //        OracleParameter[] par = new OracleParameter[5];
        //        par[0] = new OracleParameter("p_Region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
        //        par[1] = new OracleParameter("p_CaseNo", OracleDbType.NVarchar2, CaseNo, ParameterDirection.Input);
        //        par[2] = new OracleParameter("p_CSNO", OracleDbType.NVarchar2, CallSno, ParameterDirection.Input);
        //        par[3] = new OracleParameter("p_Rdt", OracleDbType.NVarchar2, CallRdt, ParameterDirection.Input);
        //        par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

        //        var ds = DataAccessDB.GetDataSet("GetSampleInfoIndex", par, 4);

        //        List<LabSampleInfoModel> modelList = new List<LabSampleInfoModel>();
        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in ds.Tables[0].Rows)
        //            {
        //                LabSampleInfoModel model = new LabSampleInfoModel
        //                {
        //                    CaseNo = Convert.ToString(row["case_no"]),
        //                    CallRecDt = Convert.ToString(row["call_recv_dt"]),
        //                    CallSNO = Convert.ToString(row["call_sno"]),
        //                    IEName = Convert.ToString(row["ie_name"]),

        //                };

        //                modelList.Add(model);
        //            }
        //        }

        //        return modelList;
        //    }

        //    //return dTResult;
        //}
        public DTResult<LabSampleInfoModel> LapSampleIndex(DTParameters dtParameters, string Regin)
        {

            DTResult<LabSampleInfoModel> dTResult = new() { draw = 0 };
            IQueryable<LabSampleInfoModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CallRecDt";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CallRecDt";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_Region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("p_CaseNo", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CaseNo"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_CSNO", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CallSNo"), ParameterDirection.Input);
            par[3] = new OracleParameter("p_Rdt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CallRdt"), ParameterDirection.Input);
            par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetSampleInfoIndex", par, 4);

            List<LabSampleInfoModel> modelList = new List<LabSampleInfoModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    LabSampleInfoModel model = new LabSampleInfoModel
                    {
                        CaseNo = Convert.ToString(row["case_no"]),
                        CallRecDt = Convert.ToString(row["call_recv_dt"]),
                        CallSNO = Convert.ToString(row["call_sno"]),
                        IEName = Convert.ToString(row["ie_name"]),

                    };

                    modelList.Add(model);
                }
            }



            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
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
        public LabSampleInfoModel SampleDtlData(string CaseNo, string CallRdt, string CallSno, string Regin)
        {
            string PaymentSlip = "";
            string PaymentStatus = "";
            string Hlink = "";
            string sqlQuery = "SELECT DECODE(DOC_STATUS_VEND,'Y','UPLOADED','NOT UPLOADED') FROM T110_LAB_DOC WHERE  case_no='" + CaseNo.Trim() + "' and to_char(call_recv_dt,'dd/mm/yyyy')='" + CallRdt.Trim() + "' and call_sno='" + CallSno.Trim() + "'";
            PaymentSlip = GetDateString(sqlQuery);
            if(PaymentSlip != null)
            {
                
                string sqlQuery1 = "SELECT DECODE(DOC_STATUS_FIN,'A','APPROVED'||' On: '||to_char(DOC_APP_DATETIME,'dd/mm/yyyy-HH24:MI:SS'),'R','REJECTED'||' On: '||to_char(DOC_APP_DATETIME,'dd/mm/yyyy-HH24:MI:SS')||DOC_REJ_REMARK,'PENDING') FROM T110_LAB_DOC WHERE  case_no='" + CaseNo.Trim() + "' and to_char(call_recv_dt,'dd/mm/yyyy')='" + CallRdt.Trim() + "' and call_sno='" + CallSno.Trim() + "'";
                PaymentStatus = GetDateString(sqlQuery1);
            }
            else
            {
                PaymentSlip = "Not Uploaded";
                    PaymentStatus = "Not Approved";
            }
            Hlink = Showfile(CaseNo, CallRdt, CallSno);
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[5];
                par[0] = new OracleParameter("p_lblCaseNo", OracleDbType.NVarchar2, CaseNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_lblCallDT", OracleDbType.NVarchar2, CallRdt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_lblCSNO", OracleDbType.NVarchar2, CallSno, ParameterDirection.Input);
                par[3] = new OracleParameter("p_Region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
                par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("GetLabSampleDtl", par, 4);

                LabSampleInfoModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new LabSampleInfoModel
                    {
                        IEName = row["ie_name"] as string, // Replace "Property1" with the actual column name in the table
                        VendorName = Convert.ToString(row["vendor"]),
                        Vendor = Convert.ToString(row["vend_cd"]),
                        IE = Convert.ToString(row["ie_cd"]),
                        CaseNo = CaseNo,
                        CallRecDt = CallRdt,
                        CallSNO = CallSno,
                        DateofRecSample = Convert.ToString(row["sample_recv_dt"]),
                        LikelyDt = Convert.ToString(row["likely_dt_report"]),
                        TotalTFee = Convert.ToString(row["testing_charges"]),
                        Status = Convert.ToString(row["status"]),
                        Remarks = Convert.ToString(row["remarks"]),
                        PaymentSlip = PaymentSlip,
                        PaymentStatus = PaymentStatus,
                        Hyperlink2  = Hlink,
                    };
                }
                
                return model;
            }
        }
        public string Showfile(string CaseNo, string CallRdt, string CallSno)
        {
            string link = "";
            string MyFile_ex = "";
            string mdt_ex = dateconcate2(CallRdt.Trim());
            MyFile_ex = CaseNo.Trim() + '_' + CallSno.Trim() + '_' + mdt_ex;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB",  MyFile_ex + ".PDF");
            if (File.Exists(filePath) == false)
            {
                link = "false";
            }
            else
            {
                link = filePath;
               
            }
            return link;
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
        public bool SaveDataDetails(LabSampleInfoModel LabSampleInfoModel)
        {
            
            string ss;
            string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";

            ss = GetDateString(sqlQuery);
            try
            {
                
                OracleParameter[] par = new OracleParameter[11];
                par[0] = new OracleParameter("p_CaseNo", OracleDbType.Varchar2, LabSampleInfoModel.CaseNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CallDT", OracleDbType.Date, LabSampleInfoModel.CallRecDt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_CSNO", OracleDbType.Varchar2, LabSampleInfoModel.CallSNO, ParameterDirection.Input);
                par[3] = new OracleParameter("p_IECD", OracleDbType.Varchar2, LabSampleInfoModel.IE, ParameterDirection.Input);
                par[4] = new OracleParameter("p_Status", OracleDbType.Varchar2, LabSampleInfoModel.Status, ParameterDirection.Input);
                par[5] = new OracleParameter("p_SampleReceiptDT", OracleDbType.Date, LabSampleInfoModel.DateofRecSample, ParameterDirection.Input);
                par[6] = new OracleParameter("p_TestingFee", OracleDbType.Varchar2, LabSampleInfoModel.TotalTFee, ParameterDirection.Input);
                par[7] = new OracleParameter("p_LikelyTRDt", OracleDbType.Date, LabSampleInfoModel.LikelyDt, ParameterDirection.Input);
                par[8] = new OracleParameter("p_Remarks", OracleDbType.Varchar2, LabSampleInfoModel.Remarks, ParameterDirection.Input);               
                par[9] = new OracleParameter("p_UserId", OracleDbType.Varchar2, LabSampleInfoModel.UName, ParameterDirection.Input);
                par[10] = new OracleParameter("p_DateTime", OracleDbType.Date, ss, ParameterDirection.Input);
               

                var ds = DataAccessDB.ExecuteNonQuery("InsertLabSampleInfo", par, 1);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        string dateconcate(string dt)
        {
            string myYear, myMonth, myDay;

            myYear = dt.Substring(6, 4);
            myMonth = dt.Substring(3, 2);
            myDay = dt.Substring(0, 2);
            string dt1 = myYear + myMonth + myDay;
            return (dt1);
        }
        string dateconcate2(string dt)
        {
            string myYear, myMonth, myDay;

            myYear = dt.Substring(6, 4);
            myMonth = dt.Substring(3, 2);
            myDay = dt.Substring(0, 2);
            string dt1 = myYear + myDay + myMonth;
            return (dt1);
        }

        public bool UpdateDetails(LabSampleInfoModel LabSampleInfoModel)
        {
            
            //if (LabSampleInfoModel.UploadLab != null && LabSampleInfoModel.UploadLab.Length > 0)
            //{
                
            //    List<string> savedFilePaths = new List<string>();
            //    string fn = "", MyFile = "", fx = "", fl = "";
            //    string mdt = dateconcate(LabSampleInfoModel.CallRecDt);
            //    MyFile = LabSampleInfoModel.CaseNo + '_' + LabSampleInfoModel.CallSNO + '_' + mdt;
            //    fn = System.IO.Path.GetFileName(LabSampleInfoModel.UploadLab.FileName);
            //    fx = System.IO.Path.GetExtension(LabSampleInfoModel.UploadLab.FileName).ToUpper().Substring(1);
            //    string saveLocation = null;
            //    if (fx == "PDF")
            //    {
            //        saveLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LAB", MyFile + ".PDF");
            //        using (var stream = new FileStream(saveLocation, FileMode.Create))
            //        {
            //            LabSampleInfoModel.UploadLab.CopyTo(stream);
            //        }
            //        savedFilePaths.Add(saveLocation);
            //    }
                
            //}
                string ss;
            string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";

            ss = GetDateString(sqlQuery);
            try
            {

                OracleParameter[] par = new OracleParameter[10];
                par[0] = new OracleParameter("p_Status", OracleDbType.Varchar2, LabSampleInfoModel.Status, ParameterDirection.Input);
                par[1] = new OracleParameter("p_SampleRecvDT", OracleDbType.Date, LabSampleInfoModel.DateofRecSample, ParameterDirection.Input);
                par[2] = new OracleParameter("p_TestingCharges", OracleDbType.Varchar2, LabSampleInfoModel.TotalTFee, ParameterDirection.Input);
                par[3] = new OracleParameter("p_LikelyDtReport", OracleDbType.Date, LabSampleInfoModel.LikelyDt, ParameterDirection.Input);
                par[4] = new OracleParameter("p_Remarks", OracleDbType.Varchar2, LabSampleInfoModel.Remarks, ParameterDirection.Input);
                par[5] = new OracleParameter("p_UserId", OracleDbType.Varchar2, LabSampleInfoModel.UName, ParameterDirection.Input);
                par[6] = new OracleParameter("p_DateTime", OracleDbType.Date, ss, ParameterDirection.Input);
                par[7] = new OracleParameter("p_CaseNo", OracleDbType.Varchar2, LabSampleInfoModel.CaseNo, ParameterDirection.Input);
                par[8] = new OracleParameter("p_CallRecvDT", OracleDbType.Date, LabSampleInfoModel.CallRecDt, ParameterDirection.Input);
                par[9] = new OracleParameter("p_CallSno", OracleDbType.Varchar2, LabSampleInfoModel.CallSNO, ParameterDirection.Input);



                var ds = DataAccessDB.ExecuteNonQuery("UPDATE_SAMPLE_INFO", par, 1);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public string CheckExist(string CaseNo, string CallRdt, string CallSno,string Regin)
        {
            
            string query = "SELECT COUNT(*) FROM T109_LAB_SAMPLE_INFO T109 " +
                           "WHERE TO_CHAR(T109.CALL_RECV_DT, 'dd/mm/yyyy') = '" + CallRdt + "' " +
                           "AND T109.case_no = '" + CaseNo + "' AND T109.call_sno = '" + CallSno + "' AND " +
                           "SUBSTR(T109.case_no, 1, 1) = '" + Regin + "'";

            
            string count = GetDateString(query);

            
            //string nextSerialNumber = (count + 1).ToString();

            return count;
        }
        public bool UploadDate(LabSampleInfoModel LabSampleInfoModel)
        {
            string ss;
            string sqlQuery = "Select to_char(sysdate,'mm/dd/yyyy') from dual";

            ss = GetDateString(sqlQuery);
            using (var conn = context.Database.GetDbConnection())
            {
                conn.Open();

                string query = "UPDATE t109_lab_sample_info SET lab_rep_uploaded_dt = :lab_rep_uploaded_dt, USER_ID = :USER_ID, DATETIME = : DATETIME WHERE CASE_NO = :CASE_NO AND CALL_RECV_DT =: CALL_RECV_DT AND CALL_SNO = : CALL_SNO";
                using (var cmd = new OracleCommand(query, (OracleConnection)conn))
                {
                    cmd.Parameters.Add(new OracleParameter("lab_rep_uploaded_dt", OracleDbType.Date)).Value = ss;
                    cmd.Parameters.Add(new OracleParameter("USER_ID", OracleDbType.NVarchar2)).Value = LabSampleInfoModel.UName;
                    cmd.Parameters.Add(new OracleParameter("DATETIME", OracleDbType.Date)).Value = ss;
                    cmd.Parameters.Add(new OracleParameter("CASE_NO", OracleDbType.NVarchar2)).Value = LabSampleInfoModel.CaseNo;
                    cmd.Parameters.Add(new OracleParameter("CALL_RECV_DT", OracleDbType.Date)).Value = LabSampleInfoModel.CallRecDt;
                    cmd.Parameters.Add(new OracleParameter("CALL_SNO", OracleDbType.NVarchar2)).Value = LabSampleInfoModel.CallSNO;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return true;

        }
    }
}
