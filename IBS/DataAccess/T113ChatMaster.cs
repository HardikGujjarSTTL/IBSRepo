using System;
using System.Collections.Generic;

namespace IBS.DataAccess;

public partial class T113ChatMaster
{
    public int Id { get; set; }

    public string? ConnectionId { get; set; }

    public string? MsgSendId { get; set; }

    public string? MsgRecvId { get; set; }

    public string? Message { get; set; }

    public string? UserId { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }
}
