using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IBS.Repositories
{
    public class SearchPaymentRepository : ISearchPaymentsRepository
    {
        private readonly ModelContext context;

        public SearchPaymentRepository(ModelContext context)
        {
            this.context = context;
        }


        public DTResult<SearchPaymentsModel> GetSearchPayment(DTParameters dtParameters , string AMOUNT , string CASE_NO , string CHQ_NO , string BankNameDropdown , string NARRATION,string VCHR_DT,string ACC_CD , string region , int Role)
            {
            DTResult<SearchPaymentsModel> dTResult = new() { draw = 0 };
            IQueryable<SearchPaymentsModel>? query = null;


            query = (from t25 in context.T25RvDetails
                     join t24 in context.T24Rvs on t25.VchrNo equals t24.VchrNo
                     join t94 in context.T94Banks on t25.BankCd equals t94.BankCd
                     from b in context.T12BillPayingOfficers.Where(x => t25.BpoCd != null && x.BpoCd == t25.BpoCd).DefaultIfEmpty()
                     where t25.CaseNo == CASE_NO || t25.Amount == Convert.ToDecimal(AMOUNT) || t25.ChqNo == CHQ_NO

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
                query = query; // Set the base query if no filter is applied
            }

            if (!string.IsNullOrEmpty(CHQ_NO))
            {
                query = query.Where(t => t.CHQ_NO.Trim().ToUpper().StartsWith(CHQ_NO.Trim().ToUpper()));
            }

            // Add similar conditions for other parameters

            if (!string.IsNullOrEmpty(CASE_NO))
            {
                query = query.Where(t => t.CASE_NO == CASE_NO);
            }

            var results = query.ToList();
            dTResult.recordsTotal = query.Count();
            dTResult.data = results;
            dTResult.recordsFiltered = query.Count();
            return dTResult;
        }
    }
}
