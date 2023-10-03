using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface ICoComplaintJIRequiredRepository
    {
        public JIRequiredReport GetJIComplaintsList(string FinancialYearsText, string FinancialYearsValue);
    }
}
