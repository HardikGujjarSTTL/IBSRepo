using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class LabSampleInfoModel
    {
        public string CallRecDt { get; set; }
        public string CaseNo { get; set; }
        public string CallSNO { get; set; }
        public string SNO { get; set; }
        public string IEName { get; set; }

        public string Vendor { get; set;}
        public string VendorName { get; set;}
        public string IE { get; set;}
        public string DateofRecSample { get; set; }
        public string TotalTFee { get; set; }

        public string LikelyDt { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string PaymentSlip { get; set; }
        public string PaymentStatus { get; set; }
        public IFormFile UploadLab { get; set; }
        public string Data { get; set; }
        public string UName { get; set; }
        public string NetTesting { get; set; }
        public string TDS { get; set; }
        public string UTRNO { get; set; }
        public string UTRDT { get; set; }
        public IFormFile UploadPayment { get; set; }
        public string File { get; set; }
        public string FileNm { get; set; }
        public string FileLink { get; set; }
        public string Hyperlink2 { get; set; }
        public string LabelExist { get; set; }


    }

}
