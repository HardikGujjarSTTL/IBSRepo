
using System;
using System.Collections.Generic;

namespace IBSWindowsService.Models
{
    #region AuthenticateModel
    public class AuthenticateModel
    {
        public string token { get; set; }
    }
    #endregion

    #region PO
    public class POModel
    {
        public string RLY { get; set; }
        public string POKEY { get; set; }
    }
    public class POResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<object> error { get; set; }
        public string timestamp { get; set; }
        public List<POModel> data { get; set; }
    }
    #endregion

    #region PoDetail
    public class PoDetail
    {
        public string RLY { get; set; }
        public string CASE_NO { get; set; }
        public string PL_NO { get; set; }
        public string ITEM_SRNO { get; set; }
        public string ITEM_DESC { get; set; }
        public string CONSIGNEE_CD { get; set; }
        public string IMMS_CONSIGNEE_CD { get; set; }
        public string IMMS_CONSIGNEE_NAME { get; set; }
        public string CONSIGNEE_DETAIL { get; set; }
        public string QTY { get; set; }
        public string QTY_CANCELLED { get; set; }
        public string RATE { get; set; }
        public string UOM_CD { get; set; }
        public string UOM { get; set; }
        public string BASIC_VALUE { get; set; }
        public string SALES_TAX_PER { get; set; }
        public string SALES_TAX { get; set; }
        public string EXCISE_TYPE { get; set; }
        public string EXCISE_PER { get; set; }
        public string EXCISE { get; set; }
        public string DISCOUNT_TYPE { get; set; }
        public string DISCOUNT_PER { get; set; }
        public string DISCOUNT { get; set; }
        public string OT_CHARGE_TYPE { get; set; }
        public string OT_CHARGE_PER { get; set; }
        public string OTHER_CHARGES { get; set; }
        public string VALUE { get; set; }
        public string DELV_DT { get; set; }
        public string EXT_DELV_DT { get; set; }
        public string USER_ID { get; set; }
        public string DATETIME { get; set; }
        public string ALLOCATION { get; set; }
    }
    public class PoHdr
    {
        public string CASE_NO { get; set; }
        public string PURCHASER_CD { get; set; }
        public string IMMS_PURCHASER_CODE { get; set; }
        public string IMMS_PURCHASER_DETAIL { get; set; }
        public string STOCK_NONSTOCK { get; set; }
        public string RLY_NONRLY { get; set; }
        public string PO_OR_LETTER { get; set; }
        public string PO_NO { get; set; }
        public string L5NO_PO { get; set; }
        public string PO_DT { get; set; }
        public string RECV_DT { get; set; }
        public string VEND_CD { get; set; }
        public string IMMS_VENDOR_CODE { get; set; }
        public string VENDOR_DETAILS { get; set; }
        public string FIRM_DETAILS { get; set; }
        public string RLY_CD { get; set; }
        public string RLY_SHORTNAME { get; set; }
        public string REGION_CODE { get; set; }
        public string REMARKS { get; set; }
        public string BILL_PAY_OFF { get; set; }
        public string BILL_PAY_OFF_NAME { get; set; }
        public string USER_ID { get; set; }
        public string DATETIME { get; set; }
        public string INSPECTING_AGENCY { get; set; }
        public string POI_CD { get; set; }
        public string PO_STATUS { get; set; }
        public string IMMS_POKEY { get; set; }
    }
    public class PoDetailResponseData
    {
        public List<PoDetail> PoDtl { get; set; }
        public PoHdr PoHdr { get; set; }
    }
    public class PoDetailResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<object> error { get; set; }
        public string timestamp { get; set; }
        public PoDetailResponseData data { get; set; }
    }
    #endregion

    #region PO MA
    public class POMAModel
    {
        public string RLY { get; set; }
        public string MAKEY { get; set; }
    }
    public class POMAResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<object> error { get; set; }
        public string timestamp { get; set; }
        public List<POMAModel> data { get; set; }
    }
    #endregion

    #region PoMADetail
    public class MMP_POMA_HDR
    {
        public string RLY { get; set; }
        public string MAKEY { get; set; }
        public string MAKEY_DATE { get; set; }
        public string POKEY { get; set; }
        public string PO_NO { get; set; }
        public string MA_NO { get; set; }
        public string MA_DATE { get; set; }
        public string MA_TYPE { get; set; }
        public string VCODE { get; set; }
        public string SUBJECT { get; set; }
        public string REF_NO { get; set; }
        public string REF_DATE { get; set; }
        public string REMARKS { get; set; }
        public string MA_SIGN_OFF { get; set; }
        public string REQUEST_ID { get; set; }
        public string AUTH_SEQ { get; set; }
        public string AUTH_SEQ_FIN { get; set; }
        public string CURUSER { get; set; }
        public string CURUSER_IND { get; set; }
        public string SIGN_ID { get; set; }
        public string REQ_ID { get; set; }
        public string FIN_STATUS { get; set; }
        public string REC_IND { get; set; }
        public string FLAG { get; set; }
        public string STATUS { get; set; }
        public string PUR_DIV { get; set; }
        public string PUR_SEC { get; set; }
        public string OLD_PO_VALUE { get; set; }
        public string NEW_PO_VALUE { get; set; }
        public string PO_MA_SRNO { get; set; }
        public string PUBLISH_FLAG { get; set; }
        public string SENT4VET { get; set; }
        public string VET_DATE { get; set; }
        public string VET_BY { get; set; }
        public string REQ_FLAG { get; set; }
    }

    public class MMP_POMA_DTL
    {
        public string RLY { get; set; }
        public string MAKEY { get; set; }
        public string SLNO { get; set; }
        public string MA_FLD { get; set; }
        public string MA_FLD_DESCR { get; set; }
        public string OLD_VALUE { get; set; }
        public string NEW_VALUE { get; set; }
        public string NEW_VALUE_IND { get; set; }
        public string NEW_VALUE_FLAG { get; set; }
        public string PL_NO { get; set; }
        public string PO_SR { get; set; }
        public string EXP_SR { get; set; }
        public string EXP_CODE { get; set; }
        public string COND_SLNO { get; set; }
        public string COND_NO { get; set; }
        public string COND_CODE { get; set; }
        public string STATUS { get; set; }
        public string MA_SR_NO { get; set; }
        public string ORIG_DP { get; set; }
        public string PAYMENT_YEAR { get; set; }
        public string NEW_POSR_DATA { get; set; }
        public string REF_PONO { get; set; }
        public string CONSIGNEE_RLY { get; set; }
    }

    public class MMP_POMA_Data
    {
        public MMP_POMA_HDR MMP_POMA_HDR { get; set; }
        public List<MMP_POMA_DTL> MMP_POMA_DTL { get; set; }
    }

    public class MMP_POMA_Response
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<object> error { get; set; }
        public string timestamp { get; set; }
        public MMP_POMA_Data data { get; set; }
    }

    #endregion

    #region BillsStatus
    public class BillsStatusModel
    {
        public string BILL_NO { get; set; }
        public string IC_NO { get; set; }
        public string IC_DT { get; set; }
        public string INVOICENO { get; set; }
        public string CO6_NO { get; set; }
        public string CO6_DATE { get; set; }
        public string CO6_STATUS { get; set; }
        public string CO6_STATUS_DATE { get; set; }
        public string PASSED_AMT { get; set; }
        public string DEDUCTED_AMT { get; set; }
        public string NET_AMT { get; set; }
        public string BOOKDATE { get; set; }
        public string RETURN_REASON { get; set; }
        public string RETURN_DATE { get; set; }
        public string CO7_NO { get; set; }
        public string CO7_DATE { get; set; }
        public string PAYMENT_DT { get; set; }
        public string BILL_RESENT_COUNT { get; set; }
        public string IRFC_FUNDED { get; set; }
        public string INVOICE_SUPP_DOCS { get; set; }
    }
    public class BillsStatusResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<object> error { get; set; }
        public string timestamp { get; set; }
        public List<BillsStatusModel> data { get; set; }
    }
    #endregion

}
