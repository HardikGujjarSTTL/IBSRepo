using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IVendorLabSampleInfoRepository
    {

        List<LabSampleInfoModel> LapSampleIndex(string CaseNo, string CallRdt, string CallSno,string VenCod);
        LabSampleInfoModel SampleDtlData(string CaseNo, string CallRdt, string CallSno, string Regin);
        string CheckExist(string CaseNo, string CallRdt, string CallSno, string Regin);
        bool SaveDataDetails(LabSampleInfoModel LabSampleInfoModel);
        bool UpdateDetails(LabSampleInfoModel LabSampleInfoModel);
    }
}
