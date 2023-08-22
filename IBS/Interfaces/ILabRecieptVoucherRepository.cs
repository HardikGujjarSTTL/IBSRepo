using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabRecieptVoucherRepository
    {
        DTResult<LabRecieptVoucherModel> GetVoucherList(DTParameters dtParameters);
        public LabRecieptVoucherModel FindByID(string VCHR_NO, int BANK_CD, string CHQ_NO, string CHQ_DT);

        string VoucherDetailsSave(LabRecieptVoucherModel model, string Region);

        string ChkCSNO(string txtCSNO, string lstBPO, out string Narrt);

        public List<BPOlist> GetDistinctBPOsByCaseNo(string txtCSNO);

    }
}
