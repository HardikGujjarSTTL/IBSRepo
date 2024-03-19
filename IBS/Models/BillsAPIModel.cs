namespace IBS.Models
{
    #region AuthenticateModel
    public class BillsAPIModel
    {
        public string token { get; set; }
    }
    #endregion

    #region PassedBills
    public class PassedBillsModel
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
    public class PassedBillsResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<object> error { get; set; }
        public string timestamp { get; set; }
        public List<PassedBillsModel> data { get; set; }
    }
    #endregion

    #region PassedBills
    public class AllBillsModel
    {
        public string BILL_NO { get; set; }
        public string RLY_CODE { get; set; }
        public string INVOICEDATE { get; set; }
        public string BPO_CD { get; set; }
        public string BPO_TYPE { get; set; }
        public string BPO_RLY { get; set; }
        public string BPO_NAME { get; set; }
        public string BPO_ORGN { get; set; }
        public string BPO_ADD { get; set; }
        public string BPO_CITY { get; set; }
        public string CASE_NO { get; set; }
        public string REGION_CODE { get; set; }
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string VEND_CD { get; set; }
        public string VEND_NAME { get; set; }
        public string VEND_ADD1 { get; set; }
        public string VEND_ADD2 { get; set; }
        public string VENDOR_CITY { get; set; }
        public string CONSIGNEE_CD { get; set; }
        public string CONSIGNEE { get; set; }
        public string CONSIGNEE_ADD1 { get; set; }
        public string CONSIGNEE_ADD2 { get; set; }
        public string CONSIGNEE_CITY { get; set; }
        public string IE_CD { get; set; }
        public string IE_CO_CD { get; set; }
        public string IC_NO { get; set; }
        public string IC_DT { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string CALL_INSTALMENT_NO { get; set; }
        public string MATERIAL_VALUE { get; set; }
        public string VISITS { get; set; }
        public string GSTTAXABLEAMT { get; set; }
        public string CGSTAMT { get; set; }
        public string SGSTAMT { get; set; }
        public string IGSTAMT { get; set; }
        public string AMOUNT { get; set; }
        public string INVOICENO { get; set; }
        public string RLYGSTIN { get; set; }
        public string PARTYGSTIN { get; set; }
        public string PARTYCODE { get; set; }
        public string PARTYNAME { get; set; }
        public string ITEM_SRNO { get; set; }
        public string ITEMDESC { get; set; }
        public string QTY { get; set; }
        public string RATE { get; set; }
        public string UNITCODE { get; set; }
        public string UOM_FACTOR { get; set; }
        public string BASIC_VALUE { get; set; }
        public string VALUE { get; set; }
        public string PDFFILE { get; set; }
        public string BILLDESC { get; set; }
        public string PARTYSTATE { get; set; }
        public string REVERSECHARGE { get; set; }
        public string ISGSTREGISTERED { get; set; }
        public string GSTTDSDEDUCTION { get; set; }
        public string COMPOSITETAXABLE { get; set; }
        public string HSNSAC { get; set; }
        public string HSNSACCODE { get; set; }
        public string ITCELIGIBLE { get; set; }
        public string SGSTRATE { get; set; }
        public string CGSTRATE { get; set; }
        public string UGSTRATE { get; set; }
        public string UGSTAMT { get; set; }
        public string IGSTRATE { get; set; }
        public string STATESUPPLY { get; set; }
        public string RECV_DATE { get; set; }
        public string UPD_DATE { get; set; }
        public string STATUS { get; set; }
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
        public string PDF_LINK { get; set; }
        public string PAYMENT_DT { get; set; }
        public string CO7_NO { get; set; }
        public string CO7_DATE { get; set; }
        public string BANK_ACCT_NO { get; set; }
        public string IFSCCODE { get; set; }
        public string BANK_NAME { get; set; }
        public string BANKADDRESS { get; set; }
        public string IC_PDF { get; set; }
        public string INVOICE_PDF { get; set; }
        public string AU { get; set; }
        public string BILL_RESENT_COUNT { get; set; }
        public string IRFC_FUNDED { get; set; }
        public string INVOICE_SUPP_DOCS { get; set; }
    }

    public class AllBillsResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<object> error { get; set; }
        public string timestamp { get; set; }
        public List<AllBillsModel> data { get; set; }
    }
    #endregion
}
