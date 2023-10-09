using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.Models
{
    public class DEO_CRIS_PurchesOrderModel
    {
        public string RITES_CASE_NO { get; set; }
        public string IMMS_POKEY { get; set; }
        public string IMMS_RLY_CD { get; set; }
        public string RLY_CD { get; set; }
        public string PURCHASER_CD { get; set; }
        public string IMMS_PURCHASER_CD { get; set; }
        public string IMMS_PURCHASER_DETAIL { get; set; }
        public string STOCK_NONSTOCK { get; set; }
        public string INSPECTING_AGENCY { get; set; }
        public string PO_OR_LETTER { get; set; }
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string L5NO_PO { get; set; }
        public string VEND_CD { get; set; }
        public string IMMS_VENDOR_CD { get; set; }
        public string VENDOR { get; set; }
        public string IMMS_RLY_SHORTNAME { get; set; }
        public string REGION_CODE { get; set; }
        public string REMARKS { get; set; }
        public string BPO_CD { get; set; }
        public string IMMS_BPO_CD { get; set; }
        public string BPO { get; set; }
        public string POI_CD { get; set; }
        public string PO_YR { get; set; }
        public string MFG { get; set; }
        public string RAILWAY { get; set; }
        public DateTime? RecvDt { get; set; }
        public string? L5noPo { get; set; }
        public int? ddlManufac { get; set; }
        public string UserId { get; set; }
        
        //List<SelectListItem> selectVend_CDListItems { get; set; }
    }

    public class DEO_CRIS_PurchesOrderListModel
    {
        public int CASE_NO { get; set; }
        public string IMMS_POKEY { get; set; }
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string RECV_DT { get; set; }
        public string IMMS_RLY_CD { get; set; }
        public string IMMS_RLY_SHORTNAME { get; set; }
        public string RLY_CD { get; set; }
        public string VEND_NAME { get; set; }
        public string REMARKS { get; set; }
        public string PO_DOC { get; set; }
        public string POI { get; set; }
        public string REGION { get; set; }
        public string REGION_CODE { get; set; }
    }

}
