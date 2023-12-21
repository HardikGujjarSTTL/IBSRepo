using IBSAPI.Models;
namespace IBSAPI.Interfaces
{
    public interface ICRISRepository
    {
        public CRISModel FindBillDetails(string BillNo);
    }
}
