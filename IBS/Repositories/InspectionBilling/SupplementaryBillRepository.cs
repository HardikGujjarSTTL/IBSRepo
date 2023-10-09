using IBS.DataAccess;
using IBS.Interfaces.InspectionBilling;
using IBS.Models;

namespace IBS.Repositories.InspectionBilling
{
    public class SupplementaryBillRepository : ISupplementaryBillRepository
    {
        private readonly ModelContext context;

        public SupplementaryBillRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<BillDetailsModel> GetLoadTable(DTParameters dtParameters, string Region)
        {
            DTResult<BillDetailsModel> dTResult = new() { draw = 0 };
            IQueryable<BillDetailsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "";
            DateTime? FromDt = null, ToDt = null;
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDt"]))
            {
                FromDt = Convert.ToDateTime(dtParameters.AdditionalValues["FromDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDt"]))
            {
                ToDt = Convert.ToDateTime(dtParameters.AdditionalValues["ToDt"]);
            }
            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();


            query = from t22 in context.T22Bills
                    where t22.CaseNo == CaseNo
                    && (t22.BillDt >= Convert.ToDateTime(FromDt) || t22.BillDt != null)
                    && (t22.BillDt <= Convert.ToDateTime(ToDt) || t22.BillDt != null)

                    select new BillDetailsModel
                    {
                        CaseNo = t22.CaseNo,
                        BillNo = t22.BillNo,
                        FromDt = FromDt,
                        ToDt = ToDt,
                        MaterialValue = t22.MaterialValue,
                        FeeRate = t22.FeeRate,
                        InspFee = t22.InspFee,
                        BillAmount = t22.BillAmount,
                        AmountReceived = t22.AmountReceived,
                        BillAmtCleared = t22.BillAmtCleared,
                        InvoiceNo = t22.InvoiceNo,
                        CnoteBillNo = t22.CnoteBillNo,
                        CnoteAmount = t22.CnoteAmount,
                        Billadtype = t22.Billadtype,

                        MaxFee = t22.MaxFee,
                        MinFee = t22.MinFee,
                        Sgst = t22.Sgst,
                        Cgst = t22.Cgst,
                        Igst = t22.Igst,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.BillNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
