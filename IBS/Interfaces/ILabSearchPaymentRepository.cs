using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabSearchPaymentRepository
    {
        public DTResult<SearchPaymentsModel> GetSearchList(DTParameters dtParameters);

    }
}
