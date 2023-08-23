using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IVendorCallRegisterRepository
    {
        public VenderCallRegisterModel FindByID(string CaseNo, string CallRecvDt, int CallSno, string UserName);

        public VenderCallRegisterModel FindByVenderDetail(int MfgCd);

        DTResult<VenderCallRegisterModel> FindByVenderDetail1(int MfgCd);

        DTResult<VenderCallRegisterModel> GetUserList(DTParameters dtParameters,string UserName);

        DTResult<VenderCallRegisterModel> GetVenderList(DTParameters dtParameters, string UserName);

        int DetailsInsertUpdate(VenderCallRegisterModel model);

        string RegiserCallSave(VenderCallRegisterModel model);

        Task<string> send_IE_smsAsync(VenderCallRegisterModel model);

        string send_Vendor_Email(VenderCallRegisterModel model);

        string RegiserCallDelete(VenderCallRegisterModel model);

        public VendorCallRegPrintReport FindByPrintReport(string CaseNo, string CallRecvDt, int CallSno, string UserName);

        DTResult<VenderCallRegisterModel> GetDataListReport(DTParameters dtParameters);
    }
}
