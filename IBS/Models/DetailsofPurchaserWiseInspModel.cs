using IBS.DataAccess;
using IBS.Models.Reports;

namespace IBS.Models
{
    public class DetailsofPurchaserWiseInspModel
    {
        public int SrNo { get; set; }
        public string SearchPurchaser { get; set; }
        public string PurchaserCD { get; set; }
        public string PURCHASER { get; set; }

        public string CASE_NO { get; set; }

        public string PO_NO { get; set; }

        public string PO_DT { get; set; }
        public string REMARKS { get; set; }
        public string IC_NO { get; set; }
        public string IC_DT { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string BILL_NO { get; set; }
        public string BILL_DATE { get; set; }
        public string IE_SNAME { get; set; }
        public string BPO { get; set; }
        public string VENDOR { get; set; }
        public string INSP_FEE { get; set; }
        public string VISITS { get; set; }
        public string VALUE { get; set; }
        public string ITEM_DESC { get; set; }

        public string Region { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string FromDt { get; set; }
        public string ToDt { get; set; }
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
        public string ForGiven { get; set; }
        public string ReportBasedon { get; set; }
        public string TextPurchase { get; set; }
        public string ForParticular { get; set; }
        public string lstParticular { get; set; }
        public string MaterialValue { get; set; }
        public List<DetailsofPurchaserWiseInspModel> lstdetailspinsp { get; set; }
    }
}
