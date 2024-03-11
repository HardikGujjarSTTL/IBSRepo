﻿using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace IBS.Repositories
{
    public class VendorLabSampleRepository : IVendorLabSampleInfoRepository
    {
        private readonly ModelContext context;

        public VendorLabSampleRepository(ModelContext context)
        {
            this.context = context;
        }
        //public List<LabSampleInfoModel> LapSampleIndex(string CaseNo, string CallRdt,string CallSno, string VenCod)
        //{

        //    using (var dbContext = context.Database.GetDbConnection())
        //    {
        //        OracleParameter[] par = new OracleParameter[5];
        //        par[0] = new OracleParameter("p_VEND_CD", OracleDbType.NVarchar2, VenCod, ParameterDirection.Input);
        //        par[1] = new OracleParameter("p_CaseNo", OracleDbType.NVarchar2, CaseNo, ParameterDirection.Input);
        //        par[2] = new OracleParameter("p_CSNO", OracleDbType.NVarchar2, CallSno, ParameterDirection.Input);
        //        par[3] = new OracleParameter("p_Rdt", OracleDbType.NVarchar2, CallRdt, ParameterDirection.Input);
        //        par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

        //        var ds = DataAccessDB.GetDataSet("Vendor_GetSampleInfoIndex", par, 4);

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
            par[0] = new OracleParameter("p_VEND_CD", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("p_CaseNo", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CaseNo"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_CSNO", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CallSNo"), ParameterDirection.Input);
            par[3] = new OracleParameter("p_Rdt", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CallRdt"), ParameterDirection.Input);
            par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Vendor_GetSampleInfoIndex", par, 4);

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
                query = query.Where(w => Convert.ToString(w.IEName).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.IEName).ToLower().Contains(searchBy.ToLower())
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
            string Doc_Stu = "";
            string Doc_Ext = "";
            string labelexist = "";
            string sqlQuery = "select count(*) from t110_Lab_Doc T110 where to_char(T110.CALL_RECV_DT, 'dd/mm/yyyy')='" + CallRdt.Trim() + "' and T110.case_no='" + CaseNo.Trim() + "' and T110.call_sno='" + CallSno.Trim() + "'";
            Doc_Ext = GetDateString(sqlQuery);
            if (Convert.ToInt16(Doc_Ext) > 0)
            {
                string sqlQuery1 = "select DOC_STATUS_FIN from t110_Lab_Doc T110 where to_char(T110.CALL_RECV_DT, 'dd/mm/yyyy')='" + CallRdt.Trim() + "' and T110.case_no='" + CaseNo.Trim() + "' and T110.call_sno='" + CallSno.Trim() + "'";
                Doc_Stu = GetDateString(sqlQuery1);
                if (Doc_Stu == "A")
                {
                    labelexist = "Y";
                }
                else
                {
                    labelexist = "N";
                }
            }
            else
            {
                labelexist = "B";
            }
            if (labelexist == "Y")
            {
                labelexist = "S";
            }
            var file = ShowFile(CallRdt, CaseNo, CallSno);

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[5];
                par[0] = new OracleParameter("p_Case_No", OracleDbType.NVarchar2, CaseNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_Call_Recv_DT", OracleDbType.NVarchar2, CallRdt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_Call_SNO", OracleDbType.NVarchar2, CallSno, ParameterDirection.Input);
                par[3] = new OracleParameter("p_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                par[4] = new OracleParameter("p_CURSOR1", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("Vendor_GetLabSampleDtl", par, 4);

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
                        NetTesting = Convert.ToString(row["testing_charges"]),
                        TDS = Convert.ToString(row["tds"]),
                        UTRNO = Convert.ToString(row["utr_no"]),
                        UTRDT = Convert.ToString(row["utr_dt"]),
                        PaymentStatus = Convert.ToString(row["status"]),
                        File = Convert.ToString(file),
                        //DateofRecSample = Convert.ToString(row["sample_recv_dt"]),
                        //TotalTFee = Convert.ToString(row["testing_charges"]),
                        //LikelyDt = Convert.ToString(row["likely_dt_report"]),
                        //Status = Convert.ToString(row["status1"]),
                        //Remarks = Convert.ToString(row["remarks"]),
                    };
                }
                LabSampleInfoModel sampleDetail = new();
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[1].Rows[0];
                    sampleDetail = new LabSampleInfoModel
                    {
                        DateofRecSample = Convert.ToString(row["sample_recv_dt"]),
                        TotalTFee = Convert.ToString(row["techarge"]),
                        LikelyDt = Convert.ToString(row["likely_dt_report"]),
                        Status = Convert.ToString(row["status1"]),
                        Remarks = Convert.ToString(row["remarks"]),
                    };
                }
                else
                {
                    model.LabelExist = labelexist;
                }
                model.DateofRecSample = sampleDetail.DateofRecSample;
                model.TotalTFee = sampleDetail.TotalTFee;
                model.LikelyDt = sampleDetail.LikelyDt;
                model.Status = sampleDetail.Status;
                model.Remarks = sampleDetail.Remarks;
                return model;
            }
        }
        public bool ShowFile(string lblCallDT, string lblCaseNo, string lblCSNO)
        {

            //string mdtEx = dateconcate(lblCallDT.Trim());
            //string myFileEx = $"{lblCaseNo.Trim()}_{lblCSNO.Trim()}_{mdtEx}";

            //string fpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "Payment", $"{myFileEx}.PDF");
            string MyFile_ex = "";
            string mdt_ex = dateconcate1(lblCallDT.Trim());
            MyFile_ex = lblCaseNo.Trim() + '_' + lblCSNO.Trim() + '_' + mdt_ex;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReadWriteData", "LAB", "PReciept", MyFile_ex + ".PDF");

            return File.Exists(filePath);
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
            string CallDate = Convert.ToDateTime(LabSampleInfoModel.CallRecDt).ToString("MM/dd/yyyy");
            string UTRDT = Convert.ToDateTime(LabSampleInfoModel.UTRDT).ToString("MM/dd/yyyy");
           
            string? caseNo = LabSampleInfoModel.CaseNo;
            DateTime? callRecDt = Convert.ToDateTime(LabSampleInfoModel.CallRecDt);
            int? callSNO = Convert.ToInt32(LabSampleInfoModel.CallSNO);
            var query = (from x in context.T110LabDocs
                         where x.CaseNo == caseNo && x.CallRecvDt == callRecDt && x.CallSno == callSNO
                         select new LabSampleInfoModel
                         {
                             CaseNo = x.CaseNo,
                             CallRecDt = Convert.ToString(x.CallRecvDt),
                             CallSNO = Convert.ToString(x.CallSno)
                         }).Distinct();
            var result = query.FirstOrDefault();
            ss = GetDateString(sqlQuery);
            try
            {
                if (result == null)
                {
                    OracleParameter[] par = new OracleParameter[9];
                    par[0] = new OracleParameter("p_CaseNo", OracleDbType.Varchar2, LabSampleInfoModel.CaseNo, ParameterDirection.Input);
                    par[1] = new OracleParameter("p_CallRecvDT", OracleDbType.Date, CallDate, ParameterDirection.Input);
                    par[2] = new OracleParameter("p_CallSno", OracleDbType.Varchar2, LabSampleInfoModel.CallSNO, ParameterDirection.Input);
                    par[3] = new OracleParameter("p_TestingCharges", OracleDbType.Varchar2, LabSampleInfoModel.NetTesting, ParameterDirection.Input);
                    par[4] = new OracleParameter("p_DocInitDateTime", OracleDbType.Date, ss, ParameterDirection.Input);
                    par[5] = new OracleParameter("p_TDS", OracleDbType.Varchar2, LabSampleInfoModel.TDS, ParameterDirection.Input);
                    par[6] = new OracleParameter("p_UTRNo", OracleDbType.Varchar2, LabSampleInfoModel.UTRNO, ParameterDirection.Input);
                    par[7] = new OracleParameter("p_UTRDate", OracleDbType.Date, UTRDT, ParameterDirection.Input);
                    par[8] = new OracleParameter("p_VEND_CD", OracleDbType.Varchar2, LabSampleInfoModel.UName.Trim(), ParameterDirection.Input);

                    var ds = DataAccessDB.ExecuteNonQuery("Vendor_InsertLabSampleInfo", par, 1);
                }
                else
                {
                    OracleParameter[] par = new OracleParameter[9];
                    par[0] = new OracleParameter("p_TestingCharges", OracleDbType.Varchar2, LabSampleInfoModel.NetTesting, ParameterDirection.Input);
                    par[1] = new OracleParameter("p_DocInitDateTime", OracleDbType.Date, ss, ParameterDirection.Input);
                    par[2] = new OracleParameter("p_TDS", OracleDbType.Varchar2, LabSampleInfoModel.TDS, ParameterDirection.Input);
                    par[3] = new OracleParameter("p_UTRNo", OracleDbType.Varchar2, LabSampleInfoModel.UTRNO, ParameterDirection.Input);
                    par[4] = new OracleParameter("p_UTRDate", OracleDbType.Date, UTRDT, ParameterDirection.Input);
                    par[5] = new OracleParameter("p_CaseNo", OracleDbType.Varchar2, LabSampleInfoModel.CaseNo, ParameterDirection.Input);
                    par[6] = new OracleParameter("p_CallRecvDT", OracleDbType.Date, CallDate, ParameterDirection.Input);
                    par[7] = new OracleParameter("p_CallSno", OracleDbType.Varchar2, LabSampleInfoModel.CallSNO, ParameterDirection.Input);
                    par[8] = new OracleParameter("p_VEND_CD", OracleDbType.Varchar2, LabSampleInfoModel.UName, ParameterDirection.Input);


                    var ds = DataAccessDB.ExecuteNonQuery("Vendor_UPDATE_SAMPLE_INFO", par, 1);
                }
                

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
        string dateconcate1(string dt)
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

                OracleParameter[] par = new OracleParameter[9];
                par[0] = new OracleParameter("p_TestingCharges", OracleDbType.Varchar2, LabSampleInfoModel.NetTesting, ParameterDirection.Input);
                par[1] = new OracleParameter("p_DocInitDateTime", OracleDbType.Date, ss, ParameterDirection.Input);
                par[2] = new OracleParameter("p_TDS", OracleDbType.Varchar2, LabSampleInfoModel.TDS, ParameterDirection.Input);
                par[3] = new OracleParameter("p_UTRNo", OracleDbType.Varchar2, LabSampleInfoModel.UTRNO, ParameterDirection.Input);
                par[4] = new OracleParameter("p_UTRDate", OracleDbType.Date, LabSampleInfoModel.UTRDT, ParameterDirection.Input);
                par[5] = new OracleParameter("p_CaseNo", OracleDbType.Varchar2, LabSampleInfoModel.CaseNo, ParameterDirection.Input);
                par[6] = new OracleParameter("p_CallRecvDT", OracleDbType.Date, LabSampleInfoModel.CallRecDt, ParameterDirection.Input);
                par[7] = new OracleParameter("p_CallSno", OracleDbType.Varchar2, LabSampleInfoModel.CallSNO, ParameterDirection.Input);
                par[8] = new OracleParameter("p_VEND_CD", OracleDbType.Varchar2, LabSampleInfoModel.UName, ParameterDirection.Input);


                var ds = DataAccessDB.ExecuteNonQuery("Vendor_UPDATE_SAMPLE_INFO", par, 1);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public string CheckExist(string CaseNo, string CallRdt, string CallSno, string Regin)
        {

            string query = "SELECT COUNT(*) FROM T110_LAB_DOC T110 " +
                  "WHERE TO_CHAR(T110.CALL_RECV_DT, 'dd/mm/yyyy') = '" + CallRdt + "' " +
                  "AND T110.case_no = '" + CaseNo + "' AND T110.call_sno = '" + CallSno + "'";


            string count = GetDateString(query);


            //string nextSerialNumber = (count + 1).ToString();

            return count;
        }


    }
}
