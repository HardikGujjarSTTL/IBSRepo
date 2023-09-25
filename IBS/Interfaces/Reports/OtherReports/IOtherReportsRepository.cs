using IBS.Models.Reports;

namespace IBS.Interfaces.Reports.OtherReports
{
    public interface IOtherReportsRepository
    {
        ControllingOfficerIEModel GetControllingOfficerWiseIE(string Region);
    }
}
