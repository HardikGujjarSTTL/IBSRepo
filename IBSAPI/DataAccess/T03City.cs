﻿using System;
using System.Collections.Generic;

namespace IBSAPI.DataAccess;

public partial class T03City
{
    public int CityCd { get; set; }

    public string? Location { get; set; }

    public string? City { get; set; }

    public byte? StateCd { get; set; }

    public string? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? PinCode { get; set; }

    public byte? Isdeleted { get; set; }

    public int? Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }

    public int? CountryCd { get; set; }

    public string? CountryName { get; set; }

    public virtual T92State? StateCdNavigation { get; set; }

    public virtual ICollection<T05Vendor> T05Vendors { get; set; } = new List<T05Vendor>();

    public virtual ICollection<T06Consignee> T06Consignees { get; set; } = new List<T06Consignee>();
}
