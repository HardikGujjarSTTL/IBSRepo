using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICheckPostingFormRepository
    {
        DTResult<CheckPostingFormModel> BillList(DTParameters dtParameters);
        CheckPostingFormModel FindByID( string billNo);
        CheckPostingHeader GetTextboxValues(string BankNameDropdown, string CHQ_NO, string CHQ_DATE, string region);

        public CheckPostingbillInvoice ChkBillNo(string RadioBill,string BillInvoiceNo, string Region);
        public CheckPostingbillInvoice ChkInvoiceNo(string InvoiceBill, string Region);
        public string UpdateData(CheckPostingFormModel model);
    }
}
