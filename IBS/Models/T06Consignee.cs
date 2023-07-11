using System;
using System.Collections.Generic;

namespace IBS.Models;

public partial class T06Consignee
{
    public int ConsigneeCd { get; set; }

    public string? ConsigneeType { get; set; }

    public string? ConsigneeDesig { get; set; }

    public string? ConsigneeDept { get; set; }

    public string? ConsigneeFirm { get; set; }

    public string? ConsigneeAdd1 { get; set; }

    public string? ConsigneeAdd2 { get; set; }

    public byte? ConsigneeCity { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? GstinNo { get; set; }

    public string? SapCustCdCon { get; set; }

    public string? LegalName { get; set; }

    public string? PinCode { get; set; }

    public virtual T03City? ConsigneeCityNavigation { get; set; }

    public virtual ICollection<T13PoMaster> T13PoMasters { get; set; } = new List<T13PoMaster>();

    public virtual ICollection<T14PoBpo> T14PoBpos { get; set; } = new List<T14PoBpo>();

    public virtual ICollection<T18CallDetail> T18CallDetails { get; set; } = new List<T18CallDetail>();

    public virtual ICollection<T40ConsigneeComplaint> T40ConsigneeComplaints { get; set; } = new List<T40ConsigneeComplaint>();

    public virtual ICollection<T41NcMaster> T41NcMasters { get; set; } = new List<T41NcMaster>();

    public virtual ICollection<T80PoMaster> T80PoMasters { get; set; } = new List<T80PoMaster>();
}
