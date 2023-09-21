using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces
{
    public interface IConsigneeCompPeriodRepository
    {
        public ConsigneeCompPeriodReport GetCompPeriodData(string FromDate, string ToDate, string Allregion, string regionorth, string regionsouth, string regioneast, string regionwest, string jiallregion,
            string jinorth, string jisourth, string jieast, string jiwest, string compallregion, string compyes, string compno, string cancelled, string underconsider, string allaction, string particilaraction, string actiondrp,
            string actioncodedrp, string actionjidrp);
    }
}
