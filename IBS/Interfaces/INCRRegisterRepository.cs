using IBS.Models;

namespace IBS.Interfaces
{
    public interface INCRRegisterRepository
    {
        DTResult<NCRRegister> GetDataList(DTParameters dtParameters);
    }
}
