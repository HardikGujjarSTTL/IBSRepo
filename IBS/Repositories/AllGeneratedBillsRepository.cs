using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
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
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "BILL_NO";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "BILL_NO";
                orderAscendingDirection = true;
            }

            DataTable dt = null;

            string FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : "";
            string ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : "";
            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";
            string LOA = !string.IsNullOrEmpty(dtParameters.AdditionalValues["LOA"]) ? Convert.ToString(dtParameters.AdditionalValues["LOA"]) : "";
            string ClientName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ClientName"]) ? Convert.ToString(dtParameters.AdditionalValues["ClientName"]) : "";
            string BPOName = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BPOName"]) ? Convert.ToString(dtParameters.AdditionalValues["BPOName"]) : "";
            string ClientType = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ClientType"]) ? Convert.ToString(dtParameters.AdditionalValues["ClientType"]) : "";

            OracleParameter[] par = new OracleParameter[8];
            par[0] = new OracleParameter("p_start_date", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_end_date", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_region_code", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[3] = new OracleParameter("p_bpo_type", OracleDbType.Varchar2, ClientType, ParameterDirection.Input);
            par[4] = new OracleParameter("p_bpo_rly", OracleDbType.Varchar2, ClientName, ParameterDirection.Input);
            par[5] = new OracleParameter("p_bpo_name", OracleDbType.Varchar2, BPOName, ParameterDirection.Input);
            par[6] = new OracleParameter("P_PO_OR_LETTER", OracleDbType.Varchar2, LOA, ParameterDirection.Input);
            par[7] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GenerateBillDetails", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];

                List<AllGeneratedBills> list = dt.AsEnumerable().Select(row => new AllGeneratedBills
                {
                    BILL_NO = row.Field<string>("BILL_NO"),
                    BILL_DT = row.Field<DateTime>("BILL_DT"),
                    REGION_CODE = row.Field<string>("REGION_CODE"),
                    CLIENT_TYPE = row.Field<string>("CLIENT_TYPE"),
                    CLIENT_NAME = row.Field<string>("CLIENT_NAME"),
                    BPO_NAME = row.Field<string>("BPO_NAME"),
                    LOA = row.Field<string>("LOA"),
                    FileSize = "0",
                }).ToList();

                query = list.AsQueryable();

                dTResult.recordsTotal = ds.Tables[0].Rows.Count;

                if (!string.IsNullOrEmpty(searchBy))
                    query = query.Where(w => Convert.ToString(w.BILL_NO).ToLower().Contains(searchBy.ToLower())
                    );

                dTResult.recordsFiltered = query.Count();

                dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                dTResult.draw = dtParameters.Draw;
            }

            return dTResult;
        }

        public AllGeneratedBills GenerateBill(AllGeneratedBills model)
        {
            if(model.RailwayChk == "true")
            {

            }
            else if(model.CLIENT_NAME != null)
            {

            }
            else
            {

            }
            return model;
        }

        //public bool Remove(int Id, int UserID)
        //{
        //    T94Bank bank = context.T94Banks.Find(Id);

        //    if (bank == null) { return false; }

        //    bank.Isdeleted = 1;
        //    bank.Updatedby = UserID;
        //    bank.Updateddate = DateTime.Now;

        //    context.SaveChanges();
        //    return true;
        //}

    }

}

