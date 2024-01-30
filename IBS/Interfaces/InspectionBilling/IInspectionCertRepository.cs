using IBS.Models;

namespace IBS.Interfaces.InspectionBilling
{
    public interface IInspectionCertRepository
    {
        DTResult<InspectionCertModel> GetDataList(DTParameters dtParameters, string Region);

        public InspectionCertModel FindByID(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno, string Region);

        public InspectionCertModel FindByInspDetailsID(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno, string ActionType, string GetRegionCode, int RoleId);

        DTResult<InspectionCertModel> GetBillDetails(string BillNo);

        DTResult<InspectionCertModel> GetConsignee(int ConsigneeCd);

        DTResult<InspectionCertModel> GetBPO(string BPOCd);

        int UpdateGSTDetails(InspectionCertModel model, string UserName);

        string InspectionCertSave(InspectionCertModel model, string Region);

        string ReturnBillSubmit(InspectionCertModel model, string Region);

        string Validation(InspectionCertModel model, string Region);

        DTResult<InspectionCertModel> GetLoadTableDetails(DTParameters dtParameters, string Region);

        int financial_year_check(InspectionCertModel model);

        string BillUpdate(InspectionCertModel model, string Region);

        string BillDateUpdate(InspectionCertModel model, string Region);

        public InspectionCertModel FindByItemID(string CaseNo, DateTime CallRecvDt, int CallSno, int ItemSrnoPo, string Region);

        string UpdateCallDetails(InspectionCertModel model, int ItemSrnoPo, string CaseNo, DateTime CallRecvDt, int CallSno);

        public ICPopUpModel FindByBillDetails(string BillNo, string CaseNo, DateTime CallRecvDt, int CallSno, string Region);

        public string DocUpdate(string BillNo, string UserId);

        public InspectionCertModel FindByCallMaterialReadiness(string CaseNo, DateTime CallRecvDt, int CallSno, string Region);

        public InspectionCertModel GetChangeConsigneeDetails(string CaseNo, string Bkno, string Setno, string ActionType, string Region);

        public int SaveChangeConsignee(InspectionCertModel model);

        public InspectionCertModel GetReturned_Bills_ChangesDetails(string CaseNo, string Bkno, string Setno, string ActionType, string Region);

        public int SaveReturned_Bills_Changes(InspectionCertModel model);

        public InspectionCertModel GetLocalOutstation(string CaseNo, DateTime? DesireDt, int CallSno, string selectedValue);
    }
}
