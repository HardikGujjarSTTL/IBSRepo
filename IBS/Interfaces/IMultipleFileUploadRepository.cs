using IBS.Repositories;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IMultipleFileUploadRepository 
    {
        int InsertPDFDetails(string FileName, string Bill_NO,int CreatedBy);

        DTResult<MultipleFileUploadModel> GetDocList(DTParameters dtParameters);
    }
}
