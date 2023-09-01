using IBS.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IBS.Models
{
    public class InspectionEngineersModel
    {
        public int IeCd { get; set; }

        [Display(Name = "Ie Name")]
        [Required]
        public string? IeName { get; set; }

        [Display(Name = "Ie Short Name")]
        [Required]
        public string? IeSname { get; set; }

        public string? IeEmpNo { get; set; }

        public int? IeDesig { get; set; }

        public int? IeSealNo { get; set; }

        public string? IeDepartment { get; set; }

        public string IeCityCd { get; set; }

        [Range(0, 1000, ErrorMessage = "City Id must number.")]
        public int IeCityId { get; set; }

        [Phone]
        public string? IePhoneNo { get; set; }

        public byte? IeCoCd { get; set; }

        public DateTime? IeJoinDt { get; set; }

        public string? IeStatus { get; set; }

        public DateTime? IeStatusDt { get; set; }

        public string? IeType { get; set; }

        public string? IeRegion { get; set; }

        public string? IePwd { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        [EmailAddress(ErrorMessage = "Email must be proper.")]
        public string? IeEmail { get; set; }

        public DateTime? IeDob { get; set; }

        public int? AltIe { get; set; }

        public string? IeCallMarking { get; set; }

        public int? AltIeTwo { get; set; }

        public int? AltIeThree { get; set; }

        public DateTime? CallMarkingStoppingDt { get; set; }

        public DateTime? DscExpiryDt { get; set; }
        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public string Cluster { get; set; }
    }
}
