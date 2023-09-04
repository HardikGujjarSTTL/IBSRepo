using IBS.Models;
using IBS.Repositories;

namespace IBS.Interfaces
{
    public interface ISearchRepository
    {
        public DTResult<Search> GetSearchList(DTParameters dtParameters);

        public List<BPOdata> GetBPOList(string BPONO);
        public List<Consignee> GetConsigneeList(string Consignee);
    }
}
