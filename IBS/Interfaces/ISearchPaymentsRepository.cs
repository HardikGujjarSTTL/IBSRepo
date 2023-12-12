using IBS.Models;

namespace IBS.Interfaces
{
    public interface ISearchPaymentsRepository
    {

        DTResult<SearchPaymentsModel> GetSearchPayment(DTParameters dtParameters, string Region, int Role);
    }
}
