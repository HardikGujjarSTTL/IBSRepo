using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface IInspectionBillingDelayRepository
    {
        DTResult<InspectionBillingDelayModel> Get_Inspection_Billing_Delay(DTParameters dTParameters, UserSessionModel model);
    }
}
