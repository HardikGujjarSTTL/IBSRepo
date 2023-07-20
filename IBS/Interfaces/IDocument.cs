using static IBS.Helper.Enums;
using System;
using IBS.Models;
using Microsoft.CodeAnalysis;

namespace IBS.Interfaces
{
    public interface IDocument
    {
        List<IBS_DocumentDTO> GetRecordsList(int DocumentCategoryID, int ApplicationID);
        int SaveDocument(List<APPDocumentDTO> objSPVMemberDTO, int[] DocumentIds = null);
        void DeleteAllFiles(int ApplicationID);
        IBS_DocumentDTO FindRecord(int ID);
    }
}
