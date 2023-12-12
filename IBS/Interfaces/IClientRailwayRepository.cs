using IBS.Models;

namespace IBS.Interfaces
{
    public interface IClientRailwayRepository
    {
        DTResult<Railway> GetRailways(DTParameters dtParameters);

        public int RailwayInsertUpdate(Railway model);

        public Railway FindRailwayByID(string id);
    }
}
