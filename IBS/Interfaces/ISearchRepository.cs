using IBS.Models;
using IBS.Repositories;

namespace IBS.Interfaces
{
    public interface ISearchRepository
    {
        public DTResult<Search> GetSearchList(DTParameters dtParameters,string region);

        public List<BPOdata> GetBPOList(string BPONO);
        public List<Consignee> GetConsigneeList(string Consignee);
        public List<VendorCls> GetVendorList(string Vendor);
    }
}
