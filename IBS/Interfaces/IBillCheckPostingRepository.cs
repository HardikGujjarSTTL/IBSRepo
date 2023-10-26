using IBS.Models;

namespace IBS.Interfaces
{
    public interface IBillCheckPostingRepository
    {
        public BillCheckPostingModel FindByID(string BankName, string ChqNo, DateTime ChqDt,string Region);
    }
}
