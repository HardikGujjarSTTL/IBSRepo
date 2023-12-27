using IBS.Models;

namespace IBS.Interfaces.Vendor
{
    public interface IPurchesOrder1LOARepository
    {
        public PurchesOrder1LOAModel FindByID(string CaseNo);

        public PurchesOrder1LOAModel FindByDetail(string CaseNo, byte ItemSrno, string type, string PoDt, int lstItemDesc);

        DTResult<PurchesOrder1LOAModel> FindByUOMDetail(decimal id);

        DTResult<PurchesOrder1LOAModel> GetDataList(DTParameters dtParameters);

        DTResult<PurchesOrder1LOAModel> GetPODataList(DTParameters dtParameters);

        int DetailsUpdate(PurchesOrder1LOAModel model);
    }
}
