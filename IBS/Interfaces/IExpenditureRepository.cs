using IBS.Models;

namespace IBS.Interfaces
{
    public interface IExpenditureRepository
    {
        public ExpenditureModel FindByID(string LabBillPer, string RegionCode);
        DTResult<ExpenditureModel> GetExpenditureList(DTParameters dtParameters, string RegionCode);

        bool Remove(string ExpPer, string strRgn, int UserID);

        string ExpenditureDetailsInsertUpdate(ExpenditureModel model);
    }
}
