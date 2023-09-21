using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface IInspectionBillingDelayRepository
    {
        List<InspectionBillingDelayModel> Get_Inspection_Billing_Delay(InspectionBillingDelayReportModel obj, UserSessionModel model);
        //DTResult<InspectionBillingDelayModel> Get_Inspection_Billing_Delay(DTParameters dTParameters, UserSessionModel model);

    }
}
