using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class ReturnedBillsModel
    {
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string BpoRly { get; set; }
        public string BkNo { get; set; }
        public string BillAmount { get; set; }
        public string SetNo { get; set; }
        public string BillResentCount { get; set; }
        public string Co6Status { get; set; }
        public string ReturnDate { get; set; }
        public string ReturnReason { get; set; }
        public string AU { get; set; }
    }

}
