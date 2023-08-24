namespace IBS.Models
{
    public class CallMarkedOnlineModel
    {
        public string CASE_NO { get; set; }
        public string CALL_RECV_DT { get; set; } //CALL_RECV_DATE
        public string CALL_INSTALL_NO { get; set; }
        public string CALL_SNO { get; set; }
        public string DATE_TIME { get; set; }
        public string CALL_STATUS { get; set; }
        public string CALL_LETTER_NO { get; set; }
        public string REMARKS { get; set; }
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string VENDOR { get; set; }
        public string IE_NAME { get; set; }

        public string RLY { get; set;}
        public string L5NO_PO { get; set; }
        public string VEND_NAME { get; set; }
        public string VEND_REMARKS { get; set; }
        public string INSP_DESIRE_DATE { get; set; }
        public string LETTER_DT { get; set; }
        public string MFG { get; set; }
        public string MFG_REMARKS { get; set; }
        public string ITEM_SRNO_PO { get; set; }
        public string ITEM_DESC_PO { get; set; }
        public string QTY_ORDERED { get; set; }
        public string QTY_TO_INSP { get; set; }
        public string MFG_CD { get; set; }
        public string STAGG_DP { get; set; }
        public string LOT_DP_1 { get; set; }
        public string LOT_DP_2 { get; set; }
        public string REG_TIME { get; set; }
        public string DEPARTMENT { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPT_DROPDOWN { get; set; }
        public string FINAL_OR_STAGE { get; set; }
        public string CALL_MATERIAL_VALUE { get; set; }

        public string REJECT_REASON { get; set; }
    }
    
    public class CallMarkedOnlineFilter
    {
        public string CASE_NO { get; set; }
        public string Date { get; set; }
        public string CALL_SNO { get; set; }
    }

    public class CallMaterialValueModel
    {
        public string QTY { get; set; }
        public string VALUE { get; set; }
        public string QTY_TO_INSP { get; set; }
    }

    public class VendorDetailModel
    {
        public string VEND_CD { get; set;}
        public string VEND_NAME { get; set; }
        public string VEND_ADDRESS { get; set; }
        public string VEND_EMAIL { get; set; }
        
        public string MANU_MAIL { get; set; }

        public string MFG_CD { get; set; }
    }
}
