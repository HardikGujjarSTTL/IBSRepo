namespace IBS.Models.Reports
{
    public class OtherReportsModel
    {
        public string ReportType { get; set; }

        public string ReportTitle { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string CO { get; set; }
        public string Status { get; set; }
        public string IE { get; set; }
        public bool IsCallDate { get; set; }
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
}
