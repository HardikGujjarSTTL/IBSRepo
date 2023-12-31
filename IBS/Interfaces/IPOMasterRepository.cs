﻿using IBS.Models;

namespace IBS.Interfaces
{
    public interface IPOMasterRepository
    {
        public PO_MasterModel FindByID(string CaseNo);
        DTResult<PO_MasterModel> GetPOMasterList(DTParameters dtParameters,int VendCd);
        DTResult<PO_MasterModel> GetPOMasterListForClient(DTParameters dtParameters, string rly_cd,string RlyNonrly);
        bool Remove(string CaseNo, int UserID);
        PO_MasterModel alreadyExistT80_PO_MASTER(PO_MasterModel model);
        PO_MasterModel alreadyExistT13_PO_MASTER(PO_MasterModel model);
        string POMasterDetailsInsertUpdate(PO_MasterModel model);
        public PO_MasterModel FindCaseNo(string CaseNo,int VendCd);
        public PO_MasterModel FindCaseNoForClient(string CaseNo);
        DTResult<PO_MasterDetailListModel> GetPOMasterDetailsList(DTParameters dtParameters);
        bool RemovePODetails(string CaseNo,string ITEM_SRNO, int UserID);
        public int GenerateITEM_SRNO(string CASE_NO);
        public PO_MasterDetailsModel FindPODetailsByID(string CASE_NO, string ITEM_SRNO);
        DTResult<PO_MasterDetailsModel> FindByUOMDetail(decimal id);
        int POMasterSubDetailsInsertUpdate(PO_MasterDetailsModel model);
        string UpdateRealCaseNo(DEOVendorPurchesOrderModel model);
        string getVendorEmail(string CASE_NO);

        string[] GenerateRealCaseNo(string REGION_CD, string CASE_NO, string USER_ID);
        public IBS_DocumentDTO FindAPPDocumentByID(string Applicationid, int DocumentID);
        public int SaveAPPDocumentByID(IBS_DocumentDTO model);
    }
}
