using IBS.DataAccess;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace IBS.Interfaces
{
    public interface IInterUnit_TransferRepository
    {

        #region My Code
        InterUnit_TransferModel Get_Inter_Unit_Transfer(string Bank, string ChqNo, string ChqDate, string Region);
        DTResult<InterUnitTransferRegionModel> GetInterUnitTransferRegion(DTParameters dTParameters, List<InterUnitTransferRegionModel> UnitTransferModel);

        bool DetailsInsertUpdate(InterUnit_TransferModel model, UserSessionModel user);
        #endregion

        //#region Old Code
        //public InterUnit_TransferModel GetJVvalues(int BankNameDropdown, string CHQ_NO, string CHQ_DATE);
        //public InterUnit_TransferModel GetTextboxValues(int BankNameDropdown, string CHQ_NO, string CHQ_DATE, string region);
        //public DTResult<InterUnit_TransferModel> BillList(DTParameters dtParameters);
        //public InterUnit_TransferModel SelectInterUnit(string CHQ_NO , string CHQ_DT , string BANK_CD);
        //public bool InsertInterUnit(InterUnit_TransferModel InterUnit_TransferModel , string Region);
        //public bool UpdateInterUnit(InterUnit_TransferModel InterUnit_TransferModel);
        //public bool InsertJV_Details(InterUnit_TransferModel InterUnit_TransferModel);
        //public string GetNewJVNumber( string region, string VCHR_DT);
        //public bool Save(InterUnit_TransferModel InterUnit_TransferModel, string Region);

        //public bool modify(InterUnit_TransferModel InterUnit_TransferModel, string Region);

        //public InterUnit_TransferModel Del_Select(InterUnit_TransferModel InterUnit_TransferModel);
        //public InterUnit_TransferModel Update_RV(InterUnit_TransferModel model);
        //public bool updt_RV(InterUnit_TransferModel InterUnit_TransferModel);
        //public bool  UpdateJVDetails(InterUnit_TransferModel InterUnit_TransferModel);
        //#endregion
    }
}
