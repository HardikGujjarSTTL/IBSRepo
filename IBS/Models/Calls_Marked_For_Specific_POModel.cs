namespace IBS.Models
{
    public class Calls_Marked_For_Specific_POModel
    {
        public string CLIENT_TYPE { get; set; }
        public string PO_DATE { get; set; }
        public string SelectedRailway { get; set; }

        public List<railway_dropdown> Railways { get; set; }
        public List<lstSpecificPO> lstSpecificPOs { get; set; }
        public string L5NO_PO { get; set; }
        
        
        public string RLY_NONRLY { get; set; }
        public string RLY_CD { get; set; }


        public string PO_NO { get; set; }

        public string PO_DT { get; set; }


    }

    public class lstSpecificPO {
        public string VENDOR { get; set; }
        public string CONSIGNEE { get; set; }
        public int VEND_CD { get; set; }
        public int MFG_CD { get; set; }
        public string PURCHASER { get; set; }
        public string REMARK { get; set; }
        public string CALL_STATUS { get; set; }
        public string COLOUR { get; set; }
        public string MFG_PERS { get; set; }
        public string MFG_PHONE { get; set; }
        public string CALL_SNO { get; set; }
        public int COUNT { get; set; }
        public string MANUFACTURER { get; set; }
        public string CO_NAME { get; set; }

        public string PO_NO { get; set; }
        public string CASE_NO { get; set; }
        public string IE_PHONE_NO { get; set; }

        public string PO_DT { get; set; }

        public string IC_NO { get; set; }

        public string IC_DATE { get; set; }
        public string CALL_MARK_DT { get; set; }

        public string BkNo { get; set; }
        public string SetNo { get; set; }
        public string IeName { get; set; }
        public string ITEM_DESC_PO { get; set; }

        public decimal QtyRejected { get; set; }

        public string Hologram { get; set; }
        public decimal QtyPassed { get; set; }
        public string BillNo { get; set; }
        public decimal QTY_TO_INSP { get; set; }

        public string ICPhoto { get; set; }
        public string ICPhotoA1 { get; set; }
        public string ICPhotoA2 { get; set; }

        public string SNO { get; set; }
    }

    public class railway_dropdown
    {
        public string RLY_CD { get; set; }
        public string RAILWAY_ORGN { get; set; }
    }

}
