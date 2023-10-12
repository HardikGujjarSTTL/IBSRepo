using IBS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Interfaces
{
    public interface IRecieptVoucherRepository
    {
        public DTResult<RecieptVoucherModel> GetVoucherList(DTParameters dtParameters);

        public RecieptVoucherModel FindByID(string VoucherNo);

        public string GetAccountName(int AccCd);

        public string GetBankName(int BankCd);

        public string GetBPOName(string BpoCd);

        public IEnumerable<SelectListItem> GetBPO(int Acc_cd, string BpoType, string BPO_cd);

        public BPODetailsModel FindBPODetails(string CaseNo);

        public bool SaveDetails(RecieptVoucherModel model);

        public string GenerateVoucherNo(string Region, DateTime VoucherDate);
    }
}
