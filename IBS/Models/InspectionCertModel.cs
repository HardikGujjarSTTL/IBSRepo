using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class InspectionCertModel
    {
        public string? Icno { get; set; }

        public string Caseno { get; set; } = null!;

        public string? Bkno { get; set; }

        public string? Setno { get; set; }

        public string? Status { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime Callrecvdt { get; set; }

        public short Callsno { get; set; }

        public string? Iesname { get; set; }

        public string? Consignee { get; set; }

        public string? Callstatusdesc { get; set; }

        public string? Regioncode { get; set; }

        public string? Callstatus { get; set; }

        public string Createdby { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Updatedby { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UserId { get; set; }
    }
}
