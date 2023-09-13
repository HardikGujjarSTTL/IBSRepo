using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface ISummaryConsigneeWiseInspRepository
    {

        DTResult<SummaryConsigneeWiseInspModel> SummaryConsigneeWiseInsp(DTParameters dtParameters,string Regin);
       
    }
}
