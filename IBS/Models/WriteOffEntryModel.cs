namespace IBS.Models
{
    public class WriteOffEntryModel
    {
        public DateTime Chq_DT { get; set; }

        public string BPO_CD { get; set; }

        public string Bill_No { get; set; }

        public DateTime BillDt { get; set; }

        public Decimal BillAmt { get; set; }

        public Decimal BillAmtRec { get; set; }

        public Decimal BillAmtClr { get; set; }

        public Decimal WRITE_OFF_AMT { get; set; }
    }

    public class UpdateDataModel
    {
        public string Bill_No { get; set; }
        public decimal Write_Off_Amt { get; set; }
    }
}
