namespace IBS.Models
{
    public class DashboardModel
    {
        public int TotalCallsCount { get; set; }

        public int PendingCallsCount { get; set; }

        public int AcceptedCallsCount { get; set; }

        public int CancelledCallsCount { get; set; }

        public int UnderLabTestingCount { get; set; }

        public int StillUnderInspectionCount { get; set; }

        public int StageRejectionCount { get; set; }

        public int DSCExpiryDateCount { get; set; }

        public int NCIsuedAgainstIECount { get; set; }

        public int OutstandingNCCount { get; set; }

        public int NotRecievedCount { get; set; }

        public int ConsigneeCompaintCount { get; set; }
    }
}
