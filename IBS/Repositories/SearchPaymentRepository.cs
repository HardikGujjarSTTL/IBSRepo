using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class SearchPaymentRepository : ISearchPaymentsRepository
    {
        private readonly ModelContext context;

        public SearchPaymentRepository(ModelContext context)
        {
            this.context = context;
        }


        public DTResult<SearchPaymentsModel> GetSearchPayment(DTParameters dtParameters, string Region, int Role)
        {
            DTResult<SearchPaymentsModel> dTResult = new() { draw = 0 };
            IQueryable<SearchPaymentsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "BANK_NAME";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "BANK_NAME";
                orderAscendingDirection = true;
            }

            string AMOUNT = !string.IsNullOrEmpty(dtParameters.AdditionalValues["AMOUNT"]) ? Convert.ToString(dtParameters.AdditionalValues["AMOUNT"]) : "";
            string NARRATION = !string.IsNullOrEmpty(dtParameters.AdditionalValues["NARRATION"]) ? Convert.ToString(dtParameters.AdditionalValues["NARRATION"]) : "";
            string CASE_NO = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CASE_NO"]) ? Convert.ToString(dtParameters.AdditionalValues["CASE_NO"]) : "";
            string BANK_NAME = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BANK_NAME"]) ? Convert.ToString(dtParameters.AdditionalValues["BANK_NAME"]) : "";
            string CHQ_NO = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CHQ_NO"]) ? Convert.ToString(dtParameters.AdditionalValues["CHQ_NO"]) : "";
            string CHQ_DT = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CHQ_DT"]) ? Convert.ToString(dtParameters.AdditionalValues["CHQ_DT"]) : "";
            //string VCHR_DT = !string.IsNullOrEmpty(dtParameters.AdditionalValues["VCHR_DT"]) ? Convert.ToString(dtParameters.AdditionalValues["VCHR_DT"]) : null;
            //string ACC_CD = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ACC_CD"]) ? Convert.ToString(dtParameters.AdditionalValues["ACC_CD"]) : null;

            //query = (from t25 in context.T25RvDetails
            //         join t24 in context.T24Rvs on t25.VchrNo equals t24.VchrNo
            //         join t94 in context.T94Banks on t25.BankCd equals t94.BankCd
            //         from b in context.T12BillPayingOfficers.Where(x => t25.BpoCd != null && x.BpoCd == t25.BpoCd).DefaultIfEmpty()
            //         where
            //         //t25.CaseNo == CASE_NO 
            //         //|| t25.Amount == Convert.ToDecimal(AMOUNT) 
            //         //|| t25.ChqNo == CHQ_NO
            //         (CASE_NO == null || CASE_NO == "" || t25.CaseNo == CASE_NO)
            //         || (Convert.ToDecimal(AMOUNT) == null || Convert.ToDecimal(AMOUNT) == 0 || t25.Amount == Convert.ToDecimal(AMOUNT))
            //         || (CHQ_NO == null || CHQ_NO == "" || t25.ChqNo == CHQ_NO)
            //         || (BANK_NAME == null || BANK_NAME == "" || t25.BankCd == Convert.ToInt32(BANK_NAME))
            //         || (NARRATION == null || NARRATION == "" || t25.Narration.ToLower().Contains(NARRATION.ToLower()))
            //         || (CHQ_DT == null || CHQ_DT == "" || t25.ChqDt == Convert.ToDateTime(CHQ_DT))

            //         select new SearchPaymentsModel
            //         {
            //             VCHR_NO = t24.VchrNo,
            //             VCHR_DT = Convert.ToString(t24.VchrDt),
            //             SNO = Convert.ToInt32(t25.Sno),
            //             BANK_CD = Convert.ToString(t25.BankCd),

            //             BANK_NAME = t94.BankName,
            //             CHQ_NO = t25.ChqNo,
            //             CHQ_DT = t25.ChqDt.ToString("dd/MM/yyyy"),
            //             AMOUNT = t25.Amount ?? 0,
            //             AMOUNT_ADJUSTED = t25.AmountAdjusted ?? 0,
            //             SUSPENSE_AMT = t25.SuspenseAmt ?? 0,
            //             AMT_TRANSFERRED = t25.AmtTransferred ?? 0,
            //             BPO = t25.BpoCd != null ? $"{b.BpoName}/{b.BpoAdd}/{b.BpoRly}" : "",
            //             CASE_NO = t25.CaseNo,
            //             NARRATION = t25.Narration,
            //             ACC_CD = Convert.ToString(t25.AccCd)
            //         });

            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_Case_No", OracleDbType.Varchar2, CASE_NO, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Amount", OracleDbType.Varchar2, AMOUNT, ParameterDirection.Input);
            par[2] = new OracleParameter("p_Chq_No", OracleDbType.Varchar2, CHQ_NO, ParameterDirection.Input);
            par[3] = new OracleParameter("p_Chq_Dt", OracleDbType.Varchar2, CHQ_DT, ParameterDirection.Input);
            par[4] = new OracleParameter("p_Bank_Name", OracleDbType.Varchar2, BANK_NAME, ParameterDirection.Input);
            par[5] = new OracleParameter("p_Narration", OracleDbType.Varchar2, NARRATION, ParameterDirection.Input);
            par[6] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("GET_SEARCH_PAYMENT_DETAILS", par, 1);

            SearchPaymentsModel model = new();
            List<SearchPaymentsModel> LstNew = new();
            //if (ds != null && ds.Tables.Count > 0)
            //{
            //    string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            //    model = JsonConvert.DeserializeObject<List<SearchPaymentsModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
            //}

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                LstNew = JsonConvert.DeserializeObject<List<SearchPaymentsModel>>(serializeddt, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                LstNew.ToList().ForEach(i =>
                {
                    i.CASE_NO = Convert.ToString(i.CASE_NO);
                    i.BANK_CD = i.BANK_CD;
                    i.VCHR_NO = i.VCHR_NO;
                    i.VCHR_DT = i.VCHR_DT;
                    i.SNO = i.SNO;
                    i.BANK_NAME = i.BANK_NAME;
                    i.CHQ_NO = i.CHQ_NO;
                    i.CHQ_DT = i.CHQ_DT;
                    i.AMOUNT = i.AMOUNT;
                    i.AMOUNT_ADJUSTED = i.AMOUNT_ADJUSTED;
                    i.SUSPENSE_AMT = i.SUSPENSE_AMT;
                    i.AMT_TRANSFERRED = i.AMT_TRANSFERRED;
                    i.BPO = i.BPO;
                    i.CASE_NO = i.CASE_NO;
                    i.NARRATION = i.NARRATION;
                    i.ACC_CD = i.ACC_CD;
                });
            }


            query = LstNew.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => (w.BANK_NAME != null && w.BANK_NAME.ToLower().Contains(searchBy.ToLower()))
                                                || (w.CHQ_NO != null && w.CHQ_NO.ToLower().Contains(searchBy.ToLower()))
                                                || (w.CHQ_DT != null && w.CHQ_DT.ToLower().Contains(searchBy.ToLower()))
                                                || (w.CASE_NO != null && w.CASE_NO.ToLower().Contains(searchBy.ToLower()))
                                                || (w.NARRATION != null && w.NARRATION.ToLower().Contains(searchBy.ToLower()))
                                           );

            dTResult.recordsFiltered = query.Count();
            if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;

            //if (!string.IsNullOrEmpty(BANK_NAME))
            //{
            //    query = query.Where(t => t.BANK_CD == BANK_NAME);
            //}
            //else
            //{
            //    query = query; // Set the base query if no filter is applied
            //}

            //if (!string.IsNullOrEmpty(CHQ_NO))
            //{
            //    query = query.Where(t => t.CHQ_NO.Trim().ToUpper().StartsWith(CHQ_NO.Trim().ToUpper()));
            //}

            //// Add similar conditions for other parameters

            //if (!string.IsNullOrEmpty(CASE_NO))
            //{
            //    query = query.Where(t => t.CASE_NO == CASE_NO);
            //}

            //var results = query.ToList();
            //dTResult.recordsTotal = query.Count();
            //dTResult.data = results;
            //dTResult.recordsFiltered = query.Count();
            //return dTResult;
        }
    }
}
