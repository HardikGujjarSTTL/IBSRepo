using IBS.Models;

namespace IBS.Interfaces
{
    public interface IHologramAccountalRepository
    {
        DTResult<HologramAccountalModel> GetHologramAcountList(DTParameters dTParameters);
    }
}
