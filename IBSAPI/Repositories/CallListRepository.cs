using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;

namespace IBSAPI.Repositories
{
    public class CallListRepository : ICallListRepository
    {

        private readonly ModelContext context;
        public CallListRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<CallListModel> GetCallList()
        {
            List<CallListModel> lst = new();

            lst = (from x in context.T01Regions
                   select new CallListModel
                   {
                       ID = x.RegionCode,
                       Name = x.Region,
                       MobileNo = "+91 " + x.MobileNo,
                   }).ToList();
            return lst;
        }

    }
}
