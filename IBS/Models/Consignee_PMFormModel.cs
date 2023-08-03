using IBS.DataAccess;

namespace IBS.Models
{
    public class Consignee_PMFormModel
    {
        public int ConsigneeCd { get; set; }

        public string? ConsigneeType { get; set; }

        public string? ConsigneeDesig { get; set; }

        public string? ConsigneeDept { get; set; }

        public string? ConsigneeFirm { get; set; }

        public string? ConsigneeAdd1 { get; set; }

        public string? ConsigneeAdd2 { get; set; }

        public int? ConsigneeCity { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? GstinNo { get; set; }

        public string? SapCustCdCon { get; set; }

        public string? LegalName { get; set; }

        public string? PinCode { get; set; }
        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public virtual T03City? ConsigneeCityNavigation { get; set; }

        public virtual ICollection<T13PoMaster> T13PoMasters { get; set; } = new List<T13PoMaster>();

        public virtual ICollection<T14PoBpo> T14PoBpos { get; set; } = new List<T14PoBpo>();

        public virtual ICollection<T18CallDetail> T18CallDetails { get; set; } = new List<T18CallDetail>();

        public virtual ICollection<T40ConsigneeComplaint> T40ConsigneeComplaints { get; set; } = new List<T40ConsigneeComplaint>();

        public virtual ICollection<T41NcMaster> T41NcMasters { get; set; } = new List<T41NcMaster>();

        public virtual ICollection<T80PoMaster> T80PoMasters { get; set; } = new List<T80PoMaster>();
    }

}

