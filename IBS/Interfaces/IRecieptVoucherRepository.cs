using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Interfaces
{
    public interface IRecieptVoucherRepository
    {
        public DTResult<RecieptVoucherModel> GetVoucherList(DTParameters dtParameters);

        public RecieptVoucherModel FindByID(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT);

        public string VoucherDetailsSave(RecieptVoucherModel model, string Region);

        public string ChkCSNO(string txtCSNO, string lstBPO, out string Narrt);

        public List<BPOlist> GetDistinctBPOsByCaseNo(string txtCSNO);

        public string Insert(RecieptVoucherModel model, string VoucherDate, string Bank_Code, string VoucherType, string Region);

        public string GetAccountName(int AccCd);

        public string GetBankName(int BankCd);

        public string GetBPOName(string BpoCd);

        public IEnumerable<SelectListItem> GetBPO(int Acc_cd, string BpoType, string BPO_cd);

        public BPODetailsModel FindBPODetails(string CaseNo);
    }
}
