using IBS.Models;

namespace IBS.Interfaces
{
    public interface IConsignee_PMForm 
    {
        public Consignee_PMFormModel FindByID(int ConsigneeCd);
        DTResult<Consignee_PMFormModel> GetConsignee_PMFormList(DTParameters dtParameters);
        bool Remove(int ConsigneeCd, int UserID);
        int Consignee_PMFormDetailsInsertUpdate(Consignee_PMFormModel model);
    }
}

