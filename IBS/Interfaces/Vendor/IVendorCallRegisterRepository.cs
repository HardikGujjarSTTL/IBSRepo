using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IVendorCallRegisterRepository
    {
        public VenderCallRegisterModel FindByID(string CaseNo, string CallRecvDt, int CallSno);

        DTResult<VenderCallRegisterModel> GetUserList(DTParameters dtParameters,string UserName);
    }
}
