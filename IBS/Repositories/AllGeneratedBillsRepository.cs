using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using MessagePack;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class AllGeneratedBillsRepository : IAllGeneratedBillsRepository
    {
        private readonly ModelContext context;

        public AllGeneratedBillsRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<AllGeneratedBills> GetBillDetails(DTParameters dtParameters)
        {
            DTResult<AllGeneratedBills> dTResult = new() { draw = 0 };
            IQueryable<AllGeneratedBills>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = false;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "BILL_DT";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "BILL_DT";
                orderAscendingDirection = true;
            }

            DataTable dt = null;

            string FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : null;
            string ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : null;
            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : null;
            string LOA = !string.IsNullOrEmpty(dtParameters.AdditionalValues["LOA"]) ? Convert.ToString(dtParameters.AdditionalValues["LOA"]) : null;
            string ClientName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ClientName"]) ? Convert.ToString(dtParameters.AdditionalValues["ClientName"]) : null;
            string BPOName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BPOName"]) ? Convert.ToString(dtParameters.AdditionalValues["BPOName"]) : null;
            string ClientType = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ClientType"]) ? Convert.ToString(dtParameters.AdditionalValues["ClientType"]) : null;

            LOA = LOA == "A" ? null : LOA;

            OracleParameter[] par = new OracleParameter[8];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Date, Convert.ToDateTime(FromDate).Date, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Date, Convert.ToDateTime(ToDate).Date, ParameterDirection.Input);
            par[2] = new OracleParameter("P_BPO_TYPE", OracleDbType.Varchar2, ClientType, ParameterDirection.Input);
            par[3] = new OracleParameter("P_REGION_CODE", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[4] = new OracleParameter("P_BPO_RLY", OracleDbType.Varchar2, ClientName, ParameterDirection.Input);
            par[5] = new OracleParameter("P_PO_OR_LETTER", OracleDbType.Varchar2, LOA, ParameterDirection.Input);
            par[6] = new OracleParameter("P_BPO_CD", OracleDbType.Varchar2, BPOName, ParameterDirection.Input);
            par[7] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_RLY_PAYMENT_FOR_CRIS_GET_BILL", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];

                List<AllGeneratedBills> list = new List<AllGeneratedBills>();
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<AllGeneratedBills>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                query = list.AsQueryable();

                if (!string.IsNullOrEmpty(searchBy))
                    query = query.Where(w => Convert.ToString(w.BILL_NO).ToLower().Contains(searchBy.ToLower())
                    );

                dTResult.recordsTotal = ds.Tables[0].Rows.Count;
                dTResult.recordsFiltered = query.Count();
                dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
                dTResult.draw = dtParameters.Draw;
            }

            return dTResult;
        }

        public AllGeneratedBills CreateBills(AllGeneratedBills model)
        {
            return CreateBillReturnBillDetails(model, "SP_GET_PDFBILL_DETAILS");
        }

        public AllGeneratedBills ReturnBills(AllGeneratedBills model)
        {
            return CreateBillReturnBillDetails(model, "SP_GET_PDFRETURNBILL_DETAILS");
        }

        private AllGeneratedBills CreateBillReturnBillDetails(AllGeneratedBills model, string procedureName)
        {
            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("P_FROMDT", OracleDbType.Varchar2, model.FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODT", OracleDbType.Varchar2, model.ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION_CODE", OracleDbType.Varchar2, model.REGION_CODE, ParameterDirection.Input);
            par[3] = new OracleParameter("P_LOA", OracleDbType.Varchar2, model.LOA, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RAILWAY_RDO", OracleDbType.Varchar2, model.RailwayChk, ParameterDirection.Input);
            par[5] = new OracleParameter("P_CLIENT_NAME", OracleDbType.Varchar2, model.CLIENT_NAME, ParameterDirection.Input);
            par[6] = new OracleParameter("P_CLIENT_TYPE", OracleDbType.Varchar2, model.CLIENT_TYPE, ParameterDirection.Input);
            par[7] = new OracleParameter("P_BPO_NAME", OracleDbType.Varchar2, model.BPO_NAME, ParameterDirection.Input);
            par[8] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet(procedureName, par, 1);
            List<AllGeneratedBills> list = new();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                list = JsonConvert.DeserializeObject<List<AllGeneratedBills>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            model.lstBillDetailsForPDF = list;

            return model;
        }

        public List<ItemsDetail> GetBillItems(string Bill_No)
        {
            List<ItemsDetail> list = new List<ItemsDetail>();
            list = (from vbi in context.V23BillItems
                    where vbi.BillNo == Bill_No
                    select new ItemsDetail
                    {
                        Item_SrNo = vbi.ItemSrno,
                        item_desc = vbi.ItemDesc,
                        qty = vbi.Qty,
                        rate = vbi.Rate,
                        uom_s_desc = vbi.UomSDesc,
                        uom_factor = vbi.UomFactor,
                        basic_value = vbi.BasicValue,
                        Value = vbi.Value
                    }).ToList();
            return list;
        }

        public List<T22Bill> GetBillByBillNo(string Bill_No)
        {
            var Bills = context.T22Bills
                      .Where(b => b.BillNo == Bill_No)
                      .ToList();

            return Bills;
        }

        public string UpdateBillCount(string Bill_No,int count)
        {
            var billsToUpdate = context.T22Bills.Where(b => b.BillNo == Bill_No).ToList();

            foreach (var bill in billsToUpdate)
            {
                bill.BillResentStatus = "S";
                bill.BillResentCount = Convert.ToBoolean(count);
            }
            string msg = "Update Successfull !!";

            return msg;
        }

        public string UpdateGEN_Bill_Date(string Bill_No) 
        {
            var billsToUpdate = context.T22Bills.Where(b => b.BillNo == Bill_No).ToList();

            foreach (var bill in billsToUpdate)
            {
                bill.DigBillGenDt = DateTime.Now.Date;
            }
            string msg = "Update Successfull !!";

            return msg;
        }
    }
}

