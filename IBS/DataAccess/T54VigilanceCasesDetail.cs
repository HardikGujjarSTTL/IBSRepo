using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T54VigilanceCasesDetail
{
    public string? RefRegNo { get; set; }

    public int? Sno { get; set; }

    public string? CaseNo { get; set; }

    public string? BkNo { get; set; }

    public string? SetNo { get; set; }

    public int? ConsigneeCd { get; set; }

    public string? BpoCd { get; set; }

    public int? IeCd { get; set; }

    public string? BillNo { get; set; }

    public DateTime? BillDt { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public int Id { get; set; }

    public virtual T53VigilanceCasesMaster? RefRegNoNavigation { get; set; }
}
