using IBS.Models;

namespace IBS.Interfaces.InspectionBilling
{
    public interface IBillAdjustmentsRepository
    {
        public InspectionCertModel FindByBillDetails(string BillNo, string Region);

        public InspectionCertModel FindByItemID(string CaseNo, DateTime CallRecvDt, int CallSno, int ItemSrnoPo, string Region);
    }
}
