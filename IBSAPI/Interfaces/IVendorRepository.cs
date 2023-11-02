using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface IVendorRepository
    {
        List<CallRegiModel> GetCaseDetailsforvendor(int UserID);
    }
}
