namespace IBS.Models
{
    public class Print_Bank_Statement_VoucherModel
    {
        public string CHQ_NO { get; set; }
        public string CHQ_DT { get; set;}
        public string AMOUNT { get; set;}
        public string BANK_NAME { get; set;}

        public List<Print_Bank_Statement_VoucherModel> Print_Bank_Statement { get; set;}
    }
}
