﻿using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface IDetailsofPurchaserWiseInspRepository
    {
        public DetailsofPurchaserWiseInspModel SummaryInsp(string ReportType, string Month, string Year, string ForGiven, string ReportBasedon, string MaterialValue, string FromDate, string ToDate, string ForParticular, string lstParticular, string Regin);
       
    }
}
