using IBS.Models;
using IBS.Repositories;

namespace IBS.Interfaces
{
    public interface ILabSearchPaymentRepository
    {
        public DTResult<SearchPaymentsModel> GetSearchList(DTParameters dtParameters);

    }
}
