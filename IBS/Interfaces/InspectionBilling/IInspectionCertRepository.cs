using IBS.Models;

namespace IBS.Interfaces.InspectionBilling
{
    public interface IInspectionCertRepository
    {
        DTResult<InspectionCertModel> GetDataList(DTParameters dtParameters, string GetRegionCode);

        public InspectionCertModel FindByID(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno, string GetRegionCode);

        public InspectionCertModel FindByInspDetailsID(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno, string ActionType, string GetRegionCode, int RoleId);

        DTResult<InspectionCertModel> GetBillDetails(string BillNo);

        DTResult<InspectionCertModel> GetConsignee(int ConsigneeCd);

        DTResult<InspectionCertModel> GetBPO(string BPOCd);

        int UpdateGSTDetails(InspectionCertModel model, string UserName);

        string InspectionCertSave(InspectionCertModel model, string GetRegionCode);

        string ReturnBillSubmit(InspectionCertModel model, string GetRegionCode);

        string Validation(InspectionCertModel model, string GetRegionCode);

        DTResult<InspectionCertModel> GetLoadTableDetails(DTParameters dtParameters, string GetRegionCode);

        int financial_year_check(InspectionCertModel model);

        string BillUpdate(InspectionCertModel model, string GetRegionCode);

        string BillDateUpdate(InspectionCertModel model, string Region);

        public InspectionCertModel FindByItemID(string CaseNo, DateTime CallRecvDt, int CallSno, int ItemSrnoPo, string Region);

        string UpdateCallDetails(InspectionCertModel model,int ItemSrnoPo, string CaseNo, DateTime CallRecvDt, int CallSno);

        public ICPopUpModel FindByBillDetails(string BillNo, string Region);

        public string DocUpdate(string BillNo, string UserId);
    }
}
