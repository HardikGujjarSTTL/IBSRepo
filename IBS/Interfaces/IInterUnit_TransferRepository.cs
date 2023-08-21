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
        public string GetNewJVNumber( string region, string VCHR_DT);
    }
}
