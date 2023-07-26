﻿using IBS.DataAccess;

namespace IBS.Models
{
	public class RDMModel
	{
        public byte RDesigCd { get; set; }

        public string? RDesignation { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public virtual ICollection<T08IeControllOfficer> T08IeControllOfficers { get; set; } = new List<T08IeControllOfficer>();

        public virtual ICollection<T09Ie> T09Ies { get; set; } = new List<T09Ie>();

    }
}