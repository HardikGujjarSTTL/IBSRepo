using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IVendorCallRegisterRepository
    {
        public VenderCallRegisterModel FindByID(string CaseNo, string CallRecvDt, int CallSno, string UserName);

        DTResult<VenderCallRegisterModel> GetUserList(DTParameters dtParameters,string UserName);

        DTResult<VenderCallRegisterModel> GetVenderList(DTParameters dtParameters, string UserName);
    }
}
