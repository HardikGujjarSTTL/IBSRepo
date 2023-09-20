namespace IBS.Models.Reports
{
    public class RemitanceReportsModel
    {
        public string ReportType { get; set; }

        public string ReportTitle { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ACodeReport { get; set; }

        public string AccCode { get; set; }

        public string RReport { get; set; }

        public string BPO { get; set; }

        public string ClientType { get; set; }

        public string ClientName { get; set; }

        public string BPOName { get; set; }
    }

    public class RemitanceModel
    {
        public string ACC_CD { get; set; }

        public string ACC_DESC { get; set; }

        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public string BPOName { get; set; }

        public string ClientType { get; set; }

        public string ClientName { get; set; }

        public List<RemitanceListModel> lstRemitance { get; set; }

        public List<RemitanceBillWisePeriodListModel> lstRemitanceBillWisePeriod { get; set; }

        public List<RemitanceBillWiseCreatedBillListModel> lstRemitanceBillWiseCreatedBill { get; set; }

        public List<RemitanceChequeWiseBillListModel> lstRemitanceChequeWiseBill { get; set; }

        public List<RemitanceAccountCodeWiseListModel> lstRemitanceAccountCodeWise { get; set; }

        public List<RemitanceClientBPOWiseListModel> lstRemitanceClientBPOWise { get; set; }

        public List<RemitanceStatementExcessListModel> lstRemitanceStatementExcess { get; set; }

    }

    public class RemitanceListModel
    {
        public string VCHR_NO { get; set; }

        public string BANK_NAME { get; set; }

        public string CHQ_NO { get; set; }

        public DateTime CHQ_DT { get; set; }

        public decimal AMOUNT { get; set; }

        public int ACC_CD { get; set; }

        public string CASE_NO { get; set; }

        public DateTime VCHR_DT { get; set; }

        public string NARRATION { get; set; }

        public string BPO { get; set; }
    }

    public class RemitanceBillWisePeriodListModel
    {
        public string VCHR_NO { get; set; }

        public DateTime VCHR_DT { get; set; }

        public string BANK_NAME { get; set; }

        public string CHQ_NO { get; set; }

        public DateTime CHQ_DT { get; set; }

        public decimal CHQ_AMT { get; set; }

        public int ACC_CD { get; set; }

        public decimal BILL_AMT { get; set; }

        public decimal AMT_CLEARED { get; set; }

        public decimal EXCESSORSHORT { get; set; }

        public string BILL_NO { get; set; }

        public DateTime POSTING_DT { get; set; }

        public DateTime BILL_DT { get; set; }

        public string BPO { get; set; }
    }

    public class RemitanceBillWiseCreatedBillListModel
    {
        public string BANK_NAME { get; set; }

        public string CHQ_NO { get; set; }

        public DateTime CHQ_DT { get; set; }

        public decimal CHQ_AMT { get; set; }

        public decimal BILL_AMT { get; set; }

        public decimal AMT_CLEARED { get; set; }

        public decimal EXCESSORSHORT { get; set; }

        public int ACC_CD { get; set; }

        public string BILL_NO { get; set; }

        public DateTime POSTING_DT { get; set; }

        public DateTime BILL_DT { get; set; }

        public string BPO { get; set; }
    }

    public class RemitanceChequeWiseBillListModel
    {
        public string BANK_NAME { get; set; }

        public string CHQ_NO { get; set; }

        public DateTime CHQ_DT { get; set; }

        public decimal AMOUNT { get; set; }

        public string BILL_NO { get; set; }

        public decimal BILL_AMOUNT { get; set; }

        public decimal AMOUNT_CLEARED { get; set; }

        public DateTime POSTING_DT { get; set; }
    }

    public class RemitanceAccountCodeWiseListModel
    {
        public string VCHR_NO { get; set; }

        public DateTime VCHR_DT { get; set; }

        public string BANK_NAME { get; set; }

        public string CHQ_NO { get; set; }

        public DateTime CHQ_DT { get; set; }

        public decimal AMOUNT { get; set; }

        public decimal SUSPENSE_AMT { get; set; }

        public string NARRATION { get; set; }

        public string SAP_CUST_CD { get; set; }

        public string BILL_NO { get; set; }

        public decimal BILL_AMOUNT { get; set; }

        public decimal AMOUNT_CLEARED { get; set; }

        public DateTime POSTING_DT { get; set; }

        public string ACC_GROUP { get; set; }

        public string RECIPIENT_GSTIN_NO { get; set; }

        public DateTime BILL_DT { get; set; }

        public decimal TDS_AMT { get; set; }

        public DateTime TDS_DT { get; set; }
    }

    public class RemitanceClientBPOWiseListModel
    {
        public string BANK_NAME { get; set; }

        public string CHQ_NO { get; set; }

        public DateTime CHQ_DT { get; set; }

        public decimal AMOUNT { get; set; }

        public string BILL_NO { get; set; }

        public DateTime BILL_DT { get; set; }

        public decimal BILL_AMOUNT { get; set; }

        public decimal AMOUNT_CLEARED { get; set; }

        public DateTime POSTING_DT { get; set; }

        public string BPO_CD { get; set; }

        public string BPO { get; set; }
    }

    public class RemitanceStatementExcessListModel
    {
        public string BILL_NO { get; set; }

        public DateTime BILL_DT { get; set; }

        public decimal BILL_AMOUNT { get; set; }

        public decimal BILL_AMT_CLEARED { get; set; }

        public decimal EXCESS_SHORT { get; set; }

        public string BPO { get; set; }
    }
}
