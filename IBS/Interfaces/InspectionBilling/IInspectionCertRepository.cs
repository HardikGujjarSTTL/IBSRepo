using IBS.Models;

namespace IBS.Interfaces.InspectionBilling
{
    public interface IInspectionCertRepository
    {
        DTResult<InspectionCertModel> GetDataList(DTParameters dtParameters, string GetRegionCode);

        public InspectionCertModel FindByID(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno, string GetRegionCode);

        public InspectionCertModel FindByInspDetailsID(string CaseNo, DateTime? CallRecvDt, int CallSno, string Bkno, string Setno,string ActionType, string GetRegionCode,int RoleId);

        DTResult<InspectionCertModel> GetBillDetails(string BillNo);

        DTResult<InspectionCertModel> GetConsignee(int ConsigneeCd);

        DTResult<InspectionCertModel> GetBPO(string BPOCd);

        int UpdateGSTDetails(InspectionCertModel model, string UserName);

        string InspectionCertSave(InspectionCertModel model,string GetRegionCode);

        string ReturnBillSubmit(InspectionCertModel model, string GetRegionCode);
    }
}
