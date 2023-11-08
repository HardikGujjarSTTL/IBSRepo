namespace IBSAPI.Models
{
    public class VendorPedingInspectionModel
    {
        public string Case_No { get; set; }
        public string Call_Recv_Dt { get; set; }
        public int Call_Sno { get; set; }
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string Client_Name { get; set; }
        public string IE_Name { get; set; }
        public decimal? Qty { get; set; }
        public string Status { get; set; }
    }
}
