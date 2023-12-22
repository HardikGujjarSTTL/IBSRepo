using IBSAPI.Models;
namespace IBSAPI.Interfaces
{
    public interface ICRISRepository
    {
        public CRISModel FindBillDetails(string BillNo);

        List<CRISBillListing> FindListBillNo(DateTime frmdt, DateTime todt);

        List<CRISGetBillListing> Findgetbill(DateTime frmdt, DateTime todt, string billno);
    }
}
