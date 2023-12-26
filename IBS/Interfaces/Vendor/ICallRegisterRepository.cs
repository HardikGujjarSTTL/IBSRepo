using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface ICallRegisterRepository
    {
        public CallRegisterModel FindByID(string CaseNo, string CallRecvDt, string CallSNo, string UserName);

        public CallRegisterModel FindManageByID(string CaseNo, string CallRecvDt, int CallSNo, int ItemSrNoPo, string UserName);

        DTResult<CallRegisterModel> FindByModifyDetail(string CaseNo, string CallRecvDt, int CallSNo, string UserName);

        DTResult<CallRegisterModel> FindMatchDetail(string CaseNo, string CallRecvDt, int CallSNo, string UserName);

        DTResult<CallRegisterModel> GetDataList(DTParameters dtParameters, string UserName);

        DTResult<CallRegisterModel> GetCallDetailsList(DTParameters dtParameters);

        int DetailsSave(CallRegisterModel model, int UserId);
    }
}
