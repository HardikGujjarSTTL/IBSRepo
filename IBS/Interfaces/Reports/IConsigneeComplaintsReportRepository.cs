using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface IConsigneeComplaintsReportRepository
    {
        DTResult<ConsigneeComplaintsReportModel> Get_Consignee_Complaints(DTParameters dtParameters,UserSessionModel model);
        List<ConsigneeComplaintsReportModel> Get_Consignee_Complaints(DTParameters dtParameters, string FromDate, string ToDate, UserSessionModel model);
    }
}
