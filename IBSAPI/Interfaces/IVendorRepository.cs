using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IVendorRepository
    {
        List<CallRegiModel> GetCaseDetailsforvendor(int UserID);
        List<CallRegiModel> GetCaseDetailsforClient(string UserID, string Organisation, string OrgnType);
    }
}
