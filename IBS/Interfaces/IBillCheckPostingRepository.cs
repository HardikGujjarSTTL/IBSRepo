using IBS.Models;

namespace IBS.Interfaces
{
    public interface IBillCheckPostingRepository
    {
        public BillCheckPostingModel FindByID(string ChqNo, DateTime ChqDt, string BankName, string BillNo, int AmountCleared, string Region);

        public BillCheckPostingModel GetBankDetails(int BankCd, string ChqNo, DateTime ChqDt, string Region);

        public BillCheckPostingModel GetBillDetails(string BillInvoiceNo, string BillTypes, string Region);

        DTResult<BillCheckPostingModelList> GetBillList(DTParameters dtParameters);

        int BillDetailSave(BillCheckPostingModel model, string UserName);
    }
}
