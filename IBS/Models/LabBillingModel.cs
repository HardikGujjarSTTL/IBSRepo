﻿using System.ComponentModel.DataAnnotations;
using System.Composition;

namespace IBS.Models
{
    public class LabBillingModel
    {
        public string Region_Code { get; set; }

        public string Lab_Bill_Per { get; set; }

        [Required]
        public string LabBillPerMon { get; set; }

        [Required]
        public string LabBillPerYear { get; set; }
        [Required]
        public decimal? Lab_Exp { get; set; }

        public string? User_Id { get; set; }

        public DateTime? Datetime { get; set; }

        //public int Id { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public int? Updatedby { get; set; }

    }
}
