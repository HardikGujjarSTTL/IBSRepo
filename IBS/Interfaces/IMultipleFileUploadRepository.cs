using IBS.Repositories;

namespace IBS.Interfaces
{
    public interface IMultipleFileUploadRepository 
    {
        int InsertPDFDetails(string FileName, string Bill_NO,int CreatedBy);
    }
}
