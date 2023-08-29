using IBS.DataAccess;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace IBS.Interfaces
{
    public interface IInterUnit_TransferRepository
    {
        public InterUnit_TransferModel GetJVvalues(int BankNameDropdown, string CHQ_NO, string CHQ_DATE);
        public InterUnit_TransferModel GetTextboxValues(int BankNameDropdown, string CHQ_NO, string CHQ_DATE, string region);
        public DTResult<InterUnit_TransferModel> BillList(DTParameters dtParameters);
        public InterUnit_TransferModel SelectInterUnit(string CHQ_NO , string CHQ_DT , string BANK_CD);
        public bool InsertInterUnit(InterUnit_TransferModel InterUnit_TransferModel , string Region);
        public bool UpdateInterUnit(InterUnit_TransferModel InterUnit_TransferModel);
        public bool InsertJV_Details(InterUnit_TransferModel InterUnit_TransferModel);
        public string GetNewJVNumber( string region, string VCHR_DT);
        public bool Save(InterUnit_TransferModel InterUnit_TransferModel, string Region);

        public bool modify(InterUnit_TransferModel InterUnit_TransferModel, string Region);

        public InterUnit_TransferModel Del_Select(InterUnit_TransferModel InterUnit_TransferModel);
        public InterUnit_TransferModel Update_RV(InterUnit_TransferModel model);
        public bool updt_RV(InterUnit_TransferModel InterUnit_TransferModel);
        public bool  UpdateJVDetails(InterUnit_TransferModel InterUnit_TransferModel);
    }
}
