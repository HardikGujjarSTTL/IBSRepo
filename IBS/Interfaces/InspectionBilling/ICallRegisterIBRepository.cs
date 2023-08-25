using IBS.Models;

namespace IBS.Interfaces.InspectionBilling
{
    public interface ICallRegisterIBRepository
    {
        DTResult<VenderCallRegisterModel> GetDataList(DTParameters dtParameters, string GetRegionCode);

        public VenderCallRegisterModel FindByID(string CaseNo, string CallRecvDt, string CallSno, string GetRegionCode);

        DTResult<VenderCallRegisterModel> FindByModifyDetail(string CaseNo, string CallRecvDt, int CallSno, string GetRegionCode);

        DTResult<VenderCallRegisterModel> FindMatchDetail(string CaseNo, string CallRecvDt, int CallSno, string GetRegionCode);

        public string GetRegionValue(string CaseNo, string CallRecvDt, string CallSno);

        public VenderCallRegisterModel FindByManageID(string CaseNo, string CallRecvDt, int CallSno,string ActionType, string UserName);

        DTResult<VenderCallRegisterModel> FindByVenderDetail1(int MfgCd,string CaseNo);

        string RegiserCallSave(VenderCallRegisterModel model);

        Task<string> send_IE_smsAsync(VenderCallRegisterModel model);

        string send_Vendor_Email(VenderCallRegisterModel model);

        string RegiserCallDelete(VenderCallRegisterModel model);

        public VenderCallRegisterModel FindAddDetails(string CaseNo);

        public string GetMatch(string CaseNo,string GetRegionCode);

        public int show2(string CaseNo,string CallRecvDt,int CallSno);

    }
}
