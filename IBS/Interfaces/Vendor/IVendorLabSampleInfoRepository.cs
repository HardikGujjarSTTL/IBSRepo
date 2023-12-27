using IBS.Models;

namespace IBS.Interfaces
{
    public interface IVendorLabSampleInfoRepository
    {

        DTResult<LabSampleInfoModel> LapSampleIndex(DTParameters dtParameters, string Regin);
        LabSampleInfoModel SampleDtlData(string CaseNo, string CallRdt, string CallSno, string Regin);
        string CheckExist(string CaseNo, string CallRdt, string CallSno, string Regin);
        bool SaveDataDetails(LabSampleInfoModel LabSampleInfoModel);
        bool UpdateDetails(LabSampleInfoModel LabSampleInfoModel);
    }
}
