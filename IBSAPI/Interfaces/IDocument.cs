using static IBSAPI.Helper.Enums;
using System;
using IBSAPI.Models;
using Microsoft.CodeAnalysis;

namespace IBSAPI.Interfaces
{
    public interface IDocument
    {
        List<IBS_DocumentDTO> GetRecordsList(int DocumentCategoryID, string ApplicationID);
        int GetRecordsMaxID(int DocumentCategoryID);
        int SaveDocument(List<APPDocumentDTO> objSPVMemberDTO, int[] DocumentIds = null);
        void DeleteAllFiles(string ApplicationID);
        IBS_DocumentDTO FindRecord(int ID);
    }
}
