using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
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
                        CALL_RECV_DT = Convert.ToString(row["call_recv_dt"]),
                        CALL_SNO = Convert.ToInt32(row["call_sno"]),
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
                || Convert.ToString(w.BARCODE).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CASE_NO).ToLower().Contains(searchBy.ToLower())
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
            var CALL_RECV_DT = Convert.ToDateTime(BarcodeGenerate.CALL_RECV_DT).ToString("MM/dd/yyyy");
            var TARGETED_DATE = Convert.ToDateTime(BarcodeGenerate.TARGETED_DATE).ToString("MM/dd/yyyy");
            var CURRENT_DATE = Convert.ToDateTime(BarcodeGenerate.CURRENT_DATE).ToString("MM/dd/yyyy");


            try
            {
                //string BarCodeNo = GenerateBarCodeNo(BarcodeGenerate);
                OracleParameter[] par = new OracleParameter[19];
                par[0] = new OracleParameter("p_Barcode", OracleDbType.Varchar2, BarcodeGenerate.BARCODE, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CaseNo", OracleDbType.Varchar2, BarcodeGenerate.CASE_NO, ParameterDirection.Input);
                par[2] = new OracleParameter("p_Call_Recv_dt", OracleDbType.Date, CALL_RECV_DT, ParameterDirection.Input);
                par[3] = new OracleParameter("p_CALL_SNO", OracleDbType.Int32, BarcodeGenerate.CALL_SNO, ParameterDirection.Input);
                par[4] = new OracleParameter("p_Item_SRNO_PO", OracleDbType.Varchar2, BarcodeGenerate.ITEM_SRNO_PO, ParameterDirection.Input);
                par[5] = new OracleParameter("p_VendCd", OracleDbType.Varchar2, BarcodeGenerate.VEND_CD, ParameterDirection.Input);
                par[6] = new OracleParameter("p_VendName", OracleDbType.Varchar2, BarcodeGenerate.CUSTOMER_NAME, ParameterDirection.Input);
                par[7] = new OracleParameter("p_SealingType", OracleDbType.Varchar2, BarcodeGenerate.SEALING_TYPE, ParameterDirection.Input);
                par[8] = new OracleParameter("p_CustomerGSTN", OracleDbType.Varchar2, BarcodeGenerate.CUSTOMER_GSTN, ParameterDirection.Input);
                par[9] = new OracleParameter("p_Description", OracleDbType.Varchar2, BarcodeGenerate.DESCRIPTION, ParameterDirection.Input);
                par[10] = new OracleParameter("p_TargetDate", OracleDbType.Date, TARGETED_DATE, ParameterDirection.Input);
                par[11] = new OracleParameter("p_CURRENT_DATE", OracleDbType.Date, CURRENT_DATE, ParameterDirection.Input);
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
            //string date = BarcodeGenerate.CURRENT_DATE.ToString().Substring(0, 10);
            //OracleParameter[] par = new OracleParameter[3];
            //par[0] = new OracleParameter("in_lab_code", OracleDbType.Char, BarcodeGenerate.Region, ParameterDirection.Input);
            //par[1] = new OracleParameter("in_transaction_date", OracleDbType.Date, BarcodeGenerate.CURRENT_DATE, ParameterDirection.Input);
            //par[2] = new OracleParameter("out_transaction_number", OracleDbType.Varchar2, ParameterDirection.Output);
            ////par[3] = new OracleParameter("p_barcode_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            //var ds = DataAccessDB.GetDataSet("GENERATE_Barcode_NO", par, 1);

            //BarcodeGenerate model1 = new();
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{

            //    DataRow row = ds.Tables[0].Rows[0];
            //    model1 = new BarcodeGenerate
            //    {
            //        BARCODE = row["BARCODE"] as string,
            //    };
            //}
            //var Barcode = model1.BARCODE.Trim();
            //return Barcode;
            try
            {
                using (var conn1 = context.Database.GetDbConnection())
                {
                    using (OracleCommand cmd = new OracleCommand("New_BarcodeGenarate", (OracleConnection)conn1))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("lab_code", OracleDbType.Varchar2).Value = BarcodeGenerate.Region;
                        OracleParameter outParam = new OracleParameter("transaction_number", OracleDbType.Varchar2, 255);
                        outParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outParam);
                        conn1.Open();
                        cmd.ExecuteNonQuery();

                        BarcodeGenerate.BARCODE = cmd.Parameters["transaction_number"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return BarcodeGenerate.BARCODE;
            }


            return BarcodeGenerate.BARCODE;
        }
        public DTResult<BarcodeGenerate> LoadCalculation(DTParameters dtParameters)
        {
            string DisID = dtParameters.AdditionalValues?.GetValueOrDefault("DisId");
            DTResult<BarcodeGenerate> dTResult = new() { draw = 0 };

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;

            //var query1 = (from l in context.TestTables
            //              where l.DisciplineId == Convert.ToInt32(DisID)
            //              select new BarcodeGenerate
            //              {
            //                  LABRATEID = Convert.ToString(l.Labrateid),
            //                  TEST_NAME = Convert.ToString(l.TestName),
            //                  PRICE = Convert.ToString(l.Price),
            //                  DISCIPLINE_ID = Convert.ToString(l.DisciplineId),
            //                  QTY = "1"
            //              }).ToList();
            var query1 = (from l in context.Labratemasters
                          where l.DisciplineId == Convert.ToInt32(DisID)
                          select new BarcodeGenerate
                          {
                              LABRATEID = Convert.ToString(l.Labrateid),
                              TEST_NAME = Convert.ToString(l.TestName),
                              PRICE = Convert.ToString(l.Price),
                              DISCIPLINE_ID = Convert.ToString(l.DisciplineId),
                              QTY = "1"
                          }).ToList();

            //var query2 = (from b in query1
            //              select new BarcodeGenerate
            //              {
            //                  LABRATEID = b.LABRATEID,
            //                  TEST_NAME = b.TEST_NAME,
            //                  PRICE = b.PRICE,
            //                  DISCIPLINE_ID = b.DISCIPLINE_ID,
            //                  QTY = b.QTY,
            //                  ExistsField = (
            //                      from lct in context.LabCalTranDetails
            //                      join t1 in context.LabCalTranHeaders on lct.CalTranHeaderId equals t1.CalTranHeaderId
            //                      where
            //                          lct.DisciplineId == Convert.ToInt32(DisID) &&
            //                          lct.Price.ToString() == b.PRICE &&
            //                          t1.BarcodeId == "NR24120001" &&
            //                          lct.CalTranHeaderId.ToString() == "43"
            //                      select 1
            //                  ).Any() ? "1" : "0"
            //              }).ToList();

            var result = query1.ToList();

            dTResult.recordsTotal = query1.Count();
            dTResult.data = query1;
            dTResult.recordsFiltered = query1.Count();
            return dTResult;
        }
        public bool InsertDataForLabTran(BarcodeGenerate BarcodeGenerate)
        {

            try
            {
                string BarCodeNo = GenerateBarCodeNo(BarcodeGenerate);
                BarcodeGenerate.BARCODE = BarCodeNo;
                OracleParameter[] par = new OracleParameter[16];
                par[0] = new OracleParameter("p_Barcode", OracleDbType.Varchar2, BarCodeNo, ParameterDirection.Input);
                par[1] = new OracleParameter("TYPEOF_GST", OracleDbType.Varchar2, BarcodeGenerate.TypeGST, ParameterDirection.Input);
                par[2] = new OracleParameter("SGST", OracleDbType.Varchar2, BarcodeGenerate.SGST, ParameterDirection.Input);
                par[3] = new OracleParameter("CGST", OracleDbType.Varchar2, BarcodeGenerate.CGST, ParameterDirection.Input);
                par[4] = new OracleParameter("IGST", OracleDbType.Varchar2, BarcodeGenerate.IGST, ParameterDirection.Input);
                par[5] = new OracleParameter("TOTAL", OracleDbType.Varchar2, BarcodeGenerate.Total, ParameterDirection.Input);
                par[6] = new OracleParameter("TAX", OracleDbType.Varchar2, BarcodeGenerate.Tax, ParameterDirection.Input);
                par[7] = new OracleParameter("GRANDTOTAL", OracleDbType.Varchar2, BarcodeGenerate.GTotal, ParameterDirection.Input);
                par[8] = new OracleParameter("HANDLING_CHARGE", OracleDbType.Varchar2, BarcodeGenerate.HandlingCharge, ParameterDirection.Input);
                par[9] = new OracleParameter("EXTRA_CHARGE", OracleDbType.Varchar2, BarcodeGenerate.ExtraCharge, ParameterDirection.Input);
                par[10] = new OracleParameter("RTOTAL", OracleDbType.Varchar2, BarcodeGenerate.RTotal, ParameterDirection.Input);
                par[11] = new OracleParameter("p_CREATEDBY", OracleDbType.Varchar2, BarcodeGenerate.CREATEDBY, ParameterDirection.Input);
                par[12] = new OracleParameter("p_CREATEDDATE", OracleDbType.Date, DateTime.Now, ParameterDirection.Input);
                par[13] = new OracleParameter("p_USERID", OracleDbType.Varchar2, BarcodeGenerate.USERID, ParameterDirection.Input);
                par[14] = new OracleParameter("p_IPADDRESS", OracleDbType.Varchar2, BarcodeGenerate.IPADDRESS, ParameterDirection.Input);
                par[15] = new OracleParameter("p_cal_tran_header_id", OracleDbType.Int32, ParameterDirection.Output);

                var ds = DataAccessDB.ExecuteNonQuery("InsertLabTranTable", par, 1);
                string calTranHeaderId = par[15].Value.ToString();
                InsertDataForLabTranDtl(BarcodeGenerate, calTranHeaderId);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public bool InsertDataForLabTranDtl(BarcodeGenerate BarcodeGenerate, string calTranHeaderId)
        {

            //string[] nam = BarcodeGenerate.Names.Split('/');
            //string[] amt = BarcodeGenerate.Amounts.Split('/');
            //string[] qty = BarcodeGenerate.Quantities.Split('/');
            //string[] subt = BarcodeGenerate.Subtotals.Split('/');
            //string[] othv = BarcodeGenerate.Othervalues.Split('/');

            string[] nam1 = BarcodeGenerate.Names.Split(',');
            string[] amt1 = BarcodeGenerate.Amounts.Split(',');
            string[] qty1 = BarcodeGenerate.Quantities.Split(',');
            string[] subt1 = BarcodeGenerate.Subtotals.Split(',');
            string[] othv1 = BarcodeGenerate.Othervalues.Split(',');
            for (int i = 0; i < nam1.Length; i++)
            {

                try
                {
                    if (!string.IsNullOrEmpty(nam1[i].ToString()))
                    {
                        OracleParameter[] par = new OracleParameter[5];
                        par[0] = new OracleParameter("CAL_TRAN_HEADER_ID", OracleDbType.Int32, calTranHeaderId, ParameterDirection.Input);
                        par[1] = new OracleParameter("DISCIPLINE_ID", OracleDbType.Int32, othv1[i], ParameterDirection.Input);
                        par[2] = new OracleParameter("TEST_NAME", OracleDbType.Varchar2, nam1[i], ParameterDirection.Input);
                        par[3] = new OracleParameter("PRICE", OracleDbType.Int32, amt1[i], ParameterDirection.Input);
                        par[4] = new OracleParameter("QTY", OracleDbType.Int32, qty1[i], ParameterDirection.Input);

                        var ds = DataAccessDB.ExecuteNonQuery("InsertLabTranDtlTable", par, 1);
                    }
                }

                catch (Exception ex)
                {
                    return false;
                }

            }
            return true;
        }
        public bool SaveBarcodeGenerated(string Barcode, int quantity, string caseno, int callsno, string calldate, string IPADDRESS, string USERID, string CREATEDBY)
        {
            calldate = Convert.ToDateTime(calldate).ToString("MM/dd/yyyy");
            try
            {
                //string BarCodeNo = GenerateBarCodeNo(BarcodeGenerate);
                OracleParameter[] par = new OracleParameter[9];
                par[0] = new OracleParameter("p_BARCODE_NO", OracleDbType.Varchar2, Barcode, ParameterDirection.Input);
                par[1] = new OracleParameter("p_QTY", OracleDbType.Int32, quantity, ParameterDirection.Input);
                par[2] = new OracleParameter("p_CASE_NO", OracleDbType.Varchar2, caseno, ParameterDirection.Input);
                par[3] = new OracleParameter("p_CALL_SNO", OracleDbType.Int32, callsno, ParameterDirection.Input);
                par[4] = new OracleParameter("p_CALL_DATE", OracleDbType.Date, calldate, ParameterDirection.Input);
                par[5] = new OracleParameter("p_CREATEDDATE", OracleDbType.Date, DateTime.Now, ParameterDirection.Input);
                par[6] = new OracleParameter("p_CREATEDBY", OracleDbType.Varchar2, CREATEDBY, ParameterDirection.Input);
                par[7] = new OracleParameter("p_USERID", OracleDbType.Varchar2, USERID, ParameterDirection.Input);
                par[8] = new OracleParameter("p_IPADDRESS", OracleDbType.Varchar2, IPADDRESS, ParameterDirection.Input);
                var ds = DataAccessDB.ExecuteNonQuery("InsertBarcodeGenerated", par, 1);
            }

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
