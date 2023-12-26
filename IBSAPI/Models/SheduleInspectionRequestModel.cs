using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IBSAPI.Models
{
    public class SheduleInspectionRequestModel
    {
        public string CaseNo { get; set; }
        public DateTime CallRecvDt { get; set; }
        public int CallSno { get; set; }
        public string InspectionDay { get; set; }
        public string RegionCode { get; set; }
        public string UserId { get; set; }
    }

    public class CancelInspectionRequestModel
    {
        public int IeCd { get; set; }
        public string CaseNo { get; set; }
        public DateTime PlanDt { get; set; }
        public DateTime CallRecvDt { get; set; }
        public int CallSno { get; set; }
    }

    public class ICPhotoUploadRequestModel
    {
        public string CaseNo { get; set; }
        public string DocBkNo { get; set; }
        public string DocSetNo { get; set; }
        public string? Consignee { get; set; }
        public decimal? QtyPassed { get; set; }
        public decimal? QtyRejected { get; set; }
        public DateTime CallRecvDt { get; set; }
        public int CallSno { get; set; }
        public string? PoNo { get; set; }
        public int? IeCd { get; set; }
        public string userId { get; set; }
        public int User_Id { get; set; }
        public string AlertMsg { get; set; }
    }

    public class BookNoSetNoModel
    {
        public string DocBkNo { get; set; }
        public string DocSetNo { get; set; }
    }

    public class DeleteICPhotoRequestModel
    {
        public long? ID { get; set; }
        public string ApplicationID { get; set; }
        public int DocumentCategoryID { get; set; }
        public int DocumentID { get; set; }
    }

    public class VenderCallStatusModel
    {
        public string CaseNo { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallRecvDt { get; set; }

        public int? CallSno { get; set; }

        public string? VendName { get; set; }

        public bool[] chkItems { get; set; }

        public string? Consignee { get; set; }

        public string ConsigneeFirm { get; set; }

        public string RejectionCharge { get; set; }

        public string LocalOutstation { get; set; }

        public List<SelectListItem> ConsigneeFirmList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> CallRlyFirmList { get; set; } = new List<SelectListItem>();

        public string? ItemDescPo { get; set; }

        public string? CancellationDescription { get; set; }

        public string? ReasonFIFO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallMarkDt { get; set; }

        public string? IeName { get; set; }
        [Phone]
        public string? IePhoneNo { get; set; }

        public string? PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        [Required(ErrorMessage = "Please Enter Book No.")]
        public string? BkNo { get; set; }
        [Required(ErrorMessage = "Please Enter Set No.")]
        public string? SetNo { get; set; }

        public string? CallStatus1 { get; set; }

        public string? CallStatus { get; set; }

        public string? CallCancelStatus { get; set; }

        public string? CallCancelCharges { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? CallStatusDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DesireDt { get; set; }

        public string? UpdateAllowed { get; set; }

        public string? MfgPers { get; set; }

        public string? MfgPhone { get; set; }

        public decimal? Count { get; set; }

        public byte ItemSrnoPo { get; set; }

        public string Createdby { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Updatedby { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string ActionType { get; set; }

        public string? IeCd { get; set; }
        public int? IssueToIecd { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public string? MaterialValue { get; set; }

        public string? CallListByRly { get; set; }
        public string? ChkFIFO { get; set; }
        public string? AlertMsg { get; set; }
        public string? Chk1 { get; set; }
        public string? Chk2 { get; set; }
        public string? Chk3 { get; set; }
        public string? Chk4 { get; set; }
        public string? Chk5 { get; set; }
        public string? Chk6 { get; set; }
        public string? Chk7 { get; set; }
        public string? Chk8 { get; set; }
        public string? Chk9 { get; set; }
        public string? Chk10 { get; set; }
        public string? Chk11 { get; set; }
        public string? Chk12 { get; set; }
        public string Hologram { get; set; }
        public string Remarks { get; set; }
        public string Remarkslbl { get; set; }
        public DateTime CallLetterDt { get; set; }
        [Required(ErrorMessage = "Please Enter Book No.")]
        public string? DocBkNo { get; set; }
        [Required(ErrorMessage = "Please Enter Set No.")]
        public string? DocSetNo { get; set; }
    }
}
