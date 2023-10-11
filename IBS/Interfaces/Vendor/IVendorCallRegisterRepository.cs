using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IVendorCallRegisterRepository
    {
        public VenderCallRegisterModel FindByID(string ActionType, string CaseNo, DateTime? CallRecvDt,int CallSno, string FOS, string UserName);

        public VenderCallRegisterModel FindByVenderDetail(int MfgCd);

        DTResult<VenderCallRegisterModel> FindByVenderDetail1(int MfgCd);

        //DTResult<VenderCallRegisterModel> FindByAddDetails(string CaseNo,int UserId);

        DTResult<VenderCallRegisterModel> GetUserList(DTParameters dtParameters, string UserName);

        DTResult<VenderCallRegisterModel> GetVenderListM(DTParameters dtParameters, string UserName);

        DTResult<VenderCallRegisterModel> GetVenderListA(DTParameters dtParameters, string UserName);

        int DetailsInsertUpdate(VenderCallRegisterModel model);

        string RegiserCallSave(VenderCallRegisterModel model);

        public VenderCallRegisterModel GetValidate(VenderCallRegisterModel model);

        Task<string> send_IE_smsAsync(VenderCallRegisterModel model);

        string send_Vendor_Email(VenderCallRegisterModel model);

        string RegiserCallDelete(VenderCallRegisterModel model);

        public VendorCallRegPrintReport FindByPrintReport(string CaseNo, string CallRecvDt, int CallSno, string UserName);

        DTResult<VenderCallRegisterModel> GetDataListReport(DTParameters dtParameters);

        public VenderCallRegisterModel FindByAddDetails(string CaseNo, DateTime? CallRecvDt, string CallStage, int UserId);

        string GetMatch(string CaseNo, string UserName);

        public VenderCallRegisterModel FindByItemID(VenderCallRegisterModel model);

        string UpdateCallDetails(VenderCallRegisterModel model, int ItemSrnoPo);

        int GetItemList(string CaseNo);
    }
}
