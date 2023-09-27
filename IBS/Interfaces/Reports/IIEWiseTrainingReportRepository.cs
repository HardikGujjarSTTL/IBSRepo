using IBS.Models.Reports;

namespace IBS.Interfaces.Reports
{
    public interface IIEWiseTrainingReportRepository
    {
        public IEWiseTrainingReportModel GetIEWiseTrainingDetails(string IENAME, string TrainingArea, string Mechanical, string Electrical, string Civil, string Regular, string Deputaion, string Particularie, string ParticularArea,string Region);
    }
}
