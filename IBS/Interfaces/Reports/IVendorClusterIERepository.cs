using IBS.Models;
using static IBS.Helper.Enums;

namespace IBS.Interfaces.Reports
{
    public interface IVendorClusterIERepository
    {
        public VendorClusterReportModel GetVendorClusterReport(string department,string Region);
    }
}
