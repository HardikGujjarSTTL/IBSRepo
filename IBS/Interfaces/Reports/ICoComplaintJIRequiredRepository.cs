using IBS.Models;

namespace IBS.Interfaces.Reports
{
    public interface ICoComplaintJIRequiredRepository
    {
        public JIRequiredReport GetJIComplaintsList(string FinancialYearsText, string FinancialYearsValue);
    }
}
