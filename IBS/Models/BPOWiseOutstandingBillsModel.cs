﻿using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class BPOWiseOutstandingBillsModel
    {
        public string FromDt { get; set; }

        public string ToDt { get; set; }

        public string BpoCd { get; set; }
        public string BpoType { get; set; }
        public string BpoRly { get; set; }
        public string BpoRegion { get; set; }
        public string Railway { get; set; }
        public string PSU { get; set; }
        public string StateGovt { get; set; }
        public string ForeignRailways { get; set; }
        public string PrivateSector { get; set; }
        public string TypeofOutStandingBills { get; set; }
        public string Region { get; set; }
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
        public List<BPOWiseOutstandingBillsModel> lstBPOReport { get; set; }

        public string BILL_NO { get; set; }
        public string BILL_DT { get; set; }
        public string BILL_AMOUNT { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string TOTAL_TDS { get; set; }
        public string RETENTION_MONEY { get; set; }
        public string CNOTE_AMOUNT { get; set; }
        public string WRITE_OFF_AMT { get; set; }
        public string AMOUNT_POSTED { get; set; }
        public string AMOUNT_REALISED { get; set; }
        public string AMOUNT_OUTSTADING { get; set; }
        public string CASE_NO { get; set; }
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string CONSIGNEE { get; set; }
        public string BPO { get; set; }
        public string SAP_CUST_CD_BPO { get; set; }
        public string VENDOR { get; set; }
        public string IE { get; set; }
        public string INVOICE_NO { get; set; }
        public string LO_REMARKS { get; set; }
    }
}
