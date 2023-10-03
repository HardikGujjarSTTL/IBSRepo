using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LabSamplePaymentRptModel
    {
        public string ReportStatus { get; set; }
        public string case_no { get; set; }
        public string CallRecvDate { get; set; }
        public string sample_reg_no { get; set; }
        public string CallSno { get; set; }
        public string vend_name { get; set; }
        public string mfg_name { get; set; }
        public string ie_name { get; set; }
        public string REMARKS { get; set; }
        public string LAB_STATUS { get; set; }
        public string DOC_REJ_REMARK { get; set; }
        public string likely_dt_report { get; set; }
        public string testing_charges_by_lab { get; set; }
        public string testing_charges_by_vendor { get; set; }
        public string tds_charges_by_vendor { get; set; }
        public string CallDocDate { get; set; }
        public string doc_status_fin { get; set; }
        public string Vend_INIT_DT { get; set; }
        public string FIN_INIT_DT { get; set; }
        public string call_recv_dt { get; set; }
        public string UTR_NO { get; set; }
        public string UTR_DATE { get; set; }
        public string wFrmDtO { get; set; }
        public string wToDt { get; set; }
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
        public List<LabSamplePaymentRptModel> lstLabSample { get; set; }

    }

}
