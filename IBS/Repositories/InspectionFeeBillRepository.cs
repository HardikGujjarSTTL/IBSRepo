using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class InspectionFeeBillRepository : IInspectionFeeBillRepository
    {
        private readonly ModelContext context;

        public InspectionFeeBillRepository(ModelContext context)
        {
            this.context = context;
        }

        public InspectionFeeBillModel FindByBillNo(string BillNo)
        {
            InspectionFeeBillModel model = new();
            T22Bill t22Bill = context.T22Bills.Find(BillNo);

            if (t22Bill == null)
                return null;
            else
            {
                model.BillNo = t22Bill.BillNo;
                model.BillDt = t22Bill.BillDt;
                model.CaseNo = t22Bill.CaseNo;
                model.MaxFee = t22Bill.MaxFee ?? 0;
                model.MinFee = t22Bill.MinFee ?? 0;
                model.FeeRate = t22Bill.FeeRate ?? 0;
                model.FeeType = t22Bill.FeeType;
                model.TaxType = string.IsNullOrEmpty(t22Bill.TaxType) ? "X" : t22Bill.TaxType;
                model.MaterialValue = t22Bill.MaterialValue ?? 0;
                model.InspFee = t22Bill.InspFee ?? 0;
                model.ServiceTax = t22Bill.ServiceTax ?? 0;
                model.EduCess = t22Bill.EduCess ?? 0;
                model.SheCess = t22Bill.SheCess ?? 0;
                model.SwachhBharatCess = t22Bill.SwachhBharatCess ?? 0;
                model.KrishiKalyanCess = t22Bill.KrishiKalyanCess ?? 0;
                model.Cgst = t22Bill.Cgst ?? 0;
                model.Sgst = t22Bill.Sgst ?? 0;
                model.Igst = t22Bill.Igst ?? 0;
                model.BillAmount = t22Bill.BillAmount ?? 0;
                model.Tds = t22Bill.Tds ?? 0;
                model.RetentionMoney = t22Bill.RetentionMoney ?? 0;
                model.WriteOffAmt = t22Bill.WriteOffAmt ?? 0;
                model.CnoteAmount = t22Bill.CnoteAmount ?? 0;

                model.AmountCleared = (from t25 in context.T25RvDetails
                                       join t26 in context.T26ChequePostings on new { t25.ChqNo, t25.ChqDt, t25.BankCd } equals new { t26.ChqNo, ChqDt = (DateTime)t26.ChqDt, BankCd = (int)t26.BankCd }
                                       join t94 in context.T94Banks on t25.BankCd equals t94.BankCd
                                       where t26.BillNo == BillNo
                                       select t26.AmountCleared).Sum();


                model.TotalAmountReceived = model.AmountCleared + model.Tds + model.RetentionMoney + model.WriteOffAmt + model.CnoteAmount;
                model.AmountRecievedThruChequeDD = model.AmountCleared;
                model.AmountRecover = model.BillAmount - model.TotalAmountReceived;

                return model;
            }
        }

        public DTResult<BillItemsListModel> GetBillItemsList(DTParameters dtParameters)
        {
            DTResult<BillItemsListModel> dTResult = new() { draw = 0 };
            IQueryable<BillItemsListModel>? query = null;

            string BillNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BillNo"]) ? Convert.ToString(dtParameters.AdditionalValues["BillNo"]) : "";

            query = from bill in context.T23BillItems
                    join uom in context.T04Uoms on bill.UomCd equals uom.UomCd
                    where bill.BillNo == BillNo
                    select new BillItemsListModel
                    {
                        BillNo = bill.BillNo,
                        ItemSrno = bill.ItemSrno ?? 0,
                        ItemDesc = bill.ItemDesc,
                        Qty = bill.Qty,
                        Rate = bill.Rate,
                        UomSDesc = uom.UomSDesc,
                        BasicValue = bill.BasicValue,
                        SalesTaxPer = bill.SalesTaxPer,
                        SalesTax = bill.SalesTax,
                        ExcisePer = bill.ExcisePer,
                        Excise = bill.Excise,
                        DiscountPer = bill.DiscountPer,
                        Discount = bill.Discount,
                        OtherCharges = bill.OtherCharges,
                        Value = bill.Value,
                    };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        public DTResult<ChequeDetailsListModel> GetChequeDetailsList(DTParameters dtParameters)
        {
            DTResult<ChequeDetailsListModel> dTResult = new() { draw = 0 };
            IQueryable<ChequeDetailsListModel>? query = null;

            string BillNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BillNo"]) ? Convert.ToString(dtParameters.AdditionalValues["BillNo"]) : "";

            query = from t25 in context.T25RvDetails
                    join t26 in context.T26ChequePostings on new { t25.ChqNo, t25.ChqDt, t25.BankCd } equals new { t26.ChqNo, ChqDt = (DateTime)t26.ChqDt, BankCd = (int)t26.BankCd }
                    join t94 in context.T94Banks on t25.BankCd equals t94.BankCd
                    where t26.BillNo == BillNo
                    select new ChequeDetailsListModel
                    {
                        BankName = t94.BankName,
                        ChqNo = t25.ChqNo,
                        ChqDt = t25.ChqDt,
                        Amount = t25.Amount,
                        AmountCleared = t26.AmountCleared
                    };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

    }
}
