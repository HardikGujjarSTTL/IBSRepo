namespace IBS.Models
{
    #region AuthenticateModel
    public class BillsAPIModel
    {
        public string token { get; set; }
    }
    #endregion

    #region PassedBills
    public class PassedBillsModel
    {
        public string BILL_NO { get; set; }
        public string IC_NO { get; set; }
        public string IC_DT { get; set; }
        public string INVOICENO { get; set; }
        public string CO6_NO { get; set; }
        public string CO6_DATE { get; set; }
        public string CO6_STATUS { get; set; }
        public string CO6_STATUS_DATE { get; set; }
        public string PASSED_AMT { get; set; }
        public string DEDUCTED_AMT { get; set; }
        public string NET_AMT { get; set; }
        public string BOOKDATE { get; set; }
        public string RETURN_REASON { get; set; }
        public string RETURN_DATE { get; set; }
        public string CO7_NO { get; set; }
        public string CO7_DATE { get; set; }
        public string PAYMENT_DT { get; set; }
        public string BILL_RESENT_COUNT { get; set; }
        public string IRFC_FUNDED { get; set; }
        public string INVOICE_SUPP_DOCS { get; set; }
    }
    public class PassedBillsResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<object> error { get; set; }
        public string timestamp { get; set; }
        public List<PassedBillsModel> data { get; set; }
    }
    #endregion

    #region PassedBills
    public class AllBillsModel
    {
        public string BILL_NO { get; set; }
        public string IC_NO { get; set; }
        public string IC_DT { get; set; }
        public string INVOICENO { get; set; }
        public string CO6_NO { get; set; }
        public string CO6_DATE { get; set; }
        public string CO6_STATUS { get; set; }
        public string CO6_STATUS_DATE { get; set; }
        public string PASSED_AMT { get; set; }
        public string DEDUCTED_AMT { get; set; }
        public string NET_AMT { get; set; }
        public string BOOKDATE { get; set; }
        public string RETURN_REASON { get; set; }
        public string RETURN_DATE { get; set; }
        public string CO7_NO { get; set; }
        public string CO7_DATE { get; set; }
        public string PAYMENT_DT { get; set; }
        public string BILL_RESENT_COUNT { get; set; }
        public string IRFC_FUNDED { get; set; }
        public string INVOICE_SUPP_DOCS { get; set; }
    }
    public class AllBillsResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<object> error { get; set; }
        public string timestamp { get; set; }
        public List<AllBillsModel> data { get; set; }
    }
    #endregion
}
