using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T53VigilanceCasesMaster
{
    public string RefRegNo { get; set; } = null!;

    public DateTime? RefDt { get; set; }

    public string? RefNo { get; set; }

    public string? RefDetails { get; set; }

    public DateTime? RefReplyDt { get; set; }

    public string? PrelimInvDetails { get; set; }

    public string? ActionProposed { get; set; }

    public DateTime? ActionProposedDt { get; set; }

    public string? FinalAction { get; set; }

    public DateTime? FinalActionDt { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public int Id { get; set; }

    public virtual ICollection<T54VigilanceCasesDetail> T54VigilanceCasesDetails { get; set; } = new List<T54VigilanceCasesDetail>();
}
