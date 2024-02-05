using IBS.Models;

namespace IBS.Interfaces.InspectionBilling
{
    public interface ICallRegisterIBRepository
    {
        DTResult<VenderCallRegisterModel> GetDataList(DTParameters dtParameters, string GetRegionCode);

        public VenderCallRegisterModel FindByID(string CaseNo, DateTime? CallRecvDt, string CallSno, string GetRegionCode);

        public VenderCallRegisterModel GetUpdateIC(string CaseNo, DateTime? CallRecvDt, string CallSno);

        DTResult<VenderCallRegisterModel> FindByModifyDetail(string CaseNo, string CallRecvDt, int CallSno, string GetRegionCode);

        DTResult<VenderCallRegisterModel> FindMatchDetail(string CaseNo, string CallRecvDt, int CallSno, string GetRegionCode);

        public string GetRegionValue(string CaseNo, string CallRecvDt, string CallSno);

        public VenderCallRegisterModel FindByManageID(string CaseNo, DateTime? CallRecvDt, int CallSno, string ActionType, string Region);

        DTResult<VenderCallRegisterModel> FindByVenderDetail1(int MfgCd, string CaseNo);

        string RegiserCallSave(VenderCallRegisterModel model);

        Task<string> send_IE_smsAsync(VenderCallRegisterModel model);

        string send_Vendor_Email(VenderCallRegisterModel model);

        string RegiserCallDelete(VenderCallRegisterModel model);

        public VenderCallRegisterModel FindAddDetails(string CaseNo);

        public string GetMatch(string CaseNo, string GetRegionCode);

        public int show2(string CaseNo);

        public string GetCaseNoFind(string CaseNo, string CallRecvDt, int CallSno);

        public VenderCallCancellationModel CancelCallFindByID(string CaseNo, string CallRecvDt, int CallSno, string ActionType);

        public VenderCallCancellationModel CancelCallFindByIDM(string CaseNo, string CallRecvDt, int CallSno, string ActionType);

        string CallCancellationSave(VenderCallCancellationModel model, string UserName);

        string CallCancelDelete(string CaseNo, string CallRecvDt, int CallSno);

        public VenderCallStatusModel FindCallStatus(string CaseNo, DateTime? CallRecvDt, int CallSno, int IE_CD);

        string Save(VenderCallStatusModel model, List<APPDocumentDTO> DocumentsList);

        public VendrorCallDetailsModel CallDetailsFindByID(string CaseNo, string CallRecvDt, int CallSno, int ItemSrNoPo);

        DTResult<VendrorCallDetailsModel> GetCallDetailsList(DTParameters dtParameters);

        int CallDetailsSave(VendrorCallDetailsModel model, string UserName);

        public VenderCallStatusModel CallStatusFilesSave(VenderCallStatusModel model, List<APPDocumentDTO> DocumentsList);

        public VenderCallStatusModel CallCancellationSave(VenderCallStatusModel model, List<APPDocumentDTO> DocumentsList);

        public VenderCallStatusModel RefreshAllDlt(VenderCallStatusModel model);

        public VenderCallStatusModel CallStatusUploadSave(VenderCallStatusModel model, List<APPDocumentDTO> DocumentsList);

        public VenderCallStatusModel CallStatusAcceptRej(VenderCallStatusModel model);

        bool CallDetailsRemove(VendrorCallDetailsModel model);

        public VenderCallStatusModel GetBkNoAndSetNoByConsignee(string CaseNo, DateTime? DesireDt, int CallSno, VenderCallStatusModel model, int selectedConsigneeCd, int IE_CD);

        public VenderCallStatusModel GetCancelChargeByStatus(string CaseNo, DateTime? DesireDt, int CallSno, string selectedValue);

        public VenderCallStatusModel GetRlyDrp(string CaseNo, DateTime? DesireDt, int CallSno, string selectedValue, string IeCd, string Region);

        public VenderCallStatusModel GetLocalOutstation(string CaseNo, DateTime? DesireDt, int CallSno, string selectedValue);
        bool SaveRPTPRMInspectionCertificate(string CASE_NO, string CALL_RECV_DT, string CALL_SNO, string CONSIGNEE_CD);
    }
}
