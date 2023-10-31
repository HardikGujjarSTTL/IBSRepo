using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class OnlineComplaints
    {
        public string? Name { get; set; }
        public string? Designation { get; set; }
        public string? TEMP_COMPLAINT_ID { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? MobileNO { get; set; }
        public string? BKNo { get; set; }
        public string? SetNo { get; set; }
        public string? Contract { get; set; }
        public DateTime? IC_DT { get; set; }
        public DateTime? JiFixDt { get; set; }
        public string? IC_NO { get; set; }
        public string? InspRegion { get; set; }
        public string? JiRequired { get; set; }
        public string? NoJIReason { get; set; }
        public byte? JiIeCd { get; set; }
        public string Item { get; set; }
        public string UserId { get; set; }
        public decimal? QtyperIC { get; set; }
        public decimal? Rate { get; set; }
        public decimal? QtyRejected { get; set; }
        public decimal? RejectionValue { get; set; }
        public string? RejectionReason { get; set; }
        public string? Reasonforreject { get; set; }
        public string? Remarks { get; set; }
        public string? CaseNo { get; set; }
        public int? VendCd { get; set; }
        public int? ConsigneeCd { get; set; }
        public string? Vendor { get; set; }
        public string? Consignee { get; set; }
        public string? RejMemono { get; set; }
        public string? InspER { get; set; }
        public string? ITEM_DESC_PO { get; set; }
        public string? Regioncode { get; set; }
        public string? AcceptRejornot { get; set; }
        public byte? ItemSrnoPo { get; set; }
        public byte? UomCd { get; set; }
        public byte? IE_CD { get; set; }
        public byte? CoCd { get; set; }
        public DateTime? TempComplaintDt { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? JIDate { get; set; }
        public DateTime? Registrationdt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? RejMemodate { get; set; }

        public DateTime? CreatedDate { get; set; }  
        public int createdBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }

        public IFormFile ComplaintFile { get; set; }
        public string ComplaintID { get; set; }
    }
}
