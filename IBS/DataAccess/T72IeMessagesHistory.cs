using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T72IeMessagesHistory
{
    public int MessageId { get; set; }

    public string? LetterNo { get; set; }

    public DateTime? LetterDt { get; set; }

    public string? Message { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? RegionCode { get; set; }

    public DateTime? MessageDt { get; set; }

    public string? Createdby { get; set; }

    public string? Updatedby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public decimal? Isdeleted { get; set; }

    public string? Actiontype { get; set; }

    public DateTime? Actiondate { get; set; }

    public string? Actionuserid { get; set; }

    public int Id { get; set; }
}
