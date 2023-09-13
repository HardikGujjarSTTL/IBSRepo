using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICentralItemMasterRepository
    {
        #region Master
        public CentralItemMasterModel FindByID(int ID);
        DTResult<CentralItemMasterModel> GetList(DTParameters dtParameters);
        bool Remove(int ID, int UserID);
        int InsertUpdate(CentralItemMasterModel model);
        #endregion

        #region Details
        public CentralItemDetailModel FindDetailsByID(int ID);
        DTResult<CentralItemDetailModel> GetDetailsList(DTParameters dtParameters);
        bool RemoveDetails(int ID, int UserID);
        int DetailsInsertUpdate(CentralItemDetailModel model);
        #endregion
    }
}
