namespace IBS.Models
{
    public class CertificateDetails
    {
        public string CommonName { get; set; }
        public string Email { get; set; }
        public string SerialNumber { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Locality { get; set; }
        public string PostalCode { get; set; }
        public string OrganizationUnit { get; set; }
        public string Organization { get; set; }
        public string Country { get; set; }
        public DateTime? DSC_Exp_DT { get; set; }
        public string IE_Email { get; set; }
        public string IE_Phone_No { get; set; }
    }
}
