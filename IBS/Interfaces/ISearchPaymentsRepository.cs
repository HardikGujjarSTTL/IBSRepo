using IBS.Models;

namespace IBS.Interfaces
{
    public interface ISearchPaymentsRepository
    {

        DTResult<SearchPaymentsModel> GetSearchPayment(DTParameters dtParameters , string AMOUNT, string CASE_NO , string CHQ_NO, string BankNameDropdown, string NARRATION, string VCHR_DT, string ACC_CD , string region, int Role);
    }
}
