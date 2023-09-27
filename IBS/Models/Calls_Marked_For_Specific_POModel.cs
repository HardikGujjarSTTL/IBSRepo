namespace IBS.Models
{
    public class Calls_Marked_For_Specific_POModel
    {
        public string CLIENT_TYPE { get; set; }
        public string PO_DATE { get; set; }
        public string SelectedRailway { get; set; }

        public List<railway_dropdown> Railways { get; set; }
        public string L5NO_PO { get; set; }
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string RLY_NONRLY { get; set; }
        public string RLY_CD { get; set; }

        public string PURCHASER { get; set; }
    
        public string IC_NO { get; set; }
        public string IC_DATE { get; set; }
        public string BkNo { get; set; }
        public string SetNo { get; set; }
        public string BillNo { get; set; }
        public string IeName { get; set; }
        public string VENDOR { get; set; }
        public string ItemDescPo { get; set; }
        public decimal QtyToInsp { get; set; }
        public decimal QtyPassed { get; set; }
        public decimal QtyRejected { get; set; }
        public string Hologram { get; set; }
        public string ICPhoto { get; set; }
        public string ICPhotoA1 { get; set; }
        public string ICPhotoA2 { get; set; }

    }
    public class railway_dropdown
    {
        public string RLY_CD { get; set; }
        public string RAILWAY_ORGN { get; set; }
    }

}
