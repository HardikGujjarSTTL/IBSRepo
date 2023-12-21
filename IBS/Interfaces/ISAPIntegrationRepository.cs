using System.Data;

namespace IBS.Interfaces
{
    public interface ISAPIntegrationRepository
    {
        public DataSet ExportExcelBPO(string BPO_Cd);
        public DataSet ExportExcelSelectiveBPO(string BPO_Cd);
        public DataSet ExportExcelConsigneSelect(string BPO_Cd);

        public int UpdateBPO(DataSet ds);
        public int UpdateConsigne(DataSet ds);
    }
}