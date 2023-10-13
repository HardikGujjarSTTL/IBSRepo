namespace IBS.Models
{
    public class Print_Call_letter_Model
    {
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string MFG_NAME { get; set; }
        public string MFG_ADD { get; set; }
        public string PURCHASER { get; set; }
       public string CONSIGNEE { get; set; }
        public string CASE_NO { get; set; }                                                                                                                                                                         
        public string CALL_RECV_DATE { get; set; }
        public int CALL_SNO { get; set; }
        public string CALL_LETTER_NO { get; set; }
        public string CALL_LETTER_DT { get; set; }
        public int CALL_INSTALL_NO { get; set; }
        public string ONLINE_CALL { get; set; }
        public string FINAL_OR_STAGE { get; set; }
        public string REMARKS { get; set; }
        public string ITEM_RDSO { get; set; }
        public string VEND_RDSO { get; set; }
        public string VEND_APP_FR { get; set; }
        public string VEND_APP_TO { get; set; }
        public string STAG_DP { get; set; }
        public string LOT_DP_1 { get; set; }
        public string LOT_DP_2 { get; set; }
        public string IE_NAME { get; set; }
      public string ITEM_DESC_PO { get; set; }
        public decimal QTY_ORDERED { get; set; }
        public decimal QTY_TO_INSP { get; set; }
       public decimal CUM_QTY_PREV_OFFERED { get; set; }
        public decimal CUM_QTY_PREV_PASSED { get; set; }
        public string VEND_CONTACT_PER_1 { get; set; }
        public string VEND_CONTACT_TEL_1 { get; set; }
        public string VEND_EMAIL { get; set; }
        public string BPO { get; set; }
        public string DELV_DT { get; set; }
      public string ITEM_CD { get; set; }
        public string IRFC_FUNDED { get; set; }

        public string VEND_CD { get; set; }
        public string VEND_NAME { get; set; }
        public string VEND_ADDRESS { get; set; }
      
        public string SOURCE { get; set; }

       public List<subreport> subreport { get; set; } 
        public List<Print_Call_letter_Model> printcalllater { get; set; }
    }

    public class subreport
    {
        public string CONSIGNEE { get; set; }
        public string ITEM_DESC_PO { get; set; }
        public decimal QTY_ORDERED { get; set; }
        public decimal QTY_TO_INSP { get; set; }
        public decimal CUM_QTY_PREV_OFFERED { get; set; }
        public string DELV_DT { get; set; }
        public string BPO { get; set; }
        public string ITEM_CD { get; set; }
        public decimal CUM_QTY_PREV_PASSED { get; set; }

    }
}
