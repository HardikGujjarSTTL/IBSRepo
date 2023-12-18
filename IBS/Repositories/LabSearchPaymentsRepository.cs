using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class LabSearchPaymentsRepository : ILabSearchPaymentRepository
    {
        private readonly ModelContext context;

        public LabSearchPaymentsRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<SearchPaymentsModel> GetSearchList(DTParameters dtParameters)
        {
            DTResult<SearchPaymentsModel> dTResult = new() { draw = 0 };
            IQueryable<SearchPaymentsModel>? query = null;
            var CASE_NO = dtParameters.AdditionalValues?.GetValueOrDefault("CASE_NO");
            var AMOUNT = dtParameters.AdditionalValues?.GetValueOrDefault("AMOUNT");
            var CHQ_NO = dtParameters.AdditionalValues?.GetValueOrDefault("CHQ_NO");
            var BankNameDropdown = dtParameters.AdditionalValues?.GetValueOrDefault("BANK_NAME");
            var CHQ_DT = dtParameters.AdditionalValues?.GetValueOrDefault("CHQ_DT");

            query = (from t25 in context.T25RvDetails
                     join t24 in context.T24Rvs on t25.VchrNo equals t24.VchrNo
                     join t94 in context.T94Banks on t25.BankCd equals t94.BankCd
                     from b in context.T12BillPayingOfficers.Where(x => t25.BpoCd != null && x.BpoCd == t25.BpoCd).DefaultIfEmpty()
                    // where t25.CaseNo == CASE_NO || t25.Amount == Convert.ToDecimal(AMOUNT) || t25.ChqNo == CHQ_NO

                     select new SearchPaymentsModel
                     {
                         VCHR_NO = t24.VchrNo,
                         VCHR_DT = Convert.ToString(t24.VchrDt),
                         SNO = Convert.ToInt32(t25.Sno),
                         BANK_CD = Convert.ToString(t25.BankCd),

                         BANK_NAME = t94.BankName,
                         CHQ_NO = t25.ChqNo,
                         CHQ_DT = t25.ChqDt.ToString("dd/MM/yyyy"),
                         AMOUNT = t25.Amount ?? 0,
                         AMOUNT_ADJUSTED = t25.AmountAdjusted ?? 0,
                         SUSPENSE_AMT = t25.SuspenseAmt ?? 0,
                         AMT_TRANSFERRED = t25.AmtTransferred ?? 0,
                         BPO = t25.BpoCd != null ? $"{b.BpoName}/{b.BpoAdd}/{b.BpoRly}" : "",
                         CASE_NO = t25.CaseNo,
                         NARRATION = t25.Narration,
                         ACC_CD = Convert.ToString(t25.AccCd)
                     });

            if (!string.IsNullOrEmpty(BankNameDropdown))
            {
                query = query.Where(t => t.BANK_CD == BankNameDropdown);
            }
            else
            {
                query = query; 
            }

            if (!string.IsNullOrEmpty(CHQ_NO))
            {
                query = query.Where(t => t.CHQ_NO.Trim().ToUpper().StartsWith(CHQ_NO.Trim().ToUpper()));
            }
            
            if (!string.IsNullOrEmpty(AMOUNT))
            {
                query = query.Where(t => t.AMOUNT == Convert.ToDecimal(AMOUNT));
            }
            //if (!string.IsNullOrEmpty(CHQ_DT))
            //{
            //    query = query.Where(t => t.CHQ_DT == CHQ_DT);
            //}


            if (!string.IsNullOrEmpty(CASE_NO))
            {
                query = query.Where(t => t.CASE_NO.Trim().ToUpper().StartsWith(CASE_NO.Trim().ToUpper()));
            }
            var results = query.Take(10).ToList();
            //var results = query.ToList();
            dTResult.recordsTotal = query.Count();
            dTResult.data = results;
            dTResult.recordsFiltered = query.Count();
            return dTResult;
        }
        //public DTResult<Search> GetSearchList(DTParameters dtParameters, string region)
        //{
        //    DTResult<Search> dTResult = new() { draw = 0 };
        //    IQueryable<Search>? query = null;

        //    var searchBy = dtParameters.Search?.Value;
        //    var orderCriteria = string.Empty;
        //    var orderAscendingDirection = true;

        //    if (dtParameters.Order != null)
        //    {
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

        //        if (orderCriteria == "")
        //        {
        //            orderCriteria = "CaseNo";
        //        }
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }
        //    else
        //    {
        //        orderCriteria = "CaseNo";
        //        orderAscendingDirection = true;
        //    }

        //    OracleParameter[] par = new OracleParameter[5];
        //    par[0] = new OracleParameter("p_Region", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
        //    par[1] = new OracleParameter("p_PaymentID", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("PaymentID"), ParameterDirection.Input);
        //    par[2] = new OracleParameter("p_PaymentDT", OracleDbType.Date, labPaymentFormModel.PaymentDt, ParameterDirection.Input);
        //    par[3] = new OracleParameter("p_LabID", OracleDbType.NVarchar2, dtParameters.AdditionalValues?.GetValueOrDefault("Lab"), ParameterDirection.Input);
        //    par[4] = new OracleParameter("p_Cursor", OracleDbType.RefCursor, ParameterDirection.Output);

        //    var ds = DataAccessDB.GetDataSet("GetLabPayments", par, 4);

        //    List<Search> modelList = new List<Search>();
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            Search model = new Search
        //            {
        //                PaymentID = row["PAYMENT_ID"] as string,
        //                PaymentDt = Convert.ToString(row["PAYMENT_DATE"]),
        //                Lab = Convert.ToString(row["LAB"]),
        //                Amount = Convert.ToString(row["AMOUNT"]),
        //            };

        //            modelList.Add(model);
        //        }
        //    }
        //    query = modelList.AsQueryable();

        //    dTResult.recordsTotal = ds.Tables[0].Rows.Count;

        //    if (!string.IsNullOrEmpty(searchBy))
        //        query = query.Where(w => Convert.ToString(w.PONO).ToLower().Contains(searchBy.ToLower())
        //        || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
        //        );

        //    dTResult.recordsFiltered = query.Count();

        //    dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

        //    dTResult.draw = dtParameters.Draw;

        //    return dTResult;
        //}
    }
}

