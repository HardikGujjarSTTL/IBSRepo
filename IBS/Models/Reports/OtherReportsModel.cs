namespace IBS.Models.Reports
{
    public class OtherReportsModel
    {
        public string ReportType { get; set; }

        public string ReportTitle { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        //public string CO { get; set; }
        //public string Status { get; set; }
        //public string IE { get; set; }
        //public bool IsCallDate { get; set; }
        public string department { get; set; }
        public string allreport { get; set; }
        public string departreport { get; set; }
        public string Case_No { get; set; }
        public string Call_Recv_Date { get; set; }
        public string Call_SNo { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string AllCM { get; set; }
        public string forCM { get; set; }
        public string All { get; set; }
        public string Outstanding { get; set; }
        public string formonth { get; set; }
        public string monthChar { get; set; }
        public string forperiod { get; set; }
        public string iecmname { get; set; }
        public string reporttype { get; set; }
        public string COName { get; set; }
        public string IEName { get; set; }
        public string IENametext { get; set; }
        public string TrainingArea { get; set; }
        public string Mechanical { get; set; }
        public string Electrical { get; set; }
        public string Civil { get; set; }
        public string Regular { get; set; }
        public string Deputaion { get; set; }
        public string Particularie { get; set; }
        public string ParticularArea { get; set; }
        public string StatusOffer { get; set; }
        public string Region { get; set; }
        public string StatusOffertxt { get; set; }
        public string Regiontxt { get; set; }
        public string rdoregionwise { get; set; }
        public string clientname { get; set; }
        public string vendor { get; set; }
        public string vendorcd { get; set; }
        public string monthtxt { get; set; }
        public string lstIE { get; set; }
        public string lstCM { get; set; }
        public string AllIEs { get; set; }
        public string ParticularIEs { get; set; }
        public string ParticularCMs { get; set; }
        public string IEWise { get; set; }
        public string CMWise { get; set; }
        public string SortedIE { get; set; }
        public string visitdate { get; set; }
        public string CaseNo { get; set; }
        public string CallRecDT { get; set; }
        public string CallSno { get; set; }
        public string BKNO { get; set; }
        public string SETNO { get; set; }
        public string DSCMonth { get; set; }
        public int DSCYear { get; set; }
        public string DSCToMonth { get; set; }
        public int DSCToYear { get; set; }
        public string DSCMonthText { get; set; }
        public string DSCToMonthText { get; set; }
    }

    public class ControllingOfficerIEModel
    {
        public string Region { get; set; }
        public List<ControllingOfficerModel> lstControllingOfficer { get; set; }
        public List<ControllingOfficerModel> lstCluster { get; set; }
        public List<IEModel> lstLocalIE { get; set; }
        public List<IEModel> lstOutstationIE { get; set; }
    }

    public class ControllingOfficerModel
    {
        public int CO_CD { get; set; }
        public string CO_NAME { get; set; }
        public string cluster_name { get; set; }
    }
    public class IEModel
    {
        public int IE_CO_CD { get; set; }
        public string IE_NAME { get; set; }
    }

    public class CoIeWiseCallsListModel
    {
        public string VENDOR { get; set; }
        public string MANUFACTURER { get; set; }
        public int VEND_CD { get; set; }
        public int MFG_CD { get; set; }
        public string CONSIGNEE { get; set; }
        public string ITEM_DESC_PO { get; set; }
        public string EXT_DELV_DT { get; set; }
        public string CALL_MARK_DT { get; set; }
        public string INSP_DESIRE_DT { get; set; }
        public string CALL_RECV_DT { get; set; }
        public string IE_NAME { get; set; }
        public string IE_PHONE_NO { get; set; }
        public string PO_NO { get; set; }
        public string PO_DATE { get; set; }
        public string PO_YR { get; set; }
        public string PO_SOURCE { get; set; }
        public string SOURCE { get; set; }
        public string RLY_CD { get; set; }
        public string CASE_NO { get; set; }
        public string USER_ID { get; set; }
        public string DATETIME { get; set; }
        public string REMARKS { get; set; }
        public string NEW_VENDOR { get; set; }
        public string CALL_STATUS { get; set; }
        public string CALL_STATUS_FULL { get; set; }
        public string COLOUR { get; set; }
        public string MFG_PERS { get; set; }
        public string MFG_PHONE { get; set; }
        public string CALL_SNO { get; set; }
        public string IC_PHOTO { get; set; }
        public string IC_PHOTO_A1 { get; set; }
        public string IC_PHOTO_A2 { get; set; }
        public int COUNT { get; set; }

        public bool IsCallDocument { get; set; }
        public string SS { get; set; }

        public bool IsCaseNoTif { get; set; }
        public bool IsCaseNoPdf{ get; set; }

        public string VISIT { get; set; }
        public string Lab_Status { get; set; }
        public bool IsLabPdf { get; set; }
        public string MyFile_ex { get; set; }
    }

    public class CoIeWiseCallsModel
    {
        public string Case_No { get; set; }
        public string Call_Recv_Date { get; set; }
        public string Call_SNo { get; set; }
        public List<CoIeWiseCallsList1Model> coIeWiseCallsList1 { get; set; }
        public List<CoIeWiseCallsList2Model> coIeWiseCallsList2 { get; set; }
    }

    public class CoIeWiseCallsList1Model
    {
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string MFG_NAME { get; set; }
        public string MFG_ADD { get; set; }
        public string PURCHASER { get; set; }
        public string CONSIGNEE { get; set; }
        public string CASE_NO { get; set; }
        public string CALL_RECV_DATE { get; set; }
        public string CALL_SNO { get; set; }
        public string CALL_LETTER_NO { get; set; }
        public string CALL_LETTER_DT { get; set; }
        public string CALL_INSTALL_NO { get; set; }
        public string ONLINE_CALL { get; set; }
        public string FINAL_OR_STAGE { get; set; }
        public  string REMARKS { get; set; }
        public string ITEM_RDSO { get; set; }
        public string VEND_RDSO { get; set; }
        public string VEND_APP_FR { get; set; }
        public string VEND_APP_TO { get; set; }
        public string STAG_DP { get; set; }
        public string LOT_DP_1 { get; set; }
        public string LOT_DP_2 { get; set; }
        public string IE_NAME { get; set; }
        public string ITEM_DESC_PO { get; set; }
        public string QTY_ORDERED { get; set; }
        public string QTY_TO_INSP { get; set; }
        public string CUM_QTY_PREV_OFFERED { get; set; }
        public string CUM_QTY_PREV_PASSED { get; set; }
        public string VEND_CONTACT_PER_1 { get; set; }
        public string VEND_CONTACT_TEL_1 { get; set; }
        public string VEND_EMAIL { get; set; }
        public string BPO { get; set; }
        public string DELV_DT { get; set; }
        public string ITEM_CD { get; set; }
        public string IRFC_FUNDED { get; set; }       
    }

    public class CoIeWiseCallsList2Model
    {
        public int VEND_CD { get; set; }
        public string VEND_NAME { get; set; }
        public string VEND_ADDRESS { get; set; }
        public string VEND_EMAIL { get; set; }
        public string VEND_CONTACT_PER_1 { get; set; }
        public string VEND_CONTACT_TEL_1 { get; set; }
        public string SOURCE { get; set; }
    }
}
