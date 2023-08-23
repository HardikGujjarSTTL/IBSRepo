using IBS.Models;
namespace IBS.Interfaces.Inspection_Billing
{
    public interface ICallMarkedOnlineRepository
    {
        DTResult<CallMarkedOnlineModel> Get_Call_Marked_Online(DTParameters dTParameters);
    }
}
