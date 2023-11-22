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

    public class CancelInspectionRequestModel
    {
        public int IeCd { get; set; }
        public string CaseNo { get; set; }
        public DateTime PlanDt { get; set; }
        public DateTime CallRecvDt { get; set; }
        public int CallSno { get; set; }
    }

    public class ICPhotoUploadRequestModel
    {
        public string CaseNo { get; set; }
        public string DocBkNo { get; set; }
        public string DocSetNo { get; set; }
    }

    public class DeleteICPhotoRequestModel
    {
        public long? ID { get; set; }
        public string ApplicationID { get; set; }
        public int DocumentCategoryID { get; set; }
        public int DocumentID { get; set; }
    }
}
