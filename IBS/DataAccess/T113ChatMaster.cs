using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T113ChatMaster
{
    public int Id { get; set; }

    public int? MsgSendId { get; set; }

    public int? MsgRecvId { get; set; }

    public string? Message { get; set; }

    public DateTimeOffset? SendMsgDate { get; set; }

    public byte? Isdeleted { get; set; }

    public string? Relativepath { get; set; }

    public string? FieldId { get; set; }

    public string? Extension { get; set; }

    public string? Filedisplayname { get; set; }
}
