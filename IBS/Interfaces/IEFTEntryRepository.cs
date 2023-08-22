using IBS.Models;

namespace IBS.Interfaces
{
    public interface IEFTEntryRepository
    {
        DTResult<EFTEntryModel> GetVoucherList(DTParameters dtParameters);
        public EFTEntryModel FindByID(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT);

        string VoucherDetailsSave(EFTEntryModel model, string Region);

        string ChkCSNO(string txtCSNO, string lstBPO, out string Narrt);

        public List<BPOlist> GetDistinctBPOsByCaseNo(string txtCSNO);
    }
}
