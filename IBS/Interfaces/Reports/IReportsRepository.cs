using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface IReportsRepository
    {
        DTResult<PendingJICasesReportModel> Get_Pending_JI_Cases(DTParameters dtParameters, string iecd);
        DTResult<IEDairyModel> Get_IE_Dairy(DTParameters dtParameters, UserSessionModel userModel);
        
    }
}
