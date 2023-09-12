namespace IBS.Models
{
    public class InspectionBillingDelayModel
    {
        public string Ic_No { get; set; }
        public string Ic_Dt { get; set; }
        public DateTime IC_SUBMIT_DATE { get; set; }
        public string Bk_No { get; set; }
        public string Set_No { get; set; }
        public DateTime Call_Dt { get; set; }
        public DateTime First_Insp_Dt { get; set; }
        public DateTime Last_Insp_Dt { get; set; }
        public double Delay_Insp { get; set; }
        public double Delay_Ic { get; set; }
        public double Delay_Subm { get; set; }
        public double Delay_Bill { get; set; }
        public string Bill_No { get; set; }
        public DateTime Bill_Dt { get; set; }
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
}
