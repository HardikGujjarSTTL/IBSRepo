using IBS.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class DailyWorkPlanModel
    {
        public int IeCd { get; set; }

        public string? CaseNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime CallRecvDt { get; set; }

        public int CallSno { get; set; }

        public string CallStatus { get; set; }

        public int? MfgCd { get; set; }

        public string? MfgPlace { get; set; }

        public int? CoCd { get; set; }

        public string? VendName { get; set; }

        public int CityCd { get; set; }

        public string? MFGCity { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DtInspDesire { get; set; }

        [Required]
        public string? Reason { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime ReasonDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? NwpDt { get; set; }

        public string Display_NwpDt { get { return this.NwpDt != null ? Common.ConvertDateFormat(this.NwpDt.Value) : ""; } }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? VisitDt { get; set; }

        public string PlanDt { get; set; }

        public string InspWorkType { get; set; }

        public string? RegionCode { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? Createdby { get; set; }

        public string? Updatedby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public decimal? Isdeleted { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? FromDt { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? ToDt { get; set; }

        public int errcode { get; set; }

        public string checkedWork { get; set; }

        public string NIWorkType { get; set; }

        public string OtherDesc { get; set; }

        public string ActionType { get; set; }

        public string IsUrgency { get; set; }

        public int PlanDHours { get; set; }
    }

    public class DeSerializeDailyWorkModel
    {
        public string CaseNo { get; set; }

        public DateTime CallRecvDt { get; set; }

        public int CallSno { get; set; }
        public string IsUrgency { get; set; }
    }
}
