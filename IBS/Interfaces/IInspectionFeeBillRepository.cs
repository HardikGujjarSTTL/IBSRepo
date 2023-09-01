using IBS.Models;

namespace IBS.Interfaces
{
    public interface IInspectionFeeBillRepository
    {
        public InspectionFeeBillModel FindByBillNo(string BillNo);

        public DTResult<BillItemsListModel> GetBillItemsList(DTParameters dtParameters);

        public DTResult<ChequeDetailsListModel> GetChequeDetailsList(DTParameters dtParameters);
    }
}
