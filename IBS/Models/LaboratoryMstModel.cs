using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LaboratoryMstModel
    {
        public int LabId { get; set; }

        public string? LabName { get; set; }

        public string? LabAddress { get; set; }

        public string LabCity { get; set; }

        public string? LabContactPer { get; set; }

        public string? LabContactTel { get; set; }

        public string? LabEmail { get; set; }

        public string? LabApproval { get; set; }

        public string? LabApprovalFr { get; set; }

        public string? LabApprovalTo { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }


    }

}
