using IBS.Models;
using System.Data;

namespace IBS.Interfaces.Reports
{
    public interface IBPOWiseOutstandingBillsRepository
    {
        DataSet GenerateReport(BPOWiseOutstandingBillsModel BPOWiseOutstandingBillsModel);
    }
}
