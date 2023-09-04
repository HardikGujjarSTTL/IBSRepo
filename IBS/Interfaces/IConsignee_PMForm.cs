using IBS.Models;

namespace IBS.Interfaces
{
    public interface IConsignee_PMForm 
    {
        public ConsigneePurchaseModel FindByID(int ConsigneeCd);
        DTResult<ConsigneePurchaseModel> GetConsignee_PMFormList(DTParameters dtParameters);
        bool Remove(int ConsigneeCd, int UserID);
        int Consignee_PMFormDetailsInsertUpdate(ConsigneePurchaseModel model);
    }
}

