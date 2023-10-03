using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface IReportsRepository
    {
        PendingJICasesReportModel Get_Pending_JI_Cases(DateTime FromDate, DateTime ToDate, string iecd);
        IEDairyModel Get_IE_Dairy(DateTime FromDate, DateTime ToDate, string DpIE, string OrderByVisit, string IsAllIE, UserSessionModel userModel);

        DTResult<IE7thCopyListModel> Get_IE_7thCopyList(DTParameters dtParameters, UserSessionModel model);

        IE7thCopyListModel GetIE7thCopyReport(string Bk_No, string Set_No, UserSessionModel model);

        ICStatusModel Get_IC_Status(DateTime FromDate, DateTime ToDate, string IE_CD, string Region);

        IEWorkPlanModel Get_IE_WorkPlan(DateTime FromDate, DateTime ToDate, string IECD, string Region);

    }
}
