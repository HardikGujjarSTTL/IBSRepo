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
            T22Bill t22Bill = context.T22Bills.Where(x => x.BillNo == BillNo).FirstOrDefault();

            if (t22Bill == null)
                return null;
            else
            {
                model.BillNo = t22Bill.BillNo;
                model.BillDt = t22Bill.BillDt;
                model.CaseNo = t22Bill.CaseNo;
                model.MaxFee = t22Bill.MaxFee;
                model.MinFee = t22Bill.MinFee;
                model.FeeRate = t22Bill.FeeRate;
                model.FeeType = t22Bill.FeeType;
                model.TaxType = string.IsNullOrEmpty(t22Bill.TaxType) ? "X" : t22Bill.TaxType;

                return model;
            }
        }
    }
}
