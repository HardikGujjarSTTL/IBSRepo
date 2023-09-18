namespace IBS.Models
{
    public class InspectionBillingDelayModel
    {
        public string Ic_No { get; set; }
        public string Ic_Dt { get; set; }
        public string IC_SUBMIT_DATE { get; set; } //DateTime?
        public string Bk_No { get; set; }
        public string Set_No { get; set; }
        public DateTime? Call_Dt { get; set; }
        public string Display_Call_Dt { get { return this.Call_Dt != null ? Common.ConvertDateFormat(this.Call_Dt.Value) : ""; } }
        public DateTime? First_Insp_Dt { get; set; }
        public string Display_FInsp { get { return this.First_Insp_Dt != null ? Common.ConvertDateFormat(this.First_Insp_Dt.Value) : ""; } }
        public DateTime? Last_Insp_Dt { get; set; }
        public string Display_LInsp { get { return this.Last_Insp_Dt != null ? Common.ConvertDateFormat(this.Last_Insp_Dt.Value) : ""; } }
        public double Delay_Insp { get; set; }
        public double Delay_Ic { get; set; }
        public double Delay_Subm { get; set; }
        public double Delay_Bill { get; set; }
        public string Bill_No { get; set; }
        public DateTime? Bill_Dt { get; set; }
        public string Display_BillDt { get { return this.Bill_Dt != null ? Common.ConvertDateFormat(this.Bill_Dt.Value) : ""; } }
        public string Ie_Name { get; set; }
        public string Vendor { get; set; }
        public decimal Bill_Amount { get; set; }
        public string Vendor_City { get; set; }
    }

    public class InspectionBillingDelayFilterModel
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SortBy { get; set; }
    }

    public class InspectionBillingDelayReportModel
    {
        public bool MonthWise { get; set; }
        public bool DateWise { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public bool IsBillDate { get; set; }
        public bool IsIEName { get; set; }
        public bool IsIcDt { get; set; }
        public bool IsFInspDt { get; set; }
        public bool IsLFnspDt { get; set; }
        public bool IsAllIE { get; set; }
        public bool IsPartiIE { get; set; }
        public string IECD { get; set; }
        public string ReportTitle { get; set; }
        public string Region { get; set; }
        public List<InspectionBillingDelayModel> InspBillDelayList { get; set; }
    }
}
