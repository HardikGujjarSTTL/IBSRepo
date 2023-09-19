using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface IReportsRepository
    {
        DTResult<PendingJICasesReportModel> Get_Pending_JI_Cases(DTParameters dtParameters, string iecd);
        DTResult<IEDairyModel> Get_IE_Dairy(DTParameters dtParameters, UserSessionModel userModel);

        DTResult<IE7thCopyListModel> Get_IE_7thCopyList(DTParameters dtParameters, UserSessionModel model);


        IE7thCopyListModel GetIE7thCopyReport(string Bk_No, string Set_No, UserSessionModel model);
    }
}
