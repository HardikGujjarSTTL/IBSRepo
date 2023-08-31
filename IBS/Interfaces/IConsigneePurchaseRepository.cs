using IBS.Models;

namespace IBS.Interfaces
{
    public interface IConsigneePurchaseRepository
    {
        DTResult<ConsigneePurchaseMasterSearchModel> Get_Consignee_Purchase(DTParameters dtParameters);
        string Get_State(string city_CD);
        ConsigneePurchaseModel Get_Consignee_Purchase_Detail(int Consignee_CD);
        int ConsigneePurchaseDelete(int Consignee_CD);
        int CongsigneePurchaseInsertUpdate(ConsigneePurchaseModel model, UserSessionModel uModel);

    }
}
