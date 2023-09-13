using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class DownloadBillsModel
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BillNo { get; set; }
        public string BpoRly { get; set; }
        public string BpoName { get; set; }
        public string BillDt { get; set; }
        public string DigBillGenDate { get; set; }
        public string BillAmount { get; set; }
        public string BkNo { get; set; }
        public string SetNo { get; set; }
        public string TotalTds { get; set; }
        public string RetentionMoney { get; set; }
        public string WriteOffAmt { get; set; }
        public string AmountPosted { get; set; }
        public string AmountRealised { get; set; }
        public string AmountOutstanding { get; set; }
        public string CaseNo { get; set; }
        public string PoNo { get; set; }
        public string PoDt { get; set; }
        public string PoOrLetter { get; set; }
        public string Consignee { get; set; }
        public string Bpo { get; set; }
        public string Vendor { get; set; }
        public string IeName { get; set; }
        public string InvoiceNo { get; set; }
        public string RecipientGstinNo { get; set; }
        public string IcNo { get; set; }
        public string IcDt { get; set; }
        public string AuDesc { get; set; }
        public string OnlineInvoice { get; set; }
        public string IcPhoto { get; set; }
        public string PoSource { get; set; }
        public string PoYr { get; set; }
        public string ImmsRlyCd { get; set; }
        public string ICExists { get; set; }
        public string CaseNoExists { get; set; }
        public string ICPATH { get; set; }
        public string CASENOPATH { get; set; }

    }

}
