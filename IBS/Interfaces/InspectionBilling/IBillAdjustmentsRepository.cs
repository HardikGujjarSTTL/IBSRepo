using IBS.Models;

namespace IBS.Interfaces.InspectionBilling
{
    public interface IBillAdjustmentsRepository
    {
        public InspectionCertModel FindByBillDetails(string BillNo, string Region);

        DTResult<InspectionCertModel> GetLoadTableDetails(DTParameters dtParameters, string Region);

        public InspectionCertModel FindByItemID(string Caseno, DateTime Callrecvdt, int Callsno, int ItemSrnoPo);

        string UpdateCallDetails(InspectionCertModel model, int ItemSrnoPo);

        public ICPopUpModel FindByBillDetailsPopUp(string BillNo, string Region);

        int financial_year_check(InspectionCertModel model);

        string BillUpdate(InspectionCertModel model, string Region);

        //DTResult<InspectionCertItemListModel> FindByFeesDetails(string Caseno, string Callrecvdt, int Callsno, string Consignee, string BillNo, decimal AdjustmentFee);
        public InspectionCertModel FindByFeesDetails(string Caseno, string Callrecvdt, int Callsno, string Consignee, string BillNo, decimal AdjustmentFee, int ConsigneeCd);
    }
}
