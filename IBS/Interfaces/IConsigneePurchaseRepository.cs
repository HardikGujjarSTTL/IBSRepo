using IBS.Models;

namespace IBS.Interfaces
{
    public interface IConsigneePurchaseRepository
    {
        public DTResult<ConsigneePurchaseMasterSearchModel> GetConsigneePurchaseList(DTParameters dtParameters);

        public ConsigneePurchaseModel FindByID(int Id);

        public string Get_State(string city_CD);

        public int SaveDetails(ConsigneePurchaseModel model);

        public bool Remove(int Id, int UserID);
    }
}
