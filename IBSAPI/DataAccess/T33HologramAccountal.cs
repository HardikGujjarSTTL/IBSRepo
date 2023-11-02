using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T33HologramAccountal
{
    public string CaseNo { get; set; } = null!;

    public DateTime CallRecvDt { get; set; }

    public int ConsigneeCd { get; set; }

    public short CallSno { get; set; }

    public byte RecNo { get; set; }

    public string? HgRegion { get; set; }

    public string? HgNoMaterialFr { get; set; }

    public string? HgNoMaterialTo { get; set; }

    public string? HgNoSampleFr { get; set; }

    public string? HgNoSampleTo { get; set; }

    public string? HgNoTestFr { get; set; }

    public string? HgNoTestTo { get; set; }

    public string? HgNoIcFr { get; set; }

    public string? HgNoIcTo { get; set; }

    public string? HgNoIcDoc { get; set; }

    public string? HgOtDesc { get; set; }

    public string? HgNoOtFr { get; set; }

    public string? HgNoOtTo { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public int? Createdby { get; set; }

    public DateTimeOffset? Createddate { get; set; }

    public int? Updatedby { get; set; }

    public DateTimeOffset? Updateddate { get; set; }

    public byte? Isdeleted { get; set; }
}
