namespace IBS.Models
{
    public class TDSEntryModel
    {

        public string BILL_NO { get; set; }
        public string CASE_NO { get; set; }

        public decimal TDS { get; set; }

            
        public decimal Retention_Money{ get; set; }


        public decimal WrtOffAmount { get; set; }


        public decimal Bill_Amount	 { get; set; }

        public decimal TDSCGST { get; set; }

        public decimal TDSSGST	 { get; set; }

        public decimal TDSIGST	 { get; set; }

        public string TDSposting_DT { get; set; }



    }
}
