using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;


namespace IBS.Repositories
{
    public interface IPrint_Bank_Statement_VoucherRepository
    {
        public Print_Bank_Statement_VoucherModel ReportData(string VCHR_NO, string Region);
    }
}
