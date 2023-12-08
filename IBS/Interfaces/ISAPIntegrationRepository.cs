using System.Data;

namespace IBS.Interfaces
{
    public interface ISAPIntegrationRepository
    {
        public DataSet ExportExcelBPO(string BPO_Cd);
    }
}