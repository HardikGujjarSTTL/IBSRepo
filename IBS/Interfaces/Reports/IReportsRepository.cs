using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface IReportsRepository
    {
        PendingJICasesReportModel Get_Pending_JI_Cases(DateTime FromDate, DateTime ToDate, string iecd);
        DTResult<IEDairyModel> Get_IE_Dairy(DTParameters dtParameters, UserSessionModel userModel);

        DTResult<IE7thCopyListModel> Get_IE_7thCopyList(DTParameters dtParameters, UserSessionModel model);

        IE7thCopyListModel GetIE7thCopyReport(string Bk_No, string Set_No, UserSessionModel model);

        ICStatusModel Get_IC_Status(DateTime FromDate, DateTime ToDate, string IE_CD, string Region);
    }
}
