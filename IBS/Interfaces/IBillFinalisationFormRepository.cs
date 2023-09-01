using IBS.Models;

namespace IBS.Interfaces
{
    public interface IBillFinalisationFormRepository
    {
        public DTResult<BillFinalisationFormModel> GetBillFinalisationList(DTParameters dtParameters);

        public void UpdateBillFinalisationStatus(string[] BillNos);
    }
}
