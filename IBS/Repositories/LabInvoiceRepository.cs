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
    public class LabInvoiceRepository : ILabInvoiceRepository
    {
        private readonly ModelContext context;

        public LabInvoiceRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<LabInvoiceReportModel> GetLabInvoice(DTParameters dtParameters)
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
    }
}

