﻿using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDEOCRISPurchesOrderWCaseNoRepository
    {
        public DEO_CRIS_PurchesOrderModel FindByID(string ImmsPokey, string ImmsRlyCd);

        DTResult<DEO_CRIS_PurchesOrderListModel> GetDataList(DTParameters dtParameters, string Region);

        bool DetailsUpdate(DEO_CRIS_PurchesOrderModel model);

        bool UpdateREMARKS(string REMARKS, int IMMS_POKEY, string IMMS_RLY_CD);

        string getVendorEmail(string CASE_NO);
        string[] GenerateRealCaseNoCRIS(string REGION_CD, string IMMS_POKEY, string IMMS_RLY_CD, string USER_ID);

        public DEO_CRIS_PO_MasterDetailsModel DetailsFindByID(string IMMS_POKEY, string ITEM_SRNO, string IMMS_RLY_CD);

        DTResult<DEO_CRIS_PO_MasterDetailsListModel> GetPOMasterDetailsList(DTParameters dtParameters);

        int POMasterSubDetailsInsertUpdate(DEO_CRIS_PO_MasterDetailsModel model);
    }
}
