namespace IBS.Models
{
    public class DigitalSignatureModel
    {
        public byte[] PDFDocumentByte { get; set; }

        public string PDFDocument { get; set; }

        public int PageNo { get; set; }

        public bool IsMultipleSign { get; set; }

        public bool IsLeft { get; set; }

        public string SearchText { get; set; }

        public int Level { get; set; }

        public decimal X1 { get; set; }

        public decimal Y1 { get; set; }

        public decimal X2 { get; set; }

        public decimal Y2 { get; set; }
    }
}
