using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LabPaymentListModel
    {
        public string CaseNo { get; set; }
        public string CallRecvDt { get; set; }
        public string CallSno { get; set; }
        public string GrossTestingChargesLab { get; set; }
        public string NetTestingChargesVend { get; set; }
        public string TDS { get; set; }
        public string GrossVendor { get; set; }
        public string Vendor { get; set; }
        public string Mfg { get; set; }
        public string PaymentRecUpload { get; set; }
        public string DocStatusFin { get; set; }
        public string ReDoc { get; set; }
        public string UTRNO { get; set; }
        public string UTRDT { get; set; }
        public string Remarks { get; set; }
        public string UName { get; set; }
    }

}
