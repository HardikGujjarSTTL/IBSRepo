namespace IBS.Models
{
    public class Statement_IeVendorWiseModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
        public string WiseRadio { get; set; }
        public string IeStatus { get; set; }
        public int Days { get; set; }
        public string includeNSIC { get; set; }
        public string pendingCallsOnly { get; set; }

        public string RLY_CD { get; set; }
        public string wSortkEy { get; set; }
        public DateTime EXT_DELV_DT { get; set; }
        public string IE_NAME { get; set; }
        public string VENDOR { get; set; }
        public string CASE_NO { get; set; }
        public DateTime CALL_RECV_DT { get; set; }
        public DateTime CANCEL_DT { get; set; }
        public string CALL_CANCEL_STATUS { get; set; }
        public decimal? CALL_CANCEL_CHARGES { get; set; }
        public string CANCEL_REASON { get; set; }
            public string CALL_DATE { get; set; }
        public int CALL_SNO { get; set; }
        public string CALL_STATUS_DESC { get; set; }
        public DateTime CALL_STATUS_DATE { get; set; }
        public string CO_NAME { get; set; }
        public DateTime DESIRE_DT { get; set; }
        public DateTime PO_DT { get; set; }
        public string DESIRE_DATE { get; set; }
        public DateTime CALL_DT_CONCAT { get; set; }
        public DateTime APPROVAL_DATE { get; set; }
        public string MFG_CD { get; set; }
        public string MFG_PLACE { get; set; }
        public string MFG    { get; set; }
        public string CANC_DOC { get; set; }
        public int CO_CD { get; set; }
        public string USER_ID { get; set; }
        public string CLIENT_TYPE { get; set; }
        public string SelectedRailway { get; set; }
        public string L5NO_PO { get; set; }
        public string PO_NO { get; set; }
        public string RLY_NONRLY { get; set; }
        public string MANUFACTURER { get; set; }
        public string ITEM_DESC_PO { get; set; }
        public string QTY_TO_INSP { get; set; }
        public DateTime CALL_MARK_DT { get; set; }
        public string CONSIGNEE  { get; set; }
        public string IE_PHONE_NO { get; set; }
        public string REMARK { get; set; }
        public string COLOUR { get; set; }
        public string MFG_PERS { get; set; }
        public string MFG_PHONE { get; set; }
        public string Hologram { get; set; }
        public string ICPhoto { get; set; }
        public string ICPhotoA1 { get; set; }
        public string ICPhotoA2 { get; set; }
        public int COUNT  { get; set; }
        public string CALL_STATUS { get; set; }
        public int VEND_CD { get; set; }
        public int IE { get; set; }
        
        public List<railway_dropdown1> Railways { get; set; }




        public List<Statement_IeVendorWiseModel> statement_IeVendorWiseModels { get; set; }

    }
    public class railway_dropdown1
    {
        public string RLY_CD { get; set; }
        public string RAILWAY_ORGN { get; set; }
    }
}
