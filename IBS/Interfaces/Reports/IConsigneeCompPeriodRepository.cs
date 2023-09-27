using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces
{
    public interface IConsigneeCompPeriodRepository
    {
        public ConsigneeCompPeriodReport GetCompPeriodData(string FromDate, string ToDate, string actiondrp, string actioncodedrp, string actionjidrp);
    }
}
