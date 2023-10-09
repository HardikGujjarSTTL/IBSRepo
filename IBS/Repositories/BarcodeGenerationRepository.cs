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
using System.Text;
using static IBS.Helper.Enums;
namespace IBS.Repositories
{
    public class BarcodeGenerationRepository : IBarcodeGeneration
    {
        private readonly ModelContext context;

        public BarcodeGenerationRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<BarcodeGenerate> GetBarcodeData(DTParameters dtParameters)
        {

            DTResult<BarcodeGenerate> dTResult = new() { draw = 0 };
            IQueryable<BarcodeGenerate>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {

                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "BARCODE";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {

                orderCriteria = "BARCODE";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[1];

            par[0] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetBarcodeData", par, 1);

            List<BarcodeGenerate> modelList = new List<BarcodeGenerate>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    BarcodeGenerate model = new BarcodeGenerate
                    {
                        BARCODE = Convert.ToString(row["BARCODE"]),
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CURRENT_DATE = Convert.ToString(row["CURRENT_DATE"]),
                        INSPECTOR_CUSTOMER = Convert.ToString(row["INSPECTOR_CUSTOMER"]),
                        DESCRIPTION = Convert.ToString(row["DESCRIPTION"]),
                        SEALING_TYPE = Convert.ToString(row["SEALING_TYPE"]),
                        TotalRate = Convert.ToString(row["RATE"]),
                        TARGETED_DATE = Convert.ToString(row["TARGETED_DATE"]),
                        GSTAmount = Convert.ToString(row["RTAX"]),

                    };

                    modelList.Add(model);
                }
            }



            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.INSPECTOR_CUSTOMER).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.INSPECTOR_CUSTOMER).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;


        }
        public DTResult<BarcodeGenerate> CaseNoSearch(DTParameters dtParameters)
        {

            DTResult<BarcodeGenerate> dTResult = new() { draw = 0 };
            IQueryable<BarcodeGenerate>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {

                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CASE_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {

                orderCriteria = "CASE_NO";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[2];

            par[0] = new OracleParameter("p_CaseNo", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("CaseNo"), ParameterDirection.Input);
            par[1] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetCaseNoSearch", par, 1);

            List<BarcodeGenerate> modelList = new List<BarcodeGenerate>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    BarcodeGenerate model = new BarcodeGenerate
                    {
                        VEND_CD = Convert.ToString(row["VEND_CD"]),
                        CUSTOMER_NAME = Convert.ToString(row["VEND_NAME"]),
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DT = Convert.ToString(row["CALL_RECV_DT"]),
                        CALL_SNO = Convert.ToInt32(row["CALL_SNO"]),
                        ITEM_SRNO_PO = Convert.ToString(row["ITEM_SRNO_PO"]),
                        DESCRIPTION = Convert.ToString(row["ITEM_DESC_PO"]),
                        IE_CD = Convert.ToString(row["IE_CD"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        CUSTOMER_GSTN = Convert.ToString(row["vend_gstno"]),

                    };

                    modelList.Add(model);
                }
            }

            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.INSPECTOR_CUSTOMER).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.INSPECTOR_CUSTOMER).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;


        }
        public bool SaveBarCode(BarcodeGenerate BarcodeGenerate, string IPADDRESS)
        {
            //var CallDt = Convert.ToDateTime(SuperSurpirseFormModel.CallDt).ToString("MM/dd/yyyy");
            //var SuperDt = Convert.ToDateTime(SuperSurpirseFormModel.SuperSurpriseDt).ToString("MM/dd/yyyy");

            try
            {
                string BarCodeNo = GenerateBarCodeNo(BarcodeGenerate);
                OracleParameter[] par = new OracleParameter[19];
                par[0] = new OracleParameter("p_Barcode", OracleDbType.Varchar2, BarCodeNo, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CaseNo", OracleDbType.Varchar2, BarcodeGenerate.CASE_NO, ParameterDirection.Input);
                par[2] = new OracleParameter("p_Call_Recv_dt", OracleDbType.Date, BarcodeGenerate.CALL_RECV_DT, ParameterDirection.Input);
                par[3] = new OracleParameter("p_CALL_SNO", OracleDbType.Int32, BarcodeGenerate.CALL_SNO, ParameterDirection.Input);
                par[4] = new OracleParameter("p_Item_SRNO_PO", OracleDbType.Varchar2, BarcodeGenerate.ITEM_SRNO_PO, ParameterDirection.Input);
                par[5] = new OracleParameter("p_VendCd", OracleDbType.Varchar2, BarcodeGenerate.VEND_CD, ParameterDirection.Input);
                par[6] = new OracleParameter("p_VendName", OracleDbType.Varchar2, BarcodeGenerate.CUSTOMER_NAME, ParameterDirection.Input);
                par[7] = new OracleParameter("p_SealingType", OracleDbType.Varchar2, BarcodeGenerate.SEALING_TYPE, ParameterDirection.Input);
                par[8] = new OracleParameter("p_CustomerGSTN", OracleDbType.Varchar2, BarcodeGenerate.CUSTOMER_GSTN, ParameterDirection.Input);
                par[9] = new OracleParameter("p_Description", OracleDbType.Varchar2, BarcodeGenerate.DESCRIPTION, ParameterDirection.Input);
                par[10] = new OracleParameter("p_TargetDate", OracleDbType.Date, BarcodeGenerate.TARGETED_DATE, ParameterDirection.Input);
                par[11] = new OracleParameter("p_CURRENT_DATE", OracleDbType.Date, BarcodeGenerate.CURRENT_DATE, ParameterDirection.Input);
                par[12] = new OracleParameter("p_INSPECTOR_CUSTOMER", OracleDbType.Varchar2, BarcodeGenerate.INSPECTOR_CUSTOMER, ParameterDirection.Input);
                par[13] = new OracleParameter("p_CREATEDBY", OracleDbType.Varchar2, BarcodeGenerate.CREATEDBY, ParameterDirection.Input);
                par[14] = new OracleParameter("p_CREATEDDATE", OracleDbType.Date, DateTime.Now, ParameterDirection.Input);
                par[15] = new OracleParameter("p_USERID", OracleDbType.Varchar2, BarcodeGenerate.USERID, ParameterDirection.Input);
                par[16] = new OracleParameter("p_IPADDRESS", OracleDbType.Varchar2, IPADDRESS, ParameterDirection.Input);
                par[17] = new OracleParameter("p_RATE", OracleDbType.Varchar2, BarcodeGenerate.TotalRate, ParameterDirection.Input);
                par[18] = new OracleParameter("p_RTAX", OracleDbType.Varchar2, BarcodeGenerate.GSTAmount, ParameterDirection.Input);

                var ds = DataAccessDB.ExecuteNonQuery("InsertBarcodeData", par, 1);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public string GenerateBarCodeNo(BarcodeGenerate BarcodeGenerate)
        {
            string date = BarcodeGenerate.CURRENT_DATE.ToString().Substring(0, 10);
            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Char, BarcodeGenerate.Region, ParameterDirection.Input);
            par[1] = new OracleParameter("IN_Current_DT", OracleDbType.Varchar2, BarcodeGenerate.CURRENT_DATE, ParameterDirection.Input);
            par[2] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GENERATE_Barcode_NO", par, 1);

            BarcodeGenerate model1 = new();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                DataRow row = ds.Tables[0].Rows[0];
                model1 = new BarcodeGenerate
                {
                    BARCODE = row["BARCODE"] as string,
                };
            }
            var Barcode = model1.BARCODE.Trim();
            return Barcode;
        }
        public DTResult<BarcodeGenerate> LoadCalculation(DTParameters dtParameters)
        {
            string DisID = dtParameters.AdditionalValues?.GetValueOrDefault("DisId");
            DTResult<BarcodeGenerate> dTResult = new() { draw = 0 };
            IQueryable<BarcodeGenerate>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            var query1 = (from l in context.TestTables
                          where l.DisciplineId == Convert.ToInt32(DisID)
                          select new BarcodeGenerate
                          {
                              LABRATEID = Convert.ToString(l.Labrateid),
                              TEST_NAME = Convert.ToString(l.TestName),
                              PRICE = Convert.ToString(l.Price),
                              QTY = "1"
                          }).ToList();

            var result = query1.ToList();

            dTResult.recordsTotal = query1.Count();
            dTResult.data = query1;
            dTResult.recordsFiltered = query1.Count();
            return dTResult;
        }
    }
}
