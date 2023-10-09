using System.Net;

namespace IBS.Models
{
    public class BarcodeGenerate
    {
       public string BARCODE{ get; set; }
       public string CASE_NO{ get; set; }
       public string CALL_RECV_DT{ get; set; }
       public Int32 CALL_SNO{ get; set; }
       public string ITEM_SRNO_PO{ get; set; }
       public string VEND_CD{ get; set; }
        public string IE_CD { get; set; }
        public string IE_NAME { get; set; }
        public string IPADDRESS { get; set; }
        public string CUSTOMER_NAME{ get; set; }
       public string SEALING_TYPE{ get; set; }
       public string CUSTOMER_GSTN{ get; set; }
       public string DESCRIPTION{ get; set; }
       public string TARGETED_DATE{ get; set; }
       public string CURRENT_DATE{ get; set; }
       public string INSPECTOR_CUSTOMER{ get; set; }
       public string CREATEDBY{ get; set; }
       public string CREATEDDATE{ get; set; }
       public string USERID{ get; set; }
       public string VEND_NAME{ get; set; }
       public string TotalRate { get; set; }
       public string GSTAmount { get; set; }
        public string Region { get; set; }
        public string Discipline { get; set; }
        public string TypeGST { get; set; }
        public string SGST { get; set; }
        public string CGST { get; set; }
        public string IGST { get; set; }
        public string Total { get; set; }
        public string Tax { get; set; }
        public string GTotal { get; set; }
        public string HandlingCharge { get; set; }
        public string ExtraCharge { get; set; }
        public string RTotal { get; set; }
        public string LABRATEID { get; set; }
        public string DISCIPLINE_ID { get; set; }
        public string TEST_NAME { get; set; }
        public string PRICE { get; set; }
        public string QTY { get; set; }

    }
}
