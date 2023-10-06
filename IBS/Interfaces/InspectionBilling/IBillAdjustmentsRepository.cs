using IBS.Models;

namespace IBS.Interfaces.InspectionBilling
{
    public interface IBillAdjustmentsRepository
    {
        public InspectionCertModel FindByBillDetails(string BillNo, string Region);

        DTResult<InspectionCertModel> GetLoadTableDetails(DTParameters dtParameters, string Region);

        public InspectionCertModel FindByItemID(InspectionCertModel model);

        string UpdateCallDetails(InspectionCertModel model, int ItemSrnoPo);

        public ICPopUpModel FindByBillDetailsPopUp(string BillNo, string Region);

        int financial_year_check(InspectionCertModel model);

        string BillUpdate(InspectionCertModel model, string Region);
    }
}
