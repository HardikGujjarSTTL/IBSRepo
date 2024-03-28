using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBSAPI.Models
{
    public class CaseDetailIEModel
    {
        public string Case_No { get; set; }
        public DateTime? Call_Recv_DT { get; set; }
        public int Call_SNo { get; set; }
        public decimal? Qty { get; set; }
        public string Status { get; set; }
        public string Vendor { get; set; }
        public string Vendor_Code { get; set; }
        public string Vendor_City { get; set; }
        public string Vendor_Address { get; set; }
        public string PO_Type { get; set; }
        public string PO_No { get; set; }
        public DateTime? PO_DT { get; set; }
        public string PO_Source { get; set; }
        public string MobileNo { get; set; }
        public string EMail { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string? Consignee { get; set; }
        public decimal? QtyPassed { get; set; }
        public decimal? QtyRejected { get; set; }
        public List<PhotosModel> photosModel { get; set; }
        public List<PhotosModel> pdfModel { get; set; }
        public List<SelectListItem> ConsigneeFirmList { get; set; } = new List<SelectListItem>();
    }

    public class PhotosModel
    {
        public long? ID { get; set; }
        public string? ApplicationID { get; set; }
        public int? DocumentCategoryID { get; set; }
        public int? DocumentID { get; set; }
        public string OtherDocumentName { get; set; }
        public string Imageurl { get; set; }
    }
}
