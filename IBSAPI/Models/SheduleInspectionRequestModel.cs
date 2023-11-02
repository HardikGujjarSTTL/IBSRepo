namespace IBSAPI.Models
{
    public class SheduleInspectionRequestModel
    {
        public string CaseNo { get; set; }
        public DateTime CallRecvDt { get; set; }
        public int CallSno { get; set; }
        public string InspectionDay { get; set; }
        public string RegionCode { get; set; }
        public string UserId { get; set; }
    }
}
