using IBS.Models;

namespace IBS.Interfaces
{
    public interface IUnregisteredCallsRepository
    {
        public DTResult<UnregisteredCallsModel> GetUnregisteredCallsList(DTParameters dtParameters);

        public bool IsExists(int IeCd);

        public UnregisteredCallsModel FindByID(int id);

        public int SaveDetails(UnregisteredCallsModel model);

        public bool Remove(int id);
    }
}
